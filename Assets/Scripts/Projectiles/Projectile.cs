using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Projectile : MonoBehaviour
{
    Camera cam;

    [Header("Base Projectile Fields")]
    public int projectileID;
    [SerializeField] private string element;
    [Range(1, 3)] [SerializeField] public int tier;
    [SerializeField] private float damage;
    [SerializeField] private bool projectileDisabled;
    [SerializeField] public bool isBuffed;
    [SerializeField] public bool isRotatable = true;
    [Range(1f, 30f)] [SerializeField] public float bulletSpeed;
    [Range(0f, 15f)] [SerializeField] public float lifeSpan;

    [Header("Tornado Settings")]
    [SerializeField] private float thrust;
    private Vector2 velocityTranslate;

    [Header("Homing Settings")]
    [SerializeField] private float rotationSpeed = 1000;
    [SerializeField] private float focusDistance = 5;
    private bool isLookingAtObject = true;
    private Transform homingTarget;
    private Vector3 targetDirection;

    [Header("Ice Settings")]
    [SerializeReference] GameObject freezeObject; 

    private Collider2D thisCollider;
    private ProjectilePool pool;
    private IEnumerator coroutine;
    private bool damageEnemies = false;

    void Awake()
    {
        thisCollider = gameObject.GetComponent<Collider2D>();
        cam = Camera.main;

        if (cam == null)
        {
            Debug.Log("Camera is null");
        }
    }

    void FixedUpdate()
    {
        if (gameObject.tag == "Tornado")
        {
            transform.Translate(velocityTranslate * Time.fixedDeltaTime*bulletSpeed);
        }

        if (gameObject.tag == "Homing")
        {
            if (homingTarget)
            {
                targetDirection = homingTarget.position - transform.position;
                targetDirection = targetDirection.normalized;

                if (Vector3.Distance(transform.position, homingTarget.position) < focusDistance)
                {
                    isLookingAtObject = false;
                }

                if (isLookingAtObject)
                {
                    transform.rotation = Quaternion.LookRotation(transform.forward, targetDirection);
                }
            }
            transform.position += targetDirection * bulletSpeed * Time.fixedDeltaTime;
        }
    }

    private Transform GetClosestEnemy(GameObject[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }

    public void SetLooking(bool isLooking)
    {
        homingTarget = GetClosestEnemy(GameObject.FindGameObjectsWithTag("Enemy"));
        isLookingAtObject = isLooking;
    }

    public void SetTranslateVelocity(Vector2 velocity) // ONLY for tornado and homing currently
    {
        velocityTranslate = velocity;
        targetDirection = velocityTranslate.normalized;
    }

    public void SetPool(ProjectilePool pool)
    {
        this.pool = pool;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!projectileDisabled && gameObject.tag != "Tornado")
        {
            if (col.gameObject.layer == 8) // If collides with ground
            {
                pool.DestroyBullet(gameObject);
            }

            Vector2 endPos = col.contacts[0].point + new Vector2(1.5f, 0);

            //Vector3 endPos = thisCollider.bounds.center + new Vector3(gameObject.transform.position.x,0,0);

            GameObject target = col.gameObject;

            if ((target.layer == 10 || target.layer == 9)) // Check if collider is enemy or player
            {
                if (isBuffed)
                    damage *= 1.2f;
                target.transform.Find("HealthBar").GetComponent<Health>().TakeDamage(damage);
                CharacterController2D targetController = target.GetComponent<CharacterController2D>();
                FlyingCharacterController2D flyingTargetController = target.GetComponent<FlyingCharacterController2D>();

                if (target.layer == 9) // if hit player then initiate hit effects
                {
                    targetController.HitInflicted();
                }
                else
                {
                    if (targetController)
                        InitiateEnemyHitEffect(targetController);
                    if (flyingTargetController)
                        InitiateEnemyHitEffect(flyingTargetController);
                }
                pool.DestroyBullet(gameObject);
            }
        }
        else if (gameObject.tag == "Tornado")
        {
            GameObject target = col.gameObject;
            if (target.layer == 10) // if target is enemy
            {
                target.GetComponent<Rigidbody2D>().AddForce(target.transform.up*thrust, ForceMode2D.Impulse);
            }
        }
    }
    // TODO: 
    // Collapse both functions below into one
    // (they needed to be separate bc of different object types, "CharacterController2D" and "FlyingCharacterController2D")
    private void InitiateEnemyHitEffect(CharacterController2D targetController)
    {
        switch(element)
        {
            case "Snow":
                if (tier == 2)
                    targetController.SlowInflicted();
                if (tier == 3)
                    targetController.FreezeInflicted(freezeObject);
                break;
        }
    }

    private void InitiateEnemyHitEffect(FlyingCharacterController2D targetController)
    {
        switch (element)
        {
            case "Snow":
                if (tier == 2)
                    targetController.SlowInflicted();
                if (tier == 3)
                    targetController.FreezeInflicted(freezeObject);
                break;
        }
    }

    private IEnumerator MoveSmoothly(TMP_Text tmp, Transform tra, Vector3 from, Vector3 to)
    {
        var t = 0f;

        while (t < 1.5f)
        {
            t += Time.deltaTime;
            tmp.fontMaterial.SetColor("_FaceColor", Color.Lerp(Color.white, Color.clear, t));
            tra.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }
}
