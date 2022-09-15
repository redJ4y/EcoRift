using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    private Collider2D collider;
    [SerializeReference] private GameObject pairedPortal;
    private Vector3 teleportOffset;

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
            CharacterController2D controller = obj.GetComponent<CharacterController2D>();
            if (controller.currentlyTeleporting == false)
            {
                controller.currentlyTeleporting = true;
                obj.transform.localPosition = pairedPortal.transform.localPosition + teleportOffset;

                StartCoroutine(TeleportDelay(controller));
            }
        }
    }

    private IEnumerator TeleportDelay(CharacterController2D controller)
    {
        yield return new WaitForSeconds(3);

        controller.currentlyTeleporting = false;
    }
}
