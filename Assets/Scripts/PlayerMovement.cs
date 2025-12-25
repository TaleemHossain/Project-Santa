using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    [SerializeField] private float speed = 5f;
    float horizontal;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        capsuleCollider = transform.GetComponent<CapsuleCollider2D>();
    }
    void Update()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>().normalized;
        horizontal = inputVector.x;
        if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
            capsuleCollider.offset = new Vector2(Mathf.Abs(capsuleCollider.offset.x), capsuleCollider.offset.y);
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
            capsuleCollider.offset = new Vector2(-Mathf.Abs(capsuleCollider.offset.x), capsuleCollider.offset.y);
        }
    }
}
