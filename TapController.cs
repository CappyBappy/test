using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;
    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    Rigidbody2D rb;
    Quaternion downRotation;
    Quaternion forwardRotation;
    GameManager game;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 35);
        game = GameManager.Instance;

    }

        void OnEnable() {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable() {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;

    }

    void OnGameStarted() {
        rb.velocity = Vector3.zero;
        rb.simulated = true;
    }
    void OnGameOverConfirmed() {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    void Update() {
        if (game.GameOver) return;
        if (Input.GetMouseButtonDown(0)) {
            transform.rotation = forwardRotation;
            rb.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
            rb.velocity = Vector3.zero;

        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "DeadZone") {
                rb.simulated = false;

                //register a dead event
                //play a sound
                OnPlayerDied();
        }
        if (col.gameObject.tag == "ScoreZone") {
        //Play a sound
        //register a score event
        OnPlayerScored(); //event sent to game manager
        }
    }

}
