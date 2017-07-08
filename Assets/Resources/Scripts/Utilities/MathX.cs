using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Math utility functions.
	public static class MathX {

		// Distance between two points.
		public static float Distance(float x1, float y1, float x2, float y2) {
			return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
		}
	}
}