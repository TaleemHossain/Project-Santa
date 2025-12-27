using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] public Vector3 offset;
    void Start()
    {
        FollowPlayer();
    }
    void Update()
    {
        FollowPlayer();
    }
    void FollowPlayer()
    {
        transform.position = Player.transform.position + offset;
    }
}
