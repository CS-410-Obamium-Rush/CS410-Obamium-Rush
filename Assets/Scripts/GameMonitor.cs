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
    
    public int headHealth1 = 300;
    public int headHealth2 = 0;
    public int headHealth3 = 0;

    private int maxEnemyHealth;   // Max amount of health
    private int enemyTotalHealth; // Current health

    // For Player
    private int maxPlayerHealth; // Max amount of health
    public int playerHealth = 100; // Current health
    private bool gameOver = false;
    
    // For Powerups (to be implemented)
    // (ex. Percent = 0.45 means that enemy must have (0.45 * maxHealth) for Val or less to give the next power up)
    public float[] enemyThresholdPercent; // Holds the percent of health needed to damage to drop powerup
    private float[] enemyThresholdVal; // The value of the enemy health needs to be at or lower to drop the powerup
    public List<GameObject> powerups;
    public bool powerup1 = false;

    // To notify when the enemy has lost all of its health
    public GameEnding end;              // Used to run code that will continue the game after the enemy loses all their health
    private bool startPhase2 = true;    // Used to trigger the second enemy phase once
    private bool startPhase3 = true;    // Used to trigger the third enemy phase once
    private bool allowPhase3 = false;
    private bool allowWin = false;
    private int phaseCount = 0;         // Used to indicate how many phases that the player has defeated

    // For adjusting the lock on aiming with hands that are still active
    public lockOnAiming loa;

    // For updating increase in player score
    public ScoreKeeper scoreKeep;

    // Public Function used by NextPhase to establish the next phases' health; returns the maxium health for reference
    public int setNewHealth(int r1, int l1, int r2, int l2, int head1, int head2, int head3) {
        rightHandHealth1 = r1;
        leftHandHealth1 = l1;
        rightHandHealth2 = r2;
        leftHandHealth2 = l2;
        headHealth1 = head1;
        headHealth2 = head2;
        headHealth3 = head3;
        maxEnemyHealth = headHealth1 + rightHandHealth1 + leftHandHealth1 + rightHandHealth2 + leftHandHealth2 + headHealth2 + headHealth3;
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
    // Public function to indicate that the player can win once phase 3's health has been depleted
    public void setAllowWin() {
        allowWin = true;
    }

    public void setAllowPhase3() {
        allowPhase3 = true;
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
        // If the game is over, make sure the enemy cannot recieve damage
        if (gameOver)
            return;
        // Check if right hand is active
        if (body == 0 && rightHandHealth1 > 0) {
            rightHandHealth1 -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            if (rightHandHealth1 <= 0) {
                rightHandHealth1 = 0;
                atkPat.disableBody(body);
                loa.removeTarget("RH1");
            }
        }
        else if (body == 1 && leftHandHealth1 > 0) {
            leftHandHealth1 -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            if (leftHandHealth1 <= 0) {
                leftHandHealth1 = 0;
                atkPat.disableBody(body);
                loa.removeTarget("LH1");
            }
        }
        else if (body == 3 && rightHandHealth2 > 0) {
            rightHandHealth2 -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            if (rightHandHealth2 <= 0) {
                rightHandHealth2 = 0;
                atkPat.disableBody(body);
                loa.removeTarget("RH2");
            }
        }
        else if (body == 4 && leftHandHealth2 > 0) {
            leftHandHealth2 -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            if (leftHandHealth2 <= 0) {
                leftHandHealth2 = 0;
                atkPat.disableBody(body);
                loa.removeTarget("LH2");
            }
        }
        else if (body == 2) {
            if (leftHandHealth1 <= 0 && rightHandHealth1 <= 0 && leftHandHealth2 <= 0 && rightHandHealth2 <= 0 && headHealth1 > 0) {
                headHealth1 -= amt;
                calcEnemyHealth();
                enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
                if (headHealth1 <= 0) {
                    headHealth1 = 0;
                    atkPat.disableBody(body);
                    loa.removeTarget("Head1");
                }
            }
        }
        else if (body == 5) {
            if (leftHandHealth1 <= 0 && rightHandHealth1 <= 0 && leftHandHealth2 <= 0 && rightHandHealth2 <= 0 && headHealth2 > 0) {
                headHealth2 -= amt;
                calcEnemyHealth();
                enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
                if (headHealth2 <= 0) {
                    headHealth2 = 0;
                    atkPat.disableBody(body);
                    loa.removeTarget("Head2");
                }
            }
        }
        else if (body == 6) {
            if (leftHandHealth1 <= 0 && rightHandHealth1 <= 0 && leftHandHealth2 <= 0 && rightHandHealth2 <= 0 && headHealth3 > 0) {
                headHealth3 -= amt;
                calcEnemyHealth();
                enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
                if (headHealth3 <= 0) {
                    headHealth3 = 0;
                    atkPat.disableBody(body);
                    loa.removeTarget("Head3");
                }
            }
        }
    }

    /* 
    Public Functions for player's health; these are used by enemy attacks and attack behaviors
    */
    public void playerTakeDamage(int amt) {
        if (!gameOver) {
            playerHealth -= amt;
            playerBar.fillAmount = (float) playerHealth / maxPlayerHealth;
        }
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
        maxEnemyHealth = headHealth1 + rightHandHealth1 + leftHandHealth1 + rightHandHealth2 + leftHandHealth2 + headHealth2 + headHealth3;
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
            scoreKeep.setStopTimer();
            //music.playLoss();
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
            else if (allowPhase3 && startPhase3 && atkPat.getKey()) {
                music.playP3();
                startPhase3 = false;
                end.setPhase3();
                phaseCount++;
            }
            else if (allowWin) {
                phaseCount++;
            }
        }
        if (phaseCount >= 3) {
            gameOver = true;
            scoreKeep.setStopTimer();
            music.playWin();
            end.setWin();
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
        int healthH1 = headHealth1;
        int healthH2 = headHealth2;
        int healthH3 = headHealth3;

        if (rightHandHealth1 < 0)
            healthR1 = 0;
        if (leftHandHealth1 < 0)
            healthL1 = 0;
        if (rightHandHealth2 < 0)
            healthR2 = 0;
        if (leftHandHealth2 < 0)
            healthL2 = 0;
        if (headHealth1 < 0)
            healthH1 = 0;
        if (headHealth2 < 0)
            healthH2 = 0;
        if (headHealth3 < 0)
            healthH3 = 0;
        enemyTotalHealth = healthR1 + healthL1 + healthR2 + healthL2 + healthH1 + healthH2 + healthH3;
    }

    

    
}
