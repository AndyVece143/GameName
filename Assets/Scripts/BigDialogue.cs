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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        player = Player.FindAnyObjectByType<Player>();
        textComponent.text = string.Empty;
        BeginningSprite();
        StartDialogue();
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
            player.state = Player.State.Standard;
            Destroy(gameObject);
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
}
