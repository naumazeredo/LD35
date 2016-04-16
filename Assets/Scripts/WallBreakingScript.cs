using UnityEngine;

public class WallBreakingScript : MonoBehaviour {
    public Transform[] checks;
    public LayerMask breakableLayer;

	void FixedUpdate () {
        Collider2D col = Physics2D.OverlapArea(checks[0].position, checks[1].position, breakableLayer);
        if (col) {
            Destroy(col.gameObject);
        }
	}
}
