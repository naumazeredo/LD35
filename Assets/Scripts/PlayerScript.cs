using UnityEngine;

public class PlayerScript : MonoBehaviour {
    Rigidbody2D rb;

    public Transform[] groundChecks;
    public LayerMask groundLayers;
    bool isGrounded;

    enum State { S, H, A, P, E }
    [SerializeField]
    State state;

    // E
    public GameObject wallBreaker;
    public bool isBreaking;

    const float totalWallBreakTime = 1f;
    float wallBreakTime;
    float wallBreakCooldown;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

	void Update () {
        isGrounded = Physics2D.OverlapArea(groundChecks[0].position, groundChecks[1].position, groundLayers);

        // H
        if (isGrounded && Input.GetKey(KeyCode.H)) {
            Vector3 vec = rb.velocity;
            vec.y = 10;
            rb.velocity = vec;

            state = State.H;
        }

        // E
        else if (Input.GetKey(KeyCode.E)) {
            wallBreaker.SetActive(true);

            state = State.H;
        }
	}
}
