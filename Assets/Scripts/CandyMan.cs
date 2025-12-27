using System.Collections;
using UnityEngine;

public class CandyMan : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 5f;
    [SerializeField] private float chaseSpeed = 10f;
    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolWaitTime = 2f;
    bool WaitingOnPatrol = false;
    private int nextPatrolIndex;
    [Header("Detection")]
    [SerializeField] private Transform DetectionPoint;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float BoxCastSize = 0.5f;
    [Header("Chase")]
    private Vector2 direction = Vector2.zero;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject explosionPosition;
    bool Collided = false;
    bool Exploded = false;
    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckOffset = Vector2.zero;
    bool isGrounded;
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    [Header("Other Settings")]
    private int state = 0; // 0 = Patrolling, 1 = Chasing
    private float moveAmount;

    void Start()
    {
        animator = transform.GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody2D>();
        nextPatrolIndex = 0;
        moveAmount = 0f;
    }

    void Update()
    {
        isGrounded = GroundCheck();
        Detect();
        if (state == 0)
        {
            Patrol();
        }
        else if (state == 1)
        {
            Chase();
        }
    }
    void Detect()
    {
        if (state == 1) return;

        Vector2 CastDirection = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        RaycastHit2D hitInfo = Physics2D.BoxCast(DetectionPoint.position, new Vector2(BoxCastSize, BoxCastSize), 0f, CastDirection, detectionRange);
        
        if (hitInfo.collider != null && hitInfo.collider.gameObject.CompareTag("Player"))
        {
            state = 1;
        }
    }
    void Patrol()
    {
        Transform targetPoint = patrolPoints[nextPatrolIndex];
        Vector2 direction = targetPoint.position - transform.position;
        direction = new(Mathf.Clamp(direction.x, -1f, 1f), 0f);
        moveAmount = direction.x;
        Animate();
        rb.linearVelocity = direction * patrolSpeed;

        if (Mathf.Abs(direction.x) < 0.1f)
        {
            if (!WaitingOnPatrol)
                StartCoroutine(WaitAtPatrolPoint());
        }
    }
    void DrawBoxCastGizmo(Vector2 origin, Vector2 size, Vector2 direction, float distance, Color color)
    {
        Gizmos.color = color;

        Vector2 half = size * 0.5f;

        Vector2 startCenter = origin;
        Vector2 endCenter = origin + direction.normalized * distance;

        // Start box
        Gizmos.DrawWireCube(startCenter, size);

        // End box
        Gizmos.DrawWireCube(endCenter, size);

        // Connect corners (sweep)
        Vector2[] startCorners = new Vector2[]
        {
        startCenter + new Vector2(-half.x, -half.y),
        startCenter + new Vector2(-half.x,  half.y),
        startCenter + new Vector2( half.x,  half.y),
        startCenter + new Vector2( half.x, -half.y),
        };

        Vector2[] endCorners = new Vector2[]
        {
        endCenter + new Vector2(-half.x, -half.y),
        endCenter + new Vector2(-half.x,  half.y),
        endCenter + new Vector2( half.x,  half.y),
        endCenter + new Vector2( half.x, -half.y),
        };

        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(startCorners[i], endCorners[i]);
        }
    }
    void OnDrawGizmos()
    {
        // Draw patrol points and lines between them
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    if (i == nextPatrolIndex)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                    }
                    Gizmos.DrawSphere(patrolPoints[i].position, 0.1f);
                    if (i > 0 && patrolPoints[i - 1] != null)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(patrolPoints[i - 1].position, patrolPoints[i].position);
                    }
                }
            }
            // Connect last point to first
            if (patrolPoints.Length > 1 && patrolPoints[0] != null && patrolPoints[patrolPoints.Length - 1] != null)
            {
                Gizmos.DrawLine(patrolPoints[patrolPoints.Length - 1].position, patrolPoints[0].position);
            }
        }
        Vector2 position = new(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y);
        if (isGrounded)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, groundCheckRadius);

        Vector2 castDirection = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        DrawBoxCastGizmo(DetectionPoint.position, new Vector2(BoxCastSize, BoxCastSize), castDirection, detectionRange, Color.yellow);
    }
    IEnumerator WaitAtPatrolPoint()
    {
        WaitingOnPatrol = true;
        rb.linearVelocity = Vector2.zero;
        moveAmount = 0f;
        Animate();
        yield return new WaitForSeconds(patrolWaitTime);
        WaitingOnPatrol = false;
        nextPatrolIndex = (nextPatrolIndex + 1) % patrolPoints.Length;
    }
    void Chase()
    {
        if (direction == Vector2.zero)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            direction = (player.transform.position - transform.position).normalized;
            direction = new(direction.x, 0);
            moveAmount = direction.x;
            Animate();
        }
        rb.linearVelocity = direction * chaseSpeed;
        if ((!isGrounded || Collided) && !Exploded)
        {
            Explode();
        }
    }
    void Explode()
    {
        Debug.Log("CandyMan Exploded");
        Instantiate(explosionPrefab, explosionPosition.transform.position, Quaternion.Euler(0f, 0f, 0f));
        animator.SetTrigger("Death");
        Exploded = true;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (state != 1) return;
        if (Collided) return;
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint2D contact = collision.GetContact(i);
            bool isGround = contact.otherCollider.CompareTag("Ground");

            if (!isGround)
            {
                Collided = true;
                break;
            }
        }
    }
    bool GroundCheck()
    {
        Vector2 position = new(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y);
        Collider2D collider = Physics2D.OverlapCircle(position, groundCheckRadius, groundLayer);
        return collider != null;
    }
    void Animate()
    {
        if (Mathf.Abs(moveAmount) > 0.05f)
        {
            if (state == 0)
            {
                animator.SetBool("isMoving", true);
                animator.SetBool("isChasing", false);
            }
            else if (state == 1)
            {
                animator.SetBool("isChasing", true);
                animator.SetBool("isMoving", false);
            }
            if (moveAmount > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isChasing", false);
        }
    }
    public void Stop()
    {
        rb.linearVelocity = Vector2.zero;
    }
}
