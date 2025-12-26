using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    int jumpGiftCount = 0;
    int dashGiftCount = 0;
    [SerializeField] BoxCollider2D collider2d;
    public void AddJumpGift()
    {
        jumpGiftCount++;
    }
    public void AddDashGift()
    {
        dashGiftCount++;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with " + other.name);
        if(other.GetComponent<IItems>() != null)
        {
            IItems item = other.GetComponent<IItems>();
            if(other.GetComponent<JumpGift>() != null)
            {
                AddJumpGift();
            }
            else if(other.GetComponent<DashGift>() != null)
            {
                AddDashGift();
            }
            item.Collect();
        }
    }
}