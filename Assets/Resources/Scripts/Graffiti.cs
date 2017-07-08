using UnityEngine;

namespace SubwaySpraypainter {

	// A graffiti instance.
	[RequireComponent(typeof(BoxCollider2D))]
	public class Graffiti : MonoBehaviour {

		private const int MASK_TEXTURE_RES = 100;

		private const int BRUSH_RADIUS = 10;

		// Reference to the mask renderer.
		[SerializeField]
		private SpriteMask mask;

		// Reference to the procedural mask texture
		private Texture2D tex;

		// If set to true, user is painting;
		private bool painting = false;

		// Sizes.
		int width = 5;
		int height = 2;

		// Use this for initialization
		void Start() {
			tex = new Texture2D(width * MASK_TEXTURE_RES, height * MASK_TEXTURE_RES);
			for(int x = 0; x < tex.width; x++) {
				for(int y = 0; y < tex.height; y++) {
					tex.SetPixel(x, y, new Color(1, 1, 1, 0));
				}
			}
			tex.alphaIsTransparency = true;
			tex.Apply();

			mask.texture = tex;
			mask.size = new Vector2(width, height);

			BoxCollider2D col = GetComponent<BoxCollider2D>();
			col.size = new Vector2(width, height);
		}


		// On mouse down.
		void OnMouseDown() {
			painting = true;
		}

		// On mouse drag.
		void OnMouseDrag() {
			if(painting) {
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos -= transform.position;
				int cx = (int)(pos.x * MASK_TEXTURE_RES) + (width * MASK_TEXTURE_RES) / 2;
				int cy = (int)(pos.y * MASK_TEXTURE_RES) + (height * MASK_TEXTURE_RES) / 2;

				for(int x = cx - BRUSH_RADIUS; x < cx + BRUSH_RADIUS; x++) {
					for(int y = cy - BRUSH_RADIUS; y < cy + BRUSH_RADIUS; y++) {
						if(x >= 0 && x < tex.width && y >= 0 && y < tex.height) {
							if(MathX.Distance(x, y, cx, cy) < BRUSH_RADIUS) {
								tex.SetPixel(x, y, Color.white);
							}
						}
					}
				}
				tex.Apply();
			}
		}

		// On mouse exit.
		void OnMouseExit() {
			painting = false;
		}
	}
}
