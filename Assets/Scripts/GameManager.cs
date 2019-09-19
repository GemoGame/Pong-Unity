using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerControl player1;
    private Rigidbody2D player1rigidbody2d;
    public PlayerControl player2;
    private Rigidbody2D player2rigidbody2d;
    public BallControl ball;
    private Rigidbody2D ballRigidbody;
    private CircleCollider2D ballCollider;
    public int maxScore = 10;
    public Trajectory trajectory;
    public PowerUpSpawner powerUpSpawner;
    private Coroutine CR_powerUpSpawner;
    private bool isDebugWindowShown = false;
    private float spawnInterval = 10.0f;
    void Start()
    {
        player1rigidbody2d = player1.GetComponent<Rigidbody2D>();
        player2rigidbody2d = player2.GetComponent<Rigidbody2D>();
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
        startPowerUpSpawner();
    }

    public int MaxScore
    {
        get {return maxScore;}
    }

    void startPowerUpSpawner()
    {
        if (CR_powerUpSpawner == null)
        {
            Debug.Log("Power-Up Spawner started");
            CR_powerUpSpawner = StartCoroutine(spawnPowerUp());
        }
    }

    void stopPowerUpSpawner()
    {
        if (CR_powerUpSpawner != null)
        {
            StopCoroutine(CR_powerUpSpawner);
        }
    }

    IEnumerator spawnPowerUp()
    {
        while(true)
        {
            powerUpSpawner.spawnPowerUp();

            yield return new WaitForSeconds(spawnInterval);
        }
        
    }

    void OnGUI()
    {
        Color oldColor = GUI.backgroundColor;

        float ballMass = ballRigidbody.mass;
        Vector2 ballVelocity = ballRigidbody.velocity;
        float ballSpeed = ballRigidbody.velocity.magnitude;
        Vector2 ballMomentum = ballMass * ballVelocity;
        float ballFriction = ballCollider.friction;

        float impulsePlayer1X = player1.LastContactPoint.normalImpulse;
        float impulsePlayer1Y = player1.LastContactPoint.tangentImpulse;
        float impulsePlayer2X = player2.LastContactPoint.normalImpulse;
        float impulsePlayer2Y = player2.LastContactPoint.tangentImpulse;

        string debugText =
                "Ball mass = " + ballMass + "\n" +
                "Ball velocity = " + ballVelocity + "\n" +
                "Ball speed = " + ballSpeed + "\n" +
                "Ball momentum = " + ballMomentum + "\n" +
                "Ball friction = " + ballFriction + "\n" +
                "Last impulse from player 1 = (" + impulsePlayer1X + ", " + impulsePlayer1Y + ")\n" +
                "Last impulse from player 2 = (" + impulsePlayer2X + ", " + impulsePlayer2Y + ")\n";

        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100),"" + player1.Score);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + player2.Score);
    
        if (GUI.Button(new Rect(Screen.width/2 - 60, 35 , 120, 53), "RESTART"))
        {
            player1.ResetScore();
            player2.ResetScore();

            ball.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
        }

        if (player1.Score == maxScore)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 10, 2000,1000), "PLAYER ONE WINS");

            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }

        else if (player2.Score == maxScore)
        {
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000), "PLAYER TWO WINS");

            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }

        if(isDebugWindowShown)
        {
            GUI.backgroundColor = Color.red;
            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width / 2 - 200, Screen.height - 200, 400, 110), debugText, guiStyle);
        }
        
        else 
        {
            GUI.backgroundColor = oldColor;
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 73, 120, 53), "TOGGLE\nDEBUG INFO"))
        {
            isDebugWindowShown = !isDebugWindowShown;
            trajectory.enabled = !trajectory.enabled;
        }
    }
}