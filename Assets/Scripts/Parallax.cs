using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPositionX;
    private float startPositionY;
    public CameraController mainCamera;
    public float parallaxEffect;

    private float smoothTime = 0.1f;
    private Vector2 velocity = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPositionX = transform.position.x;
        startPositionY = transform.position.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float distanceX = (mainCamera.transform.position.x * parallaxEffect);
        float distanceY = (mainCamera.transform.position.y * parallaxEffect);
        transform.position = new Vector3(startPositionX + distanceX, startPositionY + distanceY, transform.position.z);

        //float distanceX = (mainCamera.transform.position.x * parallaxEffect);
        //float distanceY = (mainCamera.transform.position.y * parallaxEffect);
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(distanceX, distanceY, transform.position.z), ref velocity, smoothTime);


        //float distanceX = (mainCamera.transform.position.x * parallaxEffect);
        //float distanceY = (mainCamera.transform.position.y * parallaxEffect);
        //transform.position = Vector2.Lerp(new Vector2(startPositionX, startPositionX), new Vector2(startPositionX + distanceX, startPositionY + distanceY), smoothTime);
    }
}
