using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls controls;
    float playerDirection = 0f;
    public Rigidbody2D rb;
    //public Animator animator;
    public bool isFacingRight = true;
    public float speed = 800f;
    public float jumpForce = 5;
    AudioSource jumpsound;

    public LayerMask groundLayer;

    bool isGrounded;
    public Transform groundCheck;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.Ground.Run.performed += ctx =>
        {
            playerDirection = ctx.ReadValue<float>();
        };

        controls.Ground.Jump.performed += ctx => Jump();

    }

    // Start is called before the first frame update
    void Start()
    {
        jumpsound = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        //animator.SetBool("isGrounded", isGrounded);
        rb.velocity = new Vector2(playerDirection * speed * Time.fixedDeltaTime, rb.velocity.y);
        //animator.SetFloat("speed", Mathf.Abs(playerDirection));

        if (isFacingRight && playerDirection < 0 || !isFacingRight && playerDirection > 0)
        {
            Flip();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void Jump()
    {
        if(isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpsound.Play();
        }
    }
}
