using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMonitor : MonoBehaviour
{
    // For Enemy
    public float rightHandHealth;
    public AttackPatterns atkPat;
    public float leftHandHealth;
    public float headHealth;
    private float enemyTotalHealth;

    // For Player
    public float playerHealth;
    
    // For Powerups
    public float enemyThreshold;
    private bool powerUp1 = false;

    // Update() manages the progress of game in terms of player and enemy health
    void Update()
    {
        // Update the overall remaining enemy health
        calcEnemyHealth();
        // If player loses all health, player loses
        if (playerHealth <= 0.0) {
            Debug.Log("You lose");
            // Enemy Wins
        }
        // If enemy loses all health, player wins
        else if (enemyTotalHealth <= 0.0) {
            Debug.Log("You Win");
            // Player wins/move on to next phase
        }
        // Release powerup when player reduces enough of enemy health; add more for additional powerups
        else if (enemyTotalHealth < enemyThreshold && powerUp1) {
            Debug.Log("Powerup Drop");
            powerUp1 = false;
            //Drop powerup
        }
    }

    /* Public Functions for player's health to be modified; use these in playerController and adjust if needed*/
    public void playerTakeDamage(float amt) {
        playerHealth -= amt;
    }
    public void playerAddHealth(float amt) {
        playerHealth += amt;
    }

    // Update the overall enemy health with calcEnemyHealth();
    private void calcEnemyHealth() {
        enemyTotalHealth = rightHandHealth + leftHandHealth + headHealth;
    }

    /* Function for Enemy's health to be modified; used in attack patterns*/
    /*
    0 == Right Hand
    1 == Left Hand
    2 == Head
    */
    public void enemyTakeDamage(float amt, int body) {
        if (body == 0) {
            rightHandHealth -= amt;
            if (rightHandHealth <= 0.0) {
                //Function to disable right hand
                atkPat.disableRight();
            }
        }
        else if (body == 1) {
            leftHandHealth -= amt;
            //Function to disable left hand
            atkPat.disableLeft();
        }
        else {
            headHealth -= amt;
            //Function to disable head
        }
    }
}