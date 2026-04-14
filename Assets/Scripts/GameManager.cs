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

    public int coinAmount;
    public float timer;
    private bool isTimerOn = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        respawnPosition = player.transform.position;
        coinAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "" + player.health;
        if (fallingStar)
        {
            StarSpawner();
        }

        if (isTimerOn)
        {
            timer += Time.deltaTime;
        }
    }

    public void RespawnPlayer()
    {
        if (activeCheckpoint)
        {
            //player.transform.position = activeCheckpoint.transform.position;
            player.transform.position = new Vector3(activeCheckpoint.transform.position.x, activeCheckpoint.transform.position.y + 4f, activeCheckpoint.transform.position.z);
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
            float x = Random.Range(player.transform.position.x - 10f, player.transform.position.x + 10f);
            newFallingStar.transform.position = new Vector3(x, player.transform.position.y + 10f, 0);
            starTimer = 0;
        }
    }

    public void GetCoin()
    {
        coinAmount++;
    }

    public void StopTimer()
    {
        isTimerOn = false;
    }

    public IEnumerator RespawnPlayerWaiter()
    {
        doorTransition.DoTransition();
        yield return new WaitForSeconds(1.3f);
        RespawnPlayer();
    }
}
