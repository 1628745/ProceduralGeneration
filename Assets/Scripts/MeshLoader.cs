using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshLoader : MonoBehaviour
{
    public Mesh mesh;

    public Vector3[] vertices;
    public int[] triangles;
    void Start()
    {
    }

    void Update() {
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        //gameObject.AddComponent<MeshCollider>();
    }
}
