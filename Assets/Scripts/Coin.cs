using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private AudioClip pickupSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.FindAnyObjectByType(typeof(GameManager)) as GameManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(pickupSound);
            gameManager.GetCoin();
            Destroy(gameObject);
        }
    }
}
