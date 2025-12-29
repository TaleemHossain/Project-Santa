using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] float maxHealth = 1000f;
    Animator animator;
    bool isDead = false;
    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }
    public void TakeDamage(float damage)
    {
        maxHealth -= damage;
        if (maxHealth <= 0 && !isDead)
        {
            Debug.Log("Death from Damage");
            isDead = true;
            if(gameObject.CompareTag("Player"))
            {
                // Scene Reload
            }
            if(gameObject.CompareTag("Enemy"))
            {
                if(gameObject.GetComponent<Spawner>() != null)
                    gameObject.GetComponent<Spawner>().Spawn();
            }
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
