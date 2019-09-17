using UnityEngine;
using System.Collections;

public static class FalloffGenerator {

	public static float[,] GenerateFalloffMap(int size) {
		float[,] map = new float[size,size];

		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				float x = i / (float)size * 2 - 1;
				float y = j / (float)size * 2 - 1;

        map[i, j] = EvaluateCircle(x, y);
      }
		}

		return map;
	}

  static float EvaluateCircle(float x, float y) {
    return Mathf.Clamp(Mathf.Abs(Mathf.Pow(x, 2) + Mathf.Pow(y, 2))*1.5f, -1, 1);
  }
}
