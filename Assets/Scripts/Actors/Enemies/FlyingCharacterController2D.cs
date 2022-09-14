using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCharacterController2D : MonoBehaviour
{
    [SerializeReference] private Animator animator;

    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;
    private SpriteRenderer renderer;

    private void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // TODO: Animator
    }

    public void Move(Vector2 move)
    {
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, move * 10.0f, ref m_Velocity, 0.5f);

        // If the input is moving the player right and the player is facing left...
        if (move.x > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move.x < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        /*
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        */

        renderer.flipX = !renderer.flipX;
    }
}
