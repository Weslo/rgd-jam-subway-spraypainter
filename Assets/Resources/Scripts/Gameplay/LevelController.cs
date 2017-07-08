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
			float prevX = current == null ? -WALL_SEGMENT_SPACING : current.transform.position.x;
			float x = prevX + WALL_SEGMENT_SPACING;

			// Tween the camera to the next wall.
			WallSegment previous = current;
			TweenManager.Tween(1, 1).OnStep((t) => {
				Camera.main.transform.position = new Vector3(
					Mathf.Lerp(prevX, x, t),
					Camera.main.transform.position.y,
					Camera.main.transform.position.z
				);
			}).OnComplete(() => {
				if(previous != null) {
					Destroy(previous.gameObject);
				}
			});

			// Spawn next wall.
			current = Instantiate(Resources.Load<WallSegment>("Prefabs/Wall Segment"), new Vector3(x, 0, 0), Quaternion.identity);
			current.OnAllCompleted += OnWallComplete;
		}

		// Called when a wall is completed.
		private void OnWallComplete(WallSegment wall) {
			GoToNextWall();
		}
	}
}