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

        if (weapon.tag == "MiniSun") // shoots in four directions
        {
            Shoot(weapon, bulletStart, Vector2.left, weapon.GetComponent<Projectile>().bulletSpeed);
            Shoot(weapon, bulletStart, new Vector2(-.5f,.5f), weapon.GetComponent<Projectile>().bulletSpeed);
            Shoot(weapon, bulletStart, Vector2.up, weapon.GetComponent<Projectile>().bulletSpeed);
            Shoot(weapon, bulletStart, new Vector2(.5f, .5f), weapon.GetComponent<Projectile>().bulletSpeed);
            Shoot(weapon, bulletStart, Vector2.right, weapon.GetComponent<Projectile>().bulletSpeed);
        }
        else
        {
            Shoot(weapon, bulletStart, aimVector, weapon.GetComponent<Projectile>().bulletSpeed);
        }
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
        if (newBullet.tag == "Laser")
        {
            newBullet.GetComponent<LaserProjectile>().StartLaser();
            //currentLaserProjectile = newBullet;
        }
        else if (newBullet.tag == "Tornado")
        {
            newBullet.GetComponent<Projectile>().SetTranslateVelocity(aimVector);
        }
        else if (newBullet.tag == "Homing")
        {
            newBullet.GetComponent<Projectile>().SetLooking(true);
            newBullet.GetComponent<Projectile>().SetTranslateVelocity(aimVector);
        }
        else
        {
            newBullet.GetComponent<Rigidbody2D>().velocity = aimVector.normalized * bulletSpeed;
        }
    }

    IEnumerator waitThenDestroy(float lifeSpan, GameObject bullet)
    {
        yield return new WaitForSeconds(lifeSpan);
        DestroyBullet(bullet);
    }

    public void DestroyBullet(GameObject bullet)
    {
        if (bullet.tag != "Tornado" && bullet.tag != "Homing")
            bullet.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        else
            bullet.GetComponent<Projectile>().SetTranslateVelocity(Vector2.zero);

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
