using UnityEngine;

public class Goal : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public Player player;
    private float finishTimer;
    public WinScreen winScreen;
    public bool triggered;
    public CameraController mainCamera;
    public string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggered)
        {
            return;
        }

        if (collision.tag == "Player" && player.IsGrounded())
        {
            finishTimer += Time.deltaTime;

            if (finishTimer > 0.1f)
            {
                mainCamera.state = CameraController.State.StayStill;
                player.state = Player.State.NoMove;
                player.StopMoving(2);
                WinScreen newWinScreen = Instantiate(winScreen);
                newWinScreen.sceneName = sceneName;
                triggered = true;
            }
        }
    }
}
