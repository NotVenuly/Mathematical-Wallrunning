using Unity.VisualScripting;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    public bool nearWall;
    public bool wallRun;
    private Movement movement;
    private GameObject currWall;
    Vector3 pos;

    [SerializeField] GameObject orientation;
    [SerializeField] GameObject wall;

    float wallX;
    float currentSpot;
    private float startY;

    private void Start()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            print("wall orientation: " + wall.transform.eulerAngles);
            print("wall size: " + wall.GetComponent<BoxCollider>().bounds.size.z);
            print("wall height: " + wall.GetComponent<BoxCollider>().bounds.size.y);
            print("player orientation: " + orientation.transform.eulerAngles);
            print("angle: " + AngleCheck());
        }
        if (movement.grounded)
        {
            wallRun = false;
        }
        else if (wallRun)
        {
            transform.position = WallRun();
        }
        else
        {
            movement.gravity = -9.81f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            nearWall = true;
            currWall = other.gameObject;

            if (WallRunCheck())
            {
                WallRunPreset();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            currWall = null;
            nearWall = false;
            wallRun = false;
            movement.gravity = -9.81f;
        }
    }

    private float AngleCheck()
    {
        return Mathf.Abs(Mathf.Min(orientation.transform.eulerAngles.y, 180 - orientation.transform.eulerAngles.y));
    }

    public bool WallRunCheck()
    {
        if (!movement.grounded && Mathf.RoundToInt(movement.horizontalVelocity.magnitude) >= 6)
        {
            return true;
        }
        return false;
    }

    public void WallRunPreset()
    {
        wallRun = true;

        float playerX = transform.position.x;
        float wallCenter = currWall.transform.position.x;

        if (playerX < wallCenter)
            wallX = wallCenter - 0.7f;
        else
            wallX = wallCenter + 0.7f;

        currentSpot = 0f;
        startY = transform.position.y;
    }

    private Vector3 WallRun()
    {
        movement.gravity = 0f;

        float distance = currWall.GetComponent<BoxCollider>().bounds.size.z;

        currentSpot += movement.horizontalVelocity.z * Time.deltaTime;

        if (currentSpot >= distance)
        {
            wallRun = false;
            movement.gravity = -9.81f;
            return transform.position;
        }

        pos = transform.position;
        pos.x = wallX;

        float yOffset = CalculateWallRun(movement.horizontalVelocity.magnitude,currentSpot,distance, -20f);

        pos.y = yOffset;

        if (float.IsNaN(pos.y) || float.IsInfinity(pos.y))
        {
            wallRun = false;
            movement.gravity = -9.81f;
            return transform.position;
        }

        return pos;
    }

    private float CalculateWallRun(float strength, float currSpot, float distance, float angle)
    {
        float result = strength * Mathf.Log(-currSpot + distance) + angle; 
        print("formatted2: " + strength + " * ln(-" + currSpot + " + " + distance + ")" + " + " + angle + " = " + result);

        return result;
    }
}