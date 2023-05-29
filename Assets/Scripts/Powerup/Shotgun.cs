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
            Transform transform = other.gameObject.transform;
            transform.Find("Bullets").gameObject.SetActive(false);
            transform.Find("Shotgun").gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other) {
        // Destroy this powerup on collision with a powerup destroyer
        if (other.gameObject.CompareTag("pDestroy")) {
            Destroy(gameObject);
        }
    }
}
