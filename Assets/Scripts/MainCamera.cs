using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // config
    [SerializeField] GameObject player;

    // other
    float playerStartingY;

    // Start is called before the first frame update
    void Start()
    {
        playerStartingY = player.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = player.transform.position.y - playerStartingY;
        transform.position = cameraPosition;
    }
}
