using UnityEngine;

public class Parallax : MonoBehaviour
{
    //private float length;
    private float startPositionX;
    private float startPositionY;
    public CameraController mainCamera;
    public float parallaxEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPositionX = transform.position.x;
        startPositionY = transform.position.y;
        //length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceX = (mainCamera.transform.position.x * parallaxEffect);
        float distanceY = (mainCamera.transform.position.y * parallaxEffect);
        transform.position = new Vector3(startPositionX + distanceX, startPositionY + distanceY, transform.position.z);
    }
}
