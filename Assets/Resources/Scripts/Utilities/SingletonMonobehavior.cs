using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Singleton-enforced MonoBehaviour.
	public class SingletonMonobehavior<T> : MonoBehaviour where T : SingletonMonobehavior<T> {

		// The singleton instance of this component.
		protected static T Instance {
			get;
			private set;
		}

		// Self-initialize.
		protected virtual void Awake() {
			if(Instance != null) {
				Debug.LogErrorFormat(Instance, "There can only be one instance of {0}.", typeof(T).Name);
				Destroy(this);
				return;
			}
			Instance = this as T;
		}
	}
}
