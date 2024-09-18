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

    public float speed = 5f;
    public float jumpForce = 10f;
    public float rotationSpeed = 100f;
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

        rb.MovePosition(transform.position + move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        cam.transform.Rotate(Vector3.left * mouseY);
    }
}
