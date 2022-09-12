using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStaff : MonoBehaviour
{
    [SerializeReference]
    private Sprite[] staves;
    private SpriteRenderer renderer;

    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
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
}
