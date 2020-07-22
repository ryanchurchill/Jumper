using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // config
    [SerializeField] float MovementSpeed = 1.0f;
    [SerializeField] float JumpForceMultiplier = 10f;
    [SerializeField] GameSession gameSession;
    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource hazardCollisionSound;
    [SerializeField] AudioSource ceilingCollisionSound;

    // state
    bool isAlive = true;
    bool isStartingJump = false;

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

    public void Jump(float jumpForce)
    {
        Debug.Log(gameObject.GetComponent<SpriteRenderer>().color);
        Debug.Log("Jump force: " + jumpForce);
        isStartingJump = true;
        jumpSound.volume = jumpForce; // TODO: magic number. 10 is max jumpSlider.value
        jumpSound.Play();
        Vector2 playerVelocity = new Vector2(-jumpForce * JumpForceMultiplier, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        myAnimator.SetTrigger(ANIMATION_PARAM_JUMP_TRIGGER);
        StartCoroutine(StopStartingJump());
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
        if (collision.gameObject.tag == Constants.TAG_CEILING)
        {
            Debug.Log("Hit Ceiling");
            ceilingCollisionSound.Play();
        }
    }
}
