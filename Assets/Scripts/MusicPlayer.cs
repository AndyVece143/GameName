using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public bool dadfight;
    public AudioClip houseTheme;
    public AudioClip dreamTheme;
    public AudioClip bossTheme;
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
            case "Level2":
            case "Level3":
            case "Title":
                audioSource.clip = dreamTheme;
                break;
            case "Prologue":
                audioSource.clip = houseTheme;
                break;
        }

        StartSong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSong()
    {
        audioSource.Play();
        audioSource.loop = true;
    }

    public void StopSong()
    {
        audioSource.Stop();
    }

    public void BossTime()
    {
        audioSource.Stop();
        audioSource.clip = bossTheme;
        audioSource.Play();
        audioSource.loop = true;
    }
}
