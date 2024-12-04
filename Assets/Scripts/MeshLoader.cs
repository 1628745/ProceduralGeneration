using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshLoader : MonoBehaviour
{
    public Mesh mesh;
    public Material grass;
    public Material rock;

    [HideInInspector]
    public Vector2[] uvs;

    [HideInInspector]
    public Vector3[] vertices;
    [HideInInspector]
    public int[] triangles;
    [HideInInspector]
    public Dictionary<Vector2, GameObject> meshDict;
    public Vector2 chunkCoords;

    public void SetDict( Dictionary<Vector2, GameObject> dict, Vector2 coords)
    {
        meshDict = dict;
        chunkCoords = coords;
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;


        //Set mesh material based on height
        if (vertices[0].y > 250)
        {
            GetComponent<MeshRenderer>().material = rock;
        }
        else
        {
            GetComponent<MeshRenderer>().material = grass;
        }
        mesh.RecalculateNormals();
    }
}
