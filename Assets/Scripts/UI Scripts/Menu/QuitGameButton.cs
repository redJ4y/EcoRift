//Chris Young 077497 COMP602_2022_S2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameButton : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    public void Exit()
    {
      
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    
    }
}

