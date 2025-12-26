using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int damage = 100;
    public void Explode()
    {
        HealthBar healthBar = transform.GetComponentInParent<HealthBar>();
        if (healthBar != null)
        {
            healthBar.TakeDamage(damage);
        }
        Destroy(transform.gameObject);
    }
}
