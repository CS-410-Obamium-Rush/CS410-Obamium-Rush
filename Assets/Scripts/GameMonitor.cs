using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMonitor : MonoBehaviour
{
    // For Enemy
    public int rightHandHealth = 300;
    public AttackPatterns atkPat;
    public int leftHandHealth = 300;
    public int headHealth = 300;
    private int enemyTotalHealth;

    // For Player
    public int playerHealth = 100;
    
    // For Powerups
    public int enemyThreshold;
    private bool powerUp1 = false;

    private bool doOnce1 = true;
    private bool doOnce2 = true;


    // Update() manages the progress of game in terms of player and enemy health
    void Update()
    {
        // Update the overall remaining enemy health
        calcEnemyHealth();
        // If player loses all health, player loses
        if (playerHealth <= 0) {
            if (doOnce1 == true) {
                Debug.Log("You lose");
                doOnce1 = false;
            }
            
            // Enemy Wins
        }
        // If enemy loses all health, player wins
        else if (enemyTotalHealth <= 0) {
            if (doOnce2 == true) {
                Debug.Log("You Win");
                doOnce2 = false;
            }
            
            // Player wins/move on to next phase
        }
        // Release powerup when player reduces enough of enemy health; add more for additional powerups
        if (enemyTotalHealth < enemyThreshold && powerUp1) {
            powerUp1 = false;
            //Drop powerup
        }
    }

    /* Public Functions for player's health to be modified; use these in playerController and adjust if needed*/
    public void playerTakeDamage(int amt) {
        playerHealth -= amt;
    }
    public void playerAddHealth(int amt) {
        playerHealth += amt;
    }

    // Update the overall enemy health with calcEnemyHealth();
    private void calcEnemyHealth() {
        int healthR = rightHandHealth;
        int healthL = leftHandHealth;
        int healthH = headHealth;
        if (rightHandHealth < 0)
            healthR = 0;
        if (leftHandHealth < 0)
            healthL = 0;
        if (headHealth < 0)
            healthH = 0;
        enemyTotalHealth = healthR + healthL + healthH;
    }

    /* Function for Enemy's health to be modified; used in attack patterns*/
    /*
    0 == Right Hand
    1 == Left Hand
    2 == Head
    */
    public void enemyTakeDamage(int amt, int body) {
        //Debug.Log("Damage: " + body);
        if (body == 0) {
            rightHandHealth -= amt;
            if (rightHandHealth <= 0) {
                atkPat.disableRight();
            }
        }
        else if (body == 1) {
            leftHandHealth -= amt;
            //Function to disable left hand
            if (leftHandHealth <= 0) {
                 atkPat.disableLeft();
            }
        }
        else {
            if (leftHandHealth <= 0 && rightHandHealth <= 0)
                headHealth -= amt;
            //Function to disable head
        }
    }

    public bool handsDefeated() {
        return (leftHandHealth <= 0) && (rightHandHealth <= 0);
    }
}
