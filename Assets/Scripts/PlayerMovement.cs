using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Create a player movement script that uses WASD to move the player around the scene.
    //The mouse should be used to rotate the player.
    //The player should be able to jump with the space bar.
    //The player has a rigidbody component attached to it.

    public float speed = 5f; //45 seems good
    public float speedMult = 1f; //3 seems good
    public float rotationSpeed = 100f; //500 seems good
    public Camera cam;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        //get camera attached to player
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, speed, rb.velocity.z);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity = new Vector3(rb.velocity.x, -speed, rb.velocity.z);
        }

        rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0, Time.deltaTime), rb.velocity.z);

        rb.velocity = new Vector3(move.x * speed * speedMult, rb.velocity.y, move.z * speed * speedMult);

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        cam.transform.Rotate(Vector3.left * mouseY);
    }
}
