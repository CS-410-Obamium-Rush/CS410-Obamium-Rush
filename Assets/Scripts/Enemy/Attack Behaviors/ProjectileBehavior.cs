/*
ProjectileBehavior: Used by missle attacks to track the player and inflict damage as the body parts of not contacting the player
with this attack.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public int speed = 0;

    // Gather the gameobjects and details needed to have the enemy interact with the player
    public HeadBehavior head;
    public GameObject headObject;
    public Transform playerHitBox;
    public GameMonitor gm;
    //public GameObject gmObject;

    // use Update() for missles to approach the player
    void Update()
    {
        Vector3 playerPos = new Vector3(playerHitBox.position.x, 0, playerHitBox.position.z);
        transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
        transform.LookAt(playerPos);
    }

    // Check for collisions; make sure it hits either the player or the ground
    void OnTriggerEnter(Collider other) {
        // Player takes damage when the missles contact their hitbox
        if (other.gameObject.CompareTag("Player")) {
            gm.playerTakeDamage(10);
            gm.tryPowerup(transform.position);
            head.countMissle();
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Ground")) {
            gm.tryPowerup(transform.position);
            head.countMissle();
            Destroy(gameObject);
        }

    }
}
