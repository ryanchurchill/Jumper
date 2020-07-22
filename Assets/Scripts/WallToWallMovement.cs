using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallToWallMovement : MonoBehaviour
{
    // config
    [SerializeField] Direction direction = Direction.West;
    [SerializeField] long speed = 100;

    // state

    // cached components
    Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponentInParent<Rigidbody2D>();

        myRigidBody.AddForce(new Vector2(-speed, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddForceInDirection()
    {
        float xForce = 0;
        float yForce = 0;
        if (direction == Direction.West)
        {
            xForce = -speed;
        }
        else if (direction == Direction.East)
        {
            xForce = speed;
        }
        // yForce not supported
        myRigidBody.AddForce(new Vector2(xForce, yForce));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Constants.TAG_CEILING)
        {
            FlipEast();
        }
        else if (collision.gameObject.tag == Constants.TAG_GROUND)
        {
            FlipWest();
        }
    }

    private void FlipEast()
    {
        if (direction == Direction.West)
        {
            direction = Direction.East;
            AddForceInDirection();
        }
    }

    private void FlipWest()
    {
        if (direction == Direction.East)
        {
            direction = Direction.West;
            AddForceInDirection();
        }
    }
}
