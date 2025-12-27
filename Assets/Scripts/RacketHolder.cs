using UnityEngine;

public class RacketHolder : MonoBehaviour
{
    PlayerAttack playerAttack;
    void Start()
    {
        playerAttack = transform.GetComponentInParent<PlayerAttack>();
    }
    public void EnableAttack()
    {
        playerAttack.EnableAttack();
    }
    public void DisableAttack()
    {
        playerAttack.DisableAttack();
    }
    public void EnableShield()
    {
        playerAttack.EnableShield();
    }
    public void DisableShield()
    {
        playerAttack.DisableShield();
    }
}
