using UnityEngine;

public class JumpGift : MonoBehaviour, IItems
{
    public void Collect()
    {
        Destroy(transform.gameObject);
    }
}
