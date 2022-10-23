//script to control level select screen functions, including loading the scene, //opening and closing the level select panel.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScreen : MonoBehaviour
{
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject levelSelectCanvas;

    public void OpenLevelSelectScreen()
    {
        mainMenuCanvas.SetActive(false);
        levelSelectCanvas.SetActive(true);
        levelSelectCanvas.GetComponent<Canvas>().sortingOrder = 5;

    }

    public void CloseLevelSelectScreen()
    {
        mainMenuCanvas.SetActive(true);
        levelSelectCanvas.SetActive(false);
        mainMenuCanvas.GetComponent<Canvas>().sortingOrder = 5;
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
