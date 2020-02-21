using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSegment : MonoBehaviour
{
    const float SegmentSize = 15;
    bool HasSpawnedNextSegment = false;
    LevelSegment PriorSegment;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player && !HasSpawnedNextSegment)
        {
            //Debug.Log("player collision with Segment");
            LevelSegment newSegment = Instantiate(
                this,
                new Vector2(
                    this.transform.position.x,
                    this.transform.position.y + SegmentSize
                ),
                Quaternion.identity
            );
            newSegment.PriorSegment = this;


            HasSpawnedNextSegment = true;

            if (PriorSegment)
            {
                LevelSegment twoSegmentsAgo = PriorSegment.PriorSegment;
                if (twoSegmentsAgo)
                {
                    Debug.Log("destroy segment");
                    Destroy(twoSegmentsAgo.gameObject);
                }
            }
        }

    }
}