using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weakpoint")
        {
            collision.gameObject.GetComponentInParent<Pumpkin>().Death();
            GetComponentInParent<Player>().Bounce();
        }
    }
}
