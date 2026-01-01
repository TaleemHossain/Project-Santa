using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float normalGravityScale = 5f;
    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckOffset = Vector2.zero;
    [Header("Components")]
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject Shield;
    [SerializeField] private GameObject RespawnPoint;
    [Header("Rockets")]
    [SerializeField] public GameObject jumpRocket;
    [SerializeField] public GameObject dashRocket;
    [SerializeField] private float rocketSpeed = 10f;
    [SerializeField] private float rocketSteerSpeed = 5f;
    [SerializeField] private float miniUpLift = 0.1f;
    private Vector2 RocketDir = Vector2.zero;
    private GameObject Rocket = null;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    private Animator animator;
    float horizontal;
    float vertical;
    bool isGrounded = false;
    bool facingLeft = true;
    bool icePlatforms = false;
    public bool inRocket = false;
    public bool canUseAbility = true;
    AudioManager audioManager;
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        capsuleCollider = transform.GetComponent<CapsuleCollider2D>();
        animator = transform.GetComponent<Animator>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }
    void Update()
    {
        if (inRocket) return;
        isGrounded = GroundCheck();
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }
    void LateUpdate()
    {
        if(!icePlatforms) return;
        rb.linearVelocity = new(2f * rb.linearVelocity.x, rb.linearVelocity.y);
        if(rb.linearVelocity == Vector2.zero)
        {
            if(facingLeft) rb.linearVelocity = new(0.5f * speed, 0f);
            else rb.linearVelocity = new(-0.5f * speed, 0f);
        }
    }
    void FixedUpdate()
    {
        if (!inRocket) return;
        if (RocketDir.Equals(new(0f, 1f)))
        {
            float steerInput = horizontal;
            rb.linearVelocity = new(steerInput * rocketSteerSpeed, RocketDir.y * rocketSpeed);
        }
        else if (RocketDir.Equals(new(-1f, 0f)) || RocketDir.Equals(new(1f, 0f)))
        {
            float steerInput = vertical;
            rb.linearVelocity = new(RocketDir.x * rocketSpeed, steerInput * rocketSteerSpeed);
        }
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
        vertical = inputVector.y;

        if (Mathf.Abs(horizontal) < 0.05f)
        {
            horizontal = 0;
            animator.SetBool("Moving", false);
        }
        else
        {
            if (!inRocket)
            {
                animator.SetBool("Moving", true);
            }
        }
        if (inRocket) animator.SetBool("Moving", false);

        if (Mathf.Abs(vertical) < 0.05f)
        {
            vertical = 0;
        }

        if (RocketDir.Equals(new(-1f, 0f)) || RocketDir.Equals(new(1f, 0f)))
        {
            horizontal = 0;
        }
        else if (RocketDir.Equals(new(0f, 1f)))
        {
            vertical = 0;
        }
        FlipLogic();
    }
    private void FlipLogic()
    {
        if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
            capsuleCollider.offset = new Vector2(Mathf.Abs(capsuleCollider.offset.x), capsuleCollider.offset.y);
            groundCheckOffset.x = Mathf.Abs(groundCheckOffset.x);
            holder.transform.localPosition = new(-Mathf.Abs(holder.transform.localPosition.x), holder.transform.localPosition.y, holder.transform.localPosition.z);
            holder.transform.localScale = new(-math.abs(holder.transform.localScale.x), holder.transform.localScale.y, holder.transform.localScale.z);
            Shield.GetComponent<Shield>().offset = new Vector3(Shield.GetComponent<Shield>().offset1.x, Shield.GetComponent<Shield>().offset1.y, Shield.GetComponent<Shield>().offset1.z);
            Shield.transform.localScale = new(-math.abs(Shield.transform.localScale.x), Shield.transform.localScale.y, Shield.transform.localScale.z);
            facingLeft = false;
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
            capsuleCollider.offset = new Vector2(-Mathf.Abs(capsuleCollider.offset.x), capsuleCollider.offset.y);
            groundCheckOffset.x = -Mathf.Abs(groundCheckOffset.x);
            holder.transform.localPosition = new(Mathf.Abs(holder.transform.localPosition.x), holder.transform.localPosition.y, holder.transform.localPosition.z);
            holder.transform.localScale = new(math.abs(holder.transform.localScale.x), holder.transform.localScale.y, holder.transform.localScale.z);
            Shield.GetComponent<Shield>().offset = new Vector3(Shield.GetComponent<Shield>().offset2.x, Shield.GetComponent<Shield>().offset2.y, Shield.GetComponent<Shield>().offset2.z);
            Shield.transform.localScale = new(math.abs(Shield.transform.localScale.x), Shield.transform.localScale.y, Shield.transform.localScale.z);
            facingLeft = true;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (inRocket) return;
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
            audioManager.PlayJump();
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
    public void EnterRocketState(int code)
    {
        if (inRocket) return;
        inRocket = true;
        audioManager.PlayRocket();
        if (code == 0) Rocket = jumpRocket;
        else if (code == 1)
        {
            Rocket = dashRocket;
            rb.transform.position = new Vector2(rb.transform.position.x, rb.transform.position.y + miniUpLift);
        }
        Rocket.SetActive(true);
        rb.gravityScale = 0;
        if (code == 0)
        {
            RocketDir = new(0f, 1f);
        }
        else if (code == 1)
        {
            if (!facingLeft) RocketDir = new(-1f, 0f);
            else RocketDir = new(1f, 0f);
        }
    }
    public void ExitRocketState()
    {
        if (!inRocket) return;
        Rocket.SetActive(false);
        canUseAbility = true;
        inRocket = false;
        rb.gravityScale = normalGravityScale;
        RocketDir = Vector2.zero;
        Rocket = null;
    }
    public void Respawn()
    {
        transform.position = RespawnPoint.transform.position;
    }
    public void EnterIcePlatform()
    {
        icePlatforms = true;
    }
    public void ExitIcePlatform()
    {
        icePlatforms = false;
    }
    public void StopPlayer()
    {
        rb.linearVelocity = Vector2.zero;
    }
}