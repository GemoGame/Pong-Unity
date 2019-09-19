using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public BallControl ball;
    public KeyCode upButton = KeyCode.W;
    public KeyCode downButton = KeyCode.S;
    public float speed = 10.0f;
    public float yBoundary = 9.0f;
    private Rigidbody2D rigidBody2d;
    private int score;
    private ContactPoint2D lastContactPoint;
    private float currentBallForce;
    private float speedMultiplier = 0.0f;
    private bool speedMultiplierPeak = false;
    private Coroutine ballSpeedMultiplier;
    private float speedChangeInterval = 0.02f;
    private int powerUpDuration = 5;
    public PlayerControl enemy;
    public GameManager gameManager;
    private Vector2 defaultPositionP1 = new Vector2(-15, 0);
    private Vector2 defaultPositionP2 = new Vector2(15, 0);
    void Start()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
         platformMovement();
         checkBoundary();
    }

    void startSpeedMultiplier()
    {
        if (ballSpeedMultiplier == null)
        {
            ballSpeedMultiplier = StartCoroutine(adjustBallSpeed(currentBallForce));
        }
    }

    void stopSpeedMultiplier()
    {
        if (ballSpeedMultiplier != null)
        {
            speedMultiplier = 0.0f;
            StopCoroutine(ballSpeedMultiplier);
            speedMultiplierPeak = false;
            ballSpeedMultiplier = null;
            Debug.Log("current speed multiplier = " + speedMultiplier);
        }
    }
    public ContactPoint2D LastContactPoint
    {
        get {return lastContactPoint;}
    }

    private void platformMovement()
    {
        Vector2 resetSpeed = new Vector2(0,0);
        rigidBody2d.velocity = resetSpeed;
        Vector2 velocity = rigidBody2d.velocity;

        if (Input.GetKey(upButton))
        {
            velocity.y += speed;
        }

        else if (Input.GetKey(downButton))
        {
            velocity.y -= speed;
        }

        else
        {
            velocity.y = 0.0f;
        }

        if (Input.GetMouseButton(0))
        {
            startSpeedMultiplier();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            stopSpeedMultiplier();
        }

        rigidBody2d.velocity += velocity;
    }

    private void checkBoundary()
    {
        Vector3 position = transform.position;

        if (position.y > yBoundary)
        {
            position.y = yBoundary;
        }

        else if (position.y < -yBoundary)
        {
            position.y = -yBoundary;
        }

        transform.position = position;
    }

    public void IncrementScore()
    {
        score++;
    }

    public void setScore(int value)
    {
        score = value;
    }
    public void instantDefeat()
    {
        enemy.setScore(gameManager.MaxScore);
    }
    public void ResetScore()
    {
        score = 0;
    }

    public int Score
    {
        get { return score; }
    }
    


    IEnumerator adjustBallSpeed(float currentSpeed)
    {
        while(true)
        {
            if(!speedMultiplierPeak && speedMultiplier <= 0.50f)
                speedMultiplier += 0.02f;

            else if(speedMultiplier > 0.0f)
            {
                if(!speedMultiplierPeak)
                    speedMultiplierPeak = true;
                speedMultiplier -= 0.02f;
            }
            else if(speedMultiplier < 0.0f)
                speedMultiplier = 0.0f; 
            
            Debug.Log("current speed multiplier = " + speedMultiplier);
            
            yield return new WaitForSeconds(speedChangeInterval);
        }
        
    }

    public void setPlatformScale(float scale)
    {
        Vector3 customScale = new Vector3(1,scale,1);
        transform.localScale = customScale;
        Invoke("resetPlatformScale",powerUpDuration);
    }

    private void resetPlatformScale()
    {
        setPlatformScale(1.0f);
    }

    private void resetPlatform()
    {
        
        if (gameObject.name.Equals("Player1"))
        {
            transform.position = defaultPositionP1;
            enemy.transform.position = defaultPositionP2;
        }
            

        else if (gameObject.name.Equals("Player2"))
        {
            transform.position = defaultPositionP2;
            enemy.transform.position = defaultPositionP1;
        }
            
    }
    void OnCollisionEnter2D(Collision2D collosion)
    {
        if (collosion.gameObject.name.Equals("Ball"))
        {
            Debug.Log("collided");
            lastContactPoint = collosion.GetContact(0);
            stopSpeedMultiplier();
            if(ball.IsOnFire)
            {
                resetPlatform();
                instantDefeat();
                collosion.gameObject.SendMessage("RestartGame", 2.0f, SendMessageOptions.RequireReceiver);
            }
            ball.modifySpeed(speedMultiplier);
            ball.setLastPlatformContact(gameObject.name);
            ball.FireUp();
            
        }
    }

}