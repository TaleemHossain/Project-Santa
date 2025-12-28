using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    Transform cam;
    Vector2 camStartPos;
    float distance;
    GameObject[] bgs;
    Material[] mat;
    float[] backSpeed;
    [Range(0f, 0.5f)] 
    public float parallaxSpeed;
    public float yOffset;
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        bgs = new GameObject[backCount];

        for(int i = 0; i < backCount; i++)
        {
            bgs[i] = transform.GetChild(i).gameObject;
            mat[i] = bgs[i].GetComponent<Renderer>().material;
        }
        BackSpeedCalculator(backCount);
    }
    void BackSpeedCalculator(int backCount)
    {
        float farthestBack = 0;
        for(int i = 0; i < backCount; i++)
        {
            if(bgs[i].transform.position.z - cam.position.z > farthestBack)
            {
                farthestBack = bgs[i].transform.position.z - cam.position.z;
            }
        }
        for(int i = 0; i < backCount; i++)
        {
            backSpeed[i] = 1 - (bgs[i].transform.position.z - cam.position.z) / farthestBack;
        }
    }
    private void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;
        transform.position = new(cam.position.x, cam.position.y + yOffset, transform.position.z);
        for(int i = 0; i < bgs.Length; i++) {
            float speed = backSpeed[i] * parallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
