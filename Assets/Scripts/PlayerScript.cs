using UnityEngine;

public class PlayerScript : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    BoxCollider2D boxCol;

    public Transform[] groundChecks;
    public LayerMask groundLayers;
    bool isGrounded;

    public LayerMask waterLayers;
    public bool isInWater;

    enum State { S, H, A, P, E }
    [SerializeField]
    State state;

    // E
    public GameObject wallBreaker;

    // P
    Vector2 originalSize;

	void Start () {
        // Temp 
        spriteRenderer = GetComponent<SpriteRenderer>();
        // ----
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        originalSize = boxCol.size;
	}

    void ResetState() {
        wallBreaker.SetActive(false);
        boxCol.size = originalSize;
        boxCol.offset = new Vector2(0, 0);
        rb.gravityScale = 1f;
    }

    void FixedUpdate() {
        isGrounded = Physics2D.OverlapArea(groundChecks[0].position, groundChecks[1].position, groundLayers);

        Vector2 extents = spriteRenderer.sprite.bounds.extents;
        isInWater = Physics2D.OverlapArea(new Vector2(transform.position.x - extents.x, transform.position.y + extents.y),
                                          new Vector2(transform.position.x + extents.x, transform.position.y - extents.y),
                                          waterLayers);
        if (isInWater) {
            if (rb.velocity.y < -4f) rb.velocity = new Vector2(rb.velocity.x, -4f);
        }
    }

	void Update () {
        // S
        if (Input.GetKey(KeyCode.S)) {
            ResetState();
            state = State.S;
            spriteRenderer.color = Color.red;
        }

        // H
        else if (Input.GetKey(KeyCode.H)) {
            ResetState();
            state = State.H;
            spriteRenderer.color = Color.white;

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
            spriteRenderer.color = Color.green;

            if (isInWater) {
                Vector3 vec = rb.velocity;
                vec.y = 8;
                rb.velocity = vec;
            }
        }

        // P
        else if (Input.GetKey(KeyCode.P)) {
            ResetState();
            state = State.P;
            spriteRenderer.color = Color.blue;

            Vector2 offset = new Vector2(0f, -originalSize.y/4);
            Vector2 size = new Vector2(originalSize.x, originalSize.y);
            size.y /= 2;
            boxCol.size = size;
            boxCol.offset = offset;
        }

        // E
        else if (Input.GetKey(KeyCode.E)) {
            ResetState();
            state = State.E;
            spriteRenderer.color = Color.magenta;

            wallBreaker.SetActive(true);
        }
	}
}
