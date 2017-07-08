using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Procedural level manager.
	public class LevelController : MonoBehaviour {

		// Spacing between wall segments.
		private const float WALL_SEGMENT_SPACING = 20;

		// The current wall segment.
		private WallSegment current = null;

		// Initialize this component.
		void Start() {
			GoToNextWall();
		}

		// Spawn the next wall segment.
		private void GoToNextWall() {

			// Get next wall x pos.
			float x = current == null ? 0 : current.transform.position.x + WALL_SEGMENT_SPACING;

			// Spawn next wall.
			WallSegment wall = Instantiate(Resources.Load<WallSegment>("Resources/Prefabs/Wall Segment"), new Vector3(x, 0, 0), Quaternion.identity);
		}
	}
}