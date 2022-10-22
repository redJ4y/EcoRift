using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    private Vector2 aimVector; 
    private LineRenderer line;
    private Ray2D ray;
    private List<GameObject> hitEnemies;
    [SerializeField] float bulletForce;
    [SerializeField] float maxLength;
    [SerializeField] int lineLength;
    [SerializeField] float damage;
    [SerializeField] float maxChainDistance;

    // Start is called before the first frame update
    public void Awake()
    {
        hitEnemies = new List<GameObject>();
        line = GetComponent<LineRenderer>();
        line.SetWidth(.07f, .07f);
    }

    public void UpdateLaser(Vector3 startPos, Vector2 aimVector)
    {
        ray = new Ray2D(startPos, aimVector);
        RaycastHit2D hit = Physics2D.Raycast(startPos, aimVector, maxLength, LayerMask.GetMask("Enemy", "Ground"));
        line.SetPosition(0, startPos);

        if (hit.collider != null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            SetAllPoints(1, hit.point);
            if (hit.transform.gameObject.layer == 10)
            {
                hit.transform.Find("HealthBar").GetComponent<Health>().TakeDamage(damage);
                hitEnemies.Add(hit.transform.gameObject);
                Transform nextEnemy = GetClosestEnemy(hit.point, enemies);

                if (nextEnemy)
                {
                    nextEnemy.Find("HealthBar").GetComponent<Health>().TakeDamage(damage);
                    hitEnemies.Add(nextEnemy.gameObject);
                    SetAllPoints(2, nextEnemy.position);
                    Transform nextEnemy2 = GetClosestEnemy(nextEnemy.position,enemies);

                    if (nextEnemy2)
                    {
                        nextEnemy2.Find("HealthBar").GetComponent<Health>().TakeDamage(damage);
                        hitEnemies.Add(nextEnemy2.gameObject);
                        SetAllPoints(3, nextEnemy2.position);
                        Transform nextEnemy3 = GetClosestEnemy(nextEnemy2.position, enemies);

                        if (nextEnemy3)
                        {
                            nextEnemy2.Find("HealthBar").GetComponent<Health>().TakeDamage(damage);
                            hitEnemies.Add(nextEnemy3.gameObject);
                            SetAllPoints(4, nextEnemy3.position);
                        }
                    }
                }
            }
        }
        else
        {
            SetAllPoints(1, ray.GetPoint(maxLength));
        }
    }

    private void SetAllPoints(int startIndex, Vector3 newPos)
    {
        for (int i = startIndex; i < lineLength; i++)
        {
            line.SetPosition(i, newPos);
        }
    }

    private Transform GetClosestEnemy(Vector3 pos, GameObject[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = maxChainDistance;

        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - pos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr) 
            {
                bool sameAsHitEnemy = false;
                foreach (GameObject hitEnemy in hitEnemies)
                {
                    if (hitEnemy == potentialTarget)
                        sameAsHitEnemy = true;
                }

                if (!sameAsHitEnemy)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }
        }

        hitEnemies.Clear();

        return bestTarget;
    }
}
