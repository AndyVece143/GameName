using System.Collections;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinText;
    public float duration;
    public Canvas canvas;

    public GameObject textBox;
    private Vector3 textBoxPosition;
    private Vector3 textBoxEndPosition;

    public LevelLoader loader;
    public string sceneName;
    public Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        loader = LevelLoader.FindAnyObjectByType<LevelLoader>();
        gameManager = GameManager.FindAnyObjectByType<GameManager>();
        player = Player.FindAnyObjectByType<Player>();
        gameManager.StopTimer();
        SetPosition();
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void SetPosition()
    {
        textBoxPosition = textBox.transform.position;
        textBox.transform.position = new Vector3(textBox.transform.position.x + 15f, textBox.transform.position.y, textBox.transform.position.z);
        textBoxEndPosition = textBox.transform.position;
        StartCoroutine(MoveSpriteBeginning());
    }

    void SetText()
    {
        int seconds = ((int)gameManager.timer % 60);
        int minutes = ((int)gameManager.timer / 60);

        timerText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        coinText.text = "Coins: " + gameManager.coinAmount;
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

        yield return new WaitForSeconds(3f);
        StartCoroutine(MoveSpriteEnd());
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
        StaticData.playerDefense = player.defense;
        loader.LoadNextLevel(sceneName);
    }
}
