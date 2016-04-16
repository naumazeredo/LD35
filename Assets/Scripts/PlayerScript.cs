using UnityEngine;

public class PlayerScript : MonoBehaviour {
    Rigidbody2D rb;
    BoxCollider2D boxCol;

    public Transform[] groundChecks;
    public LayerMask groundLayers;
    bool isGrounded;

    enum State { S, H, A, P, E }
    [SerializeField]
    State state;

    // E
    public GameObject wallBreaker;

    // P
    Vector2 originalSize;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        originalSize = boxCol.size;
	}

    void ResetState() {
        wallBreaker.SetActive(false);
        boxCol.size = originalSize;
    }

	void Update () {
        isGrounded = Physics2D.OverlapArea(groundChecks[0].position, groundChecks[1].position, groundLayers);

        // S
        if (Input.GetKey(KeyCode.S)) {
            ResetState();
            state = State.S;
        }

        // H
        else if (Input.GetKey(KeyCode.H)) {
            ResetState();
            state = State.H;

            if (isGrounded) {
                Vector3 vec = rb.velocity;
                vec.y = 10;
                rb.velocity = vec;
            }
        }

        // A
        else if (Input.GetKey(KeyCode.A)) {
            ResetState();
            state = State.A;
        }

        // P
        else if (Input.GetKey(KeyCode.P)) {
            ResetState();
            state = State.P;

            if (Input.GetKeyDown(KeyCode.P)) {
                Vector2 size = originalSize;
                size.y /= 2;
                boxCol.size = size;
            }
        }

        // E
        else if (Input.GetKey(KeyCode.E)) {
            ResetState();
            state = State.E;

            wallBreaker.SetActive(true);
        }
	}
}
