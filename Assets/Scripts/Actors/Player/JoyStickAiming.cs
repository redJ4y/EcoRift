using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickAiming : MonoBehaviour
{

    public MobileJoyStick joyStick;
    public Transform twistPoint;
    public float returnTime = 0.4f;

    private void Awake()
    {
        joyStick = GameObject.FindObjectOfType<MobileJoyStick>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
    }

    public void Aim()
    {
        Vector3 angle = twistPoint.transform.localEulerAngles;

        float Horizontal = joyStick.aimVector.x;
        float Vertical = joyStick.aimVector.y;

        if(Horizontal < 0)
        {
            twistPoint.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Atan2(Horizontal, Vertical) * 180 / Mathf.PI);
        }
        else
        {
            twistPoint.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Atan2(Horizontal, Vertical) * -180 / Mathf.PI);
        }
        
       
       /* if(angle.z > 90 && angle.z < 200)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 89));
        }
        if (angle.z < 270 && angle.z < 200)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 271));
        }*/

    }

}
