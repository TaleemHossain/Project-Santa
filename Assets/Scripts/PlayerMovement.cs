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
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    float horizontal;
    bool isGrounded = false;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        capsuleCollider = transform.GetComponent<CapsuleCollider2D>();
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
        if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
            capsuleCollider.offset = new Vector2(Mathf.Abs(capsuleCollider.offset.x), capsuleCollider.offset.y);
            groundCheckOffset.x = Mathf.Abs(groundCheckOffset.x);
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
            capsuleCollider.offset = new Vector2(-Mathf.Abs(capsuleCollider.offset.x), capsuleCollider.offset.y);
            groundCheckOffset.x = -Mathf.Abs(groundCheckOffset.x);
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
