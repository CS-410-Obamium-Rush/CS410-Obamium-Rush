using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamageDealer : MonoBehaviour
{
    // For responding to a collision intended to deal damage
    public GameMonitor gm;
    private static bool invincible = false;

    // Attack collides with the player while the player has not recently been attacked
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !invincible) {
            gm.playerTakeDamage(10);
            invincible = true;
            StartCoroutine(flashDamageColor());
        }
    }

    // Used for a cooldown when the player takes damage
    IEnumerator flashDamageColor()
    {
        // Wait 1 second before the player can take damage again;
        // Planning to have the player sprite flashing in the near future
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
}
