using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMonitor : MonoBehaviour
{
    // For health bars
    public Image playerBar;
    public Image enemyBar;


    // For Enemy
    public int rightHandHealth = 300;
    public AttackPatterns atkPat;
    public int leftHandHealth = 300;
    public int headHealth = 300;
    private int maxEnemyHealth;
    private int enemyTotalHealth;

    // For Player
    public int playerHealth = 100;
    private int maxPlayerHealth;
    
    // For Powerups
    public int enemyThreshold;
    private bool powerUp1 = false;

    public GameEnding end;
    void Start() {
        maxPlayerHealth = playerHealth;
        maxEnemyHealth = headHealth + rightHandHealth + leftHandHealth;
    }

    // Update() manages the progress of game in terms of player and enemy health
    void Update()
    {
        // Update the overall remaining enemy health
        calcEnemyHealth();
        // If player loses all health, player loses
        if (playerHealth <= 0) {
            end.setLost();
            // Enemy Wins
        }
        // If enemy loses all health, player wins
        else if (enemyTotalHealth <= 0) {
            end.setWin();
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

    /* Function for Enemy's health to be modified; used in attack patterns*/
    /*
    0 == Right Hand
    1 == Left Hand
    2 == Head
    */
    public void enemyTakeDamage(int amt, int body) {
        //Debug.Log("Damage: " + body);
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
            //Function to disable left hand
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

    public bool handsDefeated() {
        return (leftHandHealth <= 0) && (rightHandHealth <= 0);
    }
}
