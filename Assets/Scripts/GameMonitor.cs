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
    public int headHealth1 = 300;
    public int headHealth2 = 0;
    public int headHealth3 = 0;
    private int maxEnemyHealth;   // Max amount of health
    private int enemyTotalHealth; // Current health
    public AttackPatterns atkPat;

    // For adjusting the lock on aiming with hands that are still active
    public lockOnAiming loa;

    // For Player
    private int maxPlayerHealth; // Max amount of health
    public int playerHealth = 100; // Current health
    private bool gameOver = false; // This is used to preven the player taking damage after the win/loss canvas starts to appear

    // For updating increase in player score
    public ScoreKeeper scoreKeep;
    
    // For Powerups (to be implemented)
    // (ex. Percent = 0.45 means that enemy must have (0.45 * maxHealth) for Val or less to give the next power up)
    public float[] enemyThresholdPercent; // Holds the percent of health needed to damage to drop powerup
    private float[] enemyThresholdVal; // The value of the enemy health needs to be at or lower to drop the powerup

    public ParticleSystem[] handParticles;
    public ParticleSystem[] phaseChangeParticles;
    public List<GameObject> powerups;
    public List<AudioSource> powerupPickupSounds;
    public bool powerup1 = false;

    // To notify when the enemy has lost all of its health
    public GameEnding end;              // Used to run code that will continue the game after the enemy loses all their health
    private bool startPhase2 = true;    // Used to trigger the second enemy phase once
    private bool startPhase3 = true;    // Used to trigger the third enemy phase once
    private bool allowPhase3 = false;
    private bool allowWin = false;
    private int phaseCount = 0;         // Used to indicate how many phases that the player has defeated

    [HideInInspector]
    public float screenShake = 0f;
    private float screenShakeDecay = 0.8f;

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
    5 == Head 2 (Phase 3)
    6 == Head 3 (Phase 3)

    Hand Structure:
        Check if damage can still be taken
        Reduce health for the hand
        Update on the total amount of health
        Update the enemy health bar
        Increase the player's score
        If the hand lost all of its health
            Disable the hand and it can no longer attack
    
    Head Structure:
        Only when all the hands have been defeated
            Reduce health for the head
            Update on the total amount of health
            Update the enemy health bar
            Increase the player's score
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
            scoreKeep.addScore(amt * 10);
            if (rightHandHealth1 <= 0) {
                rightHandHealth1 = 0;
                atkPat.disableBody(body);
                handParticles[0].Play();
                loa.removeTarget("RH1");
            }
        }
        else if (body == 1 && leftHandHealth1 > 0) {
            leftHandHealth1 -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            scoreKeep.addScore(amt * 10);
            if (leftHandHealth1 <= 0) {
                leftHandHealth1 = 0;
                atkPat.disableBody(body);
                handParticles[1].Play();
                loa.removeTarget("LH1");
            }
        }
        else if (body == 3 && rightHandHealth2 > 0) {
            rightHandHealth2 -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            scoreKeep.addScore(amt * 10);
            if (rightHandHealth2 <= 0) {
                rightHandHealth2 = 0;
                atkPat.disableBody(body);
                handParticles[2].Play();
                loa.removeTarget("RH2");
            }
        }
        else if (body == 4 && leftHandHealth2 > 0) {
            leftHandHealth2 -= amt;
            calcEnemyHealth();
            enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
            scoreKeep.addScore(amt * 10);
            if (leftHandHealth2 <= 0) {
                leftHandHealth2 = 0;
                atkPat.disableBody(body);
                handParticles[3].Play();
                loa.removeTarget("LH2");
            }
        }
        else if (body == 2) {
            if (leftHandHealth1 <= 0 && rightHandHealth1 <= 0 && leftHandHealth2 <= 0 && rightHandHealth2 <= 0 && headHealth1 > 0) {
                headHealth1 -= amt;
                calcEnemyHealth();
                enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
                scoreKeep.addScore(amt * 10);
                if (headHealth1 <= 0) {
                    headHealth1 = 0;
                    atkPat.disableBody(body);
                    phaseChangeParticles[0].Play();
                    loa.removeTarget("Head1");
                }
            }
        }
        else if (body == 5) {
            if (leftHandHealth1 <= 0 && rightHandHealth1 <= 0 && leftHandHealth2 <= 0 && rightHandHealth2 <= 0 && headHealth2 > 0) {
                headHealth2 -= amt;
                calcEnemyHealth();
                enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
                scoreKeep.addScore(amt * 10);
                if (headHealth2 <= 0) {
                    headHealth2 = 0;
                    atkPat.disableBody(body);
                    phaseChangeParticles[1].Play();
                    loa.removeTarget("Head2");
                }
            }
        }
        else if (body == 6) {
            if (leftHandHealth1 <= 0 && rightHandHealth1 <= 0 && leftHandHealth2 <= 0 && rightHandHealth2 <= 0 && headHealth3 > 0) {
                headHealth3 -= amt;
                calcEnemyHealth();
                enemyBar.fillAmount = (float) enemyTotalHealth / maxEnemyHealth;
                scoreKeep.addScore(amt * 10);
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
            addScreenShake(0.1f);
            playerHealth -= amt;
            playerBar.fillAmount = (float) playerHealth / maxPlayerHealth;
            scoreKeep.removeScore(amt * 10);
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
        if (Random.value < 0.6) {
            // Instantiate a random powerup at the given position
            float randn = Random.value;
            int pIndex;
            if (randn < 0.1) {
                pIndex = 3; // Collectable
            } else if (randn < 0.5) {
                pIndex = 0; // Flamethrower
            } else if (randn < 0.6) {
                pIndex = 2; // Health
            } else {
                pIndex = 1; // Shotgun
            }
            GameObject powerup = Instantiate(powerups[pIndex], position, Quaternion.identity);
            // Create a force vector
            Vector3 force = new Vector3(0, 20, 20);
            // Adjust the force vector left or right so that it lands on the floor tiles
            if (position.x < -10 || position.x > 10) {
                force += new Vector3(position.x * -1, 0, 0);
            }
            // Get the rigidbody and apply the force vector as an Impulse
            Rigidbody powerupRigidbody = powerup.GetComponent<Rigidbody>();
            powerupRigidbody.AddForce(force, ForceMode.Impulse);

            Powerup powerupScript = powerup.GetComponent<Powerup>();
            powerupScript.gameMonitor = this;
            powerupScript.scoreKeeper = scoreKeep;
            // Supply a pick up sound if one exists
            if (powerupPickupSounds[pIndex])
                powerupScript.pickupAudioSource = powerupPickupSounds[pIndex];
        }
    }

    public void addScreenShake(float amount) {
        screenShake += amount;
        if (screenShake > 1)
            screenShake = 1;
    }

    public void setScreenShake(float amount) {
        screenShake = amount;
        if (screenShake > 1)
            screenShake = 1; 
    }

    // Get the max amount of health that the player and enemy can have at a time (for phase 1)
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
        if (gameOver) {
            return;
        }

        // Update screenShake
        if (screenShake > 0)
            screenShake -= screenShakeDecay * Time.deltaTime;
        else
            screenShake = 0;

        // Update the overall remaining enemy health
        calcEnemyHealth();

        // If player loses all health, player loses and restarts level
        if (playerHealth <= 0) {
            gameOver = true;
            scoreKeep.setStopTimer();
            music.playLoss();
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
            phaseChangeParticles[2].Play();
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
