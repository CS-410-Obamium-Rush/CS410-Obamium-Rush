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
    private HeadBehavior head;
    private GameObject headObject;
    private Transform playerHitBox;
    private GameMonitor gm;
    private GameObject gmObject;

    // Use Start() to get all the information needed from the player (the missles' target) and enemy (to get countMissle())
    void Start()
    {
        gmObject = GameObject.Find("GameSettings");
        playerHitBox = GameObject.Find("MovingPlayer").transform.GetChild(0).gameObject.transform;
        gm = gmObject.GetComponent<GameMonitor>();
        headObject = GameObject.Find("Enemy").transform.GetChild(0).transform.gameObject;
        head = headObject.GetComponent<HeadBehavior>();
        if(!headObject.activeSelf) {
            headObject = GameObject.Find("Enemy").transform.GetChild(2).transform.gameObject;
            head = headObject.GetComponent<HeadBehavior>();
        }
        transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    // use Update() for missles to approach the player
    void Update()
    {
        Vector3 playerPos = new Vector3(playerHitBox.position.x, 0, playerHitBox.position.z);
        transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
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
