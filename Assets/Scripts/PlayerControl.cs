using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public KeyCode upButton = KeyCode.W;
    public KeyCode downButton = KeyCode.S;
    public float speed = 10.0f;
    public float yBoundary = 9.0f;
    private Rigidbody2D rigidBody2d;
    private int score;
    private ContactPoint2D lastContactPoint;
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

    public void ResetScore()
    {
        score = 0;
    }

    public int Score
    {
        get { return score; }
    }

    void OnCollosionEnter2D(Collision2D collosion)
    {
        if (collosion.gameObject.name.Equals("Ball"))
        {
            lastContactPoint = collosion.GetContact(0);
        }
    }

}