using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedTest : MonoBehaviour
{
    public int iterations = 1000000;
    public NoiseFunction noiseFunction;

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
    void Start()
    {
        // Start the test
        //StartCoroutine(RunTest());
        //StartCoroutine(RunTest2());
        RunTest();
        RunTest2();
    }

    void RunTest()
    {
        // Start the timer
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        // Run the test
        for (int i = 0; i < iterations; i++)
        {
            noiseFunction.noiseFunc(i, i);
        }

        // Stop the timer
        stopwatch.Stop();

        // Output the time taken
        Debug.Log("Time taken for #1: " + stopwatch.ElapsedMilliseconds + "ms");

        //yield return null;
    }

    void RunTest2()
    {
        // Start the timer
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        // Run the test
        for (int i = 0; i < iterations; i++)
        {
            // Multiple layers of noise for different scales of detail
                float y1 = Mathf.PerlinNoise(i * baseFrequency, i * baseFrequency) * baseAmplitude;
                float y2 = Mathf.PerlinNoise(i * secondaryFrequency, i * secondaryFrequency) * secondaryAmplitude;
                float y3 = Mathf.PerlinNoise(i * detailFrequency, i * detailFrequency) * detailAmplitude;
                float y4 = Mathf.PerlinNoise(i * largeScaleFrequency, i * largeScaleFrequency) * largeScaleAmplitude;

                // Finer detail noise
                float y5 = Mathf.PerlinNoise(i * finerDetailFrequency, i * finerDetailFrequency) * finerDetailAmplitude;

                // Ridged noise for more realistic terrain features like cliffs
                float y6 = Mathf.Abs(0.5f - Mathf.PerlinNoise(i * ridgedNoiseFrequency, i * ridgedNoiseFrequency)) * ridgedNoiseAmplitude;

                // Combine noise layers
                float y = y1 + y2 + y3 + y4 + y5 + y6;
        }

        // Stop the timer
        stopwatch.Stop();

        // Output the time taken
        Debug.Log("Time taken for #2: " + stopwatch.ElapsedMilliseconds + "ms");

        //yield return null;
    }
}