using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public BallControl ball;
    public PlayerControl player1;
    public PlayerControl player2;
    private Vector2 initPos = new Vector2(0,0);
    private float xInitSpeed = 10;
    private float yInitSpeed = 9;
    private Rigidbody2D rigidbody2D;

    public void pushPowerUp()
    {
        transform.position = initPos;
        rigidbody2D = GetComponent<Rigidbody2D>();
        float yRandomInitSpeed = Random.Range(-yInitSpeed,yInitSpeed);
        float randomDirection = Random.Range(0,2);

        if (randomDirection > 1.0f)
            rigidbody2D.AddForce(new Vector2(xInitSpeed,yRandomInitSpeed));

        else
            rigidbody2D.AddForce(new Vector2(-xInitSpeed, yRandomInitSpeed));

    }

        void OnTriggerEnter2D(Collider2D collosion)
        {
            if(collosion.gameObject.name.Equals("Ball"))
            {
                Debug.Log("Power-Up Hit");
                if(ball.LastPlatformContact == "Player1")
                {
                    player1.setPlatformScale(1.5f);
                    destroyPowerUp();
                }
                
                else if(ball.LastPlatformContact == "Player2")
                {
                    player2.setPlatformScale(1.5f);
                    destroyPowerUp();
                }
            }

            else if (collosion.gameObject.name.Equals("Player1"))
            {
                Debug.Log("Power-Up Hit");
                player1.setPlatformScale(1.5f);
                destroyPowerUp();
            }

            else if (collosion.gameObject.name.Equals("Player2"))
            {
                Debug.Log("Power-Up Hit");
                player2.setPlatformScale(1.5f);
                destroyPowerUp();
            }

            else if (collosion.gameObject.name.Equals("LeftWall"))
            {
                destroyPowerUp();
            }

            else if (collosion.gameObject.name.Equals("RightWall"))
            {
                destroyPowerUp();
            }

    }

    public void destroyPowerUp()
    {
        Destroy(gameObject);
    }

}