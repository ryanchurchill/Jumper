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
    [SerializeField] Vector2 initialPosition;
    [SerializeField] Vector2 endPosition;
    [SerializeField] Vector2 myVelocity = Vector2.zero;
    [SerializeField] bool headingToEndElseStart = true;

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
        transform.position = Vector2.SmoothDamp(
            transform.position, getCurrentDestination(), ref myVelocity, 1f);
        flipIfAtDestination();
    }

    private void flipIfAtDestination()
    {
        Vector2 destinationPosition = getCurrentDestination();

        if (Vector2.Distance(transform.position, destinationPosition) <= .1)
        {
            headingToEndElseStart = !headingToEndElseStart;
        }
    }

    private Vector2 getCurrentDestination()
    {
        return headingToEndElseStart ? endPosition : initialPosition;
    }
}
