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

    [Header("Behavior")]
    [Tooltip("The distance at which the enemy will begin tracking the player")]
    [Range(1, 50)] [SerializeField] private int aggroDistance = 6;
    [Tooltip("Whether to keep aggro after the player moves out of Aggro Distance")]
    [SerializeField] private bool keepAggro = false;
    [Tooltip("The prefered minimum (closest) distance from the player")]
    [Range(5, 20)] [SerializeField] private int keepDistance = 5;
    [Tooltip("The distance the enemy may travel from its origin while not aggroed")]
    [Range(0, 50)] [SerializeField] private int patrolDistance = 5;

    private Vector2 currentMovement = Vector2.zero;
    private float timeSinceDirectionChange = 0;

    // Start is called before the first frame update
    void Start()
    {

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
        float distance = Mathf.Sqrt(Mathf.Pow(myPos.y - playerPos.y, 2) + Mathf.Pow(myPos.x - playerPos.x, 2));

        return Vector2.zero;
    }

    // Returns a less annoying movement value (now scaled by movement speed)
    private Vector2 SmoothMovement(Vector2 preferredMovement)
    {
        return preferredMovement;
    }

    private Vector2 ValidateMovement(Vector2 preferredMovement)
    {
        return preferredMovement;
    }
}
