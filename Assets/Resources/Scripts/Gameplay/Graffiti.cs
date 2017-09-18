using System;
using UnityEngine;
using HighlightingSystem;

namespace SubwaySpraypainter {

	// A graffiti instance.
	[RequireComponent(typeof(BoxCollider2D))]
	public class Graffiti : MonoBehaviour {

		// If set to true, the user is painting.
		public static bool Painting {
			get;
			private set;
		}

		// If set to true, painting is enabled.
		public static bool PaintingEnabled {
			get;
			set;
		}

		// Mask texture resolution.
		private const int MASK_TEXTURE_RES = 100;

		// Paint brush radius.
		private const int BRUSH_RADIUS = 50;

		// Reference to the mask renderer.
		[SerializeField]
		private SpriteMask mask;

		// Reference to the highlight.
		[SerializeField]
		private Highlighter highlighter;

		// Fires when this graffiti is completed.
		public Action<Graffiti> OnCompleted;

		// Reference to the procedural mask texture
		private Texture2D tex;

		// If set to true, graffiti is completed.
		private bool completed = false;

		// Matrix of points available on this graffiti.
		private bool[,] pointsMatrix;

		// Total points hit.
		private int totalPoints;

		// Use this for initialization
		void Start() {
			tex = new Texture2D((int)(mask.size.x * MASK_TEXTURE_RES), (int)(mask.size.y * MASK_TEXTURE_RES), TextureFormat.Alpha8, false);
			for(int x = 0; x < tex.width; x++) {
				for(int y = 0; y < tex.height; y++) {
					tex.SetPixel(x, y, new Color(1, 1, 1, 0));
				}
			}
			tex.Apply();

			mask.texture = tex;
			pointsMatrix = new bool[tex.width, tex.height];

			BoxCollider2D col = GetComponent<BoxCollider2D>();
			col.size = new Vector2(mask.size.x, mask.size.y);

			highlighter.FlashingOn(new Color(1, 1, 1, 0), Color.red);
		}

		// On mouse down.
		void OnMouseDown() {
			Painting = true;
		}

		// On mouse up.
		void OnMouseUp() {
			Painting = false;
		}

		// On mouse drag.
		void OnMouseDrag() {
			if(!completed && PaintingEnabled) {
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos -= transform.position;
				int cx = (int)(pos.x * MASK_TEXTURE_RES + (mask.size.x * MASK_TEXTURE_RES) / 2);
				int cy = (int)(pos.y * MASK_TEXTURE_RES + (mask.size.y * MASK_TEXTURE_RES) / 2);

				int brushRadius = BRUSH_RADIUS;
				for(int x = cx - brushRadius; x < cx + brushRadius; x++) {
					for(int y = cy - brushRadius; y < cy + brushRadius; y++) {
						if(x >= 0 && x < tex.width && y >= 0 && y < tex.height) {
							if(MathX.Distance(x, y, cx, cy) < brushRadius) {
								tex.SetPixel(x, y, Color.white);
								if(!pointsMatrix[x, y]) {
									pointsMatrix[x, y] = true;
									totalPoints++;
								}
							}
						}
					}
				}
				if(PercentCompleted() >= 0.7f) {
					Complete();
				}
				tex.Apply();
			}
		}

		// Returns the percent of this image that is completed.
		private float PercentCompleted() {
			int max = tex.width * tex.height;
			return (float)totalPoints / max;
		}

		// Complete this graffiti.
		private void Complete() {
			highlighter.FlashingOff();
			highlighter.ConstantOn(Color.yellow);
			GetComponent<Animator>().Play("Completed");
			completed = true;
			mask.texture = new Texture2D((int)mask.size.x, (int)mask.size.y);
			if(OnCompleted != null) {
				OnCompleted(this);
			}
		}
	}
}
