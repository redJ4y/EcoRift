//script to toggle sound
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToggleSound : MonoBehaviour
{
  public void ToggleSoundOnOff()
  {
    AudioListener.pause = !AudioListener.pause;
  }
}
