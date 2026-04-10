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
    private float starTimer;
    public float starTimerMax;
    public FallingStar fallingStar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        respawnPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "" + player.health;
        StarSpawner();
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

    private void StarSpawner()
    {
        starTimer += Time.deltaTime;

        if (starTimer >= starTimerMax)
        {
            FallingStar newFallingStar = Instantiate(fallingStar);
            float x = Random.Range(-10f, 10f);
            newFallingStar.transform.position = new Vector3(x, player.transform.position.y + 6f, 0);
            starTimer = 0;
        }
    }

    public IEnumerator RespawnPlayerWaiter()
    {
        doorTransition.DoTransition();
        yield return new WaitForSeconds(1.3f);
        RespawnPlayer();
    }
}
