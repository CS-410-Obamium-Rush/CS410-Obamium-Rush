using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamageDealer : MonoBehaviour
{
    // For responding to a collision intended to deal damage
    public GameMonitor gm;
    public GameObject explosionPrefab;
    public float invincibilityDuration = 1f;
    private static float lastHit = 0;

    // Attack collides with the player while the player has not recently been attacked
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && (Time.time - lastHit) > invincibilityDuration) {
            // Apply damage
            gm.playerTakeDamage(10);
            // Place explosion
            Instantiate(explosionPrefab, other.gameObject.transform.position, Quaternion.identity);
            // Launch player
            other.gameObject.GetComponent<ThirdPersonMovement>().OnJump(null);
            // Make sure this object doesn't apply damage for some time
            lastHit = Time.time;
        }
    }
}
