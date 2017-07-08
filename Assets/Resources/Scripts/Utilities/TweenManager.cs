using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Manages tweens.
	public class TweenManager : SchedulingManager<TweenManager> {

		// Add a new tween.
		public static TimerInstance Tween(float duration, float delay = 0, string id = null) {
			return Begin(duration, delay, id);
		}
	}
}