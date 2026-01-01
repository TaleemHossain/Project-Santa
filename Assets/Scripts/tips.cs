using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class tips : MonoBehaviour
{
    public string text;
    [SerializeField] GameObject tipUI;
    [SerializeField] TextMeshProUGUI tip;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            tip.text = text;
            tipUI.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            tip.text = "";
            tipUI.SetActive(false);
        }
    }
}
