using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBrickController : MonoBehaviour
{
    public Vector3 TargetPos = Vector3.zero;
    public float SmoothTime = 1f;

    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != TargetPos)
        {
            // Do this in polynomial time
            if (velocity.magnitude > 0 && velocity.magnitude < 0.001f)
            {
                transform.position = TargetPos;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, TargetPos, ref velocity, SmoothTime);
            }
        }
    }

    // Set the destination of our object
    public void SetDestination(Vector3 destination, float time)
    {
        TargetPos = destination;
        SmoothTime = time;
    }
}
