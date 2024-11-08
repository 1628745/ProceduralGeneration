using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//From dev
//height_value = base_height_noise.get_noise_2d(x, z)
        

        
//   #temperature
        
//   var temperature_value: float = 1.0 - (temperature_noise.get_noise_2d(x, z) + 1.0) / 2.0
        
//   var mountain_value: float = mountain_noise.get_noise_2d(x * mountain_scale, z * mountain_scale)
        
//   height_value -= mountain_value * ((mountainness_noise.get_noise_2d(x * mountain_scale, z * mountain_scale) + 1) / 2.0) * mountain_height_scale / (1.0 - height_value)
        
//   height_value *= pow(temperature_value, temperature_pow) * temperature_mul

        
        
//   vertices[v].y = height_value * elevation_amplitude


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
