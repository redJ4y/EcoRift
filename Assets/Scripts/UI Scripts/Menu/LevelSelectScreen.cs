using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScreen : MonoBehaviour
{
    [SerializeField] GameObject levelSelectCanvas;

    public void OpenLevelSelectScreen()
    {
        levelSelectCanvas.SetActive(true);
    }

    public void CloseLevelSelectScreen()
    {
        levelSelectCanvas.SetActive(false);
    }
    
    public void LoadSnowLevel() {
        SceneManager.LoadScene(1);
    }

    public void LoadSunLevel() {
        SceneManager.LoadScene(2);
    }

    public void LoadStormLevel() {
        SceneManager.LoadScene(3);
    }

    public void LoadRainLevel() {
        SceneManager.LoadScene(4);
    }
}
