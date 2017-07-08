using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// A wall segment in a level.
	public class WallSegment : MonoBehaviour {

		// List of potential graffiti spawn points.
		[SerializeField]
		private Transform[] graffitiSpawnPoints;

		// List of potential police spawn points.
		[SerializeField]
		private Transform[] policeSpawnPoints;

		// Fires when all the graffiti on this wall is completed.
		public Action<WallSegment> OnAllCompleted;

		// Fires when the player is caught.
		public Action<WallSegment> OnPlayerCaught;

		// Total number of graffitis on this wall.
		private int numGraffitis;

		// Number of completed graffitis on this wall.
		private int completed;

		// Initialize this component.
		void Start() {
			SpawnPolice();
			ScheduleSpawnPolice();
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

		// Spawn a police officer on this wall.
		public void SpawnPolice() {
			int ri = UnityEngine.Random.Range(0, policeSpawnPoints.Length);
			Transform spawn = policeSpawnPoints[ri];
			Police police = Instantiate(Resources.Load<Police>("Prefabs/Police"));
			police.transform.SetParent(transform, false);
			police.transform.position = spawn.transform.position;
			police.OnCatchPlayer += OnPoliceCatchesPlayer;
			police.Activate();
		}

		// Schedule spawning a police officer.
		private void ScheduleSpawnPolice() {
			TimerManager.Schedule(10, "police").OnComplete(() => {
				SpawnPolice();
			});
		}

		// Called when a graffiti is completed.
		private void OnCompleteGraffiti(Graffiti graffiti) {
			completed++;
			if(completed >= numGraffitis) {
				if(OnAllCompleted != null) {
					OnAllCompleted(this);
				}
			}
		}

		// Called when the police catches the player.
		private void OnPoliceCatchesPlayer(Police police) {
			if(OnPlayerCaught != null) {
				OnPlayerCaught(this);
			}
		}
	}
}