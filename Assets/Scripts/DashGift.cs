using UnityEngine;

public class DashGift : MonoBehaviour, IItems
{
    public void Collect()
    {
        Destroy(transform.gameObject);
    }
}
