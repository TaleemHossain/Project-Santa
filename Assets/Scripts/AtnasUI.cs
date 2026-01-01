using UnityEngine;
using UnityEngine.UI;

public class AtnasUI : MonoBehaviour
{
    [SerializeField] GameObject Atnas;
    HealthBar health;
    [SerializeField] Image healthFill;
    void Start()
    {
        health = Atnas.GetComponent<HealthBar>();
    }
    void Update()
    {
        healthFill.fillAmount = health.GetCurrentHealth() / health.GetMaxHealth();
    }
}
