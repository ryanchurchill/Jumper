using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // config
    [SerializeField] float MovementSpeed = 1.0f;
    //[SerializeField] float JumpForce = 5.0f; // temp!

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
        Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, MovementSpeed);
        myRigidBody.velocity = playerVelocity;
    }

    public void Jump(float JumpForce)
    {
        Vector2 playerVelocity = new Vector2(-JumpForce, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
    }
}
