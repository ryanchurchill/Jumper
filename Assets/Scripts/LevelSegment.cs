using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSegment : MonoBehaviour
{
    const float SegmentSize = 15;
    bool hasSpawnedNextSegment = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player && !hasSpawnedNextSegment)
        {
            //Debug.Log("player collision with Segment");
            Instantiate(
                this,
                new Vector2(
                    this.transform.position.x,
                    this.transform.position.y + SegmentSize
                ),
                Quaternion.identity
            );

            hasSpawnedNextSegment = true;
        }

    }
}