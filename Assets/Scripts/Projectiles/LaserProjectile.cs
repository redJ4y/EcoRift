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
    public void StartLaser()
    {
        line = GetComponent<LineRenderer>();
        line.SetWidth(.3f, .3f);
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
                hit.rigidbody.AddForceAtPosition(aimVector * bulletForce, hit.point);
            }
        }
        else
        {
            line.SetPosition(1, ray.GetPoint(maxLength));
        }
    }
}
