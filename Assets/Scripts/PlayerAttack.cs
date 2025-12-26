using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerMovement playerMovement;
    bool canAttack = true;
    void Start()
    {
        playerMovement = transform.GetComponent<PlayerMovement>();  
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(!canAttack) return;
        if(playerMovement.inRocket) return;
        if (context.performed && playerMovement.IsGrounded())
        {
            animator.SetTrigger("GroundAttack");
        } 
        else if (context.performed && !playerMovement.IsGrounded())
        {
            animator.SetTrigger("AirAttack");
        }
    }
    public void EnableAttack()
    {
        canAttack = true;
    }
    public void DisableAttack()
    {
        canAttack = false;
    }
}
