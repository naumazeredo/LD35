using UnityEngine;

public class WorldMoveScript : MonoBehaviour {
    static float moveSpeed = 8.0f;
	
	void FixedUpdate () {
        Vector3 vec = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        transform.position = vec;
	}
}
