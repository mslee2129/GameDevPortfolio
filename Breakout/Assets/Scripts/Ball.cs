using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private Vector3 startPosition = new Vector3(0, (float) -3.5f, 0);
    public Transform playerTransform;
    private float maxSpeed = 10.0f;
    private float minSpeed = 5.0f;
    private float startSpeed = 10.0f;
    private bool ballLaunched = false;
    // Start is called before the first frame update
    void Start()                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
    {
        rb = GetComponent<Rigidbody2D>();
        ResetBall();
    }

    // Update is called once per frame
    void Update()
    {
        if(!ballLaunched) { // should start logic be in GameController? Issue is ShootBall is a one-time event
            FollowPlayer();
            bool input = Input.GetKeyDown(KeyCode.Space);
            if(input) {
                ShootBall();
                if(!GameController.instance.GetStarted()) {
                    GameController.instance.StartGame();
                }
                
            }
        }
    }

    void FixedUpdate()
    {
        ClampVelocity();
    }

    private void FollowPlayer() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(playerTransform.position.x, (float) startPosition.y + 0.4f, startPosition.z);
    }

    private void ShootBall() {
        Vector3 launchDirection = new Vector3(0, 1, 0);
        rb.velocity = launchDirection * startSpeed;
        ballLaunched = true;
    }

    private void ClampVelocity() // TODO issue of horizontal slow velocity
    {
        Vector2 currentVelocity = rb.velocity;
        float speed = currentVelocity.magnitude;

        if (speed < minSpeed)
        {
            currentVelocity = currentVelocity.normalized * minSpeed;
        }
        if (speed > maxSpeed)
        {
            currentVelocity = currentVelocity.normalized * maxSpeed;
        }
        rb.velocity = currentVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name); 
            GameController.instance.DestroyBrick(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BottomWall")) {
            Debug.Log("Collision detected with: " + collision.gameObject.name); 
            
            GameController.instance.LoseLife();
            Debug.Log("Lives: " + GameController.instance.GetLife());
            ResetBall();
            
        }
    }

    private void ResetBall() {
        ballLaunched = false;
        rb.velocity = Vector3.zero;
        transform.position = startPosition; // snaps back to start position -> should be player position?
    }
}
