using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Player player;
    public BoxCollider2D boxCollider;
    public bool interactable;
    public Vector3 teleportPoint;
    public CameraController bigCamera;
    public Vector3 cameraTeleportPoint;
    public DoorTransition doorTransition;
    public bool changeCameraState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
        interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCollider.IsTouching(player.boxCollider) && player.IsGrounded())
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (interactable == true)
                {
                    //player.thoughtBubble.enabled = false;
                    interactable = false;
                    //player.state = Player.State.NoMove;
                    StartCoroutine(Transition());
                }
            }
        }
    }

    IEnumerator Transition()
    {
        player.state = Player.State.NoMove;
        doorTransition.DoTransition();
        yield return new WaitForSeconds(1.3f);
        player.transform.position = teleportPoint;
        bigCamera.transform.position = cameraTeleportPoint;
        if (changeCameraState)
        {
            bigCamera.state = CameraController.State.StayStill;
        }
        player.state = player.initialState;
    }
}
