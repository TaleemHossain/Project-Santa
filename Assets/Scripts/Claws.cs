using UnityEngine;

public class Claws : MonoBehaviour
{
    [SerializeField] private float damage = 100f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthBar healthBar = collision.gameObject.GetComponent<HealthBar>();
            if (healthBar != null)
            {
                healthBar.TakeDamage(damage);
            }
        }
    }
}
