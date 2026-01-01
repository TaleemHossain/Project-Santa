using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    HealthBar health;
    Image healthBar;

    void Start()
    {
        health = transform.gameObject.GetComponentInParent<HealthBar>();
        healthBar = transform.gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if(health == null || healthBar == null) return;
        healthBar.fillAmount = health.GetCurrentHealth() / health.GetMaxHealth();
    }
}
