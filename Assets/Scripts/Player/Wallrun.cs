using Unity.VisualScripting;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    public bool nearWall;
    public bool wallRun;
    private Movement movement;
    private GameObject currWall;
    Vector3 wallPos;
    Vector3 pos;
    [SerializeField] GameObject orientation;
    CameraScript camScript;
    float realRightPos;
    float realLeftPos;
    float realPlayerPos;
    float distanceToRight;
    float distanceToLeft;
    float wallX;
    float e = 2.719f;
    float glide = 2.8f;
    float ratio = 1f;


    private void Start()
    {
        movement = GetComponent<Movement>();
        camScript = orientation.GetComponent<CameraScript>();
    }

    private void Update()
    {
        if (wallRun)
        {
            movement.gravity = 0f;
            pos = transform.position;
            pos.x = wallX;
            pos.y = CalculateWallRun(currWall.GetComponent<BoxCollider>().bounds.size.z, movement.horizontalVelocity.magnitude, (pos.z-pos.z));

            transform.position = pos;
        }else
        {
            movement.gravity = -9.81f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") && !movement.grounded && Mathf.RoundToInt(movement.horizontalVelocity.magnitude) >= 6)
        {
            currWall = other.gameObject;

            wallRun = true;

            float playerX = transform.position.x;
            float wallCenter = currWall.transform.position.x;

            if (playerX < wallCenter)
                wallX = wallCenter - 0.7f;
            else
                wallX = wallCenter + 0.7f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            currWall = null;
            nearWall = false;
        }
    }

    private float CalculateWallRun(float distance, float strength, float currSpot)
    {
        float result = strength * Mathf.Log(e, -currSpot + distance) + glide;
        print(result);
        return result;
    }
}
