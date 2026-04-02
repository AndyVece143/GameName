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

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Standard;
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
        float horizontalInput = Input.GetAxis("Horizontal");

        body.linearVelocity = new Vector2((horizontalInput * speed) / 2, body.linearVelocity.y);

        //Flip Sprite

    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public void StopMoving()
    {
        body.linearVelocity = new Vector2(0, 0);
    }
}
