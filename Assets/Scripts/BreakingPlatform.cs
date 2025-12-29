using UnityEngine;

public class BreakingPlatform: MonoBehaviour
{
    [SerializeField] public float breakDelay = 5f;
    [SerializeField] public float reappearDelay = 5f;
    int index;
    private BPmanager manager;
    void Start()
    {
        manager = transform.gameObject.GetComponentInParent<BPmanager>();
    }
    public void SetIndex(int i)
    {
        index = i;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            manager.SetTime(index);
        }
    }
}
