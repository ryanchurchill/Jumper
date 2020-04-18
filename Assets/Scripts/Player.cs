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
    bool isJumping = false;

    // cached components
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider;

    // constants
    const string ANIMATION_PARAM_IS_JUMPING = "IsJumping";
    const string ANIMATION_PARAM_IS_FALLING = "IsFalling";
    const string ANIMATION_PARAM_JUMP_TRIGGER = "Jump";
    const string ANIMATION_PARAM_LAND_TRIGGER = "Land";

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
            //if (isJumping)
            //{
            //    HandleLandAnimation();
            //}
        }
    }

    private void MoveForward()
    {
        Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, MovementSpeed);
        myRigidBody.velocity = playerVelocity;
    }

    //private void HandleLandAnimation()
    //{
    //    //Debug.Log("Velocity: " + myRigidBody.velocity.x);
    //    //Debug.Log("Epsilon: " + Mathf.Epsilon);

    //    if (IsOnGround())
    //    {
    //        Debug.Log("Land");
            
            
    //    }

    //    //if (myRigidBody.velocity.x < Mathf.Epsilon)
    //    //{
    //    //    Debug.Log("Jumping");
    //    //    myAnimator.SetBool(ANIMATION_PARAM_IS_JUMPING, true);
    //    //}
    //    //else if (myRigidBody.velocity.x > Mathf.Epsilon)
    //    //{
    //    //    Debug.Log("Falling");
    //    //    myAnimator.SetBool(ANIMATION_PARAM_IS_FALLING, true);
    //    //    myAnimator.SetBool(ANIMATION_PARAM_IS_JUMPING, false);
    //    //}
    //    //else
    //    //{
    //    //    Debug.Log("Jump Complete");
    //    //    myAnimator.SetBool(ANIMATION_PARAM_IS_JUMPING, false);
    //    //    myAnimator.SetBool(ANIMATION_PARAM_IS_FALLING, false);
    //    //}
        
    //}

    private void Land()
    {
        Debug.Log("Land");
        myAnimator.SetTrigger(ANIMATION_PARAM_LAND_TRIGGER);
        isJumping = false;
    }

    public void Jump(float JumpForce)
    {
        Vector2 playerVelocity = new Vector2(-JumpForce, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        isJumping = true;
        myAnimator.SetTrigger(ANIMATION_PARAM_JUMP_TRIGGER);
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

    //private bool IsOnGround()
    //{
    //    return (myCollider.IsTouchingLayers(LayerMask.GetMask(Constants.LAYER_FOREGROUND)));
        
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision: " + collision.gameObject.ToString());
        if (isJumping && collision.gameObject.CompareTag(Constants.TAG_FOREGROUND))
        {
            Land();
        }
    }
}
