using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItems : MonoBehaviour
{
    [SerializeField] int jumpGiftCount = 0;
    [SerializeField] int dashGiftCount = 0;
    [SerializeField] BoxCollider2D collider2d;
    PlayerMovement playerMovement;
    public void Start()
    {
        playerMovement = transform.GetComponent<PlayerMovement>();
    }
    public void AddJumpGift()
    {
        jumpGiftCount++;
    }
    public void AddDashGift()
    {
        dashGiftCount++;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<IItems>() != null)
        {
            IItems item = other.GetComponent<IItems>();
            if (other.GetComponent<JumpGift>() != null)
            {
                AddJumpGift();
            }
            else if (other.GetComponent<DashGift>() != null)
            {
                AddDashGift();
            }
            item.Collect();
        }
    }
    public void OnJumpGift(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!playerMovement.canUseAbility) return;
            if (jumpGiftCount > 0)
            {
                jumpGiftCount--;
                playerMovement.canUseAbility = false;
                playerMovement.EnterRocketState(0);
            }
        }
        else if (context.canceled)
        {
            playerMovement.ExitRocketState();
        }
    }
    public void OnDashGift(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!playerMovement.canUseAbility) return;
            if (dashGiftCount > 0)
            {
                dashGiftCount--;
                playerMovement.canUseAbility = false;
                playerMovement.EnterRocketState(1);
            }
        }
        else if (context.canceled)
        {
            playerMovement.ExitRocketState();
        }
    }
}