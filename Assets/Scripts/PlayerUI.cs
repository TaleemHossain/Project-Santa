using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject Santa;
    HealthBar health;
    PlayerAttack playerAttack;
    PlayerItems playerItems;
    [SerializeField] Image healthFill;
    [SerializeField] Image Racket;
    [SerializeField] TextMeshProUGUI DashRocket;
    [SerializeField] TextMeshProUGUI JumpRocket;
    void Start()
    {
        health = Santa.GetComponent<HealthBar>();
        playerAttack = Santa.GetComponent<PlayerAttack>();
        playerItems = Santa.GetComponent<PlayerItems>();
    }
    void Update()
    {
        healthFill.fillAmount = health.GetCurrentHealth() / health.GetMaxHealth();
        Racket.enabled = playerAttack.HasRacket();
        DashRocket.text = "x " + playerItems.DashRocketCount();
        JumpRocket.text = "x " + playerItems.JumpRocketCount();
    }
}
