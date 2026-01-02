using UnityEngine;
using UnityEngine.UI;

public class AtnasUI : MonoBehaviour
{
    [SerializeField] GameObject Atnas;
    HealthBar health;
    [SerializeField] Image healthFill;
    [SerializeField] GameObject Dropper;
    void Start()
    {
        health = Atnas.GetComponent<HealthBar>();
    }
    void Update()
    {
        healthFill.fillAmount = health.GetCurrentHealth() / health.GetMaxHealth();
        if(Atnas.GetComponent<HealthBar>().GetCurrentHealth() == 0)
        {
            Dropper.SetActive(false);
        }
    }
}
