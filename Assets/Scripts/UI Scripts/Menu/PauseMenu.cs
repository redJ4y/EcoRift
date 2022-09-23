//script to control the pause menu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
  [SerializeField] GameObject pauseMenu;

  public void Pause()// pauses game
  {
      pauseMenu.SetActive(true);
      Time.timeScale = 0f;
  }

  public void Resume()// resumes game
  {
      pauseMenu.SetActive(false);
      Time.timeScale = 1f;
  }

  public void Home(int sceneID)//return to main menu
  {
    Time.timeScale = 1f;
    SceneManager.LoadScene(sceneID);
  }
}
