using UnityEngine;

public class Dad : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public Rigidbody2D body;
    public Bullet laser;
    public float shootTimer;
    private float shootTimerMax;
    public Player player;
    public Transform laserSpawn;
    public bool facingRight = true;
    public bool straightShot;
    public Animator anim;
    public float damageTimer;
    private float damageTimerMax;
    public int health = 10;
    public BigDialogueTrigger trigger;
    public AudioClip shootSound;
    public AudioClip damageSound;
    public enum State
    {
        Standard,
        Fight,
        Defeat,
        HitStun,
    }
    public State state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        player = Player.FindAnyObjectByType<Player>();
        shootTimerMax = shootTimer;
        damageTimerMax = damageTimer;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Standard:
                StandardMovement();
                break;
            case State.Fight:
                Fighting();
                break;
            case State.HitStun:
                HitStun();
                break;
            case State.Defeat:
                Defeated();
                break;
        }
    }

    void StandardMovement()
    {
        anim.SetBool("fight", false);
        if (player.transform.position.x < gameObject.transform.position.x && facingRight)
        {
            Rotate();
        }

        if (player.transform.position.x > gameObject.transform.position.x && !facingRight)
        {
            Rotate();
        }
    }

    void Fighting()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            SoundManager.instance.PlaySound(shootSound);
            switch (straightShot)
            {
                case true:
                    Bullet newBullet = Instantiate(laser, laserSpawn.transform.position, gameObject.transform.rotation);
                    straightShot = false;
                    break;
                case false:
                    if (facingRight)
                    {

                        Bullet newerBullet = Instantiate(laser, laserSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, 15)));
                    }
                    else
                    {

                        Bullet newerBullet = Instantiate(laser, laserSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, -205)));
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

        anim.SetBool("fight", true);
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

    void Defeated()
    {
        anim.SetBool("defeat", true);
        boxCollider.isTrigger = true;
        body.gravityScale = 0;

        if (player.transform.position.x < gameObject.transform.position.x && facingRight)
        {
            Rotate();
        }

        if (player.transform.position.x > gameObject.transform.position.x && !facingRight)
        {
            Rotate();
        }
    }

    public void TakeDamage()
    {
        SoundManager.instance.PlaySound(damageSound);
        health--;
        if (health == 0)
        {
            state = State.Defeat;
            trigger.afterBossFight = false;
            return;
        }

        state = State.HitStun;
    }

    public void HitStun()
    {
        anim.SetBool("hurt", true);
        damageTimer -= Time.deltaTime;
        GetComponent<SpriteRenderer>().color = Color.gray;
        if (damageTimer <= 0)
        {
            state = State.Fight;
            damageTimer = damageTimerMax;
            GetComponent<SpriteRenderer>().color = Color.white;
            anim.SetBool("hurt", false);
        }
    }
}
