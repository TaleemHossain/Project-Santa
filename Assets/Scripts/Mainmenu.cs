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
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadLevel(int i)
    {
        if (isPaused)
        {
            PauseMenu();
        }
        SceneManager.LoadSceneAsync(i);
    }
    public void PlayLevel1WithIntro()
    {
        foreach (Button b in levels)
            b.interactable = false;

        videoCanvas.SetActive(true);
        videoPlayer.Play();

        videoPlayer.loopPointReached += OnIntroFinished;
    }
    void OnIntroFinished(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnIntroFinished;

        videoCanvas.SetActive(false);

        SceneManager.LoadSceneAsync(2);
    }

    public void ReloadLevel()
    {
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
