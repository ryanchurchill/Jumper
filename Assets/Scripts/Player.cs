using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // config
    [SerializeField] float MovementSpeed = 1.0f;

    // cached components
    Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        StartMoving();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerVelocity = new Vector2(0, MovementSpeed);
        myRigidBody.velocity = playerVelocity;
    }

    void StartMoving()
    {
    //    Vector2 playerVelocity = new Vector2(0, MovementSpeed);
    //    myRigidBody.velocity = playerVelocity;
        //myRigidBody.AddForce(new Vector2(0, MovementSpeed));
    }
}
