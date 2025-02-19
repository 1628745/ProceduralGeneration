using System;
using System.Collections;
using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool spectating = true;
    public float speed = 5f; //45 seems good
    public float speedMult = 1f; //3 seems good
    public float rotationSpeed = 100f; //500 seems good
    public int xSize = 50;
    public int zSize = 50;  
    public Camera cam;
    public MeshGenerator meshGen;
    public int renderDistance = 1;
    int maxBounces = 5;
    float skinWidth = .015f;
    float maxSlopeAngle = 55f;
    //get layer mask for collision
    int layerMask = 1 << 8;

    private Rigidbody rb;

    private Tuple<float, float> prevCoords;
    private bool coroutineRunning = false;
    //variable to keep track of queued chunks
    private List<Tuple<float, float>> queuedChunks = new List<Tuple<float, float>>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        //get camera attached to player
        cam = GetComponentInChildren<Camera>();
        if (spectating){
            speed *= 9;
            speedMult *= 3;
        }
    }

    void Update()
    {
        if (prevCoords != null)
        {
            if (Math.Floor(prevCoords.Item1 / xSize) != Math.Floor(rb.position.x / xSize) || Math.Floor(prevCoords.Item2 / zSize) != Math.Floor(rb.position.z / zSize))
            {
                queuedChunks.Add(new Tuple<float, float>(rb.position.x, rb.position.z));
            }
        }

        if (!coroutineRunning && queuedChunks.Count > 0)
        {
            coroutineRunning = true;
            StartCoroutine(LoadChunks(queuedChunks[0].Item1, queuedChunks[0].Item2));
            queuedChunks.RemoveAt(0);
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

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        cam.transform.Rotate(Vector3.left * mouseY);

        //Only call if player isn't spectating
        if (!spectating){
            rb.velocity = CollideAndSlide(rb.velocity, rb.position, 0, false, rb.velocity);
        }
        prevCoords = new Tuple<float, float>(rb.position.x, rb.position.z);
    }


    private Vector3 ProjectAndScale(Vector3 vec, Vector3 normal)
    {
        float mag = vec.magnitude;
        vec = Vector3.ProjectOnPlane(vec, normal).normalized;
        vec *= mag;
        return vec;
    }


    private Vector3 CollideAndSlide(Vector3 vel, Vector3 pos, int depth, bool gravityPass, Vector3 velInit)
    {
        if (depth >= maxBounces)
        {
            return Vector3.zero;
        }

        float dist = vel.magnitude + skinWidth;
        Bounds bounds = GetComponent<Collider>().bounds;

        RaycastHit hit;

        if (Physics.SphereCast(pos, bounds.extents.x, vel.normalized, out hit, dist, layerMask)){
            Vector3 snapToSurface = vel.normalized * (hit.distance - skinWidth);
            Vector3 leftover = vel - snapToSurface;
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if (snapToSurface.magnitude <= skinWidth)
            {
                snapToSurface = Vector3.zero;
            }

            if(angle <= maxSlopeAngle)
            {
                if (gravityPass)
                {
                    return snapToSurface;
                }

                //He moved into separate function
                leftover = ProjectAndScale(leftover, hit.normal);
            }
            else
            {
                float scale = 1 - Vector3.Dot(
                    new Vector3(hit.normal.x, 0, hit.normal.z).normalized,
                    new Vector3(velInit.x, 0, velInit.z).normalized
                );

                /*if(isGrounded && !gravityPass)
                {
                    leftover = ProjectAndScale(
                        new Vector3(leftover.x, 0, leftover.z),
                        new Vector3(hit.normal.x, 0, hit.normal.z)
                    ).normalized;
                    leftover *= scale;
                }
                else{*/
                    leftover = ProjectAndScale(leftover, hit.normal) * scale;
                //}
            }//Not finished
            return snapToSurface + CollideAndSlide(leftover, pos + snapToSurface, depth + 1, gravityPass, velInit);
        }

        return vel;
    }

    //coroutine to load in chunks
    IEnumerator LoadChunks(float xVal, float zVal){
        Debug.Log("New Chunk");
        Debug.Log(Math.Floor(xVal / xSize) + " " + Math.Floor(zVal / zSize));
        //Load new chunk
        int xCoord = (int)Math.Floor(xVal / xSize);
        int zCoord = (int)Math.Floor(zVal / zSize);
        for (int i = -renderDistance; i <= renderDistance; i++)
        {
            for (int j = -renderDistance; j <= renderDistance; j++)
            {
                meshGen.CreateShape(xCoord + i, zCoord + j);
                yield return null;
            }
        }
        coroutineRunning = false;
    }
}
