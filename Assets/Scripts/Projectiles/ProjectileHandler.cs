using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] private GameObject playerWeapon;
    [SerializeField] private GameObject player;
    [SerializeField] private MobileJoyStick joyStick;
    [SerializeField] private float bulletSpeed;

    [SerializeReference] private GameObject[] weapons;
    private GameObject projectileStorage;

    void Start()
    {
        bulletSpeed = 20.0f;
        projectileStorage = GameObject.Find("ProjectileStorage");
    }

    public void OnShoot()
    {
        GameObject bullet = Instantiate(playerWeapon, player.transform.position, player.transform.rotation);
        bullet.transform.SetParent(projectileStorage.transform);
        Destroy(bullet, 3.0f);
        bullet.GetComponent<Projectile>().SetIgnoreCollision(gameObject.GetComponent<Collider2D>(), true);

        float horizontalOffset = 0.1f;
        float verticalOffset = 0.1f;
        
        // Set starting position
        bullet.transform.position += new Vector3(horizontalOffset, verticalOffset, 0);

        // Rotate sprite
        float hori = joyStick.aimVector.x;
        float vert = joyStick.aimVector.y;
        float angle = 0.0f ;

        if (vert < 0.0f)
        {
            angle = (Mathf.Atan2(hori, Mathf.Abs(vert)) * Mathf.Rad2Deg) + 270.0f;
        }
        else
        {
            angle = 90.0f - (Mathf.Atan2(hori, vert) * Mathf.Rad2Deg);
        }

        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        // Scale properly
        /*
        Vector3 scaleChange = new Vector3(0.4397703f, 0.4397703f, 0.4397703f);
        bullet.transform.localScale += scaleChange;
        */

        // Move the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = joyStick.aimVector.normalized * bulletSpeed;

        // Use 2D collider
        CircleCollider2D collider = bullet.GetComponent<CircleCollider2D>();
        collider.enabled = true;
    }

    public void SwitchWeapon(string weapon)
    {
        foreach (GameObject item in weapons)
        {
            if (item.name == weapon)
            {
                playerWeapon = item;
            }
        }
    }
}
