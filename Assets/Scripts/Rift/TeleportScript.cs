using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportScript : MonoBehaviour
{
    [SerializeReference] private GetWeather weatherState;
    [SerializeReference] private GameObject pairedPortal;
    [SerializeReference] private Animator crossFade;
    [SerializeField] private string currentLevel;

    private Vector3 teleportOffset;
    private Collider2D collider;
    private bool collided;

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
        collided = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!collided && col.gameObject.tag == "Player")
        {
            collided = true;
            Teleport(col);
        }
    }

    private void Teleport(Collision2D col)
    {
        GameObject obj = col.gameObject;

        obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        CharacterController2D controller = obj.GetComponent<CharacterController2D>();
        if (controller.currentlyTeleporting == false)
        {
            UpdateLevelData();
            controller.currentlyTeleporting = true;
            TeleportToMainMenu();

            StartCoroutine(TeleportDelay(controller));
        }
    }

    private void UpdateLevelData()
    {
        DataManager dataManager = DataManager.Instance;
        switch (currentLevel)
        {
            case "sun":
                dataManager.SunLevelComplete(weatherState.getWeatherType());
                break;
            case "rain":
                dataManager.RainLevelComplete(weatherState.getWeatherType());
                break;
            case "storm":
                dataManager.StormLevelComplete(weatherState.getWeatherType());
                break;
            case "snow":
                dataManager.SnowLevelComplete(weatherState.getWeatherType());
                break;
            default:
                break;
        }
    }

    private void TeleportToMainMenu()
    {
        StartCoroutine(GoToMainMenu());
    }

    private void TeleportToOtherPortal(Transform objTrans)
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
        crossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1.4f);
        SceneManager.LoadScene(0);
    }
}
