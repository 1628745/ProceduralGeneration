using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Vector3[] vertices;
    int[] triangles;
    //Create an array that stores the coordinates that have already been loaded in
    public List<Vector2> loadedChunks = new List<Vector2>();

    public int xSize = 40;
    public int zSize = 40;

    private int xOffset;
    private int zOffset;

    public float baseFrequency = 0.2f;   // Controls frequency of main noise layer
    public float baseAmplitude = 5f;     // Controls amplitude of main noise layer

    public float secondaryFrequency = 0.1f;
    public float secondaryAmplitude = 2f;

    public float detailFrequency = 0.01f;
    public float detailAmplitude = 60f;

    public float largeScaleFrequency = 0.002f;
    public float largeScaleAmplitude = 200f;

    public float finerDetailFrequency = 0.005f;  // New finer detail frequency
    public float finerDetailAmplitude = 20f;     // New finer detail amplitude

    public float ridgedNoiseFrequency = 0.05f;   // Ridged noise frequency
    public float ridgedNoiseAmplitude = 10f;     // Ridged noise amplitude

    public GameObject meshLoaderPrefab;

    void Start()
    {
        xOffset = Random.Range(0, 9999);
        zOffset = Random.Range(0, 9999);

        CreateShape(0, 0);
    }

    public void CreateShape(int xOff, int zOff)
    {
        //Check to see if the chunk has already been loaded in
        if (loadedChunks.Contains(new Vector2(xOff, zOff)))
        {
            return;
        }
        loadedChunks.Add(new Vector2(xOff, zOff));
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;
        for (int z = zOff * zSize + zOffset; z <= zSize + zOff * zSize + zOffset; z++)
        {
            for (int x = xOff * xSize + xOffset; x <= xSize + xOff * xSize + xOffset; x++)
            {
                // Multiple layers of noise for different scales of detail
                float y1 = Mathf.PerlinNoise(x * baseFrequency, z * baseFrequency) * baseAmplitude;
                float y2 = Mathf.PerlinNoise(x * secondaryFrequency, z * secondaryFrequency) * secondaryAmplitude;
                float y3 = Mathf.PerlinNoise(x * detailFrequency, z * detailFrequency) * detailAmplitude;
                float y4 = Mathf.PerlinNoise(x * largeScaleFrequency, z * largeScaleFrequency) * largeScaleAmplitude;

                // Finer detail noise
                float y5 = Mathf.PerlinNoise(x * finerDetailFrequency, z * finerDetailFrequency) * finerDetailAmplitude;

                // Ridged noise for more realistic terrain features like cliffs
                float y6 = Mathf.Abs(0.5f - Mathf.PerlinNoise(x * ridgedNoiseFrequency, z * ridgedNoiseFrequency)) * ridgedNoiseAmplitude;

                // Combine noise layers
                float y = y1 + y2 + y3 + y4 + y5 + y6;

                vertices[i] = new Vector3(x - (xOff * xSize + xOffset), y, z - (zOff * zSize + zOffset));
                i++;
                
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
        GameObject meshLoader = Instantiate(meshLoaderPrefab, new Vector3(xOff * xSize, 0, zOff * zSize), Quaternion.identity);
        MeshLoader meshLoaderScript = meshLoader.GetComponent<MeshLoader>();
        meshLoaderScript.vertices = vertices;
        meshLoaderScript.triangles = triangles;
        meshLoaderScript.mesh = new Mesh();
        meshLoaderScript.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshLoaderScript.GetComponent<MeshFilter>().mesh = meshLoader.GetComponent<MeshLoader>().mesh;
        meshLoaderScript.UpdateMesh();

    }
}
