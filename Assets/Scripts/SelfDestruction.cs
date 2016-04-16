using UnityEngine;
using System.Collections;

public class SelfDestruction : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		var vertExtent = Camera.main.orthographicSize;
		var hortExtent = vertExtent * Screen.width / Screen.height;
		if (Camera.main.transform.position.x - hortExtent > transform.position.x + transform.localScale.x * 2) {
			Destroy (this.gameObject);
		}
	}
}
