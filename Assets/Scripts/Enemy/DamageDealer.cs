/*
DamageDealer: Allow the enemy to inflict damage to the player when colliding with them.

Extra Note: This is the same across enemy parts, but each one has their own instance so dmgAmt is static
to allow the damage infliction to be the same across all parts.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    // For responding to a collision intended to deal damage
    public GameMonitor gm;
    public static int dmgAmt = 0;
    private static bool invincible = false;

    // For alterning the sprite when taking damage (to be added)
    private GameObject player;
    private Renderer playerRenderer;
    Color initColor;

    // Allow the damage dealt to the player be altered in other scripts
    public void setDmg(int amt) {
        dmgAmt = amt;
    }


    // Start() is for alterning the sprite in the near future
    void start() {
        player = GameObject.Find("PlayerSprite");
        playerRenderer = player.GetComponent<Renderer>();
        initColor = playerRenderer.material.color;
    }

    // Attack collides with the player while the player has not recently been attacked
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !invincible) {
            // Set invincibility and gather the damage
            invincible = true;
            StartCoroutine(flashDamageColor());
            gm.playerTakeDamage(dmgAmt);
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
