using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    float moveSpeed;

	void Start () {
        moveSpeed = 8.0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 vec = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        transform.position = vec;
	}
}
