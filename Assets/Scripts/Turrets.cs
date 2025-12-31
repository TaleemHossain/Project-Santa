using UnityEngine;

public class Turrets : MonoBehaviour
{
    [SerializeField] GameObject JingleBall;
    [SerializeField] Transform Head;
    [SerializeField] float throwSpeed = 10f;
    [SerializeField] float lastThrown = 0f;
    [SerializeField] float timer = 2.5f;
    [SerializeField] float maxDistance = 10f;
    void Update()
    {
        float distance = (transform.position - Head.position).magnitude;
        if (distance < maxDistance)
        {
            if (lastThrown + timer < Time.time)
            {
                lastThrown = Time.time;
                GameObject jingleBall = Instantiate(JingleBall, transform.position, Quaternion.identity);
                jingleBall.transform.parent = null;
                Vector2 throwDirection = (Head.position - transform.position).normalized;
                throwDirection = throwDirection.normalized;
                Rigidbody2D jingleBallRb = jingleBall.GetComponent<Rigidbody2D>();
                jingleBallRb.linearVelocity = throwDirection * throwSpeed;
                Destroy(jingleBall, 42f);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 throwDirection = (Head.position - transform.position).normalized;
        Gizmos.DrawRay(transform.position, throwDirection * maxDistance);
    }
}
