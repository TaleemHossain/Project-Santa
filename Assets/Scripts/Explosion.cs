using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float damage = 100f;
    [SerializeField] int ExplosionType = 0; // 0 - does not effect player, 1 - effects player
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(ExplosionType == 0 && collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        HealthBar HealthBar = collision.gameObject.GetComponent<HealthBar>();
        if (HealthBar != null)
        {
            HealthBar.TakeDamage(damage);
        }
    }
    public void DestroyExplosion()
    {
        Destroy(transform.gameObject);
    }
}
