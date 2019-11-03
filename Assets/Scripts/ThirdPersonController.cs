using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    // Controllers speed of character
    public float TopSpeed = 30;
    public float MoveSpeed = 10;
    public float JumpPower = 10;
    public float GroundLevel = 0;

    // The transform (position and rotation) of the camera
    public Transform Camera;

    // The attached rigidbody of the player
    private Rigidbody rb;


    // Variables to hold movement information
    private bool isTouching = true;
    private float moveX;
    private float moveZ;
    private Vector3 moveVector = new Vector3(0f, 0f, 0f);


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // FixedUpdate is called before the frame, and should be used for physics updates
    void FixedUpdate()
    {
        // Get movement vector based on inputs
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        moveVector = new Vector3(moveX, 0f, moveZ);

        // Execute transforms
        if (rb != null)
        {
            // Keep player above the "GroundLevel" position, to prevent falling endlessly...
            if (rb.position.y < GroundLevel)
            {
                rb.position = new Vector3(rb.position.x, GroundLevel, rb.position.z);
            }

            // If velocity of player is below our "TopSpeed", then apply movement force, otherwise don't
            if (rb.velocity.magnitude < TopSpeed)
            {
                rb.AddForce(moveVector * MoveSpeed, ForceMode.Acceleration);

                if (moveX != 0f || moveZ != 0f)
                {
                    var lookRotation = Quaternion.LookRotation(new Vector3(moveX, 0f, moveZ), Vector3.up);
                    var rotation = Quaternion.Lerp(rb.transform.rotation, lookRotation, Time.deltaTime * 10);
                    rb.transform.rotation = rotation;
                }
            }
            else
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, TopSpeed);
            }

            // Apply "JumpPower" as an upward force on the rigidbody
            if (isTouching && Input.GetKeyDown("space"))
            {
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Acceleration);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isTouching = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isTouching = false;
    }
}
