/*
PlayerBelow: A script to prevent the player from accidently clipping through the floor and be beneath the road
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBelow : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // There is a glitch where a head punch attack causes the player to be knocked below the map, so this checks 
        // if the y value is below the map, and will put the player back on the map when the glitch occurs
        if (transform.position.y < 0f) {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }
        // It may also be possible for the player to be too far to the left or right, so put the player back in the map if those
        // glitches occur
        else if (transform.position.x > 12.5f) {
            transform.position = new Vector3(12, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -12.5f) {
            transform.position = new Vector3(-12, transform.position.y, transform.position.z);
        }

    }
}
