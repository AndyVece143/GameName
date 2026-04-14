using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weakpoint" && GetComponentInParent<Player>().state != Player.State.Death)
        {
            switch (collision.gameObject.transform.parent.tag) 
            {
                case "Enemy":
                    collision.gameObject.GetComponentInParent<Pumpkin>().Death();
                    GetComponentInParent<Player>().Bounce();
                    break;
                case "Scarecrow":
                    collision.gameObject.GetComponentInParent<Scarecrow>().Death();
                    GetComponentInParent<Player>().Bounce();
                    break;
                case "Ghost":
                    collision.gameObject.GetComponentInParent<Ghost>().Death();
                    GetComponentInParent<Player>().Bounce();
                    break;
                case "Dad":
                    collision.gameObject.GetComponentInParent<Dad>().TakeDamage();
                    GetComponentInParent<Player>().SuperBounce();
                    break;
            }


        }
    }
}
