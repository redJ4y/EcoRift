using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlyingCharacterController2D))]

public class FlyingEnemyBrain : MonoBehaviour
{
    [SerializeReference] public FlyingCharacterController2D controller;
    [SerializeReference] public GameObject player;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Movement")]
    [Range(1.0f, 60.0f)] [SerializeField] private float movementSpeed = 30f;
    [Range(1, 10)] [SerializeField] private int targetHeight = 5;
    [Range(1, 5)] [SerializeField] private int minimumHeight = 2;
    [SerializeField] private int minimumYLevel = 5;

    [Header("Behavior")]
    [Tooltip("The distance at which the enemy will begin tracking the player")]
    [Range(10, 50)] [SerializeField] private int aggroDistance = 6;
    [Tooltip("Whether to keep aggro after the player moves out of Aggro Distance")]
    [SerializeField] private bool keepAggro = false;
    [Tooltip("The distance the enemy may travel from its origin while not aggroed")]
    [Range(0, 50)] [SerializeField] private int patrolDistance = 5;

    // For ray casting:
    private Vector2 downRightRay;
    private Vector2 downLeftRay;

    private Vector2 currentMovement = Vector2.zero;
    private bool patrolling = false;
    private float patrolPostX = 0; // The x position to patrol around
    private bool permanentAggro = false; // If enemy was aggroed and keepAgro is enabled
    private float timeSinceDirectionChange = 0;

    // Start is called before the first frame update
    void Start()
    {
        targetHeight *= 2; // Start correcting sooner
        // Set up ray casting variables:
        downRightRay = (new Vector2(0.05f, -1)).normalized;
        downLeftRay = downRightRay;
        downLeftRay[0] *= -1; // Flip downRightRay over y-axis
    }

    // FixedUpdate is called a fixed number of times per second
    void FixedUpdate()
    {
        currentMovement = ValidateMovement(SmoothMovement(GetPreferredMovement()));

        controller.Move(currentMovement * Time.fixedDeltaTime);
        timeSinceDirectionChange += Time.fixedDeltaTime;
    }

    // Returns the preferred movement value (not scaled by movement speed)
    private Vector2 GetPreferredMovement()
    {
        Vector3 myPos = gameObject.transform.position;
        Vector3 playerPos = player.transform.position;
        Vector2 toPlayer = playerPos - myPos;

        Vector2 preferredMovement;
        if (permanentAggro || toPlayer.magnitude < aggroDistance)
        { // The enemy is aggroed...
            patrolling = false;
            if (keepAggro)
                permanentAggro = true; // Lock in aggro if keepAggro is enabled

            preferredMovement = toPlayer.normalized;
        }
        else
        { // The enemy is not aggroed, patrol...
            if (!patrolling)
                patrolPostX = myPos.x; // Update patrol position
            patrolling = true;
            if (myPos.x < patrolPostX - patrolDistance)
            {
                preferredMovement = Vector2.right;
                if (Random.value < 0.5f)
                    preferredMovement = Vector2.zero;
            }
            else if (myPos.x > patrolPostX + patrolDistance)
            {
                preferredMovement = Vector2.left;
                if (Random.value < 0.5f)
                    preferredMovement = Vector2.zero;
            }
            else
            { // Move freely...
                if (Random.value < 0.1f)
                {
                    if (Random.value < 0.5f)
                    {
                        preferredMovement = Vector2.right;
                    }
                    else
                    {
                        preferredMovement = Vector2.left;
                    }
                }
                else
                { // High chance of resuming previous movement
                    preferredMovement = currentMovement.normalized;
                }
            }
        }
        // Try to keep target height:
        RaycastHit2D downRightRayHit = Physics2D.Raycast(transform.position, downRightRay, 15, whatIsGround);
        RaycastHit2D downLeftRayHit = Physics2D.Raycast(transform.position, downLeftRay, 15, whatIsGround);
        float distanceToGround = Mathf.Min(downRightRayHit.collider == null ? float.MaxValue : downRightRayHit.distance, downLeftRayHit.collider == null ? float.MaxValue : downLeftRayHit.distance);
        if (distanceToGround < targetHeight)
        {
            float distanceRatio = Mathf.Max(1 - (distanceToGround / targetHeight), 0); // Ratio increases as height decreases below target height (0-1)
            if (Random.value < distanceRatio)
            {
                preferredMovement.y += Mathf.Pow(distanceRatio, 2); // Aim more upwards
                preferredMovement = preferredMovement.normalized; // Re-normalize
            }
        }
        return preferredMovement;
    }

    // Returns a less annoying movement value (now scaled by movement speed)
    private Vector2 SmoothMovement(Vector2 preferredMovement)
    {
        if (Random.value < timeSinceDirectionChange / 5.0f) // Do not always respond immediately (increase chance as time passes)
        {
            timeSinceDirectionChange = 0; // Reset duration since change (increased in every fixed update)
            return preferredMovement * movementSpeed; // Accept preferredMovement
        }
        else
        { // Randomly pick one of two options instead of switching directions...
            if (Random.value < 0.9f)
            {
                currentMovement.y /= 2.0f;
                return currentMovement; // Assume previous direction with less upwards movement
            }
            else
            {
                if (currentMovement.magnitude > 0)
                    timeSinceDirectionChange = 0;
                return Vector2.zero; // Stop and wait
            }
        }
    }

    private Vector2 ValidateMovement(Vector2 preferredMovement)
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, minimumHeight, whatIsGround))
        { // Within minimum height, move up...
            Vector2 correctedMovement = preferredMovement.normalized;
            correctedMovement.y += 0.1f;
            return correctedMovement.normalized * movementSpeed;
        }
        return preferredMovement;
    }
}
