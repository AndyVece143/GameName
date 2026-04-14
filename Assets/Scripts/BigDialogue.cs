using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Rendering;

public class BigDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    public bool[] talkChanges;

    public int[] emotionChanges;

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

    public AudioClip audioClip;

    public CameraController mainCamera;

    private const string HTML_ALPHA = "<color=#00000000>";
    public bool ready = false;

    private GameObject gameUI;
    public bool isBossFightTrigger;
    private Dad dad;
    public bool changeCameraState;
    public bool sceneTransition;
    public LevelLoader loader;
    public string sceneName;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        player = Player.FindAnyObjectByType<Player>();
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
        textComponent.text = string.Empty;
        gameUI = GameObject.FindWithTag("GameUI");
        gameUI.gameObject.SetActive(false);
        if (isBossFightTrigger)
        {
            dad = Dad.FindAnyObjectByType<Dad>();
        }

        if (sceneTransition)
        {
            loader = LevelLoader.FindAnyObjectByType<LevelLoader>();
        }
        BeginningSprite();
        SetPositions();
        //StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (ready == true)
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
        ready = false;
        if (index < lines.Length - 1)
        {
            index++;

            if (talkChanges[index] == true)
            {
                ChangeBothSprites();
            }
            ChangeEmotion();
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
        if (!character1.isActiveSpeaker)
        {
            StartCoroutine(ChangeSprite(false, character1));
            
        }

        if (!character2.isActiveSpeaker)
        {
            StartCoroutine(ChangeSprite(false, character2));
        }
        ChangeEmotion();
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

    void ChangeEmotion()
    {
        if (character1.isActiveSpeaker)
        {
            character1.ChangeEmotion(emotionChanges[index]);
        }

        if (character2.isActiveSpeaker)
        {
            character2.ChangeEmotion(emotionChanges[index]);
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
        int i = 4;
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

    IEnumerator ChangeSprite(bool activeSpeaker, BigDialogueSprite character)
    {
        float time = 0;

        //Become brighter and bigger
        if (activeSpeaker)
        {
            character.isActiveSpeaker = true;
            Vector3 targetSize = character.gameObject.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f);

            if (character.transform.localScale.x < 0)
            {
                targetSize = character.gameObject.transform.localScale + new Vector3(-0.1f, 0.1f, 0.1f);
            }

            //Vector3 targetSize = character.gameObject.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f);
            while (time < duration)
            {
                time += Time.deltaTime;
                character.image.color = Color.Lerp(darkColor, baseColor, time / duration);
                character.gameObject.transform.localScale = Vector3.Lerp(character.gameObject.transform.localScale, targetSize, time / duration);
                yield return null;
            }

        }

        else
        {
            character.isActiveSpeaker = false;
            Vector3 targetSize = character.gameObject.transform.localScale - new Vector3(0.1f, 0.1f, 0.1f);
            if (character.transform.localScale.x < 0)
            {
                targetSize = character.gameObject.transform.localScale - new Vector3(-0.1f, 0.1f, 0.1f);
            }

            while (time < duration)
            {
                time += Time.deltaTime;
                character.image.color = Color.Lerp(baseColor, darkColor, time / duration);
                character.gameObject.transform.localScale = Vector3.Lerp(character.gameObject.transform.localScale, targetSize, time / duration);
                yield return null;
            }

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
        if (sceneTransition)
        {
            loader.LoadNextLevel(sceneName);
        }

        player.state = player.initialState;

        if (!changeCameraState)
        {
            mainCamera.state = mainCamera.initialState;
        }

        gameUI.SetActive(true);

        if (isBossFightTrigger)
        {
            dad.state = Dad.State.Fight;
        }
        Destroy(gameObject);
    }
}
