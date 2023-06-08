using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamageDealer : MonoBehaviour
{
    // For responding to a collision intended to deal damage
    public GameMonitor gm;
    public GameObject explosionPrefab;
    private static bool invincible = false;

    // Attack collides with the player while the player has not recently been attacked
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !invincible) {
            // Apply damage
            gm.playerTakeDamage(10);
            // Place explosion
            GameObject explosionInstance = Instantiate(explosionPrefab, other.gameObject.transform.position, Quaternion.identity);
            // Launch player
            other.gameObject.GetComponent<ThirdPersonMovement>().OnJump(null);
            // Make sure this object doesn't apply damage for some time
            invincible = true;
            // Start co-routine to disable invincibility after some time
            StartCoroutine(flashDamageColor(explosionInstance));
        }
    }

    // Used for a cooldown when the player takes damage
    IEnumerator flashDamageColor(GameObject explosionInstance)
    {
        // Wait 1 second before the player can take damage again;
        // Planning to have the player sprite flashing in the near future
        yield return new WaitForSeconds(1f);
        Destroy(explosionInstance);
        invincible = false;
    }
}
