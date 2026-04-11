using UnityEngine;

public class FallingStar : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        body.linearVelocity = new Vector2(-speed, -speed);

        if (transform.position.y <= -20)
        {
            Destroy(gameObject);
        }
    }
}
