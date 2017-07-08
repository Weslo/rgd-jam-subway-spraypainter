using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubwaySpraypainter;

public class TweenTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TweenManager.Tween(3,
			   (t) => {
				   Debug.LogWarning(t);
			  	   float x = Mathf.Lerp(-5, 5, t);
				   transform.position = new Vector3(x, 0);
			   },
			   () => {
				   Debug.LogWarning("finished");
			   });
	}
}
