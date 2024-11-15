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
    public int xSize = 50;
    public int zSize = 50;  
    public Camera cam;
    public MeshGenerator meshGen;
    public int renderDistance = 1;

    private Rigidbody rb;

    private Tuple<float, float> prevCoords;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        //get camera attached to player
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        //Check to see if current coords have passed into a new chunk seeing if prevCoords/500 != currentCoords/500
        if (prevCoords != null)
        {
            if (Math.Floor(prevCoords.Item1 / xSize) != Math.Floor(rb.position.x / xSize) || Math.Floor(prevCoords.Item2 / zSize) != Math.Floor(rb.position.z / zSize))
            {
                Debug.Log("New Chunk");
                Debug.Log(Math.Floor(rb.position.x / xSize) + " " + Math.Floor(rb.position.z / zSize));
                //Load new chunk
                int xCoord = (int)Math.Floor(rb.position.x / xSize);
                int zCoord = (int)Math.Floor(rb.position.z / zSize);
                for (int i = -renderDistance; i <= renderDistance; i++)
                {
                    for (int j = -renderDistance; j <= renderDistance; j++)
                    {
                        meshGen.CreateShape(xCoord + i, zCoord + j);
                    }
                }
            }
        }

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

        rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0, Time.deltaTime*4), rb.velocity.z);

        rb.velocity = new Vector3(move.x * speed * speedMult, rb.velocity.y, move.z * speed * speedMult);

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        cam.transform.Rotate(Vector3.left * mouseY);

        prevCoords = new Tuple<float, float>(rb.position.x, rb.position.z);
    }
}
