using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public Dictionary<int, Stack<GameObject>> poolMap;
    public Dictionary<GameObject, Coroutine> destroyCoroutines;

    void Awake()
    {
        poolMap = new Dictionary<int, Stack<GameObject>>();
        destroyCoroutines = new Dictionary<GameObject, Coroutine>();
    }

    public void Shoot(GameObject weapon, Transform bulletStart, Vector2 aimVector)
    {
        Shoot(weapon, bulletStart, aimVector, weapon.GetComponent<Projectile>().bulletSpeed);
    }

    public void Shoot(GameObject weapon, Transform bulletStart, Vector2 aimVector, float bulletSpeed)
    {
        Stack<GameObject> currentPool;
        try
        {
            currentPool = poolMap[weapon.GetComponent<Projectile>().projectileID];
        }
        catch (KeyNotFoundException)
        {
            currentPool = new Stack<GameObject>();
            poolMap.Add(weapon.GetComponent<Projectile>().projectileID, currentPool);
        }

        GameObject newBullet;
        if (currentPool.Count == 0)
        {
            newBullet = Instantiate(weapon, bulletStart.position, Quaternion.identity);
            newBullet.transform.SetParent(transform);
            newBullet.GetComponent<Projectile>().SetPool(this);
        }
        else
        {
            newBullet = currentPool.Pop();
            newBullet.SetActive(true);
            newBullet.transform.position = bulletStart.position;
        }

        Projectile projectileScript = newBullet.GetComponent<Projectile>();

        // Ensure bullet is destroyed after its set lifespan in seconds
        Coroutine newCoroutine = StartCoroutine(waitThenDestroy(projectileScript.lifeSpan, newBullet));
        try
        {
            destroyCoroutines[newBullet] = newCoroutine;
        }
        catch (KeyNotFoundException)
        {
            destroyCoroutines.Add(newBullet, newCoroutine);
        }

        // Rotate sprite
        if (projectileScript.isRotatable)
        {
            newBullet.transform.rotation = Quaternion.AngleAxis(GetAimAngle(aimVector), Vector3.forward);
        }

        // Move the bullet
        if (newBullet.tag != "Laser")
            newBullet.GetComponent<Rigidbody2D>().velocity = aimVector.normalized * bulletSpeed;
        else
        {
            newBullet.GetComponent<LaserProjectile>().StartLaser();
            //currentLaserProjectile = newBullet;
        }
    }

    IEnumerator waitThenDestroy(float lifeSpan, GameObject bullet)
    {
        yield return new WaitForSeconds(lifeSpan);
        DestroyBullet(bullet);
    }

    public void DestroyBullet(GameObject bullet)
    {
        bullet.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        bullet.SetActive(false);
        StopCoroutine(destroyCoroutines[bullet]);
        poolMap[bullet.GetComponent<Projectile>().projectileID].Push(bullet);
    }

    private float GetAimAngle(Vector2 aimVector)
    {
        float hori = aimVector.x;
        float vert = aimVector.y;
        float angle = 0.0f;

        if (vert < 0.0f)
        {
            angle = (Mathf.Atan2(hori, Mathf.Abs(vert)) * Mathf.Rad2Deg) + 270.0f;
        }
        else
        {
            angle = 90.0f - (Mathf.Atan2(hori, vert) * Mathf.Rad2Deg);
        }

        return angle;
    }
}
