using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    private Vector2 aimVector; 
    private LineRenderer line;
    private Ray2D ray;
    [SerializeField] float bulletForce;
    [SerializeField] float maxLength;

    // Start is called before the first frame update
    public void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.SetWidth(.1f, .1f);
    }

    public void UpdateLaser(Vector3 startPos, Vector2 aimVector)
    {
        ray = new Ray2D(startPos, aimVector);
        RaycastHit2D hit = Physics2D.Raycast(startPos, aimVector, maxLength, LayerMask.GetMask("Enemy", "Ground"));
        line.SetPosition(0, startPos);

        if (hit.collider != null)
        {
            line.SetPosition(1, hit.point);
            if (hit.transform.gameObject.layer == 10)
            {
                //hit.rigidbody.AddForceAtPosition(aimVector * bulletForce, hit.point); Not needed for lightning
                Transform nextEnemy = GetClosestEnemy(hit.point, GameObject.FindGameObjectsWithTag("Enemy"));
                if (nextEnemy)
                {
                    line.SetPosition(2, nextEnemy.position);
                }
            }
        }
        else
        {
            line.SetPosition(1, ray.GetPoint(maxLength));
        }
    }

    private Transform GetClosestEnemy(Vector3 pos, GameObject[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - pos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }
}
