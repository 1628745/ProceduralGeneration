using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshLoader : MonoBehaviour
{
    public Mesh mesh;

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

        mesh.RecalculateNormals();
        //recalculate normals of surrounding meshes
        /*if (meshDict.ContainsKey(chunkCoords + new Vector2(1, 0)))
        {
            meshDict[chunkCoords + new Vector2(1, 0)].GetComponent<MeshLoader>().mesh.RecalculateTangents();
            Debug.Log("This thing done runned");
        }   
        if (meshDict.ContainsKey(chunkCoords + new Vector2(-1, 0)))
        {
            meshDict[chunkCoords + new Vector2(-1, 0)].GetComponent<MeshLoader>().mesh.RecalculateTangents();
        }
        if (meshDict.ContainsKey(chunkCoords + new Vector2(0, 1)))
        {
            meshDict[chunkCoords + new Vector2(0, 1)].GetComponent<MeshLoader>().mesh.RecalculateTangents();
        }
        if (meshDict.ContainsKey(chunkCoords + new Vector2(0, -1)))
        {
            meshDict[chunkCoords + new Vector2(0, -1)].GetComponent<MeshLoader>().mesh.RecalculateTangents();
        }*/
    }
}
