using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject regularUI;
    public LevelLoader loader;

    void Start()
    {
        loader = LevelLoader.FindAnyObjectByType<LevelLoader>();
        regularUI = GameObject.FindWithTag("GameUI");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        regularUI.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        regularUI.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void RestartLevel()
    {
        Resume();
        string currentSceneName = SceneManager.GetActiveScene().name;
        loader.LoadNextLevel(currentSceneName);
    }

    public void QuitToMenu()
    {
        Resume();
        loader.LoadNextLevel("Title");
    }
}
