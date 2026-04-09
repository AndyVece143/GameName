using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    //public float lowerBounds;

    public enum State
    {
        FollowPlayer,
        StayStill,
    }
    public State state;
    public State initialState;

    void Start()
    {
        initialState = state;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.FollowPlayer:
                FollowPlayer();
                break;
            
            case State.StayStill:
                break;
        }
    }

    private void FollowPlayer()
    {
        //transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        //if (transform.position.y <= lowerBounds)
        //{
        //    transform.position = new Vector3(transform.position.x, lowerBounds, -10f);
        //}
    }

    private void StayStill()
    {

    }
}