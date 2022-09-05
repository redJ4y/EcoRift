using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    [SerializeReference] ProjectileHandler handler;

    private void Update()
    {
        HandleShooting();
    }
    /*
    private Vector3 worldPosition;
    private float angle;

    private void HandleAiming()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        //worldPosition.y = aimTransform.position.y;
        //aimGunEndpointPosition.position.y = aimTransform.position.y;
        Vector3 aimDirection = (worldPosition - transform.position).normalized;
        angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }*/

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            handler.OnShoot();
        }
    }

}
