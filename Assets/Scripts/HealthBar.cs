using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] float maxHealth = 1000f;
    float currentHealth;
    Animator animator;
    bool isDead = false;
    void Start()
    {
        currentHealth = maxHealth;
        animator = transform.GetComponent<Animator>();
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth) currentHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead)
        {
            Debug.Log("Death from Damage");
            isDead = true;
            if(gameObject.CompareTag("Player"))
            {
                // Scene Reload
            }
            if(gameObject.GetComponent<Spawner>() != null)
                gameObject.GetComponent<Spawner>().Spawn();
            if(animator != null)
            {
                animator.SetTrigger("Death");
            }
            else
            {
                DeathComplete();
            }
        }
    }
    public void DeathComplete()
    {
        Destroy(transform.gameObject);
    }
}
