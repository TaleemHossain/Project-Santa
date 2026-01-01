using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public AudioSource bgm;
    [SerializeField] AudioSource sfx;
    public AudioClip menu;
    public AudioClip forest;
    public AudioClip daybreak;
    public AudioClip factory;
    public AudioClip explosion;
    public AudioClip ballExplosion;
    public AudioClip racketSwing;
    public AudioClip death;
    public AudioClip bounce;
    public AudioClip alert;
    public AudioClip hurt;
    public AudioClip jump;
    public AudioClip click;
    public AudioClip breaking;
    public AudioClip rocket;
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1)
        {
            bgm.clip = menu;
        } 
        else if(SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            bgm.clip = forest;
        } 
        else if(SceneManager.GetActiveScene().buildIndex == 4 || SceneManager.GetActiveScene().buildIndex == 5)
        {
            bgm.clip = daybreak;
        }
        else
        {
            bgm.clip = factory;
        }
        bgm.Play();
    }
    public void PlayExplosion()
    {
        sfx.PlayOneShot(explosion);
    }
    public void PlayBallexp()
    {
        sfx.PlayOneShot(ballExplosion);
    }
    public void PlayRacket()
    {
        sfx.PlayOneShot(racketSwing);
    }
    public void PlayDeath()
    {
        sfx.PlayOneShot(death);
    }
    public void PlayBounce()
    {
        sfx.PlayOneShot(bounce);
    }
    public void PlayAlert()
    {
        sfx.PlayOneShot(alert);
    }
    public void PlayHurt()
    {
        sfx.PlayOneShot(hurt);
    }
    public void PlayJump()
    {
        sfx.PlayOneShot(jump);
    }
    public void PlayClick()
    {
        sfx.PlayOneShot(click);
    }
    public void PlayBreaking()
    {
        sfx.PlayOneShot(breaking);
    }
    public void PlayRocket()
    {
        sfx.PlayOneShot(rocket);
    }
}
