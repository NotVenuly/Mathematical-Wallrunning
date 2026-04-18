using UnityEngine;
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float acceleration;

    public float groundFriction;
    public float airFriction;

    public float airMultiplier;

    [Header("Jumping")]
    public float jumpForce;
    public float gravity = -9.81f;
    public float jumpCooldown;

    public bool readyToJump = true;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public float maxSlopeAngle = 45f;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Vector3 velocity;
    Vector3 horizontalVelocity;

    CharacterController controller;
    public bool grounded;
    Wallrun wallrun;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.slopeLimit = maxSlopeAngle;
        wallrun = GetComponent<Wallrun>();
    }

    void Update()
    {
        grounded = controller.isGrounded;

        if (grounded && velocity.y < 0)
            velocity.y = -2f;

        MyInput();
        MovePlayer();
        ApplyGravity();
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump)
        {
            if(grounded || wallrun.nearWall)
            {

                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
    }

    void MovePlayer()
    {
        moveDirection =
            orientation.forward * verticalInput +
            orientation.right * horizontalInput;

        float control = grounded ? 1f : airMultiplier;

        if (moveDirection.magnitude > 0)
        {
            horizontalVelocity +=
                moveDirection.normalized *
                acceleration *
                control *
                Time.deltaTime;
        }
        Vector3 flatVel = new Vector3(
            horizontalVelocity.x,
            0,
            horizontalVelocity.z
        );

        if (flatVel.magnitude > moveSpeed)
        {
            flatVel = flatVel.normalized * moveSpeed;

            horizontalVelocity.x = flatVel.x;
            horizontalVelocity.z = flatVel.z;
        }

        float friction = grounded ? groundFriction : airFriction;

        horizontalVelocity =
            Vector3.Lerp(
                horizontalVelocity,
                Vector3.zero,
                friction * Time.deltaTime
            );

        controller.Move(
            horizontalVelocity * Time.deltaTime
        );
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        controller.Move(
            velocity * Time.deltaTime
        );
    }

    void Jump()
    {
        velocity.y =
            Mathf.Sqrt(
                jumpForce * -2f * gravity
            );
    }

    void ResetJump()
    {
        readyToJump = true;
    }
}