using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private VideoPlayer endVP;
    [SerializeField] private GameObject endVideo;
    [SerializeField] private GameObject Atnas;
    [SerializeField] private GameObject dropper;
    [SerializeField] private GameObject AtnasUI;
    [SerializeField] private GameObject FinalText;
    bool isTriggered = false;

    void Update()
    {
        if(Atnas == null)
        {
            if(!isTriggered)
            {
                isTriggered = true;
                dropper.SetActive(false);
                AtnasUI.SetActive(false);
                FinalText.SetActive(true);
                StartCoroutine(WaitforSometime());
                FinalText.SetActive(false);
                Time.timeScale = 0f;
                endVideo.SetActive(true);
                endVP.Play();
                endVP.loopPointReached += OnIntroFinished;
            }
        }
    }
    IEnumerator WaitforSometime()
    {
        yield return new WaitForSeconds(2f);
    }
    void OnIntroFinished(VideoPlayer vp)
    {
        endVP.loopPointReached -= OnIntroFinished;
        endVideo.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }
}
