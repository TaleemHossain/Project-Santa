using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Shield;
    [SerializeField] private bool hasRacket;
    private PlayerMovement playerMovement;
    AudioManager audioManager;
    bool canAttack;
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        playerMovement = transform.GetComponent<PlayerMovement>();
        if(hasRacket) canAttack = true;
        else canAttack = false;
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(!canAttack) return;
        if(playerMovement.inRocket) return;
        audioManager.PlayRacket();
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
        Shield.GetComponent<PolygonCollider2D>().enabled = true;
    }
    public void DisableShield()
    {
        Shield.GetComponent<PolygonCollider2D>().enabled = false;
    }
    public void GetRacket()
    {
        hasRacket = true;
        canAttack = true;
    }
    public bool HasRacket()
    {
        return hasRacket;
    }
}
