using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Mainmenu : MonoBehaviour
{
    [SerializeField] Button[] levels;
    [SerializeField] GameObject PausePanel;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoCanvas;
    AudioManager audioManager;
    bool isPaused;
    void Awake()
    {
        isPaused = false;
        int maxUnlocked = PlayerPrefs.GetInt("HighestUnlocked", 1);
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].interactable = false;
        }
        for (int i = 0; i < Mathf.Min(maxUnlocked, levels.Length); i++)
        {
            levels[i].interactable = true;
        }
    }
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }
    public void PlayGame()
    {
        audioManager.PlayClick();
        SceneManager.LoadSceneAsync(1);
    }
    public void ExitGame()
    {
        audioManager.PlayClick();
        Application.Quit();
    }
    public void LoadLevel(int i)
    {
        audioManager.PlayClick();
        if (isPaused)
        {
            PauseMenu();
        }
        SceneManager.LoadSceneAsync(i);
    }
    public void PlayLevel1WithIntro()
    {
        audioManager.PlayClick();
        foreach (Button b in levels)
            b.interactable = false;
        audioManager.bgm.Pause();
        videoCanvas.SetActive(true);
        videoPlayer.Play();

        videoPlayer.loopPointReached += OnIntroFinished;
    }
    void OnIntroFinished(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnIntroFinished;
        SceneManager.LoadSceneAsync(2);
    }

    public void ReloadLevel()
    {
        audioManager.PlayClick();
        if (isPaused)
        {
            PauseMenu();
        }
        int i = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(i);
    }
    public void NextLevel()
    {
        PlayerItems playerItems = FindAnyObjectByType<PlayerItems>();
        if (playerItems.GetNextLevel())
        {
            if (PlayerPrefs.GetInt("HighestUnlocked") <= SceneManager.GetActiveScene().buildIndex - 1)
            {
                PlayerPrefs.SetInt("HighestUnlocked", SceneManager.GetActiveScene().buildIndex);
                Debug.Log("Set to " + SceneManager.GetActiveScene().buildIndex);
                PlayerPrefs.Save();
            }
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void PauseMenu()
    {
        audioManager.PlayClick();
        if (PausePanel == null) return;
        if (!isPaused)
        {
            Time.timeScale = 0f;
            PausePanel.SetActive(true);
            isPaused = true;
        }
        else
        {
            PausePanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }
}
