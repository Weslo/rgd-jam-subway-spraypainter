using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Manages tweens.
	public class TimerManager : SchedulingManager<TweenManager> {

		// Add a new tween.
		public static TimerInstance Schedule(float time) {
			return Begin(time, 0);
		}
	}
}