//deprecated - changed to level select button
//controls start game button
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{

    IEnumerator StartGameAfterWait() {
    yield return new WaitForSeconds(1.0f);

    SceneManager.LoadScene(gameStartScene);
}
    //String corresponding to scene to start when start button is pressed
    public int gameStartScene;


    public void StartGame() {
        StartCoroutine(StartGameAfterWait());
    }
}
