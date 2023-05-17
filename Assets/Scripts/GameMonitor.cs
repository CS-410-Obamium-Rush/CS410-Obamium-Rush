/*
GameMonitor: Manages the health of both the player and enemy throughout the game. Updates on both their amounts when a collision
is made and notify GameEnding when which one lost all their health. It will also adjust each health bar on the screen.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMonitor : MonoBehaviour
{
    // For health bar image to be adjusted
    public Image playerBar;
    public Image enemyBar;

    // For Enemy
    public int rightHandHealth = 300;
    public AttackPatterns atkPat;
    public int leftHandHealth = 300;
    public int headHealth = 300;
    private int maxEnemyHealth;   // Max amount of health
    private int enemyTotalHealth; // Current health

    // For Player
    private int maxPlayerHealth; // Max amount of health
    public int playerHealth = 100; // Current health
    
    // For Powerups (to be implemented)
    public int enemyThreshold;
    private bool powerUp1 = false;

    // To notify when the game has ended
    public GameEnding end;

    // Get the max amount of health that the player and enemy can have at a time
    void Start() {
        maxPlayerHealth = playerHealth;
        maxEnemyHealth = headHealth + rightHandHealth + leftHandHealth;
    }

    // Update() manages the progress of game in terms of player and enemy health
    void Update()
    {
        // Update the overall remaining enemy health
        calcEnemyHealth();

        // If player loses all health, player loses and restarts level
        if (playerHealth <= 0) {
            end.setLost();
        }
        // If enemy loses all health, player wins and moves on to the next phase or game ends
        else if (enemyTotalHealth <= 0) {
            end.setWin();
        }

        // Release powerup when player reduces enough of enemy health; add more for additional powerups
        if (enemyTotalHealth < enemyThreshold && powerUp1) {
            powerUp1 = false;
        }
    }

    /* 
    Public Functions for player's health; used these in playerController
    fillAmount adjusts the health bars
    */
    public void playerTakeDamage(int amt) {
        playerHealth -= amt;
        playerBar.fillAmount = (float) playerHealth / maxPlayerHealth;
    }
    public void playerAddHealth(int amt) {
        if (playerHealth + amt > maxPlayerHealth)
            playerHealth = maxPlayerHealth;
        else
            playerHealth += amt;
        playerBar.fillAmount = (float) playerHealth / maxPlayerHealth;
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

    /* Function for Enemy's health to be modified; used by EnemyDamageDedeuction scripts*/
    /*
    0 == Right Hand
    1 == Left Hand
    2 == Head

    Hand Structure:
        Check if damage can still be taken
        Reduce health for the hand
        Update on the total amount of health
        Update the enemy health bar
        If the hand lost all of its health
            Disable the hand and it can no longer attack
    
    Head Structure:
        Only when the hands have been defeated
            Reduce health for the hands
            Update on the total amount of health
            Update the enemy health bar
    */

    public void enemyTakeDamage(int amt, int body) {
        // Check if right hand is active
        if (body == 0 && rightHandHealth > 0) {
            rightHandHealth -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            if (rightHandHealth <= 0) {
                rightHandHealth = 0;
                atkPat.disableRight();
            }
        }
        else if (body == 1 && leftHandHealth > 0) {
            leftHandHealth -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            if (leftHandHealth <= 0) {
                leftHandHealth = 0;
                atkPat.disableLeft();
            }
        }
        else if (body == 2) {
            if (leftHandHealth <= 0 && rightHandHealth <= 0 && headHealth > 0) {
                headHealth -= amt;
                calcEnemyHealth();
                enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            }
        }
    }

    // Public function to let other scripts know whether all the hands have been defeated
    public bool handsDefeated() {
        return (leftHandHealth <= 0) && (rightHandHealth <= 0);
    }
}
