using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// rename to back-and-forth
public class MovingObject : MonoBehaviour
{
    // config
    [SerializeField] Direction direction = Direction.North;
    [SerializeField] long length = 10;
    [SerializeField] long speed = 1;

    // state
    Vector2 initialPosition;
    Vector2 endPosition;

    // cached components
    Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        long x = 0;
        long y = 0;

        switch (direction)
        {
            case Direction.North:
                y = speed;
                break;
            case Direction.South:
                y = -speed;
                break;
            case Direction.East:
                x = speed;
                break;
            case Direction.West:
                x = -speed;
                break;
        }

        Vector2 newVolocity = new Vector2(x, y);
        myRigidBody.velocity = newVolocity;
    }

    private void flipIfTraveledToLength()
    {
    }
}
