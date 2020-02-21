using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // config
    [SerializeField] float MovementSpeed = 1.0f;
    [SerializeField] GameSession gameSession;

    // state
    bool isAlive = true;

    // cached components
    Rigidbody2D myRigidBody;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            MoveForward();
        }
    }

    private void MoveForward()
    {
        Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, MovementSpeed);
        myRigidBody.velocity = playerVelocity;
    }

    public void Jump(float JumpForce)
    {
        Vector2 playerVelocity = new Vector2(-JumpForce, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hazard hazard = collision.GetComponent<Hazard>();
        if (hazard)
        {
            Die(hazard.DeathForce);
        } else
        {
            Powerup powerup = collision.GetComponent<Powerup>();
            if (powerup)
            {
                powerup.CollectPowerup(this);
            }
        }
    }

    private void Die(Vector2 deathForce)
    {
        Debug.Log("die");
        isAlive = false;
        gameSession.PlayerDied();

        myAnimator.enabled = false; // is there a better way?
        myRigidBody.freezeRotation = false;
        myRigidBody.AddForce(deathForce);
        myRigidBody.AddTorque(10);
        
    }
}
