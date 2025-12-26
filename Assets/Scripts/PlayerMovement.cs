using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckOffset = Vector2.zero;
    [Header("Components")]
    [SerializeField] private GameObject holder;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    private Animator animator;
    float horizontal;
    bool isGrounded = false;
    bool facingLeft = true;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        capsuleCollider = transform.GetComponent<CapsuleCollider2D>();
        animator = transform.GetComponent<Animator>();
    }
    void Update()
    {
        isGrounded = GroundCheck();
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }
    bool GroundCheck()
    {
        Vector2 position = new(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y);
        Collider2D collider = Physics2D.OverlapCircle(position, groundCheckRadius, groundLayer);
        return collider != null;
    }
    void OnDrawGizmos()
    {
        Vector2 position = new(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y);
        if (isGrounded)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, groundCheckRadius);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>().normalized;
        horizontal = inputVector.x;
        if (Mathf.Abs(horizontal) < 0.05f)
        {
            horizontal = 0;
            animator.SetBool("Moving", false);
        }
        else
        {
            animator.SetBool("Moving", true);
        }
        if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
            capsuleCollider.offset = new Vector2(Mathf.Abs(capsuleCollider.offset.x), capsuleCollider.offset.y);
            groundCheckOffset.x = Mathf.Abs(groundCheckOffset.x);
            holder.transform.localPosition = new(-Mathf.Abs(holder.transform.localPosition.x), holder.transform.localPosition.y, holder.transform.localPosition.z);
            holder.transform.localScale = new(-math.abs(holder.transform.localScale.x), holder.transform.localScale.y, holder.transform.localScale.z);
            facingLeft = false;
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
            capsuleCollider.offset = new Vector2(-Mathf.Abs(capsuleCollider.offset.x), capsuleCollider.offset.y);
            groundCheckOffset.x = -Mathf.Abs(groundCheckOffset.x);
            holder.transform.localPosition = new(Mathf.Abs(holder.transform.localPosition.x), holder.transform.localPosition.y, holder.transform.localPosition.z);
            holder.transform.localScale = new(math.abs(holder.transform.localScale.x), holder.transform.localScale.y, holder.transform.localScale.z);
            facingLeft = true;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }
    public bool IsFacingLeft()
    {
        return facingLeft;
    }
}
