using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Rendering;

public class BigDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    public bool[] talkChanges;

    public Player player;
    public Canvas canvas;

    //public SpriteRenderer character1;
    //public SpriteRenderer character2;

    public BigDialogueSprite character1;
    public BigDialogueSprite character2;

    public Color baseColor;
    public Color darkColor;
    public float duration;
    public float moveDuration;

    private Vector3 character1Position;
    private Vector3 character2Position;

    public GameObject textBox;
    private Vector3 textBoxPosition;

    private Vector3 character1EndPosition;
    private Vector3 character2EndPosition;
    private Vector3 textBoxEndPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        player = Player.FindAnyObjectByType<Player>();
        textComponent.text = string.Empty;
        BeginningSprite();
        SetPositions();
        //StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (textComponent.text == lines[index])
            {
                //ChangeBothSprites();
                NextLine();
            }
            //else
            //{
            //    StopAllCoroutines();
            //    textComponent.text = lines[index];
            //}
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
            if (talkChanges[index] == true)
            {
                ChangeBothSprites();
            }
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            StartCoroutine(MoveSpritesEnd());
            //player.state = player.initialState;
            //Destroy(gameObject);
        }
    }

    //If a character is not set to talk in the beginning, they get resized
    void BeginningSprite()
    {
        Debug.Log("Beginning Sprite");
        if (!character1.isActiveSpeaker)
        {
            StartCoroutine(ChangeSprite(false, character1));
            
        }

        if (!character2.isActiveSpeaker)
        {
            StartCoroutine(ChangeSprite(false, character2));
        }
    }

    //Swaps the active speaker
    void ChangeBothSprites()
    {
        if (character1.isActiveSpeaker)
        {
            StartCoroutine(ChangeSprite(false, character1));
            StartCoroutine(ChangeSprite(true, character2));
        }

        else
        {
            StartCoroutine(ChangeSprite(true, character1));
            StartCoroutine(ChangeSprite(false, character2));
        }
    }

    void SetPositions()
    {
        character1Position = character1.transform.position;
        character2Position = character2.transform.position;
        textBoxPosition = textBox.transform.position;

        character1.transform.position = new Vector3(character1Position.x - 7f, character1Position.y, character1Position.z);
        character2.transform.position = new Vector3(character2Position.x + 7f, character2Position.y, character2Position.z);
        textBox.transform.position = new Vector3(textBoxPosition.x, textBoxPosition.y - 4.5f, textBoxPosition.z);

        character1EndPosition = character1.transform.position;
        character2EndPosition = character2.transform.position;
        textBoxEndPosition = textBox.transform.position;

        StartCoroutine(MoveSpritesBeginning());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator ChangeSprite(bool activeSpeaker, BigDialogueSprite character)
    {
        float time = 0;

        //Become brighter and bigger
        if (activeSpeaker)
        {
            Vector3 targetSize = character.gameObject.transform.localScale + new Vector3(0.05f, 0.05f, 0.05f);
            while (time < duration)
            {
                time += Time.deltaTime;
                character.image.color = Color.Lerp(darkColor, baseColor, time / duration);
                character.gameObject.transform.localScale = Vector3.Lerp(character.gameObject.transform.localScale, targetSize, time / duration);
                yield return null;
            }
            character.isActiveSpeaker = true;
        }

        else
        {
            Vector3 targetSize = character.gameObject.transform.localScale - new Vector3(0.05f, 0.05f, 0.05f);
            while (time < duration)
            {
                time += Time.deltaTime;
                character.image.color = Color.Lerp(baseColor, darkColor, time / duration);
                character.gameObject.transform.localScale = Vector3.Lerp(character.gameObject.transform.localScale, targetSize, time / duration);
                yield return null;
            }
            character.isActiveSpeaker = false;
        }
    }

    IEnumerator MoveSpritesBeginning()
    {
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            character1.gameObject.transform.position = Vector3.Lerp(character1.gameObject.transform.position, character1Position, time / moveDuration);
            character2.gameObject.transform.position = Vector3.Lerp(character2.gameObject.transform.position, character2Position, time / moveDuration);
            textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxPosition, time / moveDuration);
            yield return null;
        }
        StartDialogue();
    }

    IEnumerator MoveSpritesEnd()
    {
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            character1.gameObject.transform.position = Vector3.Lerp(character1.gameObject.transform.position, character1EndPosition, time / moveDuration);
            character2.gameObject.transform.position = Vector3.Lerp(character2.gameObject.transform.position, character2EndPosition, time / moveDuration);
            textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxEndPosition, time / moveDuration);
            yield return null;
        }
        player.state = player.initialState;
        Destroy(gameObject);
    }
}
