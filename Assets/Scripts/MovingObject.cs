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
    Vector2 myVelocity = Vector2.zero;

    // cached components
    Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        endPosition = new Vector2(transform.position.x, transform.position.y - 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.SmoothDamp(transform.position, endPosition, ref myVelocity, 1f);
        flipIfTraveledToLength();
    }

    private void flipIfTraveledToLength()
    {
        if (myVelocity.x <= Mathf.Epsilon && myVelocity.y <= Mathf.Epsilon)
        {
            endPosition = new Vector2(transform.position.x, transform.position.y + 3);
        }
    }
}
