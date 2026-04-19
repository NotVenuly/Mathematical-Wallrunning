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


    private void Start()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (nearWall && !movement.grounded && Mathf.RoundToInt(movement.horizontalVelocity.magnitude)>=7)
        {
            movement.gravity = 0f;
            pos = transform.position;
            pos.x = wallX;

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
}
