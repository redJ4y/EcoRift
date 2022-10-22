using UnityEngine;

/* 
 * Flying Enemy Brain
 * This script is responsible for generating flying enemy movement instructions.
 * It handles shooting and player targeting.
 * Author: Jared
 */
[RequireComponent(typeof(FlyingCharacterController2D))]
public class FlyingEnemyBrain : MonoBehaviour
{
    [SerializeReference] private FlyingCharacterController2D controller;
    [SerializeField] private GameObject enemyWeapon;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private string enemyType;

    [Header("Movement")]
    [Range(1.0f, 100.0f)] [SerializeField] private float movementSpeed = 100f;
    [Range(1, 10)] [SerializeField] private int targetHeight = 5;
    [Range(1, 5)] [SerializeField] private int minimumHeight = 2;
    [Tooltip("This will need to be adjusted on a per-map basis")]
    [SerializeField] private int minimumYLevel = -5;

    [Header("Behavior")]
    [Tooltip("The distance at which the enemy will begin tracking the player")]
    [Range(10, 50)] [SerializeField] private int aggroDistance = 6;
    [Tooltip("Whether to keep aggro after the player moves out of Aggro Distance")]
    [SerializeField] private bool keepAggro = false;
    [Tooltip("The distance the enemy may travel from its origin while not aggroed")]
    [Range(0, 50)] [SerializeField] private int patrolDistance = 5;

    [Header("Combat")]
    [Range(1, 50)] [SerializeField] private int attackRange = 10;
    [Range(1, 200)] [SerializeField] private int attackSpeed = 1;
    [Range(1, 50)] [SerializeField] private float projectileSpeed = 5.0f;
    [Tooltip("Best explanation: 0 = enemy can only shoot straight down, 1 = enemy may shoot at any (downwards) angle")]
    [Range(0.4f, 0.9f)] [SerializeField] private float shootRadius = 0.5f;
    [Tooltip("Whether or not the enemy can predict player movement")]
    [SerializeField] private bool canLeadShots = false;

    private GameObject player;
    private CharacterController2D playerController;
    private GameObject projectileStorage;
    private Health healthScript;
    private Vector2 toPlayer;
    private Vector2 currentMovement = Vector2.zero;
    private bool patrolling = false;
    private float patrolPostX = 0; // The x position to patrol around
    private bool permanentAggro = false; // If enemy was aggroed and keepAgro is enabled
    private float timeSinceDirectionChange = 0;
    private int shotDelay = 0;
    private bool isBuffed;
    private readonly float shootingDeadzone = 2; // The deadzone if canLeadShots is enabled (avoid inaccurate leading)
	private ProjectilePool projectilePool;

    // For ray casting:
    private Vector2 downRightRay;
    private Vector2 downLeftRay;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<CharacterController2D>();
        projectileStorage = GameObject.Find("ProjectileStorage");
        healthScript = transform.Find("HealthBar").GetComponent<Health>();
		projectilePool = projectileStorage.GetComponent<ProjectilePool>();

        movementSpeed *= 2; // Adjust movement speed to account for increased smoothing
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
        TryShoot();

        controller.Move(currentMovement * Time.fixedDeltaTime);
        timeSinceDirectionChange += Time.fixedDeltaTime;
    }

    private void TryShoot()
    { // Only shoot when the shotDelay has exceeded the attackSpeed (inverted)
        if ((200 - attackSpeed) - shotDelay < 0 && Random.value > 0.9f)
        {
            if (toPlayer.magnitude < attackRange && toPlayer.normalized.y < shootRadius - 1)
            { // Player is within range and below the enemy (according to shootRadius)...
                if (canLeadShots)
                {
                    if (Mathf.Abs(transform.position.x - player.transform.position.x) > shootingDeadzone)
                    {
                        Shoot(); // Only shoot when the player is shootingDeadzone units away to avoid inaccurate leading
                        shotDelay = 0;
                    }
                    else
                    {
                        shotDelay++;
                    }
                }
                else
                {
                    Shoot();
                    shotDelay = 0;
                }
            }
        }
        else
        {
            shotDelay++;
        }
    }

    private void Shoot()
    {
        Vector2 aimVector = toPlayer;
        if (canLeadShots)
            aimVector = PredictTrajectory(player.transform.position, playerController.GetMovementVector(), transform.position);

        projectilePool.Shoot(enemyWeapon, transform, aimVector, projectileSpeed);
    }

    public void UpdateBuff(string weatherType)
    {
        if (weatherType == enemyType)
        {
            isBuffed = true;
            // Apply effects:
            healthScript.buffHp(1.2f);
            attackSpeed++;
            attackRange++;
            aggroDistance++;
        }
    }

    private Vector2 PredictTrajectory(Vector2 playerPosition, Vector2 playerVelocity, Vector2 projectileLaunchPos)
    {
        Vector2 targetDifference = playerPosition - projectileLaunchPos;
        targetDifference.y = 0;
        playerVelocity.y = 0;

        // Set up an approximated quadratic formula:
        float a = Vector2.Dot(playerVelocity, playerVelocity) - Mathf.Pow(projectileSpeed, 2);
        float b = 2 * Vector2.Dot(targetDifference, playerVelocity);
        float c = Vector2.Dot(targetDifference, targetDifference);

        float determinant = Mathf.Sqrt(Mathf.Pow(b, 2) - 4 * a * c);
        if (determinant > 0)
        {   // Calculate intercepts:
            float t1 = (-b + determinant) / (2 * a);
            float t2 = (-b - determinant) / (2 * a);

            Vector2 futurePosition = playerPosition + playerVelocity * Mathf.Max(t1, t2);
            Vector2 toFuturePosition = futurePosition - (Vector2)transform.position;
            return toFuturePosition;
        }
        return Vector2.zero;
    }

    // Returns the preferred movement value (not scaled by movement speed)
    private Vector2 GetPreferredMovement()
    {
        Vector2 myPos = gameObject.transform.position;
        Vector2 playerPos = player.transform.position;
        if (canLeadShots) // Try to stay slightly out of aiming dead zone...
            playerPos.x += (shootingDeadzone + 0.1f) * Mathf.Sign(myPos.x - playerPos.x);
        toPlayer = playerPos - myPos;

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
            return preferredMovement * (movementSpeed*controller.GetMovementDebuff()); // Accept preferredMovement
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
            return correctedMovement.normalized * (movementSpeed*controller.GetMovementDebuff());
        }
        if (transform.position.y < minimumYLevel)
        { // Under minimum y level, move up forcefully...
            Vector2 correctedMovement = preferredMovement.normalized;
            correctedMovement.y += 0.5f;
            return correctedMovement.normalized * movementSpeed;
        }
        return preferredMovement;
    }
}
