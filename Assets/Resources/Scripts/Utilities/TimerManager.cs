using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Manages tweens.
	public class TimerManager : SchedulingManager<TweenManager> {

		// Add a new tween.
		public static TimerInstance Schedule(float duration, float delay = 0) {
			return Begin(duration, delay);
		}
	}
}