using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public float moveSpeed;
    float difficultTimer;

	void Start () {
        moveSpeed = 8f;
        difficultTimer = 5f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 vec = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        transform.position = vec;

        difficultTimer -= Time.deltaTime;
        if (difficultTimer < 0) {
            difficultTimer += 5f;
            moveSpeed += 0.2f;
        }
	}
}
