using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoiseFunction : MonoBehaviour
{
    //Noise function
    //Perlin for base layer (FBM)
    //Mountains use Voronoi * Simplex
    //Add perlin to mountains


    FastNoiseLite noise = new FastNoiseLite();

    void Start(){
        noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        noise.SetFractalType(FastNoiseLite.FractalType.FBm);
        noise.SetFractalOctaves(3);
        noise.SetFractalLacunarity(2.0f);
        noise.SetFractalGain(0.5f);
        noise.SetFrequency(0.01f);
    }


    public float noiseFunc(int x, int z){
        return 20*noise.GetNoise(x, z);
    }
}
