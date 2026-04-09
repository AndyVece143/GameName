using UnityEngine;
using UnityEngine.UI;

public class PaperImage : MonoBehaviour
{
    public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
    }
}
