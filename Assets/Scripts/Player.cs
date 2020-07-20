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
    bool isStartingJump = false;

    // cached components
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider;

    // constants
    const string ANIMATION_PARAM_JUMP_TRIGGER = "Jump";
    const string ANIMATION_PARAM_LAND_TRIGGER = "Land";
    const string ANIMATION_STATE_JUMP = "Jump";

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Velocity: " + myRigidBody.velocity.x);
        if (isAlive)
        {
            MoveForward();
            LandIfNecessary();
        }
    }

    private void MoveForward()
    {
        Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, MovementSpeed);
        myRigidBody.velocity = playerVelocity;
    }

    private void LandIfNecessary()
    {
        if (!isStartingJump && IsOnGround() && myAnimator.GetCurrentAnimatorStateInfo(0).IsName(ANIMATION_STATE_JUMP))
        {
            Debug.Log("Land");
            myAnimator.SetTrigger(ANIMATION_PARAM_LAND_TRIGGER);
        }
    }

    public void Jump(float JumpForce)
    {
        
        Debug.Log("Jump");
        isStartingJump = true;
        Vector2 playerVelocity = new Vector2(-JumpForce, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        myAnimator.SetTrigger(ANIMATION_PARAM_JUMP_TRIGGER);
        StartCoroutine(StopStartingJump());
    }

    private IEnumerator StopStartingJump()
    {
        yield return new WaitForSeconds(.1f); // TODO: better estimate based on force
        isStartingJump = false;
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

    private bool IsOnGround()
    {
        return (myCollider.IsTouchingLayers(LayerMask.GetMask(Constants.LAYER_GROUND)));
    }
}
