using UnityEngine;

public class BPmanager : MonoBehaviour
{
    GameObject[] platforms;
    bool[] active;
    float[] breakDelay;
    float[] reappearDelay;
    float[] playerLandTime;
    float[] deactivatedTime;
    int n;
    AudioManager audioManager;
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        n = transform.childCount;
        platforms = new GameObject[n];
        active = new bool[n];
        breakDelay = new float[n];
        reappearDelay = new float[n];
        playerLandTime = new float[n];
        deactivatedTime = new float[n];
        for(int i = 0; i < n; i++)
        {
            platforms[i] = transform.GetChild(i).gameObject;
            active[i] = true;
            breakDelay[i] = platforms[i].GetComponent<BreakingPlatform>().breakDelay;
            reappearDelay[i] = platforms[i].GetComponent<BreakingPlatform>().reappearDelay;
            playerLandTime[i] = -1f;
            deactivatedTime[i] = -1f;
            platforms[i].GetComponent<BreakingPlatform>().SetIndex(i);
        }
    }
    void LateUpdate()
    {
        Deactivator();
        Activator();
    }
    void Deactivator()
    {
        for(int i = 0; i < n; i++)
        {
            if(!active[i]) continue;
            if(playerLandTime[i] == -1f) continue;
            if(playerLandTime[i] + breakDelay[i] < Time.time)
            {
                playerLandTime[i] = -1f;
                deactivatedTime[i] = Time.time;
                active[i] = false;
                platforms[i].SetActive(false);
                audioManager.PlayBreaking();
            }
        }
    }
    void Activator()
    {
        for(int i = 0; i < n; i++)
        {
            if(active[i]) continue;
            if(deactivatedTime[i] == -1f) {
                deactivatedTime[i] = Time.time;
            }
            if(deactivatedTime[i] + reappearDelay[i] < Time.time)
            {
                playerLandTime[i] = -1f;
                deactivatedTime[i] = -1f;
                active[i] = true;
                platforms[i].SetActive(true);
            }
        }
    }
    public void SetTime(int i)
    {
        if(playerLandTime[i] == -1f)
        {
            playerLandTime[i] = Time.time;
        }
    }
}
