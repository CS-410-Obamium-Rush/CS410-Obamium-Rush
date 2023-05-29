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

    // ethan's integration of audio
    public battleMusicManager music;
    AudioSource m_AudioSource;

    // For Enemy
    public int rightHandHealth1 = 300;
    public int leftHandHealth1 = 300;
    public int rightHandHealth2 = 0;
    public int leftHandHealth2 = 0;
    public AttackPatterns atkPat;
    
    public int headHealth = 300;
    private int maxEnemyHealth;   // Max amount of health
    private int enemyTotalHealth; // Current health

    // For Player
    private int maxPlayerHealth; // Max amount of health
    public int playerHealth = 100; // Current health
    
    // For Powerups (to be implemented)
    // (ex. Percent = 0.45 means that enemy must have (0.45 * maxHealth) for Val or less to give the next power up)
    public float[] enemyThresholdPercent; // Holds the percent of health needed to damage to drop powerup
    private float[] enemyThresholdVal; // The value of the enemy health needs to be at or lower to drop the powerup

    public List<GameObject> powerups;
    
    public bool powerup1 = false;

    // To notify when the enemy has lost all of its health
    public GameEnding end;              // Used to run code that will continue the game after the enemy loses all their health
    private bool startPhase2 = true;    // Used to trigger the second enemy phase once
    private int phaseCount = 0;         // Used to indicate how many phases that the player has defeated

    // Public Function used by NextPhase to establish the next phases' health; returns the maxium health for reference
    public int setNewHealth(int r1, int l1, int r2, int l2, int head) {
        rightHandHealth1 = r1;
        leftHandHealth1 = l1;
        rightHandHealth2 = r2;
        leftHandHealth2 = l2;
        headHealth = head;
        maxEnemyHealth = headHealth + rightHandHealth1 + leftHandHealth1 + rightHandHealth2 + leftHandHealth2;
        enemyTotalHealth = maxEnemyHealth;
        return maxEnemyHealth;
    }

    // Public function to let other scripts know whether all the hands have been defeated
    public bool handsDefeated() {
        return ((leftHandHealth1 <= 0) && (rightHandHealth1 <= 0) && (leftHandHealth2 <= 0) && (rightHandHealth2 <= 0));
    }

    // Public function to set enemy threshold for powerups, send the percents 
    public void setThreshold(float percent1, float percent2, float percent3) {
        enemyThresholdPercent[0] = percent1;
        enemyThresholdPercent[1] = percent2;
        enemyThresholdPercent[2] = percent3;
        for (int i = 0; i < 3; i++) {
            enemyThresholdVal[i] = maxEnemyHealth * enemyThresholdPercent[i];
        }
    }

    /* Function for Enemy's health to be modified; used by EnemyDamageDedeuction scripts*/
    /*
    0 == Right Hand
    1 == Left Hand
    2 == Head
    3 == Right Hand (Phase 2)
    4 == Left Hand (Phase 2)
    Hand Structure:
        Check if damage can still be taken
        Reduce health for the hand
        Update on the total amount of health
        Update the enemy health bar
        If the hand lost all of its health
            Disable the hand and it can no longer attack
    
    Head Structure:
        Only when all the hands have been defeated
            Reduce health for the head
            Update on the total amount of health
            Update the enemy health bar
    */

    public void enemyTakeDamage(int amt, int body) {
        // Check if right hand is active
        if (body == 0 && rightHandHealth1 > 0) {
            rightHandHealth1 -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            if (rightHandHealth1 <= 0) {
                rightHandHealth1 = 0;
                atkPat.disableBody(body);
            }
        }
        else if (body == 1 && leftHandHealth1 > 0) {
            leftHandHealth1 -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            if (leftHandHealth1 <= 0) {
                leftHandHealth1 = 0;
                atkPat.disableBody(body);
            }
        }
        else if (body == 3 && rightHandHealth2 > 0) {
            rightHandHealth2 -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            if (rightHandHealth2 <= 0) {
                rightHandHealth2 = 0;
                atkPat.disableBody(body);
            }
        }
        else if (body == 4 && leftHandHealth2 > 0) {
            leftHandHealth2 -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            if (leftHandHealth2 <= 0) {
                leftHandHealth2 = 0;
                atkPat.disableBody(body);
            }
        }
        else if (body == 2) {
            if (leftHandHealth1 <= 0 && rightHandHealth1 <= 0 && leftHandHealth2 <= 0 && rightHandHealth2 <= 0 && headHealth > 0) {
                headHealth -= amt;
                calcEnemyHealth();
                enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
                if (headHealth <= 0) {
                    headHealth = 0;
                    atkPat.disableBody(body);
                }
            }
        }
    }

    /* 
    Public Functions for player's health; these are used by enemy attacks and attack behaviors
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

    public void tryPowerup(Vector3 position) {
        // Random chance to drop a powerup
        if (Random.value < 0.3) {
            // Instantiate a random powerup at the given position
            GameObject powerup = Instantiate(powerups[Random.Range(0, powerups.Count)], position, Quaternion.identity);
            // Create a force vector
            Vector3 force = new Vector3(0, 20, 20);
            // Adjust the force vector left or right so that it lands on the floor tiles
            if (position.x < -10 || position.x > 10) {
                force += new Vector3(position.x * -1, 0, 0);
            }
            // Get the rigidbody and apply the force vector as an Impulse
            Rigidbody powerupRigidbody = powerup.GetComponent<Rigidbody>();
            powerupRigidbody.AddForce(force, ForceMode.Impulse);
        }
    } 

    // Get the max amount of health that the player and enemy can have at a time
    void Start() {
        maxPlayerHealth = playerHealth;
        maxEnemyHealth = headHealth + rightHandHealth1 + leftHandHealth1 + rightHandHealth2 + leftHandHealth2;
        enemyThresholdVal = new float[3];
        for (int i = 0; i < 3; i++) {
            enemyThresholdVal[i] = maxEnemyHealth * enemyThresholdPercent[i];
        }

        // get audioSource -- EA
        m_AudioSource = GetComponent<AudioSource>();
        music.playP1();
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
            if (startPhase2 && atkPat.getKey()) {

                music.playP2();
                startPhase2 = false;
                end.setPhase2();
                phaseCount++;
            }
            else if (phaseCount == 3) {
                Debug.Log("You Wind");
                end.setWin();
            }
                
        }

        // Release powerup when player reduces enough of enemy health; add more for additional powerups
        if ((float)enemyTotalHealth < enemyThresholdVal[0] && powerup1) {
            powerup1 = false;
        }
    }

    

    // Helper function to update the overall enemy health with calcEnemyHealth();
    private void calcEnemyHealth() {
        int healthR1 = rightHandHealth1;
        int healthL1 = leftHandHealth1;
        int healthR2 = rightHandHealth2;
        int healthL2 = leftHandHealth2;
        int healthH = headHealth;

        if (rightHandHealth1 < 0)
            healthR1 = 0;
        if (leftHandHealth1 < 0)
            healthL1 = 0;
        if (rightHandHealth2 < 0)
            healthR2 = 0;
        if (leftHandHealth2 < 0)
            healthL2 = 0;
        if (headHealth < 0)
            healthH = 0;
        enemyTotalHealth = healthR1 + healthL1 + healthR2 + healthL2 + healthH;
    }

    

    
}
