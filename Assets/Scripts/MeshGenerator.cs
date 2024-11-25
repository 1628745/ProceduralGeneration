using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Vector3[] vertices;
    Vector2[] uvs;
    int[] triangles;
    //Create an array that stores the coordinates that have already been loaded in
    public List<Vector2> loadedChunks = new List<Vector2>();
    public NoiseFunction noiseFunction;

    public int xSize = 40;
    public int zSize = 40;

    private int xOffset;
    private int zOffset;

    public GameObject meshLoaderPrefab;

    [HideInInspector]
    public Dictionary<Vector2, GameObject> meshDict = new Dictionary<Vector2, GameObject>();

    void Start()
    {
        xOffset = Random.Range(0, 9999);
        zOffset = Random.Range(0, 9999);

        //CreateShape(0, 0);
    }

    public void CreateShape(int xOff, int zOff)
    {
        //Check to see if the chunk has already been loaded in
        if (loadedChunks.Contains(new Vector2(xOff, zOff)))
        {
            return;
        }
        loadedChunks.Add(new Vector2(xOff, zOff));
        vertices = new Vector3[(xSize + 3) * (zSize + 3)];

        int i = 0;
        for (int z = zOff * zSize + zOffset - 1; z <= zSize + zOff * zSize + zOffset + 1; z++)
        {
            for (int x = xOff * xSize + xOffset - 1; x <= xSize + xOff * xSize + xOffset + 1; x++)
            {
                float y = noiseFunction.noiseFunc(x, z);
                vertices[i] = new Vector3(x - (xOff * xSize + xOffset), y, z - (zOff * zSize + zOffset));
                i++;
                
            }
        }

        triangles = new int[(xSize + 2) * (2 + zSize) * 6];
        int vert = 0;
        int tris = 0;
        for (int z = -1; z < zSize + 1; z++)
        {
            for (int x = -1; x < xSize + 1; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 2 + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 2 + 1;
                triangles[tris + 5] = vert + xSize + 2 + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices.Length];

        for (int i2 = 0, z = 0; z <= zSize + 2; z++)
        {
            for (int x = 0; x <= xSize + 2; x++)
            {
                uvs[i2] = new Vector2((float)x / (xSize + 2), (float)z / (zSize+2));
                i2++;
            }
        }

        GameObject meshLoader = Instantiate(meshLoaderPrefab, new Vector3(xOff * xSize, 0, zOff * zSize), Quaternion.identity);
        meshDict.Add(new Vector2(xOff, zOff), meshLoader);
        MeshLoader meshLoaderScript = meshLoader.GetComponent<MeshLoader>();
        meshLoaderScript.SetDict(meshDict, new Vector2(xOff, zOff));
        meshLoaderScript.vertices = vertices;
        meshLoaderScript.triangles = triangles;
        meshLoaderScript.uvs = uvs;
        meshLoaderScript.mesh = new Mesh();
        meshLoaderScript.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshLoaderScript.GetComponent<MeshFilter>().mesh = meshLoader.GetComponent<MeshLoader>().mesh;
        meshLoaderScript.UpdateMesh();

    }
}
