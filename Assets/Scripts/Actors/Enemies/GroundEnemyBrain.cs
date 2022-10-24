using UnityEngine;

/* 
 * Ground Enemy Brain
 * This script is responsible for generating ground enemy movement instructions.
 * It handles shooting and player targeting.
 * Author: Jared
 */
[RequireComponent(typeof(CharacterController2D))]
public class GroundEnemyBrain : MonoBehaviour
{
    [SerializeReference] private CharacterController2D controller;
    [SerializeReference] private GameObject shootStartPosition;
    [SerializeField] private GameObject enemyTargetedWeapon; // regular projectile shooting at player
    [SerializeField] private GameObject enemySpecialWeapon; // special projectile shooting less frequently at player
    [SerializeField] private GameObject enemyDirectionalWeapon; // projectile aiming in specific directions

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private string enemyType;

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
    [Tooltip("The health at which the enemy retreats")]
    [Range(0.0f, 99.9f)] [SerializeField] private float retreatHealth = 0;

    [Header("Combat")]
    [Range(1, 50)] [SerializeField] private int attackRange = 10;
    [Range(1, 200)] [SerializeField] private int attackSpeed = 1;
    [Range(1, 50)] [SerializeField] private float projectileSpeed = 5.0f;

    [Header("Projectile Specifications")]
    [SerializeField] private bool shootUp;
    [SerializeField] private bool shootSideways;
    [SerializeField] private bool shootDiagonally;

    private GameObject player;
    private Health healthScript;
    private GameObject projectileStorage;
    private Vector2 toPlayer;
    private float currentMovement = 0;
    private bool patrolling = false;
    private float patrolPostX = 0; // The x position to patrol around
    private bool jump = false;
    private bool permanentAggro = false; // If enemy was aggroed and keepAgro is enabled
    private float timeSinceDirectionChange = 0;
    private bool currentlyLeaping = false;
    private int shotDelay = 0;
    private int directionalShotDelay = 0;
    private int specialShotDelay = 0;
    private bool isBuffed;
    private float gravity;
    private ProjectilePool projectilePool;

    // Save common operations for performance:
    private float halfEnemyHeight;
    private float halfEnemyHeightSquared;
    private float halfMovementSpeed;

    // For ray casting:
    private float rayLength;
    private Vector2 rightRayNormalized;
    private Vector2 leftRayNormalized;

    // Start is called before the first frame update
    void Start()
    {
        if (!shootStartPosition)
            shootStartPosition = gameObject;
        player = GameObject.FindWithTag("Player");
        projectileStorage = GameObject.Find("ProjectileStorage");
        projectilePool = projectileStorage.GetComponent<ProjectilePool>();
        healthScript = transform.Find("HealthBar").GetComponent<Health>();
        gravity = gameObject.GetComponent<Rigidbody2D>().gravityScale * 9.8f;

        float enemyHeight = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        halfEnemyHeight = enemyHeight / 2.0f;
        halfEnemyHeightSquared = Mathf.Pow(halfEnemyHeight, 2);
        halfMovementSpeed = movementSpeed / 2.0f;

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
        TryShoot();

        controller.Move(currentMovement * Time.fixedDeltaTime, jump);
        timeSinceDirectionChange += Time.fixedDeltaTime;
        jump = false;
    }

    private void TryShoot()
    {
        Vector3 myPos = gameObject.transform.position;
        Vector3 playerPos = player.transform.position;
        toPlayer = playerPos - myPos;
        // Only shoot when the shotDelay has exceeded the attackSpeed (inverted)
        if (enemyTargetedWeapon != null && (200 - attackSpeed) - shotDelay < 0 && Random.value > 0.9f)
        {
            if (toPlayer.magnitude < attackRange && Mathf.Abs(playerPos.y - myPos.y) < 2)
            { // Player is within range at a similar y-value...
                projectilePool.Shoot(enemyTargetedWeapon, transform, toPlayer, projectileSpeed);
                // Make enemy face in the direction of shot:
                if (myPos.x < playerPos.x)
                {
                    currentMovement = 1;
                }
                else
                {
                    currentMovement = -1;
                }
                timeSinceDirectionChange = 0; // Count shooting as a direction change
            }
            shotDelay = 0;
        }
        else
        {
            shotDelay++;
        }

        if (enemyDirectionalWeapon != null && (shootUp || shootDiagonally || shootSideways))
        {
            directionalShotDelay++;
            if ((250 - (attackSpeed * 2)) - directionalShotDelay < 0 && Random.value > 0.9f) // try shoot directional
            {
                if (shootUp)
                {
                    projectilePool.Shoot(enemyDirectionalWeapon, shootStartPosition.transform, Vector2.up, projectileSpeed * 2); // sorry for not using Shoot() jared
                }
                if (shootDiagonally)
                {
                    projectilePool.Shoot(enemyDirectionalWeapon, shootStartPosition.transform, new Vector2(-.5f, .5f), projectileSpeed * 2);
                    projectilePool.Shoot(enemyDirectionalWeapon, shootStartPosition.transform, new Vector2(.5f, .5f), projectileSpeed * 2);
                }
                if (shootSideways)
                {
                    projectilePool.Shoot(enemyDirectionalWeapon, shootStartPosition.transform, Vector2.left, projectileSpeed * 2);
                    projectilePool.Shoot(enemyDirectionalWeapon, shootStartPosition.transform, Vector2.right, projectileSpeed * 2);
                }
                directionalShotDelay = 0;
            }
        }

        if (enemySpecialWeapon != null)
        {
            specialShotDelay++;
            if ((250 - attackSpeed) - specialShotDelay < 0 && Random.value > 0.9f) // try shoot special
            {
                if (enemySpecialWeapon.tag == "Tornado")
                    projectilePool.Shoot(enemySpecialWeapon, shootStartPosition.transform, toPlayer.normalized / 2f, 0f); // sorry for not using Shoot() jared
                else
                    projectilePool.Shoot(enemySpecialWeapon, shootStartPosition.transform, toPlayer, projectileSpeed);

                specialShotDelay = 0;
            }
        }
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

    // Returns the preferred movement value (not scaled by movement speed)
    private float GetPreferredMovement()
    {
        if (currentlyLeaping) // Don't stop forward movement when leaping
            return System.Math.Sign(currentMovement);

        Vector2 myPos = gameObject.transform.position;
        Vector2 playerPos = player.transform.position;
        float distance = Mathf.Sqrt(Mathf.Pow(myPos.y - playerPos.y, 2) + Mathf.Pow(myPos.x - playerPos.x, 2));

        float preferredMovement;
        if (permanentAggro || distance < aggroDistance)
        { // The enemy is aggroed...
            patrolling = false;
            if (keepAggro)
                permanentAggro = true; // Lock in aggro if keepAggro is enabled
                                       // Determine movement direction (towards player):
            if (myPos.x < playerPos.x)
            {
                preferredMovement = 1;
            }
            else
            {
                preferredMovement = -1;
            }
            // Invert movement (away from player) if necessary:
            if (healthScript.GetHp() <= retreatHealth && distance < aggroDistance) // Retreat
            { // Either move away or stop moving towards player...
                preferredMovement *= -1;
                if (Random.value < 0.2f)
                    preferredMovement = 0;
            }
            else if (distance < keepDistance && Random.value < 0.6f) // Ensure keepDistance is loosely obeyed
            {
                preferredMovement *= -1;
            }
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
        float movementSpeedDebuffed = movementSpeed * controller.GetMovementDebuff(); // Subtracts slowed movement speed from controller
        int currentMovementRaw = System.Math.Sign(currentMovement);

        if (currentlyLeaping) // Don't stop forward movement when leaping
            return currentMovementRaw * movementSpeedDebuffed;

        if (currentMovementRaw != preferredMovement) // Check for direction change
        { // Apply smoothing to direction change...
            if (timeSinceDirectionChange > 0.5f && Random.value < timeSinceDirectionChange / 10.0f) // Do not always switch directions immediately (increase chance as time passes)
            {
                timeSinceDirectionChange = 0; // Reset duration since change (increased in every fixed update)
                return preferredMovement * movementSpeedDebuffed; // Accept preferredMovement
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
        return currentMovementRaw * movementSpeedDebuffed;
    }

    private float ValidateMovement(float preferredMovement)
    {
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
                        if (jumpDistance > 1) // Filter out false positives at very low speed
                        {
                            float jumpRayLength = Mathf.Sqrt(Mathf.Pow(jumpDistance, 2) + halfEnemyHeightSquared) * 1.02f;
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
                    return -preferredMovement;
                }
            }
        }
        if (currentlyLeaping)
        {
            controller.DoGroundCheck();
            if (controller.IsPlayerGrounded())
                currentlyLeaping = false;
        }
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
