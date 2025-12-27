using UnityEngine;
using System.Collections;
using System;
public class Yeti : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float chaseSpeed = 4f;
    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolWaitTime = 2f;
    bool WaitingOnPatrol = false;
    private int nextPatrolIndex;
    [Header("Detection")]
    [SerializeField] private Transform DetectionPoint;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float BoxCastSize = 0.5f;
    [SerializeField] private LayerMask PlayerLayer;
    [Header("Attack")]
    [SerializeField] private float maxApproachDistance = 0.5f;
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 2f;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject groundCheckPosition;
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
    bool GroundCheck()
    {
        Vector2 position = new(groundCheckPosition.transform.position.x, groundCheckPosition.transform.position.y);
        Collider2D collider = Physics2D.OverlapCircle(position, groundCheckRadius, groundLayer);
        return collider != null;
    }
    void Detect()
    {
        Vector2 CastDirection = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        RaycastHit2D hitInfo = Physics2D.BoxCast(DetectionPoint.position, new Vector2(BoxCastSize, BoxCastSize), 0f, CastDirection, detectionRange, PlayerLayer);

        if (hitInfo.collider != null)
        {
            state = 1;
        }
        else
        {
            state = 0;
        }
    }
    void Patrol()
    {
        Transform targetPoint = patrolPoints[nextPatrolIndex];
        Vector2 direction = targetPoint.position - transform.position;
        direction = new(Mathf.Clamp(direction.x, -1f, 1f), 0f);
        if(Mathf.Abs(direction.x) > 0.1f)
            direction = direction.normalized;
        else 
            direction = Vector2.zero;
        moveAmount = direction.x;
        Animate();
        rb.linearVelocity = direction * patrolSpeed;

        if (Mathf.Abs(direction.x) < 0.1f)
        {
            if (!WaitingOnPatrol)
                StartCoroutine(WaitAtPatrolPoint());
        }
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 direction = player.transform.position - transform.position;
        direction = new(Mathf.Clamp(direction.x, -1f, 1f), 0);
        moveAmount = direction.x;
        // Debug.Log("direction.x: " + direction.x);
        Animate();
        if (!isGrounded || isAttacking || Mathf.Abs(direction.x) < maxApproachDistance)
        {
            // Debug.Log("Yeti Stopped");
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            // Debug.Log("Yeti Chasing");
            rb.linearVelocity = direction * chaseSpeed;
        }
        RaycastHit2D hitInfo = Physics2D.BoxCast(AttackPoint.position, new Vector2(BoxCastSize, BoxCastSize), 0f, direction, attackRange, PlayerLayer);
        if (hitInfo.collider != null && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }
    void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;
        lastAttackTime = Time.time;
        moveAmount = 0f;
        Animate();
        animator.SetTrigger("Attack");
    }
    public void FinishAttack()
    {
        isAttacking = false;
    }
    public void Stop()
    {
        rb.linearVelocity = Vector2.zero;
    }
    void Animate()
    {
        if (Mathf.Abs(moveAmount) > 0.05f)
        {
            animator.SetBool("isMoving", true);
            if (moveAmount > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    // Gizmos
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
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.blue;
                    }
                    Gizmos.DrawSphere(patrolPoints[i].position, 0.1f);
                    if (i > 0 && patrolPoints[i - 1] != null)
                    {
                        if(state == 1)
                            Gizmos.color = Color.white;
                        else
                            Gizmos.color = Color.green;
                        Gizmos.DrawLine(patrolPoints[i - 1].position, patrolPoints[i].position);
                    }
                }
            }
            // Connect last point to first
            if (patrolPoints.Length > 1 && patrolPoints[0] != null && patrolPoints[^1] != null)
            {
                Gizmos.DrawLine(patrolPoints[^1].position, patrolPoints[0].position);
            }
        }
        Vector2 position = new(groundCheckPosition.transform.position.x, groundCheckPosition.transform.position.y);
        if (isGrounded)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, groundCheckRadius);
        // Detection boxcast gizmo
        Vector2 castDirection = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        DrawBoxCastGizmo(DetectionPoint.position, new Vector2(BoxCastSize, BoxCastSize), castDirection, detectionRange, Color.yellow);
        // Attack range gizmo
        DrawBoxCastGizmo(AttackPoint.position, new Vector2(BoxCastSize, BoxCastSize), castDirection, attackRange, Color.magenta);
    }
}
