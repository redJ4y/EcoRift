using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] private GameObject playerWeapon;
    [SerializeField] private GameObject player;
    [SerializeField] private AimingJoyStick joyStick;

    [SerializeReference] private InfoScript infoScript;
    [SerializeReference] private GameObject[] weapons;
    [SerializeReference] private GameObject projectileStorage;

    public void OnShoot()
    {
        if (playerWeapon != null)
        {
            CreateBullet();
        }
        else
        {
            AlertGemNotSelected();
        }
    }

    private void CreateBullet()
    {
        GameObject bullet = Instantiate(playerWeapon, player.transform.position, player.transform.rotation);
        bullet.transform.SetParent(projectileStorage.transform);
        Projectile projectileScript = bullet.GetComponent<Projectile>();

        // Set starting position
        float horizontalOffset = 0.1f;
        float verticalOffset = 0.1f;
        bullet.transform.position += new Vector3(horizontalOffset, verticalOffset, 0);

        // Ensure bullet is destroyed after its set lifespan in seconds
        Destroy(bullet, projectileScript.lifeSpan);

        // Rotate sprite
        if (projectileScript.isRotatable)
        {
            float angle = GetAimAngle();
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Move the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = joyStick.aimVector.normalized * projectileScript.bulletSpeed;
    }

    private void AlertGemNotSelected()
    {
        Debug.Log("Weapon not selected");
        infoScript.Alert("You need to select a gem first!");
    }

    private float GetAimAngle()
    {
        float hori = joyStick.aimVector.x;
        float vert = joyStick.aimVector.y;
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

    public void SwitchWeapon(string weapon) // for buttons
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
