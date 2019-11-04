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

    // The attached rigidbody of the player
    private Rigidbody rb;


    // Variables to hold movement information
    private bool isTouching = false;
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
        // Keep player above the "GroundLevel" position, to prevent falling endlessly...
        if (transform.position.y < GroundLevel)
        {
            transform.position = new Vector3(rb.position.x, GroundLevel, rb.position.z);
        }
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
            rb.AddForce(moveVector * MoveSpeed, ForceMode.Acceleration);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, TopSpeed);

            if (moveX != 0f || moveZ != 0f)
            {
                var lookRotation = Quaternion.LookRotation(new Vector3(moveX, 0f, moveZ), Vector3.up);
                var rotation = Quaternion.Lerp(rb.transform.rotation, lookRotation, Time.deltaTime * 10);
                rb.transform.rotation = rotation;
            }

            // Apply "JumpPower" as an upward force on the rigidbody
            if (isTouching && Input.GetKeyDown("space"))
            {
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
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
