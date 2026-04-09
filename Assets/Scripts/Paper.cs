using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UIElements;

public class Paper : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    [TextArea]
    public string text;
    public float textSpeed;

    public Player player;
    public Canvas canvas;
    public InteractableObject interactableObject;
    public PaperImage letter;
    private Vector3 letterPosition;
    private Vector3 letterEndPosition;
    private Color fullColor;
    private Color emptyColor;
    public float duration;
    public AudioClip audioClip;
    private const string HTML_ALPHA = "<color=#00000000>";
    public bool ready = false;
    public CameraController mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        player = Player.FindAnyObjectByType<Player>();
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
        textComponent.text = string.Empty;
        SetPositionAndTrans();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (ready == true)
            {
                StartCoroutine(MovePaperEnd());
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = text;
                ready = true;
            }
        }
    }

    void SetPositionAndTrans()
    {
        letterPosition = letter.transform.position;
        fullColor = letter.image.color;
        letter.transform.position = new Vector3(letter.transform.position.x, letter.transform.position.y - 1f, letter.transform.position.z);
        letter.image.color = new Color(letter.image.color.r, letter.image.color.g, letter.image.color.b, 0);
        letterEndPosition = letter.transform.position;
        emptyColor = letter.image.color;
        StartCoroutine(MovePaperBeginning());
    }

    IEnumerator TypeLine()
    {
        string originalText = text;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in text)
        {
            alphaIndex++;
            textComponent.text = originalText;
            displayedText = textComponent.text.Insert(alphaIndex, HTML_ALPHA);
            textComponent.text = displayedText;
            yield return new WaitForSeconds(textSpeed);
        }

        ready = true;
    }

    IEnumerator MovePaperBeginning()
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            letter.transform.position = Vector3.Lerp(letter.transform.position, letterPosition, time / duration);
            letter.image.color = Color.Lerp(letter.image.color, fullColor, time / duration);
            yield return null;
        }

        StartCoroutine(TypeLine());
    }

    IEnumerator MovePaperEnd()
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            letter.transform.position = Vector3.Lerp(letter.transform.position, letterEndPosition, time / duration);
            letter.image.color = Color.Lerp(letter.image.color, emptyColor, time / duration);
            textComponent.color = Color.Lerp(textComponent.color, emptyColor, time / duration);
            yield return null;
        }
        player.state = player.initialState;
        mainCamera.state = mainCamera.initialState;
        Destroy(gameObject);
    }
}
