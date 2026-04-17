using Unity.VisualScripting;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    public bool nearWall;
    private Movement movement;
    private GameObject currWall;


    private void Start()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (nearWall)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, currWall.transform.position.x + 0.5f, currWall.transform.position.x +0.5f);
            transform.position = pos;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            currWall = other.gameObject;
            movement.gravity = 0f;
            nearWall = true;
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
