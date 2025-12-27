using UnityEngine;

public class Racket : MonoBehaviour
{
    [SerializeField] private float damage = 100f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HealthBar enemyHealthBar = collision.gameObject.GetComponent<HealthBar>();
            if (enemyHealthBar != null)
            {
                enemyHealthBar.TakeDamage(damage);
            }
        }
    }
}
