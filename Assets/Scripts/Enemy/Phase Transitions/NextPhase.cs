/*
NextPhase: A script that performs the transition from phase 1 to phase 2 in the game; this occurs when the enemy's health has been depleted
for the first time.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NextPhase : MonoBehaviour
{
    // With performing the transition, this script needs control of all scripts and GameObjects associated with the enemy
    public AttackPatterns atkPat;           // Uses the AttackPatterm to disable the enemy from attacking the player
    public EnemyDamageDetection enDamDet;   // Uses EnemyDamageDetection to disable the enemy from taking damage
    public GameMonitor gm;                  // Uses GameMonitor to establish the health for phase 2
    public lockOnAiming loa;                // Uses lockOnAiming to add the additional hands into the targeting system
    /* Hand GameObjects and Scripts are to position and activate the hands for this phase*/
    public GameObject rightHand1;   
    public GameObject leftHand1;
    public GameObject rightHand2;
    public GameObject leftHand2;
    private HandBehavior lh1;
    private HandBehavior rh1;
    private HandBehavior lh2;
    private HandBehavior rh2;
    
    /* Head GameObjects are used to animate the phase change and replace the HeadBehavior used in AttackPattern with the new head
    (Goes from sphere to cube)
    */
    public GameObject headSphere;
    public GameObject headCube;
    private HeadBehavior hc; //Cube's head behavior script

    //ethan's audio
    public enemyNextPhaseAudioManager nextAudio;
    AudioSource m_AudioSource;

    // Tools to perform the phase operation
    public TextMeshProUGUI enemyName; // Update the name of the enemy
    public Image enemyBar;          // Update the enemy health to be at full health for the second phase
    private bool[] stepList;        // Provides a linear sequence for the operation to occur; i.e the phase transition occurs in steps
    Vector3 rh1Pos;                 // These establish the new location for the hands to be at during the second phase
    Vector3 rh2Pos; 
    Vector3 lh1Pos;
    Vector3 lh2Pos;
    private bool doOnce = true;     // A bool to have specific step parts occur only one time instead of repeatedly

    // Public variables for phase transition speeds
    public int spinSpeedSphere = 1000;
    public int spinSpeedCube = 100;
    public float shrinkSpeed = 10f;
    public float growSpeed = 5f;
    public float shiftSpeed = 5f;
    public float resetSpeed = 5f;

    // Start() establishes the amount of steps and gets all scripts associated from the relevant GameObjects
    void Start()
    {
        stepList = new bool[8];
        for (int i = 0; i < 8; i++) {
            stepList[i] = false;
        }
        rh1 = rightHand1.GetComponent<HandBehavior>();
        lh1 = leftHand1.GetComponent<HandBehavior>();
        rh2 = rightHand2.GetComponent<HandBehavior>();
        lh2 = leftHand2.GetComponent<HandBehavior>();
        hc = headCube.GetComponent<HeadBehavior>();

        // get audioSource -- EA
        m_AudioSource = GetComponent<AudioSource>();
    }

    /* 
    Update performs the phase transition process
    It uses StepList[] to perform the steps by starting at index 0 (step 1), perform all the parts, then moves on to the next step
    by setting the current step to be false and the next step to be true. Update then reiterates with the next step only 
    enabled until it is sets itself false and the next one to be true.
    */
    void Update()
    {
        // Step 1: Disable the ability to interact with enemy
        if (stepList[0]) {
            enemyNextPhaseAudioManager.instance.playDefeat();
            enDamDet.setPhaseTransition(true);
            atkPat.setPhaseTransition(true);
            stepList[0] = false;
            stepList[1] = true;
        }
        // Step 2: Have the Sphere head vanish by spinning and diminishing in size until it cannot be easily seen
        else if (stepList[1]) {
            headSphere.transform.Rotate(new Vector3(0, spinSpeedSphere, 0) * Time.deltaTime);
            Vector3 updateScale = headSphere.transform.localScale;
            if (updateScale.x != 1f)
                updateScale.x -= shrinkSpeed * Time.deltaTime;
            if (updateScale.y != 1f)
                updateScale.y -= shrinkSpeed * Time.deltaTime;
            if (updateScale.z != 1f)
                updateScale.z -= shrinkSpeed * Time.deltaTime;
            headSphere.transform.localScale = updateScale;
            if (updateScale.x <= 1f && updateScale.y <= 1f && updateScale.z <= 1f) {
                stepList[1] = false;
                stepList[2] = true;
            }
        }
        // Step 3: Put the cube into the scene and have it grow in size until it is slightly larger than the original head
        else if (stepList[2]) {
            headSphere.SetActive(false);
            headCube.SetActive(true);
            headCube.transform.GetChild(0).gameObject.SetActive(true);

            hc.setIdle(false);
            Vector3 updateScale = headCube.transform.localScale;
            headCube.transform.Rotate(new Vector3(0, spinSpeedCube, 0) * Time.deltaTime);
            if (updateScale.x != 15f)
                updateScale.x += growSpeed * Time.deltaTime;
            if (updateScale.y != 15f)
                updateScale.y += growSpeed * Time.deltaTime;
            if (updateScale.z != 15f)
                updateScale.z += growSpeed * Time.deltaTime;
            headCube.transform.localScale = updateScale;
            if (updateScale.x >= 14f && updateScale.y >= 14f && updateScale.z >= 14f) {
                headCube.transform.localEulerAngles = new Vector3(0, 0, 0);
                rh1.setPause();
                lh1.setPause();
                rh2.setPause();
                lh2.setPause();
                stepList[2] = false;
                stepList[3] = true;
            }
        }
        // Step 4: Have the first pair of hands get to their initial positions for when the game started
        else if (stepList[3]) {
            if (rh1.resetHand(resetSpeed) && lh1.resetHand(resetSpeed)) {
                stepList[3] = false;
                stepList[4] = true;
            }
        }
        // Step 5: Have the second pair of hands appear from the first pair, establish each of their new locations (Pos variable), and
        // Update the target system to include the modification of the head and new hands
        else if (stepList[4]) {
            rightHand2.SetActive(true);
            leftHand2.SetActive(true);
            rh1Pos = new Vector3 (rightHand1.transform.position.x, rightHand1.transform.position.y + 8f, rightHand1.transform.position.z);
            rh2Pos = new Vector3 (rightHand2.transform.position.x, rightHand2.transform.position.y - 8f, rightHand2.transform.position.z);
            lh1Pos = new Vector3 (leftHand1.transform.position.x, leftHand1.transform.position.y + 8f, leftHand1.transform.position.z);
            lh2Pos = new Vector3 (leftHand2.transform.position.x, leftHand2.transform.position.y - 8f, leftHand2.transform.position.z);
            loa.changeTarget(headCube.transform, 1);
            loa.addTargets(rightHand2.transform.GetChild(1).gameObject.transform);
            loa.addTargets(leftHand2.transform.GetChild(1).gameObject.transform);           
            stepList[4] = false;
            stepList[5] = true;
        }
        // Step 6: Have the hands slowly shift vertically into their new positions
        else if (stepList[5]) {
            lh1.animator.SetBool("staticState", false);
            rh1.animator.SetBool("staticState", false);
            if (shiftHands()) {
                stepList[5] = false;
                stepList[6] = true;
            }
        }

        // Step 7: Reconfigure the attack pattern to include new attacks, activate the enemy motions (idle state), and restore all of 
        // their health. regenHealth() will trigger the next step once all the health is returned
        else if (stepList[6]) {
            atkPat.setAmt(4,5);
            if (doOnce) {
                atkPat.setHeadBehavior(hc);
                hc.setResume();
                rh1.setResume();
                rh2.setResume();
                lh1.setResume();
                lh2.setResume(); 
                StartCoroutine(regenHealth());
                doOnce = false;
            }
                
        }
        // Step 8: Enable the enemy to take and receive damage to interact with the player again
        else if (stepList[7]) {
            enDamDet.setPhaseTransition(false);
            atkPat.activateAllHands();
            atkPat.setPhaseTransition(false);
            gm.setAllowPhase3();
            stepList[7] = false;    
        }

             
    }
    // A helper function to position each hand into their new location; returns a bool to indicate that all hands
    // have reached their designated location

    private bool shiftHands() {
        // Bools used to verify that each hand reached their location; assumed not (false) until proven true
        bool rh1Set = false;
        bool rh2Set = false;
        bool lh1Set = false;
        bool lh2Set = false;
        // Each if statement will check the distance between the current position and designated location, then move the hand if not at the spot
        if (Vector3.Distance(rightHand1.transform.position, rh1Pos) < 0.001f)
            rh1Set = true;
        else
            rightHand1.transform.position = Vector3.MoveTowards(rightHand1.transform.position, rh1Pos, shiftSpeed * Time.deltaTime);
        if (Vector3.Distance(rightHand2.transform.position, rh2Pos) < 0.001f)
            rh2Set = true;
        else
            rightHand2.transform.position = Vector3.MoveTowards(rightHand2.transform.position, rh2Pos, shiftSpeed * Time.deltaTime);
        if (Vector3.Distance(leftHand1.transform.position, lh1Pos) < 0.001f)
            lh1Set = true;
        else
            leftHand1.transform.position = Vector3.MoveTowards(leftHand1.transform.position, lh1Pos, shiftSpeed * Time.deltaTime);
        if (Vector3.Distance(leftHand2.transform.position, lh2Pos) < 0.001f)
            lh2Set = true;
        else
            leftHand2.transform.position = Vector3.MoveTowards(leftHand2.transform.position, lh2Pos, shiftSpeed * Time.deltaTime);
        // Return an expression on whether all the hands are at the correct spot
        return (rh1Set && rh2Set && lh1Set && lh2Set);
    }

    // Function to allow the enemy's health to return and change its name
    IEnumerator regenHealth() {
        // Set the name of the enemy
        enemyName.text = "Obama Phase 2: Electric Boogaloo";
        // Establish the new health for the enemy
        int maxHealth = gm.setNewHealth(400, 400, 400, 400, 600);
        gm.setThreshold(0.75f, 0.50f, 0.25f);
        // Perform a loop that allows the health bar to heal incrementally to depict a health bar restoring all of its health
        int healthRecord = 0;
        int step = 100;
        for (int i = 0; healthRecord < maxHealth; i += step) {
            healthRecord += step;
            enemyBar.fillAmount = (float) healthRecord / maxHealth;
            yield return new WaitForSeconds(0.01f);
        }
        // Move on to the next step
        stepList[6] = false;
        stepList[7] = true;
    }


    // Used by GameEnding to start the phase transition
    public void phase2() {
        stepList[0] = true;
    }
}
