using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] public bool currentlyTeleporting;                          // A boolean determining whether player is currently teleporting
    [SerializeReference] private Animator animator; // Addition

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;
    private float moveInput = 0; // Addition
    private Vector2 startPosition;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private SpriteRenderer renderer;
    private SwitchStaff switchStaffScript;
    private HealthBarScript UIHealth;
    private bool flashing = false;
    private bool slowed = false;
    private bool frozen = false;

    IDictionary<string, Color32> colourReference;

    void Start()
    {
        startPosition = gameObject.transform.localPosition;
        m_WhatIsGround += LayerMask.GetMask("Enemy");
        // add colours to dictionary
        colourReference = new Dictionary<string, Color32>();
        colourReference.Add("Red", new Color32(255, 200, 200, 255));
        colourReference.Add("Light Blue", new Color32(153, 204, 255, 255));

        renderer = gameObject.GetComponent<SpriteRenderer>();
        UIHealth = GameObject.Find("HpBar").GetComponent<HealthBarScript>();
        if (gameObject.tag == "Player")
            switchStaffScript = transform.Find("Staff").gameObject.GetComponent<SwitchStaff>();
    }

    public Vector2 GetJumpVector() // Addition: returns the velocity vector for if the character were to jump
    {
        return new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y + m_JumpForce);
    }

    public Vector2 GetMovementVector()
    {
        return new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y);
    }

    public bool IsPlayerGrounded() // Addition
    {
        return m_Grounded && Mathf.Abs(m_Rigidbody2D.velocity.y) < 0.1f;
    }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void Update() // Addition
    {
        animator.SetBool("Jump", !m_Grounded);
    }

    private void FixedUpdate()
    {
        animator.SetBool("Run", Mathf.Abs(moveInput) > 0.0001f);
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }


        if (transform.localPosition.y < -20.0f) // -20.0f is arbitrary
        {
            if (gameObject.tag.Equals("Player"))
            {
                transform.localPosition = startPosition;
            }
            else if (gameObject.tag.Equals("Enemy"))
            {
                Destroy(gameObject);
            }
        }



        // Check if player is falling through void, if so teleport to spawn

    }


    public void Move(float move, bool jump)
    {
        moveInput = move; // Addition
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        renderer.flipX = !renderer.flipX;
        if (gameObject.tag == "Player")
            switchStaffScript.FlipStaff();
    }

    public void HitInflicted()
    {
        UIHealth.SetValue();

        if (flashing == false)
        {
            flashing = true;
            StartCoroutine(FlashColour(renderer));
        }
    }

    public void SlowInflicted()
    {
        if (slowed == false)
        {
            slowed = true;
            StartCoroutine(OverlayColour(renderer, "Light Blue"));
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

    private IEnumerator FlashColour(SpriteRenderer spriteRenderer)
    {
        Color currentColor = spriteRenderer.color;
        Color32 newColor = colourReference["Red"];

        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = newColor;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.color = currentColor;
            yield return new WaitForSeconds(.1f);
        }

        flashing = false;
    }

    private IEnumerator OverlayColour(SpriteRenderer spriteRenderer, string newColour)
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
