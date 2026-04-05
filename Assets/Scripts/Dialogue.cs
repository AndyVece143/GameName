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
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
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
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
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
