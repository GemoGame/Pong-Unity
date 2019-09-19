using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteGlow;
public class BallControl : MonoBehaviour
{
    private string myBall = "Ball";
    private Rigidbody2D rigidbody2D;
    private string lastPlatformcontact;
    public float xInitialForce = 50;
    public float yInitialForce = 0;
    private Vector2 trajectoryOrigin;
    public SpriteGlowEffect glowEffect;
    private bool isOnfire = false;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        RestartGame();

        trajectoryOrigin = transform.position;
    }
    public string LastPlatformContact
    {
        get {return lastPlatformcontact;}
    }
    public void setLastPlatformContact(string value)
    {
        lastPlatformcontact = value;
        Debug.Log("Last contact with "+lastPlatformcontact);
    }

    public bool IsOnFire
    {
        get{return isOnfire;}
    }
    public void FireUp()
    {
        float randomFireUp = Random.Range(0,100);
        if (randomFireUp < 15.0f)
            Invoke("Glow",1);
        
        else
            Invoke("resetGlow", 1);
            
    }
    public string MyBall
    {
        get {return myBall;}        
    }
    public void modifySpeed(float multiplier)
    {
        Debug.Log("speed modified");
        float speedMagnitude = rigidbody2D.velocity.magnitude;
        float xMultipliedForce = ((multiplier * 2) + 0.75f)* speedMagnitude;
        setForce(xMultipliedForce);
    }
    private void OnCollosionExit2D(Collision2D collosion)
    {
        trajectoryOrigin = transform.position;
    }

    public Vector2 TrajectoryOrigin
    {
        get {return trajectoryOrigin;}
    }

    void ResetBall()
    {
        transform.position = Vector2.zero;

        rigidbody2D.velocity = Vector2.zero;
    }

    private void Glow()
    {
        isOnfire = true;
        glowEffect.GlowBrightness = 10.0f;
        glowEffect.OutlineWidth = 10;
    }

    private void resetGlow()
    {
        isOnfire = false;
        glowEffect.GlowBrightness = 0.0f;
        glowEffect.OutlineWidth = 0;
    }
    void PushBall()
    {
        float randomDirection = Random.Range(0,2);
        float yRandomInitialForce = Random.Range(-yInitialForce,yInitialForce);
        if (randomDirection > 1.0f)
        {
            rigidbody2D.AddForce(new Vector2(-xInitialForce,yRandomInitialForce));
        }

        else
        {
            rigidbody2D.AddForce(new Vector2(xInitialForce, yRandomInitialForce));
        }
    }

    void setForce(float x)
    {
        float y = rigidbody2D.velocity.y;
        rigidbody2D.velocity = Vector2.zero;
        Vector2 currentPosition = transform.position;
        if(currentPosition.x > 0)
            {
                x = -1 * x;
            }
        Vector2 updateSpeed = new Vector2(x, y);
        rigidbody2D.velocity = updateSpeed;
    }
    void RestartGame()
    {
        ResetBall();

        Invoke("PushBall",2);
    }

}