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
    [SerializeReference] private GameObject currentLaserProjectile;
    [SerializeReference] private GameObject bulletStart;

    private ProjectilePool pool;

    void Start()
    {
        pool = projectileStorage.GetComponent<ProjectilePool>();
    }

    public void OnShoot()
    {
        if (playerWeapon != null && currentLaserProjectile == null)
        {
            CreateBullet();
        }
        else if (playerWeapon == null)
        {
            AlertGemNotSelected();
        }
    }

    private void CreateBullet()
    {
        pool.Shoot(playerWeapon, bulletStart.transform, joyStick.aimVector);
    }

    private void AlertGemNotSelected()
    {
        Debug.Log("Weapon not selected");
        infoScript.Alert("You need to select a gem first!");
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

    /*
    void FixedUpdate()
    {
        if (currentLaserProjectile)
        {
            if (joyStick.isShooting)
            {
                currentLaserProjectile.GetComponent<LaserProjectile>().UpdateLaser(bulletStart.transform.position, joyStick.aimVector);
            }
            else
            {
                Destroy(currentLaserProjectile);
                currentLaserProjectile = null;
            }
        }
    }*/

    public GameObject GetWeapon()
    {
        return playerWeapon;
    }
}
