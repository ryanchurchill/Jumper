using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpState
{
    JUMPING,
    FALLING,
    WALKING
}

public class Player : MonoBehaviour
{
    // config
    [SerializeField] float MovementSpeed = 1.0f;
    [SerializeField] float JumpForceMultiplier = 10f;
    [SerializeField] GameSession gameSession;
    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource hazardCollisionSound;
    [SerializeField] AudioSource ceilingCollisionSound;
    [SerializeField] AudioSource landOnFloorSound;
    [SerializeField] AudioSource pickupCoinSound;

    // state
    bool isAlive = true;
    public JumpState jumpState = JumpState.WALKING;

    // cached components
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider;
    SpriteRenderer spriteRenderer;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            MoveForward();
            JumpToFallTransition();
        }
    }

    private void MoveForward()
    {
        Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, MovementSpeed);
        myRigidBody.velocity = playerVelocity;
    }

    private void JumpToFallTransition()
    {
        if (jumpState == JumpState.JUMPING && myRigidBody.velocity.x > Mathf.Epsilon)
        {
            Debug.Log("Falling");
            jumpState = JumpState.FALLING;
        }
    }

    private void Land()
    {
        Debug.Log("Land");
        jumpState = JumpState.WALKING;
        myAnimator.SetTrigger(ANIMATION_PARAM_LAND_TRIGGER);
        landOnFloorSound.Play();
    }

    // jumpForce is 0-1
    public void Jump(float jumpForce)
    {
        Debug.Log("Jump force: " + jumpForce);
        jumpState = JumpState.JUMPING;
        jumpSound.volume = jumpForce;
        jumpSound.Play();

        // playing with velocity so that a jump when the player is already moving up will not cause it to stop
        float currentXVelocity = myRigidBody.velocity.x;
        float newTargetVelocty = -jumpForce * JumpForceMultiplier;
        float newVelocity = Mathf.Clamp(newTargetVelocty + Mathf.Clamp(currentXVelocity, -JumpForceMultiplier, 0), -JumpForceMultiplier, 0);

        Vector2 playerVelocity = new Vector2(newVelocity, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        myAnimator.SetTrigger(ANIMATION_PARAM_JUMP_TRIGGER);
        SetDarkness(0);
    }

    /*
     * darkness: 0-1
     */
    public void SetDarkness(float darkness)
    {
        /*
         * Lightest: RGBA(1.000,     1.000, 1.000, 1.000)
         * Darkest:  RGBA(0.7157331, 0,     1,     1)
         * 
         */

        Color lightest = new Color(1, 1, 1);
        Color darkest = new Color(.7157331f, 0, 1);
        Color color = Color.Lerp(lightest, darkest, darkness);
        spriteRenderer.color = color;
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
                pickupCoinSound.Play();
            }
        }
    }

    private void Die(Vector2 deathForce)
    {
        //Debug.Log("die");
        hazardCollisionSound.Play();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Colliding with " + collision.gameObject.name);
        //Debug.Log(myRigidBody.velocity.x);
        if (collision.gameObject.tag == Constants.TAG_CEILING)
        {
            Debug.Log("Hit Ceiling");
            ceilingCollisionSound.Play();
        }
        else if (collision.gameObject.tag == Constants.TAG_GROUND)
        {
            if (jumpState == JumpState.FALLING)
            {
                Land();
            }
        }
    }
}
