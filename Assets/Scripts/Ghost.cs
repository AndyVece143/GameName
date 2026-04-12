using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Rigidbody2D body;
    public float wanderSpeed;
    public float pursueSpeed;
    public Transform ledgeDetector;
    public LayerMask groundLayer;
    public float raycastDistance;
    public float wallDistance;
    private BoxCollider2D boxCollider;
    public float sightDistance;
    private bool facingRight = true;
    private Vector2 forwards;
    public Player player;
    private bool turning = false;
    public float jumpForce;
    [SerializeField] private LayerMask raycastLayers;
    private Vector2 initialPosition;
    private Animator anim;
    public Stars stars;
    public int damage;

    public enum State
    {
        Moving,
        Standing,
        Pursuing,
    }
    private State initialState;
    public State state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        initialPosition = transform.position;
        initialState = state;
    }

    // Update is called once per frame
    void Update()
    {
        Detection();

        switch (state)
        {
            case State.Moving:
                Moving();
                break;
            case State.Standing:
                break;
            case State.Pursuing:
                Pursuing();
                break;
        }
    }

    private void Moving()
    {
        body.linearVelocity = new Vector2(wanderSpeed, body.linearVelocity.y);
        if (facingRight)
        {
            forwards = Vector2.right;
        }
        else
        {
            forwards = Vector2.left;
        }

        RaycastHit2D hit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, raycastDistance, groundLayer);
        RaycastHit2D hitWall = Physics2D.Raycast(ledgeDetector.position, forwards, wallDistance, groundLayer);

        if (hit.collider == null || hitWall == true)
        {
            Rotate();
        }

        anim.SetBool("pursuing", false);
    }

    void Rotate()
    {
        transform.Rotate(0, 180, 0);
        wanderSpeed = -wanderSpeed;
        pursueSpeed = -pursueSpeed;

        if (facingRight)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
    }

    void Detection()
    {
        RaycastHit2D detection = Physics2D.Raycast(ledgeDetector.position, forwards, sightDistance, raycastLayers);

        if (detection.collider != null)
        {
            Debug.Log("Sas");
            if (detection.collider.CompareTag("Player"))
            {
                state = State.Pursuing;
            }
        }
    }

    void Pursuing()
    {
        body.linearVelocity = new Vector2(pursueSpeed, body.linearVelocity.y);
        if (facingRight)
        {
            forwards = Vector2.right;
        }
        else
        {
            forwards = Vector2.left;
        }

        float yDistance = player.transform.position.y - body.transform.position.y;

        if (yDistance >= 2 && IsGrounded())
        {
            JumpForceMethod();
        }

        float distance = transform.position.x - player.transform.position.x;

        if (distance < 0.0f && !facingRight && turning == false)
        {
            StartCoroutine(waiterTurn());
            //Rotate();
            turning = true;
        }
        if (distance > 0 && facingRight && turning == false)
        {
            StartCoroutine(waiterTurn());
            //Rotate();
            turning = true;
        }

        if (distance > 11f || distance < -11f)
        {
            Debug.Log("Stop chasing");
            Respawn();
        }

        anim.SetBool("pursuing", true);
    }

    void JumpForceMethod()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public void Respawn()
    {
        transform.position = initialPosition;
        state = initialState;
    }

    public void Death()
    {
        Stars newStars = Instantiate(stars);
        newStars.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }

    IEnumerator waiterTurn()
    {
        yield return new WaitForSeconds(0.2f);
        Rotate();
        turning = false;
    }
}
