//script to control main menu quit button, closes the app

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
