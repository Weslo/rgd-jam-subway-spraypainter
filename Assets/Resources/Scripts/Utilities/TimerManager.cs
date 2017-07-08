using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Manages tweens.
	public class TimerManager : SchedulingManager<TimerManager> {

		// Add a new tween.
		public static TimerInstance Schedule(float time, string id = null) {
			return Begin(time, 0, id);
		}
	}
}