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
    }
    public State state;
    public State initialState;

    public Animator anim;

    public SpriteRenderer thoughtBubble;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        thoughtBubble.enabled = false;
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
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        //Jumping Code
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            Jump();
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                Jump();
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
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Interact")
        {
            thoughtBubble.enabled = false;
        }
    }
}
