using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class bossVideoTrigger : MonoBehaviour
{
    [SerializeField] private VideoPlayer bossVP;
    [SerializeField] private GameObject bossVideo;
    [SerializeField] private GameObject dropper;
    [SerializeField] private GameObject AtnasUI;
    CircleCollider2D collider2d;
    bool isTriggered = false;
    void Start()
    {
        collider2d = transform.gameObject.GetComponent<CircleCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isTriggered)
            {
                isTriggered = true;
                Time.timeScale = 0f;
                bossVideo.SetActive(true);
                bossVP.Play();
                bossVP.loopPointReached += OnIntroFinished;
            }
        }
    }
    void OnIntroFinished(VideoPlayer vp)
    {
        bossVP.loopPointReached -= OnIntroFinished;
        bossVideo.SetActive(false);
        Time.timeScale = 1f;
        collider2d.enabled = false;
        StartCoroutine(WaitforSometime());
    }
    IEnumerator WaitforSometime()
    {
        yield return new WaitForSeconds(2f);
        dropper.SetActive(true);
        AtnasUI.SetActive(true);
    }
}
