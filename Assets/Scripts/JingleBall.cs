using UnityEngine;

public class JingleBall : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    Rigidbody2D rb;
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Breakable"))
        {
            Debug.Log("Jingle Ball Collided with " + collision.gameObject.name);
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(transform.gameObject);
        }
    }
}
