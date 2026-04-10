using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public TMP_Text healthText;
    public Vector2 respawnPosition;
    public CameraController mainCamera;
    public DoorTransition doorTransition;
    public Checkpoint activeCheckpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        respawnPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + player.health;
    }

    public void RespawnPlayer()
    {
        if (activeCheckpoint)
        {
            player.transform.position = activeCheckpoint.transform.position;
        }
        else
        {
            player.transform.position = respawnPosition;
        }

        player.state = Player.State.Standard;
        player.health = 10;
        mainCamera.state = CameraController.State.FollowPlayer;
    }

    public IEnumerator RespawnPlayerWaiter()
    {
        doorTransition.DoTransition();
        yield return new WaitForSeconds(1.3f);
        RespawnPlayer();
    }
}
