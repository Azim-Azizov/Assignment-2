using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeperControl : MonoBehaviour
{
    [SerializeField]
    float speed = 3f;
    [SerializeField]
    float distance = 3f;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Move the keeper in the XY plane using Mathf.Sin function within a specified range
        float movement = Mathf.Sin(Time.time * speed);
        float xPosition = startPosition.x + movement * distance;

        // Update the keeper's position in the XY plane
        transform.position = new Vector3(xPosition, startPosition.y, transform.position.z);
    }
}
