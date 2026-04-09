using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce;
    public float speed;
    private Rigidbody2D body;
    public BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;

    public float jumpTime;
    public float jumpTimeCounter;
    private bool isJumping;

    public enum State
    {
        Standard,
        NoMove,
        RealWorld,
        HitStun,
    }
    public State state;
    public State initialState;

    public Animator anim;

    public SpriteRenderer thoughtBubble;
    public SpriteRenderer goBubble;
    private bool bounce = false;
    private float hitStunTime;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        thoughtBubble.enabled = false;
        goBubble.enabled = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //state = State.Standard;
        initialState = state;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Standard:
                Movement();
                break;
            case State.NoMove:
                break;
            case State.RealWorld:
                RealWorldMovement();
                break;
            case State.HitStun:
                HitStun();
                break;
        }
    }

    private void Movement()
    {
        anim.SetInteger("react", 0);
        hitStunTime = 0;

        float horizontalInput = Input.GetAxis("Horizontal");

        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        //Jumping Code
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            JumpForceMethod();
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                JumpForceMethod();
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        if (IsGrounded())
        {
            body.gravityScale = 1.5f;
        }
        if (!IsGrounded() && body.linearVelocity.y <= 0)
        {
            body.gravityScale = 2;
            bounce = false;
        }

        //Flip Sprite
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }

        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        anim.SetBool("move", horizontalInput != 0);
        anim.SetBool("grounded", IsGrounded());
        anim.SetBool("falling", IsFalling());
        anim.SetBool("down", IsCrouching());
        anim.SetBool("stilldown", IsStillCrouching());
        anim.SetBool("up", IsLookingUp(true));
        anim.SetBool("stillup", IsLookingUp(false));
        anim.SetBool("bounce", bounce);
    }

    private void HitStun()
    {
        hitStunTime += Time.deltaTime;
        anim.SetBool("hitstun", true);
        if (IsGrounded() && hitStunTime >= 0.2f)
        {
            anim.SetBool("hitstun", false);
            state = State.Standard;
        }
    }

    private void KnockBack()
    {
        if (IsFacingRight())
        {
            body.linearVelocity = new Vector2(-3f, 5f);
        }
        else
        {
            body.linearVelocity = new Vector2(3f, 5f);
        }
    }

    private void JumpForceMethod()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
    }

    public void Bounce()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce * 1.5f);
        bounce = true;
    }

    public bool IsFalling()
    {
        if (body.linearVelocity.y < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsCrouching()
    {
        if (IsGrounded() && body.linearVelocity.x == 0 && Input.GetKeyDown(KeyCode.DownArrow))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsStillCrouching()
    {
        if (IsGrounded() && body.linearVelocity.x == 0 && Input.GetKey(KeyCode.DownArrow))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsLookingUp(bool b)
    {
        switch (b)
        {
            case true:
                if (IsGrounded() && body.linearVelocity.x == 0 && Input.GetKeyDown(KeyCode.UpArrow))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case false:
                if (IsGrounded() && body.linearVelocity.x == 0 && Input.GetKey(KeyCode.UpArrow))
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
    }

    private void RealWorldMovement()
    {
        anim.SetInteger("react", 0);

        float horizontalInput = Input.GetAxis("Horizontal");

        body.linearVelocity = new Vector2((horizontalInput * speed) / 2, body.linearVelocity.y);

        //Flip Sprite
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }

        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        anim.SetBool("move", horizontalInput != 0);
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool IsFacingRight()
    {
        if (transform.localScale.x == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StopMoving(int react)
    {
        body.linearVelocity = new Vector2(0, 0);
        anim.SetInteger("react", react);
        thoughtBubble.enabled = false;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Interact")
    //    {
    //        thoughtBubble.enabled = true;
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Interact" && state != State.NoMove)
        {
            thoughtBubble.enabled = true;
        }

        if (collision.gameObject.tag == "Door")
        {
            goBubble.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Interact")
        {
            thoughtBubble.enabled = false;
        }

        if (collision.gameObject.tag == "Door")
        {
            goBubble.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            KnockBack();
            state = State.HitStun;
        }
    }
}
