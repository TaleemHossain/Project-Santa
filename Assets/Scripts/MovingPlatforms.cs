using UnityEngine;
using System.Collections;
public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] Transform[] PatrolPoints;
    public float speed = 2f;
    private int currentWaypointIndex = 0;
    bool waiting;
    public float waitTime = 1f;
    void Start()
    {
        waiting = false;
    }
    void FixedUpdate()
    {
        if (!waiting)
            MovePlatform();
    }

    private void MovePlatform()
    {
        if (PatrolPoints.Length == 0) return;

        Transform target = PatrolPoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % PatrolPoints.Length;
            StartCoroutine(WaitOnWP());
        }
    }
    IEnumerator WaitOnWP()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = transform;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.parent == transform)
        {
            collision.transform.parent = null;
        }
    }
    private void OnDrawGizmos()
    {
        if (PatrolPoints.Length < 2) return;

        for (int i = 0; i < PatrolPoints.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(PatrolPoints[i].position, 0.1f);
            Gizmos.color = Color.yellow;
            if (i < PatrolPoints.Length - 1)
            {
                Gizmos.DrawLine(PatrolPoints[i].position, PatrolPoints[i + 1].position);
            }
            else
            {
                Gizmos.DrawLine(PatrolPoints[i].position, PatrolPoints[0].position);
            }
        }
    }
}
