using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] int maxHealth = 1000;
    public void TakeDamage(int damage)
    {
        maxHealth -= damage;
        if (maxHealth <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
