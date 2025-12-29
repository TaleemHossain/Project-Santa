using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject Gift;
    public bool Spawned = false;
    public void Spawn()
    {
        if(!Spawned) {
            Instantiate(Gift, transform.position, Quaternion.identity);
            Spawned = true;
        }
    }
}
