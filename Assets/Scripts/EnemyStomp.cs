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
                    break;
                case "Scarecrow":
                    collision.gameObject.GetComponentInParent<Scarecrow>().Death();
                    break;
                case "Ghost":
                    collision.gameObject.GetComponentInParent<Ghost>().Death();
                    break;
            }

            GetComponentInParent<Player>().Bounce();
        }
    }
}
