using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SnowMan : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 2f;
    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolWaitTime = 1f;
    bool WaitingOnPatrol = false;
    private int nextPatrolIndex;
    [Header("Detection")]
    [SerializeField] private Transform EyePoint;
    [SerializeField] private Transform PlayerHead;
    [SerializeField] private Transform PlayerFeet;
    [SerializeField] private LayerMask IgnoreLayer;
    [SerializeField] private float detectionRange = 10f;
    bool headHit = false;
    bool feetHit = false;
    [Header("Attack")]
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private GameObject JingleBallPrefab;
    [SerializeField] private float ThrowVelocity = 20f;
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
    private int state = 0; // 0 = Patrolling, 1 = Alert
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
        if(!isGrounded)
        {
            HealthBar healthBar = transform.GetComponent<HealthBar>();
            if(healthBar != null)
            {
                healthBar.TakeDamage(9999f);
            }
        }

        Detect();
        if (state == 0)
        {
            Patrol();
        }
        else if (state == 1)
        {
            Alert();
        }
        Debug.Log("Linear Velocity : " + rb.linearVelocity);
    }
    bool GroundCheck()
    {
        Vector2 position = new(groundCheckPosition.transform.position.x, groundCheckPosition.transform.position.y);
        Collider2D collider = Physics2D.OverlapCircle(position, groundCheckRadius, groundLayer);
        return collider != null;
    }
    void Detect()
    {
        Vector2 SnowManforward = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        Vector2 Snowman2Player = PlayerHead.position - EyePoint.position;
        float dotProduct = Vector2.Dot(SnowManforward.normalized, Snowman2Player.normalized);
        if (dotProduct <= 0f) {
            state = 0;
            return;
        }
        Vector2 CastDirectionHead = (PlayerHead.position - EyePoint.position).normalized;
        Vector2 CastDirectionFeet = (PlayerFeet.position - EyePoint.position).normalized;
        RaycastHit2D headHitInfo = Physics2D.Raycast(EyePoint.position, CastDirectionHead, detectionRange, ~IgnoreLayer);
        RaycastHit2D feetHitInfo = Physics2D.Raycast(EyePoint.position, CastDirectionFeet, detectionRange, ~IgnoreLayer);
        headHit = headHitInfo.collider != null && headHitInfo.collider.gameObject.CompareTag("Player");
        feetHit = feetHitInfo.collider != null && feetHitInfo.collider.gameObject.CompareTag("Player");
        if (headHit || feetHit)
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
    void Alert()
    {
        rb.linearVelocity = Vector2.zero;
        moveAmount = 0f;
        Animate();
        if(!isAttacking && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }
    void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");
    }
    public void Throw()
    {
        GameObject jingleBall = Instantiate(JingleBallPrefab, AttackPoint.position, Quaternion.identity);
        jingleBall.transform.parent = null;
        Vector2 throwDirection;
        if (headHit && feetHit)
        {
            throwDirection = (PlayerHead.position + PlayerFeet.position) / 2 - AttackPoint.position;
        }
        else if (headHit)
        {
            throwDirection = PlayerHead.position - AttackPoint.position;
        }
        else if (feetHit)
        {
            throwDirection = PlayerFeet.position - AttackPoint.position;
        }
        else
        {
            throwDirection = Vector2.zero;
        }
        throwDirection = throwDirection.normalized;
        Rigidbody2D jingleBallRb = jingleBall.GetComponent<Rigidbody2D>();
        jingleBallRb.linearVelocity = throwDirection * ThrowVelocity;
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
        // Draw detection rays
        if(headHit) Gizmos.color = Color.green;
        else Gizmos.color = Color.yellow;
        Vector2 CastDirectionHead = (PlayerHead.position - EyePoint.position).normalized;
        Gizmos.DrawRay(EyePoint.position, CastDirectionHead * detectionRange);
        if(feetHit) Gizmos.color = Color.green;
        else Gizmos.color = Color.yellow;
        Vector2 CastDirectionFeet = (PlayerFeet.position - EyePoint.position).normalized;
        Gizmos.DrawRay(EyePoint.position, CastDirectionFeet * detectionRange);
    }
}
