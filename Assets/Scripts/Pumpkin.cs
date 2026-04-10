using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Pumpkin : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    public float speed;
    public Transform ledgeDetector;
    public LayerMask groundLayer;
    public float raycastDistance;
    public float wallDistance;
    private BoxCollider2D boxCollider;

    private bool facingRight = true;
    private Vector2 forwards;
    //private bool canMove = true;
    public BoxCollider2D playerCollision;
    public BoxCollider2D weakpoint;

    public Stars stars;

    public int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        body.linearVelocity = new Vector2(speed, body.linearVelocity.y);
    }

    private void Movement()
    {
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
    }

    void Rotate()
    {
        transform.Rotate(0, 180, 0);
        speed = -speed;

        if (facingRight)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
    }

    public void Death()
    {
        Stars newStars = Instantiate(stars);
        newStars.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }
}
