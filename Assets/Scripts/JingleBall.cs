using UnityEngine;

public class JingleBall : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float distance = 25f;
    Rigidbody2D rb;
    AudioManager audioManager;
    GameObject Player;
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        rb = transform.GetComponent<Rigidbody2D>();
        PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
        Player = playerMovement.gameObject;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Breakable"))
        {
            Debug.Log("Jingle Ball Collided with " + collision.gameObject.name);
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            audioManager.PlayBallexp();
            Destroy(transform.gameObject);
        } else
        {
            audioManager.PlayBounce();
        }
    }
    void Update()
    {
        if(Vector2.Distance(transform.position, Player.transform.position) > distance)
        {
            Destroy(transform.gameObject);
        }
    }
}
