using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] private GameObject playerWeapon;
    [SerializeField] private GameObject player;
    [SerializeField] private MobileJoyStick joyStick;

    public GameObject[] weapons;

    public void OnShoot()
    {
        GameObject bullet = Instantiate(playerWeapon, player.transform.position, player.transform.rotation);
        bullet.transform.SetParent(gameObject.transform);
        Destroy(bullet, 1.0f);

        float bulletSpeed = 20.0f;
        float horizontalOffset = 0.1f;
        float verticalOffset = 0.1f;

        /*
        if (player.transform.localScale.x < 0)
        {
            horizontalOffset *= -1.0f;
            bullet.GetComponent<SpriteRenderer>().flipX = true;
            CircleCollider2D col = bullet.GetComponent<CircleCollider2D>();
            col.offset = new Vector2(col.offset.x * -1.0f, col.offset.y);
            bulletSpeed *= -1.0f;
        }
        */
        // Shoot at the angle 
        


        // Set starting position
        bullet.transform.position += new Vector3(horizontalOffset, verticalOffset, 0);

        // Scale properly
        /*
        Vector3 scaleChange = new Vector3(0.4397703f, 0.4397703f, 0.4397703f);
        bullet.transform.localScale += scaleChange;
        */
        // Move the bullet
        bullet.GetComponent<Rigidbody2D>().AddForce(joyStick.aimVector.normalized*bulletSpeed, ForceMode2D.Impulse);

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
