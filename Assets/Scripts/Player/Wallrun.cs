using UnityEngine;

public class Wallrun : MonoBehaviour
{
    public bool nearWall;
    private Movement movement;


    private void Start()
    {
        movement = GetComponent<Movement>();
    }
    private void Update()
    {
        if (nearWall)
        {
            movement.gravity =0f;
        }
        else
        {
            movement.gravity = -9.81f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print("AOISJFOAIJsf");
        if (other.CompareTag("Wall"))
        {
            nearWall = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            nearWall = false;
        }
    }
}
