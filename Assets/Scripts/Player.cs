using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // config
    [SerializeField] float MovementSpeed = 1.0f;

    // state
    bool isAlive = true;

    // cached components
    Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, MovementSpeed);
            myRigidBody.velocity = playerVelocity;
        }
    }

    public void Jump(float JumpForce)
    {
        Vector2 playerVelocity = new Vector2(-JumpForce, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Constants.TAG_HAZARD)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("die");
        isAlive = false;
    }
}
