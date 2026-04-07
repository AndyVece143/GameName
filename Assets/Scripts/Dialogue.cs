using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;

    public Player player;
    public Canvas canvas;
    public InteractableObject interactableObject;
    public GameObject textBox;
    private Vector3 textBoxPosition;
    private Vector3 textBoxEndPosition;
    public float duration;
    public AudioClip audioClip;
    private const string HTML_ALPHA = "<color=#00000000>";
    public bool ready = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        player = Player.FindAnyObjectByType<Player>();
        textComponent.text = string.Empty;
        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (ready == true)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                ready = true;
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        ready = false;

        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (interactableObject != null)
            {
                interactableObject.interactable = true;
            }
            StartCoroutine(MoveSpriteEnd());
            //player.state = player.initialState;
            //Destroy(gameObject);
        }
    }

    void SetPosition()
    {
        textBoxPosition = textBox.transform.position;
        textBox.transform.position = new Vector3(textBox.transform.position.x, textBox.transform.position.y + 5f, textBox.transform.position.z);
        textBoxEndPosition = textBox.transform.position;
        StartCoroutine(MoveSpriteBeginning());
    }

    IEnumerator TypeLine()
    {
        //int i = 0;
        //foreach (char c in lines[index].ToCharArray())
        //{
        //    textComponent.text += c;
        //    i++;
        //    if (i == 5)
        //    {
        //        SoundManager.instance.PlaySound(audioClip);
        //        i = 0;
        //    }

        //    yield return new WaitForSeconds(textSpeed);
        //}
        int i = 0;
        string originalText = lines[index];
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in lines[index].ToCharArray())
        {
            alphaIndex++;
            textComponent.text = originalText;
            displayedText = textComponent.text.Insert(alphaIndex, HTML_ALPHA);
            textComponent.text = displayedText;

            i++;
            if (i == 5)
            {
                SoundManager.instance.PlaySound(audioClip);
                i = 0;
            }

            yield return new WaitForSeconds(textSpeed);
        }
        ready = true;
    }

    IEnumerator MoveSpriteBeginning()
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxPosition, time / duration);
            yield return null;
        }
        StartDialogue();
    }

    IEnumerator MoveSpriteEnd()
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxEndPosition, time / duration);
            yield return null;
        }
        player.state = player.initialState;
        Destroy(gameObject);
    }
}
