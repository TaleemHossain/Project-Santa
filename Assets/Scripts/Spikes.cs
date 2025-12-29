using UnityEngine;

public class Spikes : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        HealthBar health = collision.gameObject.GetComponent<HealthBar>();
        if(health != null)
        {
            health.TakeDamage(9999f);
        }
    }
}
