using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Array extension methods.
	public static class ArrayX {

		// Pick a random element from an array.
		public static T PickRandom<T>(this T[] arr) {
			if(arr == null || arr.Length == 0) {
				return default(T);
			}
			return arr[UnityEngine.Random.Range(0, arr.Length)];
		}
	}
}