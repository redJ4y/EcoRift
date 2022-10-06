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
}
