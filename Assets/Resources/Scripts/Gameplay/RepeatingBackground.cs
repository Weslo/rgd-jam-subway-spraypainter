using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Repeating background image.
public class RepeatingBackground : MonoBehaviour {
	
	// Sprite used as the repeating asset.
	[SerializeField]
	private Sprite backgroundSpriteAsset;

	// Number of assets to repeat.
	[SerializeField]
	private int repeatBuffer;

	// The width of the sprite.
	private float spriteWidth;
	
	// Array of image game objects.
	private List<GameObject> backgrounds = new List<GameObject>();

	// Self-initialize this component.
	void Awake() {

		// Calculate sprite width.
		spriteWidth = backgroundSpriteAsset.bounds.size.x;

		// Create game objects.
		for(int i = 0; i < repeatBuffer; i++) {
			SpriteRenderer background = new GameObject(string.Format("Background {0}", i)).AddComponent<SpriteRenderer>();
			background.transform.SetParent(transform);
			background.transform.localScale = Vector3.one;
			background.transform.localPosition = Vector2.right * i * spriteWidth;
			background.sprite = backgroundSpriteAsset;
			background.sortingLayerName = "Background";
			background.sortingOrder = 1;
			backgrounds.Add(background.gameObject);
		}
	}

	// Update this component between frames.
	void Update() {
		if(backgrounds.Count > 1) {
			GameObject exiting = backgrounds[0];
			GameObject entering = backgrounds[1];
			if(Camera.main.transform.position.x >= entering.transform.position.x) {
				exiting.transform.Translate(Vector2.right * repeatBuffer * spriteWidth);
				backgrounds.RemoveAt(0);
				backgrounds.Add(exiting);
			}
		}
	}
}
