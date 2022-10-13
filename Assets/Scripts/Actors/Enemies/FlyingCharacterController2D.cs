using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCharacterController2D : MonoBehaviour
{
    [SerializeReference] private Animator animator;
    IDictionary<string, Color32> colourReference;

    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;
    private SpriteRenderer renderer;
    private bool slowed = false;
    private bool frozen = false;

    private void Start()
    {
        // add colours to dictionary
        colourReference = new Dictionary<string, Color32>();
        colourReference.Add("Red", new Color32(255, 200, 200, 255));
        colourReference.Add("Light Blue", new Color32(153, 204, 255, 255));
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 move)
    {
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, move * 10.0f, ref m_Velocity, 0.5f);
        if (move.x > 0 && !m_FacingRight) // If the input is moving the player right and the player is facing left...
        {
            Flip();
        }
        else if (move.x < 0 && m_FacingRight) // Otherwise if the input is moving the player left and the player is facing right...
        {
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing
        m_FacingRight = !m_FacingRight;
        renderer.flipX = !renderer.flipX;
    }

    public void SlowInflicted()
    {
        if (slowed == false)
        {
            slowed = true;
            StartCoroutine(overlayColour(renderer, "Light Blue"));
        }
    }

    public void FreezeInflicted(GameObject freezeObject)
    {
        if (frozen == false)
        {
            frozen = true;

            GameObject newFreeze = Instantiate(freezeObject, transform.position, Quaternion.identity);
            newFreeze.transform.SetParent(transform);
            Destroy(newFreeze, 5.0f);
            StartCoroutine(Thaw());
        }
    }

    private IEnumerator Thaw()
    {
        RigidbodyConstraints2D currentConstraints = m_Rigidbody2D.constraints;
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(5);
        m_Rigidbody2D.constraints = currentConstraints;
        frozen = false;
    }

    private IEnumerator overlayColour(SpriteRenderer spriteRenderer, string newColour)
    {
        bool staySlowed = true;
        Color currentColor = spriteRenderer.color;
        Color32 newColor = colourReference[newColour];
        spriteRenderer.color = newColor;

        while (staySlowed)
        {
            staySlowed = false;
            yield return new WaitForSeconds(3);
        }

        spriteRenderer.color = currentColor;
        slowed = false;
    }

    public float GetMovementDebuff()
    {
        float movementDebuff = 1.0f;
        if (slowed)
        {
            movementDebuff = .5f; // this will be multiplied from the normal speed
        }
        else if (frozen)
        {
            movementDebuff = 0.0f;
        }
        return movementDebuff;
    }
}
