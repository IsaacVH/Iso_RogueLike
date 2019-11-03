using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public Vector3 offset = new Vector3(0f, 1f, -0.5f);

    // Start is called before the first frame update
    void Start()
    {
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offsest distance.
        transform.position = player.transform.position + offset;
    }
}
