using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]

public class GroundEnemyBrain : MonoBehaviour
{
    [SerializeReference] public CharacterController2D controller;
    [SerializeReference] public GameObject player;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Movement")]
    [Range(1.0f, 60.0f)] [SerializeField] private float movementSpeed = 30f;
    [SerializeField] private bool canJump = true;

    [Header("Behavior")]
    [Tooltip("The distance at which the enemy will begin tracking the player")]
    [Range(1, 50)] [SerializeField] private int aggroDistance = 6;
    [Tooltip("Whether to keep aggro after the player moves out of Aggro Distance")]
    [SerializeField] private bool keepAggro = false;
    [Tooltip("The prefered minimum (closest) distance from the player")]
    [Range(0, 20)] [SerializeField] private int keepDistance = 0;
    [Tooltip("The distance the enemy may travel from its origin while not aggroed")]
    [Range(0, 50)] [SerializeField] private int patrolDistance = 5;
    // [Tooltip("The health at which the enemy retreats")]
    // [Range(0.0f, 99.9f)] [SerializeField] private float retreatHealth = 5;

    // [Header("Combat")]
    // [Range(1, 50)] [SerializeField] private int attackRange = 5;
    // [Range(1, 50)] [SerializeField] private int attackSpeed = 1;

    private float halfEnemyHeight;
    private float halfEnemyHeightSquared;
    private float halfMovementSpeed;
    private float gravity;
    // For ray casting:
    private float rayLength;
    private Vector2 rightRayNormalized;
    private Vector2 leftRayNormalized;

    private float currentMovement = 0;
    private bool patrolling = false;
    private float patrolPostX = 0; // The x position to patrol around
    private bool jump = false;
    private bool permanentAggro = false; // If enemy was aggroed and keepAgro is enabled
    private float timeSinceDirectionChange = 0;
    private bool currentlyLeaping = false;

    // Start is called before the first frame update
    void Start()
    {
        float enemyHeight = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        halfEnemyHeight = enemyHeight / 2.0f;
        halfEnemyHeightSquared = Mathf.Pow(halfEnemyHeight, 2);
        halfMovementSpeed = movementSpeed / 2.0f;
        gravity = gameObject.GetComponent<Rigidbody2D>().gravityScale * 9.8f;

        // Set up ray casting variables:
        Vector2 rightRay = new Vector2(gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 4.0f, -(enemyHeight / 2.0f));
        rayLength = rightRay.magnitude + 1; // Extend vector in ray cast
        rightRayNormalized = rightRay.normalized;
        leftRayNormalized = rightRayNormalized;
        leftRayNormalized[0] *= -1; // Flip rightRayNormalized over y-axis
    }

    // FixedUpdate is called a fixed number of times per second
    void FixedUpdate()
    {
        currentMovement = ValidateMovement(SmoothMovement(GetPreferredMovement()));

        controller.Move(currentMovement * Time.fixedDeltaTime, false, jump);
        timeSinceDirectionChange += Time.fixedDeltaTime;
        jump = false;
    }

    // Returns the preferred movement value (not scaled by movement speed)
    private float GetPreferredMovement()
    {
        Vector3 myPos = gameObject.transform.position;
        Vector3 playerPos = player.transform.position;
        float distance = Mathf.Sqrt(Mathf.Pow(myPos.y - playerPos.y, 2) + Mathf.Pow(myPos.x - playerPos.x, 2));

        float preferredMovement = 0;
        if (permanentAggro || distance < aggroDistance)
        { // The enemy is aggroed...
            patrolling = false;
            if (keepAggro)
                permanentAggro = true; // Lock in aggro if keepAggro is enabled
            // Determine movement direction:
            if (myPos.x < playerPos.x)
            {
                preferredMovement = 1;
            }
            else
            {
                preferredMovement = -1;
            }

            if (distance < keepDistance && Random.value < 0.6f) // Ensure keepDistance is loosely obeyed
                preferredMovement *= -1; // Invert movement direction (away from player)
        }
        else
        { // The enemy is not aggroed, patrol...
            if (!patrolling)
                patrolPostX = myPos.x; // Update patrol position
            patrolling = true;
            if (myPos.x < patrolPostX - patrolDistance)
            {
                preferredMovement = 1;
                if (Random.value < 0.5f)
                    preferredMovement = 0;
            }
            else if (myPos.x > patrolPostX + patrolDistance)
            {
                preferredMovement = -1;
                if (Random.value < 0.5f)
                    preferredMovement = 0;
            }
            else
            { // Move freely...
                if (Random.value < 0.1f)
                {
                    if (Random.value < 0.5f)
                    {
                        preferredMovement = 1;
                    }
                    else
                    {
                        preferredMovement = -1;
                    }
                }
                else
                { // High chance of resuming previous movement
                    preferredMovement = System.Math.Sign(currentMovement);
                }
            }
        }
        return preferredMovement;
    }

    // Returns a less annoying movement value (now scaled by movement speed)
    private float SmoothMovement(float preferredMovement)
    {
        int currentMovementRaw = System.Math.Sign(currentMovement);
        if (currentMovementRaw != preferredMovement) // Check for direction change
        { // Apply smoothing to direction change...
            if (timeSinceDirectionChange > 0.5f && Random.value < timeSinceDirectionChange / 10.0f) // Do not always switch directions immediately (increase chance as time passes)
            {
                timeSinceDirectionChange = 0; // Reset duration since change (increased in every fixed update)
                return preferredMovement * movementSpeed; // Accept preferredMovement
            }
            else
            {
                if (Random.value < 0.8f) // Randomly pick one of two options instead of switching directions...
                {
                    return currentMovementRaw * halfMovementSpeed; // Assume previous direction but slower
                }
                else
                {
                    if (currentMovementRaw != 0)
                        timeSinceDirectionChange = 0;
                    return 0; // Stop and wait
                }
            }
        }
        return currentMovementRaw * movementSpeed;
    }

    private float ValidateMovement(float preferredMovement)
    {
        /* Debugging visuals:
        * float jumpDistanceT = GetApproximateJumpDistance();
        * float jumpRayLengthT = Mathf.Sqrt(Mathf.Pow(jumpDistanceT, 2) + halfPlayerHeightSquared) * 1.01f;
        * Vector2 jumpToRayT = new Vector2(preferredMovement < 0 ? -jumpDistanceT : jumpDistanceT, -halfPlayerHeight);
        * Debug.DrawRay(transform.position, jumpToRayT.normalized * jumpRayLengthT, Color.red); 
        */
        if (currentlyLeaping)
        {
            jump = true;
        }
        else
        {
            if (preferredMovement != 0)
            {
                // Determine which ray to use based on movement direction:
                Vector2 ray = rightRayNormalized;
                if (preferredMovement < 0)
                    ray = leftRayNormalized;
                // Jump or stop if necessary (about to fall):
                if (!Physics2D.Raycast(transform.position, ray, rayLength, whatIsGround))
                {
                    if (canJump)
                    {
                        // Check if there is something to jump to:
                        float jumpDistance = GetApproximateJumpDistance();
                        if (jumpDistance > 2) // Filter out false positives at very low speed
                        {
                            float jumpRayLength = Mathf.Sqrt(Mathf.Pow(jumpDistance, 2) + halfEnemyHeightSquared) * 1.01f;
                            Vector2 jumpToRay = new Vector2(preferredMovement < 0 ? -jumpDistance : jumpDistance, -halfEnemyHeight);
                            if (Physics2D.Raycast(transform.position, jumpToRay.normalized, jumpRayLength, whatIsGround))
                            { // Jump to the next platform...
                                jump = true;
                                currentlyLeaping = true; // Maintain jump state until landing
                                return preferredMovement;
                            }
                        }
                    }
                    // There is nothing to jump to (or canJump is disabled)...
                    currentlyLeaping = false;
                    timeSinceDirectionChange = 0;
                    return Random.value < 0.8f ? 0 : -preferredMovement;
                }
            }
        }
        if (canJump && controller.IsPlayerGrounded())
            currentlyLeaping = false;
        return preferredMovement;
    }

    private float GetApproximateJumpDistance()
    {
        Vector2 jumpVector = controller.GetJumpVector();
        float vx = Mathf.Abs(jumpVector.x);
        float vy = Mathf.Abs(jumpVector.y);
        float a = Mathf.Abs(gravity / (2 * Mathf.Pow(vx, 2))); // Abs for float inaccuracy
        float b = Mathf.Abs(Mathf.Tan(vy / Mathf.Sqrt(vx + vy))); // Abs for float inaccuracy
        return 0.8f * (b / a); // Using the quadratic formula (already simplified)
    } // Return 80% of the approximate distance for confidence
}