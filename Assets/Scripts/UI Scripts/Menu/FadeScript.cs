//deprecated
//script to fade menu in and out

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{

    [SerializeField] private CanvasGroup menuGroup;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    public void ShowUI()
    {
        fadeIn = true;
    }

    public void HideUI()
    {
        fadeOut = true;
    }

    private void Update()
    {
        if (fadeIn)
        {
            if(menuGroup.alpha < 1)
            {
                menuGroup.alpha += Time.deltaTime;
                if (menuGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if(menuGroup.alpha >= 0)
            {
                menuGroup.alpha -= Time.deltaTime;
                if (menuGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }
}
