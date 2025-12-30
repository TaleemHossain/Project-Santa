using UnityEngine;

public class Gift : MonoBehaviour
{
    public int giftType; // 0 - Dash, 1 - Jump, 2 - Health
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerItems playerItems = other.gameObject.GetComponent<PlayerItems>();
            if (playerItems != null)
            {
                playerItems.ItemInteraction(giftType);
                Collect();
            }
        }
    }
    public void Collect()
    {
        Destroy(transform.gameObject);
    }
}
