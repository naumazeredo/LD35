using UnityEngine;
using System.Collections;

public class SelfDestruction : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Camera.main.transform.position.x > transform.position.x + transform.localScale.x * 2) {
			Destroy (this.gameObject);
		}
	}
}
