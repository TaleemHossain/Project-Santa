using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItems : MonoBehaviour
{
    [SerializeField] int dashGiftCount = 0; // code - 0
    [SerializeField] int jumpGiftCount = 0; // code - 1
    [SerializeField] float healAmount = 100f;
    private bool NextLevel = false;
    PlayerMovement playerMovement;
    public void Start()
    {
        NextLevel = false;
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
        else if (code == 2)
        {
            transform.gameObject.GetComponent<HealthBar>().Heal(healAmount);
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
    public bool GetNextLevel()
    {
        return NextLevel;
    }
    public void SetNextLevel()
    {
        NextLevel = true;
    }
    public void ResetNextLevel()
    {
        NextLevel = false;
    }
    public int DashRocketCount()
    {
        return dashGiftCount;
    }
    public int JumpRocketCount()
    {
        return jumpGiftCount;
    }
}