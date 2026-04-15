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
    public int health;
    public int defense;
    public enum State
    {
        Standard,
        NoMove,
        RealWorld,
        HitStun,
        Death,
        SuperBounce,
    }
    public State state;
    public State initialState;

    public Animator anim;

    public SpriteRenderer thoughtBubble;
    public SpriteRenderer goBubble;
    private bool bounce = false;
    private float hitStunTime;
    public EnemyStomp enemyStomp;
    public CameraController mainCamera;
    public GameManager gameManager;
    private float deathTime;
    private float iFrameTimer;
    public bool iFrames;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip spinSound;
    public float superBounceSpeed;

    public bool cannotJump;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        gameManager = GameManager.FindAnyObjectByType<GameManager>();
        thoughtBubble.enabled = false;
        goBubble.enabled = false;
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //state = State.Standard;
        initialState = state;
        defense = StaticData.playerDefense;
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
            case State.Death:
                DeathMovement();
                break;
            case State.SuperBounce:
                SuperBounceMovement();
                break;
        }
    }

    private void Movement()
    {
        anim.SetInteger("react", 0);
        anim.SetBool("death", false);
        boxCollider.enabled = true;
        enemyStomp.enabled = true;
        hitStunTime = 0;
        deathTime = 0;

        float horizontalInput = Input.GetAxis("Horizontal");

        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        //Jumping Code
        if (!cannotJump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                SoundManager.instance.PlaySound(jumpSound);
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

        iFrameTimer -= Time.deltaTime;
        if (iFrameTimer < 0)
        {
            iFrames = false;
        }

        if (iFrames)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
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

    private void TakeDamage(int damage)
    {
        SoundManager.instance.PlaySound(hurtSound);
        int takenDamage = damage - defense;
        Debug.Log(takenDamage);
        if (takenDamage < 0)
        {
            takenDamage = 0;
        }

        health -= takenDamage;

        if (health < 0)
        {
            health = 0;
        }

        if (health > 0)
        {
            state = State.HitStun;
            KnockBack();
        }
        else
        {
            state = State.Death;
            Death();
        }
    }

    private void HitStun()
    {
        hitStunTime += Time.deltaTime;
        anim.SetBool("hitstun", true);
        if (IsGrounded() && hitStunTime >= 0.2f)
        {
            anim.SetBool("hitstun", false);
            state = State.Standard;
            iFrames = true;
            iFrameTimer = 3;
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

    private void Death()
    {
        SoundManager.instance.PlaySound(deathSound);
        mainCamera.state = CameraController.State.StayStill;
        boxCollider.enabled = false;
        body.linearVelocity = new Vector2(0, 10);
        enemyStomp.enabled = false;
    }

    private void DeathMovement()
    {
        anim.SetBool("hitstun", false);
        deathTime += Time.deltaTime;
        anim.SetBool("death", true);
        if (deathTime >= 2.5f)
        {
            StartCoroutine(gameManager.RespawnPlayerWaiter());
            state = State.NoMove;
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
        SoundManager.instance.PlaySound(spinSound);
    }

    public void SuperBounce()
    {
        body.linearVelocity = new Vector2(-superBounceSpeed, jumpForce * 1.5f);
        bounce = true;
        state = State.SuperBounce;
    }

    public void SuperBounceMovement()
    {
        anim.SetBool("bounce", bounce);
        anim.SetBool("grounded", IsGrounded());
        anim.SetBool("falling", IsFalling());

        if (IsGrounded())
        {
            state = State.Standard;
            bounce = false;
        }
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
        if (IsGrounded() && body.linearVelocity.x == 0 && (Input.GetKeyDown(KeyCode.DownArrow)) || Input.GetKeyDown(KeyCode.S))
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
        if (IsGrounded() && body.linearVelocity.x == 0 && (Input.GetKey(KeyCode.DownArrow)) || Input.GetKey(KeyCode.S))
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
                if (IsGrounded() && body.linearVelocity.x == 0 && (Input.GetKeyDown(KeyCode.UpArrow)) || Input.GetKeyDown(KeyCode.W))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case false:
                if (IsGrounded() && body.linearVelocity.x == 0 && (Input.GetKey(KeyCode.UpArrow)) || Input.GetKey(KeyCode.W))
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
        if (collision.gameObject.tag == "Interact" && state != State.NoMove && collision.gameObject.GetComponent<InteractableObject>().isAutomatic == false)
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
        if (iFrames == false && state != State.HitStun)
        {
            if (collision.collider.tag == "Enemy")
            {
                //KnockBack();
                //state = State.HitStun;
                TakeDamage(collision.gameObject.GetComponent<Pumpkin>().damage);
            }

            if (collision.collider.tag == "Bullet")
            {
                TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
            }

            if (collision.collider.tag == "Ghost")
            {
                TakeDamage(collision.gameObject.GetComponent<Ghost>().damage);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Danger" && state != State.Death)
        {
            health = 0;
            state = State.Death;
            Death();
        }
    }
}
