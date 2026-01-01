using UnityEngine;

public class Rocket : MonoBehaviour
{
    CircleCollider2D col;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject CollisionPoint;
    PlayerMovement playerMovement;
    AudioManager audioManager;
    void Start()
    {
        playerMovement = transform.GetComponentInParent<PlayerMovement>();
        col = transform.GetComponent<CircleCollider2D>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player") || !collision.CompareTag("Shield") || !collision.CompareTag("JingleBall") || !collision.CompareTag("Respawn") || !collision.CompareTag("Racket") || !collision.CompareTag("Explosion"))
        {
            audioManager.PlayExplosion();
            Instantiate(explosionPrefab, CollisionPoint.transform.position, Quaternion.Euler(0f, 0f, 0f));
            playerMovement.ExitRocketState();
        }
    }
}
