using UnityEngine;

public class RacketCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerAttack>().GetRacket();
            Destroy(gameObject);
        }
    }
}
