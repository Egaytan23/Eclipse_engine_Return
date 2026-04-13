using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMovement : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatAmount = 0.25f;
    public float rotateSpeed = 45f;

    public Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // Store the starting position
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null) // Check if the object has a parent
        {
            return;
        }

        // Floating effect
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.position = startPos + new Vector3(0, yOffset, 0);

        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime); // Rotate around Y-axis
    }
}
