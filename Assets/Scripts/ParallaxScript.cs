using UnityEngine;

public class ParallaxScript : MonoBehaviour {
    SpriteRenderer spriteRenderer;

    public float moveSpeed = -1f;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	void FixedUpdate () {
        transform.localPosition = new Vector3(transform.localPosition.x + moveSpeed * Time.deltaTime, transform.localPosition.y, 1);
        if (transform.localPosition.x + spriteRenderer.sprite.bounds.extents.x <= -Camera.main.orthographicSize * Screen.width / Screen.height) {
            transform.localPosition = new Vector3(transform.localPosition.x + 64, transform.localPosition.y, 1);
        }
	}
}
