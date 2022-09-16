using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStaff : MonoBehaviour
{
    [SerializeReference]
    private Sprite[] staves;
    private SpriteRenderer renderer;
    private SpriteRenderer playerRenderer;

    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        playerRenderer = transform.parent.gameObject.GetComponent<SpriteRenderer>();
    }

    public void changeStaff(string element)
    {
        foreach (Sprite staff in staves)
        {
            if (staff.name == element) 
            {
                renderer.sprite = staff;
            }
        }
    }

    public void FlipStaff() // Flip staff when player flips
    {
        if (playerRenderer.flipX && !renderer.flipX)
        {
            Vector3 newPos = transform.localPosition;
            Quaternion newRot = transform.rotation;
            newPos.x *= -1.0f;
            newRot.z *= -1.0f;

            renderer.flipX = playerRenderer.flipX;
            transform.localPosition = newPos;
            transform.rotation = newRot;
        }
        else if (!playerRenderer.flipX && renderer.flipX)
        {
            Vector3 newPos = transform.localPosition;
            Quaternion newRot = transform.rotation;
            newPos.x *= -1.0f;
            newRot.z *= -1.0f;

            renderer.flipX = playerRenderer.flipX;
            transform.localPosition = newPos;
            transform.rotation = newRot;
        }
        
    }
}
