using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    void OnCollisionEnter(Collision other) {
        // Pick up the powerup
        if (other.gameObject.CompareTag("Player")) {
            // Destroy the collected powerup
            Destroy(gameObject);
            //
            // Shotgun powerup logic goes here
            //
        }
    }

    void OnTriggerEnter(Collider other) {
        // Destroy this powerup on collision with a powerup destroyer
        if (other.gameObject.CompareTag("pDestroy")) {
            Destroy(gameObject);
        }
    }
}
