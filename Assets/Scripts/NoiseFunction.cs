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
    FastNoiseLite base_height_noise = new FastNoiseLite();
    FastNoiseLite temperature_noise = new FastNoiseLite();
    FastNoiseLite mountain_noise = new FastNoiseLite();
    FastNoiseLite mountainness_noise = new FastNoiseLite();

    public float mountain_scale = 0.5f;
    public float mountain_height_scale = 2f;
    public float temperature_mul = 1.0f;
    public float temperature_pow = 0.0f;

    public float elevation_amplitude = 180f;

    void Start(){
        base_height_noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        base_height_noise.SetFrequency(0.0005f);
        base_height_noise.SetSeed(1337);
        base_height_noise.SetFractalType(FastNoiseLite.FractalType.FBm);    
        base_height_noise.SetFractalOctaves(10);
        base_height_noise.SetFractalLacunarity(2.0f);
        base_height_noise.SetFractalGain(0.5f);

        temperature_noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        temperature_noise.SetFrequency(0.0005f);
        temperature_noise.SetSeed(1337);
        temperature_noise.SetFractalType(FastNoiseLite.FractalType.FBm);
        temperature_noise.SetFractalOctaves(3);
        temperature_noise.SetFractalLacunarity(2.0f);
        temperature_noise.SetFractalGain(0.5f);

        mountain_noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        mountain_noise.SetFrequency(0.0005f);
        mountain_noise.SetSeed(1337);
        mountain_noise.SetFractalType(FastNoiseLite.FractalType.FBm);
        mountain_noise.SetFractalOctaves(10);
        mountain_noise.SetFractalLacunarity(2.0f);
        mountain_noise.SetFractalGain(0.5f);

        mountainness_noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        mountainness_noise.SetFrequency(0.0005f);
        mountainness_noise.SetSeed(1337);
        mountainness_noise.SetFractalType(FastNoiseLite.FractalType.Ridged);
        mountainness_noise.SetFractalOctaves(10);
        mountainness_noise.SetFractalLacunarity(2.0f);
        mountainness_noise.SetFractalGain(0.5f);
    }


    public float noiseFunc(int x, int z){
        float height_value = base_height_noise.GetNoise(x, z);
        float temperature_value = (float)(1.0 - (temperature_noise.GetNoise(x, z) + 1.0) / 2.0);
        float mountain_value = mountain_noise.GetNoise(x * mountain_scale, z * mountain_scale);
        height_value -= (float)(mountain_value * ((mountainness_noise.GetNoise(x * mountain_scale, z * mountain_scale) + 1) / 2.0) * mountain_height_scale / (1.0 - height_value));
        height_value *= (float)(Math.Pow(temperature_value, temperature_pow) * temperature_mul);
        return  height_value * elevation_amplitude;
    }
}
