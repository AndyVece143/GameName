using UnityEngine;

public class Scarecrow : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public Rigidbody2D body;
    public Bullet bullet;
    public float shootTimer;
    private float shootTimerMax;
    public Stars stars;
    private Animator anim;
    public Player player;
    public Transform bulletSpawn;
    public bool facingRight = true;
    public bool straightShot;
    public AudioClip shootSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        player = Player.FindAnyObjectByType<Player>();
        shootTimerMax = shootTimer;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if (distance <= 10)
        {
            Movement();
        }
    }

    private void Movement()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            SoundManager.instance.PlaySound(shootSound);
            switch (straightShot) 
            {
                case true:
                    Bullet newBullet = Instantiate(bullet, bulletSpawn.transform.position, gameObject.transform.rotation);
                    straightShot = false;
                    break;
                case false:
                    if (facingRight)
                    {

                        Bullet newerBullet = Instantiate(bullet, bulletSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, 15)));
                    }
                    else
                    {

                        Bullet newerBullet = Instantiate(bullet, bulletSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, -205)));
                    }
                    straightShot = true;
                    break;
            }

            shootTimer = shootTimerMax;
        }

        if (player.transform.position.x < gameObject.transform.position.x && facingRight)
        {
            Rotate();
        }

        if (player.transform.position.x > gameObject.transform.position.x && !facingRight)
        {
            Rotate();
        }
    }

    void Rotate()
    {
        transform.Rotate(0, 180, 0);

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
