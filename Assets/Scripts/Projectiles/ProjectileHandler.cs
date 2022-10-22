using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] private GameObject playerWeapon;
    [SerializeField] private GameObject player;
    [SerializeField] private AimingJoyStick joyStick;
    [SerializeField] private Vector2 defaultAimVector;

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
        if (playerWeapon && !currentLaserProjectile)
        {
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
        Vector2 newVector = joyStick.aimVector;

        if (newVector.magnitude == 0f)
            newVector = defaultAimVector;
        pool.Shoot(playerWeapon, bulletStart.transform, newVector);
    }

    public void AlertGemNotSelected()
    {
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

    public GameObject GetWeapon()
    {
        return playerWeapon;
    }
}
