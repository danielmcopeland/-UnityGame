using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeightMapGenerator {

	public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings settings, Vector2 sampleCentre) {
		float[,] values = Noise.GenerateNoiseMap (width, height, settings.noiseSettings, sampleCentre);
    HeightMap falloffMap = new HeightMap(FalloffGenerator.GenerateFalloffMap(width), 0, 1);


    AnimationCurve heightCurve_threadsafe = new AnimationCurve (settings.heightCurve.keys);

		float minValue = float.MaxValue;
		float maxValue = float.MinValue;


    float offset = 200000;
    System.Random rand = new System.Random((int)(sampleCentre.x + sampleCentre.y + offset));
    System.Random rand2 = new System.Random((int)(sampleCentre.x * (sampleCentre.y + offset)));
    float val = Mathf.PerlinNoise(((float)(rand.Next(0, 100)) / 61), ((float)(rand2.Next(0, 100)) / 61));
    if(sampleCentre.x == 0 && sampleCentre.y == 0) {
      val = 0f;
    }

    if(val >= settings.islandDensity) {
      for(int i = 0; i < width; i++) {
        for(int j = 0; j < height; j++) {
          values[i, j] = 0;
        }
      }

      maxValue = 0;
      minValue = 0;
    }
    else {
      for (int i = 0; i < width; i++) {
			  for (int j = 0; j < height; j++) {
				  values [i, j] *= heightCurve_threadsafe.Evaluate (values [i, j]) * settings.heightMultiplier * (1-falloffMap.values[i, j]);

				  if (values [i, j] > maxValue) {
					  maxValue = values [i, j];
				  }
				  if (values [i, j] < minValue) {
					  minValue = values [i, j];
				  }
			  }
		  }
    }

		return new HeightMap (values, minValue, maxValue);
	}

}

public struct HeightMap {
	public readonly float[,] values;
	public readonly float minValue;
	public readonly float maxValue;

	public HeightMap (float[,] values, float minValue, float maxValue)
	{
		this.values = values;
		this.minValue = minValue;
		this.maxValue = maxValue;
	}
}

