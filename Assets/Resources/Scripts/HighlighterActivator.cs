using UnityEngine;
using HighlightingSystem;

namespace SubwaySpraypainter {

	// Automatically activates a Highlighter component.
	[RequireComponent(typeof(Highlighter))]
	public class HighlighterActivator : MonoBehaviour {

		// Self-initialize.
		void Start() {
			GetComponent<Highlighter>().ConstantOnImmediate(Color.red);
		}
	}
}
