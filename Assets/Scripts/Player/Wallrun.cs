using Unity.VisualScripting;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    public bool nearWall;
    private Movement movement;
    private GameObject currWall;
    Vector3 wallPos;
    Vector3 pos;
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
    }

    private void Update()
    {
        if (nearWall && !movement.grounded && Mathf.RoundToInt(movement.horizontalVelocity.magnitude)>=9)
        {
            movement.gravity = 0f;
            pos = transform.position;
            pos.x = wallX;
            pos.y = 0.43f*CalculateWallRun(currWall.GetComponent<BoxCollider>().size.z-1, movement.horizontalVelocity.magnitude, (pos.z-pos.z));

            transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            currWall = other.gameObject;
            
            nearWall = true;

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
            movement.gravity = -9.81f;
            nearWall = false;
        }
    }

    private float CalculateWallRun(float distance, float strength, float currSpot)
    {
        return strength * Mathf.Log(e, -currSpot + distance) + glide;
    }
}
