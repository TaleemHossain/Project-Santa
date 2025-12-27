using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines.ExtrusionShapes;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Shield;
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
    public void EnableShield()
    {
        Shield.GetComponent<CircleCollider2D>().enabled = true;
    }
    public void DisableShield()
    {
        Shield.GetComponent<CircleCollider2D>().enabled = false;
    }
}
