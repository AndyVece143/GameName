using System.Collections;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public GameObject mainTitle;
    public GameObject credits;
    public GameObject skip;
    public float duration;

    private float buttonTimer;
    public LevelLoader loader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(mainTitle.transform.position);
        Debug.Log(credits.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        buttonTimer -= Time.deltaTime;
    }

    public void CreditsButton()
    {
        if (buttonTimer <= 0)
        {
            StartCoroutine(MoveElements(false));
            buttonTimer = duration;
        }

    }

    public void TitleButton()
    {
        if (buttonTimer <= 0)
        {
            StartCoroutine(MoveElements(true));
            buttonTimer = duration;
        }

    }

    public void SkipPrologue()
    {
        loader.LoadNextLevel("Level1");
    }

    public void WatchPrologue()
    {
        loader.LoadNextLevel("PrePrologue");
    }

    IEnumerator MoveElements(bool right)
    {
        float time = 0;
        float moveAmount = 18.51f;
        if (!right)
        {
            moveAmount = -18.51f;
        }

        Vector2 titleVector = new Vector2(mainTitle.transform.position.x + moveAmount, 0);
        Vector2 creditsVector = new Vector2(credits.transform.position.x + moveAmount, 0);
        Vector2 skipVector = new Vector2(skip.transform.position.x + moveAmount, 0);



        while (time < duration)
        {
            time += Time.deltaTime;
            //credits.gameObject.transform.position = Vector2.Lerp(credits.transform.position, new Vector2(credits.transform.position.x + moveAmount, credits.transform.position.y), time / duration);
            credits.gameObject.transform.position = Vector2.Lerp(credits.transform.position, creditsVector, time / duration);
            mainTitle.gameObject.transform.position = Vector2.Lerp(mainTitle.transform.position, titleVector, time / duration);
            skip.gameObject.transform.position = Vector2.Lerp(skip.transform.position, skipVector, time / duration);

            yield return null;
        }
    }
}
