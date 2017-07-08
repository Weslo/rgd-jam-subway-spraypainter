using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// A wall segment in a level.
	public class WallSegment : MonoBehaviour {

		// List of potential graffiti spawn points.
		[SerializeField]
		private Transform[] graffitiSpawnPoints;

		// Fires when all the graffiti on this wall is completed.
		public Action<WallSegment> OnAllCompleted;

		// Total number of graffitis on this wall.
		private int numGraffitis;

		// Number of completed graffitis on this wall.
		private int completed;

		// Initialize this component.
		void Start() {
			SpawnGraffiti(3);
		}

		// Spawn graffiti on this wall.
		public void SpawnGraffiti(int numGraffitis) {

			// Spawn graffiti.
			this.numGraffitis = numGraffitis;
			List<Transform> spawns = new List<Transform>(graffitiSpawnPoints);
			for(int i = 0; i < numGraffitis; i++) {
				int ri = UnityEngine.Random.Range(0, spawns.Count);
				Transform spawn = spawns[ri];
				Graffiti graffiti = Instantiate(Resources.Load<Graffiti>("Prefabs/Graffiti"));
				graffiti.transform.SetParent(transform, false);
				graffiti.transform.position = spawn.transform.position;
				graffiti.OnCompleted += OnCompleteGraffiti;
				spawns.RemoveAt(ri);
			}
		}

		// Called when a graffiti is completed.
		private void OnCompleteGraffiti(Graffiti graffiti) {
			completed++;
			Debug.LogFormat("{0}/{1}", completed, numGraffitis);
			if(completed >= numGraffitis) {
				Debug.Log("completed");
				if(OnAllCompleted != null) {
					OnAllCompleted(this);
				}
			}
		}
	}
}