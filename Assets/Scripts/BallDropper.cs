using UnityEngine;

public class BallDropper : MonoBehaviour
{
    [SerializeField] GameObject Jingleball;
    [SerializeField] Transform throwPos;
    GameObject currentJB = null;
    [SerializeField] Transform feet;
    [SerializeField] float throwSpeed;
    void Update()
    {
        if (currentJB == null)
        {
            currentJB = Instantiate(Jingleball, throwPos.position, Quaternion.identity);
            currentJB.transform.parent = null;
            Vector2 throwDirection = (feet.position - transform.position).normalized;
            throwDirection = throwDirection.normalized;
            Rigidbody2D jingleBallRb = currentJB.GetComponent<Rigidbody2D>();
            jingleBallRb.linearVelocity = throwDirection * throwSpeed;
        }
    }
}
