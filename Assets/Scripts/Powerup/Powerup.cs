using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public GameMonitor gameMonitor;
    public ScoreKeeper scoreKeeper;

    void OnCollisionEnter(Collision other) {
        // Pick up the powerup
        if (other.gameObject.CompareTag("Player")) {
            // Apply the effects of the powerup
            action(other.gameObject);
            // Destroy the collected powerup
            Destroy(gameObject);
        }
    }

    protected virtual void action(GameObject player) {
        // Override
    }

    void OnTriggerEnter(Collider other) {
        // Destroy this powerup on collision with a powerup destroyer
        if (other.gameObject.CompareTag("pDestroy")) {
            Destroy(gameObject);
        }
    }
}
