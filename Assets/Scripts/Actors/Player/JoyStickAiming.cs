using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickAiming : MonoBehaviour
{
    public MobileJoyStick joyStick;
    public Transform twistPoint;

    private void Awake()
    {
        joyStick = GameObject.FindObjectOfType<MobileJoyStick>();
    }

    void Update()
    {
        Aim();
    }

    public void Aim()
    {
        float Horizontal = joyStick.aimVector.x;
        float Vertical = joyStick.aimVector.y;

        if (Horizontal < 0)
        {
            twistPoint.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Atan2(Horizontal, Vertical) * 180 / Mathf.PI);
            Debug.Log(Mathf.Atan2(Horizontal, Vertical) * 180 / Mathf.PI);
        }
        else
        {
            twistPoint.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Atan2(Horizontal, Vertical) * -180 / Mathf.PI);
            Debug.Log(Mathf.Atan2(Horizontal, Vertical) * 180 / Mathf.PI);
        }
    }
}
