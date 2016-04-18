using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
    Animator anim;
    CameraScript cameraScript;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    BoxCollider2D boxCol;

    public Animator smoke;

    public LayerMask groundLayers;
    public LayerMask ceilingLayers;
    public LayerMask waterLayers;

    public bool isAlive;
    float curMoveSpeed;

    public float score;

    public Transform[] ceilingChecks;
    bool onCeiling;

    public Transform[] groundChecks;
    bool isGrounded;

    public bool isInWater;

    enum State { S, H, A, P, E }
    State lastState = State.H;

    // E
    public GameObject wallBreaker;

    // P
    Vector2 originalSize;

    // S
    public Transform webPosition;
    public GameObject web;

    // Boost
    const float boostDuration = 1f;
    const float boostCooldownDuration = 30f;
    const float boostMoveModifier = 1.05f;
    float boostTimer;
    float boostCooldownTimer;

    // HUD
    public Text scoreText;
    public Text[] gameOverText;

	void Start () {
        cameraScript = Camera.main.GetComponent<CameraScript>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        originalSize = boxCol.size;

        isAlive = true;
        curMoveSpeed = cameraScript.moveSpeed;

        score = 0;

        web.SetActive(false);

        anim.SetInteger("Animal", 0);
        anim.SetBool("onCeiling", false);
        anim.SetBool("goingUp", false);
	}


    void ResetState() {
        web.SetActive(false);
        wallBreaker.SetActive(false);
        boxCol.size = originalSize;
        boxCol.offset = new Vector2(0, 0);
        rb.gravityScale = 1f;
    }

    void ChangeState(State state) {
        ResetState();
        if (state != lastState) {
            smoke.SetTrigger("Play");
            lastState = state;
        }
    }

    void FixedUpdate() {
        Vector3 vec = new Vector3(transform.position.x + curMoveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        transform.position = vec;

        isGrounded = Physics2D.OverlapArea(groundChecks[0].position, groundChecks[1].position, groundLayers);
        onCeiling = Physics2D.OverlapArea(ceilingChecks[0].position, ceilingChecks[1].position, ceilingLayers);

        Vector2 extents = spriteRenderer.sprite.bounds.extents;
        isInWater = Physics2D.OverlapArea(new Vector2(transform.position.x - extents.x, transform.position.y + extents.y),
                                          new Vector2(transform.position.x + extents.x, transform.position.y - extents.y),
                                          waterLayers);
        if (isInWater) {
            if (rb.velocity.y < -4f) rb.velocity = new Vector2(rb.velocity.x, -4f);
        }

        if (isAlive) score += curMoveSpeed / 32;
    }

	void Update () {
        anim.SetBool("isInWater", isInWater);

        // Boost
        curMoveSpeed = cameraScript.moveSpeed;

        if (boostCooldownTimer > 0f)
            boostCooldownTimer -= Time.deltaTime;

        if (boostTimer > 0f) {
            curMoveSpeed = boostMoveModifier * cameraScript.moveSpeed;
            boostTimer -= Time.deltaTime;
        }

        if (boostCooldownTimer <= 0f && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))) {
            boostTimer = boostDuration;
            boostCooldownTimer = boostCooldownDuration;
        }

        // Game Over
        var vertExtent = Camera.main.orthographicSize;
        var hortExtent = vertExtent * Screen.width / Screen.height;
        if ((Camera.main.transform.position.x - hortExtent > transform.position.x + transform.localScale.x) ||
            (Camera.main.transform.position.y - vertExtent > transform.position.y + transform.localScale.y)) {
            isAlive = false;
        }
        if (!isAlive) {
            scoreText.gameObject.SetActive(false);
            gameOverText[0].text = "GAME OVER";
            gameOverText[1].text = "Score: " + ((int)score).ToString();
            gameOverText[2].text = "Press 'R' to restart";
        }

        // S
        rb.gravityScale = 1;
        if (Input.GetKey(KeyCode.S)) {
            ChangeState(State.S);
            if (Input.GetKeyDown(KeyCode.S)) {
                anim.SetInteger("Animal", 1);
            }

            anim.SetBool("onCeiling", onCeiling);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 10, ceilingLayers);
            if (hit) {
                //Debug.DrawLine(webPosition.position, hit.point);
                web.SetActive(true);

                rb.gravityScale = -2;
                if (Input.GetKeyDown(KeyCode.S)) {
                    anim.SetBool("goingUp", true);
                }
            } else {
                anim.SetBool("goingUp", false);
            }
        } else {
            anim.SetBool("onCeiling", false);
            anim.SetBool("goingUp", false);
            web.SetActive(false);
        }

        // H
        if (Input.GetKey(KeyCode.H)) {
            ChangeState(State.H);
            anim.SetInteger("Animal", 0);

            if (isGrounded) {
                Vector3 vec = rb.velocity;
                vec.y = 10;
                rb.velocity = vec;
            }
        }

        // A
        else if (Input.GetKey(KeyCode.A)) {
            ChangeState(State.A);
            anim.SetInteger("Animal", 2);

            if (isInWater) {
                Vector3 vec = rb.velocity;
                vec.y = 8;
                rb.velocity = vec;
            }
        }

        // P
        else if (Input.GetKey(KeyCode.P)) {
            ChangeState(State.P);
            anim.SetInteger("Animal", 3);

            Vector2 offset = new Vector2(0f, -originalSize.y/4);
            Vector2 size = new Vector2(originalSize.x, originalSize.y);
            size.y /= 2;
            boxCol.size = size;
            boxCol.offset = offset;
        }

        // E
        else if (Input.GetKey(KeyCode.E)) {
            ChangeState(State.E);
            anim.SetInteger("Animal", 4);

            wallBreaker.SetActive(true);
        }

        scoreText.text = ((int)score).ToString();

        //if (Input.GetKeyDown(KeyCode.R) && !isAlive) Application.LoadLevel(Application.loadedLevel);
        if (Input.GetKeyDown(KeyCode.R) && !isAlive) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}
}
