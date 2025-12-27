using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItems : MonoBehaviour
{
    [SerializeField] int dashGiftCount = 0; // code - 0
    [SerializeField] int jumpGiftCount = 0; // code - 1
    PlayerMovement playerMovement;
    public void Start()
    {
        playerMovement = transform.GetComponent<PlayerMovement>();
    }
    private void AddDashGift()
    {
        dashGiftCount++;
    }
    private void AddJumpGift()
    {
        jumpGiftCount++;
    }
    public void ItemInteraction(int code)
    {
        if (code == 0)
        {
            AddDashGift();
        }
        else if (code == 1)
        {
            AddJumpGift();
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