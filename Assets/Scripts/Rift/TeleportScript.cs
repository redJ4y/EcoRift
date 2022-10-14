using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportScript : MonoBehaviour
{
    [SerializeReference] private GameObject pairedPortal;
    [SerializeReference] private DataManager dataManager;
    [SerializeField] private string currentLevel;
    private Vector3 teleportOffset;
    private Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<Collider2D>();
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        teleportOffset = new Vector3(-1.0f, 0, 0);
        if (renderer.flipX == false)
        {
            teleportOffset = new Vector3(1.0f, 0, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject obj = col.gameObject;

        if (obj.tag == "Player")
        {
            UpdateLevelData();

            CharacterController2D controller = obj.GetComponent<CharacterController2D>();
            if (controller.currentlyTeleporting == false)
            {
                controller.currentlyTeleporting = true;
                teleportToMainMenu();

                StartCoroutine(TeleportDelay(controller));
            }
        }
    }


    private void UpdateLevelData()
    {
        switch(currentLevel)
        {
            case "sun":
                dataManager.SunLevelComplete();
                break;
            case "rain":
                dataManager.RainLevelComplete();
                break;
            case "storm":
                dataManager.StormLevelComplete();
                break;
            case "snow":
                dataManager.SnowLevelComplete();
                break;
            default:
                break;
        }
    }

    private void teleportToMainMenu()
    {
        StartCoroutine(GoToMainMenu());
    }

    private void teleportToOtherPortal(Transform objTrans)
    {
        objTrans.localPosition = pairedPortal.transform.localPosition + teleportOffset;
    }

    private IEnumerator TeleportDelay(CharacterController2D controller)
    {
        yield return new WaitForSeconds(3);

        controller.currentlyTeleporting = false;
    }

    IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(0);
    }
}
