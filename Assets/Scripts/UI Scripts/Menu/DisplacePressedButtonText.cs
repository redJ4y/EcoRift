//script to have button text move up and down with button sprites

 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;

 public class DisplacePressedButtonText : MonoBehaviour
 {
     public int offsetX = 2, offsetY = 2;
     public RectTransform textRect;
     Vector3 pos;

     void Start()
     {
         pos = textRect.localPosition;
     }

     public void Down()
     {
         textRect.localPosition = new Vector3(pos.x + (float)offsetX, pos.y - (float)offsetY, pos.z);
     }

     public void Up()
     {
         textRect.localPosition = pos;
     }
 }
