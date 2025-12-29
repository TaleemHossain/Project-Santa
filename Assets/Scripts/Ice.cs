using UnityEngine;

public class Ice : MonoBehaviour
{
    [SerializeField] private float multiplier = 3f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().EnterIcePlatform();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().ExitIcePlatform();
        }
    }
}
