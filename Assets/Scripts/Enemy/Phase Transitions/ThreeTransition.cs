/*
ThreeTransition: A script that triggers the enemy's transition from the second phase to the third and final phase
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThreeTransition : MonoBehaviour
{
    // With performing the transition, this script needs control of all scripts and GameObjects associated with the enemy
    public AttackPatterns atkPat;           // Uses the AttackPattern to disable the enemy from attacking the player
    public EnemyDamageDetection enDamDet;   // Uses EnemyDamageDetection to disable the enemy from taking damage
    public GameMonitor gm;                  // Uses GameMonitor to establish the health for phase 2
    public lockOnAiming loa;                // Uses lockOnAiming to add the additional hands into the targeting system
    public floorInitialization fi;            // Used to initialize the new floor tiles
    public SkyboxRotator sr;                // Used to rotate the skybox, transitioning to new environment
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
    (Goes from Cube to Pyramid)
    */
    public GameObject headPyramid1;
    public GameObject headPyramid2;
    public GameObject headPyramid3;
    public GameObject headCube;
    private HeadBehavior hp1; //Pyramid 1's head behavior script
    private HeadBehavior hp2; //Pyramid 2's head behavior script
    private HeadBehavior hp3; //Pyramid 3's head behavior script

    // Tools to perform the phase operation
    public TextMeshProUGUI enemyName; // Update the name of the enemy
    public Image enemyBar;          // Update the enemy health to be at full health for the second phase
    private bool[] stepList;        // Provides a linear sequence for the operation to occur; i.e the phase transition occurs in steps
    Vector3 rh1Pos;                 // These establish the new location for the hands to be at during the third phase
    Vector3 rh2Pos; 
    Vector3 lh1Pos;
    Vector3 lh2Pos;
    Vector3 head1Pos;                 // These establish the new location for the heads to be at during the third phase
    Vector3 head2Pos; 
    Vector3 head3Pos;



    private bool doOnce = true;     // A bool to have specific step parts occur only one time instead of repeatedly

    // Public variables for phase transition speeds
    public float shrinkSpeed = 15f;
    public float shiftSpeed = 5f;
    public float resetSpeed = 5f;


    // Start is called before the first frame update
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
        hp1 = headPyramid1.GetComponent<HeadBehavior>();
        hp2 = headPyramid2.GetComponent<HeadBehavior>();
        hp3 = headPyramid3.GetComponent<HeadBehavior>();

    }

    void Update() {
    // Step 1: Disable the ability to interact with enemy
        if (stepList[0]) {
            floorBehavior.doReset = true;
            sr.phase3();
            enemyNextPhaseAudioManager.instance.playDefeat();
            enDamDet.setPhaseTransition(true);
            atkPat.setPhaseTransition(true);
            gm.playerAddHealth(80);
            stepList[0] = false;
            stepList[1] = true;
        }
        // Step 2: Have the Cube head vanish by spinning and diminishing in size until it cannot be easily seen
        else if (stepList[1]) {
            Vector3 updateScale = headCube.transform.localScale;
            if (updateScale.x != 1f)
                updateScale.x -= shrinkSpeed * Time.deltaTime;
            if (updateScale.y != 1f)
                updateScale.y -= shrinkSpeed * Time.deltaTime;
            if (updateScale.z != 1f)
                updateScale.z -= shrinkSpeed * Time.deltaTime;
            headCube.transform.localScale = updateScale;
            if (updateScale.x <= 1f && updateScale.y <= 1f && updateScale.z <= 1f) {
                stepList[1] = false;
                stepList[2] = true;
            }
        }
        // Step 3: Put the pyramids instantly into the scene
        else if (stepList[2]) {
            headCube.SetActive(false);
            headPyramid1.SetActive(true);
            headPyramid2.SetActive(true);
            headPyramid3.SetActive(true);
            hp1.setDefeat();
            hp2.setDefeat();
            hp3.setDefeat();
            stepList[2] = false;
            stepList[3] = true;
        }
        // Step 4: Have all pair of hands return to their initial spot
        else if (stepList[3]) {
            hp1.setIdle(false);
            hp2.setIdle(false);
            hp3.setIdle(false);
            if (rh1.resetHand(resetSpeed) && lh1.resetHand(resetSpeed) && rh2.resetHand(resetSpeed) && lh2.resetHand(resetSpeed)) {
                stepList[3] = false;
                stepList[4] = true;
            }
        }
        // Step 5: Establish the new location for each hand (Pos variable) and Update the target system to include the modification of the head
        else if (stepList[4]) {
            head1Pos = new Vector3(headPyramid1.transform.position.x, headPyramid1.transform.position.y + 4.5f, headPyramid1.transform.position.z);
            head2Pos = new Vector3(headPyramid2.transform.position.x - 4.5f, headPyramid2.transform.position.y - 4.5f, headPyramid2.transform.position.z);
            head3Pos = new Vector3(headPyramid3.transform.position.x + 4.5f, headPyramid3.transform.position.y - 4.5f, headPyramid3.transform.position.z);
            rh1Pos = new Vector3 (rightHand1.transform.position.x - 4f, rightHand1.transform.position.y - 2.5f, rightHand1.transform.position.z - 6f);
            rh2Pos = new Vector3 (rightHand2.transform.position.x - 4f, rightHand2.transform.position.y + 2.5f, rightHand2.transform.position.z - 6f);
            lh1Pos = new Vector3 (leftHand1.transform.position.x + 4f, leftHand1.transform.position.y - 2.5f, leftHand1.transform.position.z - 6f);
            lh2Pos = new Vector3 (leftHand2.transform.position.x + 4f, leftHand2.transform.position.y + 2.5f, leftHand2.transform.position.z - 6f);         
            addTargets();
            stepList[4] = false;
            stepList[5] = true;
        }
        // Step 6: Have the hands and heads slowly shift into their new positions
        else if (stepList[5]) {
            if (shiftHands() && shiftHeads()) {
                stepList[5] = false;
                stepList[6] = true;
            }
        }

        // Step 7: Reconfigure the attack pattern to include new attacks, activate the enemy motions (idle state), and restore all of 
        // their health. regenHealth() will trigger the next step once all the health is returned
        else if (stepList[6]) {
            atkPat.setAmt(4,7);
            if (doOnce) {
                rh1.setMove(1, 3);
                rh2.setMove(1, 3);
                lh1.setMove(1, 3);
                lh2.setMove(1, 3);
                atkPat.setHeadBehavior(hp1);
                hp1.setResume();
                hp2.setResume();
                hp3.setResume();
                rh1.setResume();
                rh2.setResume();
                lh1.setResume();
                lh2.setResume(); 
                rh1.setRot(0, -30, 0);
                rh2.setRot(0, -30, 0);
                lh1.setRot(0, 30, 0);
                lh2.setRot(0, 30, 0);
                StartCoroutine(regenHealth());
                doOnce = false;
            }
                
        }
        // Step 8: Enable the enemy to take and receive damage to interact with the player again
        else if (stepList[7]) {
            fi.placeNewTiles(3);
            rh1.setAtkSpeeds(35, 35, 35, 35);
            rh2.setAtkSpeeds(35, 35, 35, 35);
            lh1.setAtkSpeeds(35, 35, 35, 35);
            lh2.setAtkSpeeds(35, 35, 35, 35);
            enDamDet.setPhaseTransition(false);
            atkPat.activateAllHands(true);
            atkPat.setTimeInterval(1f);
            atkPat.setPhaseTransition(false);
            gm.setAllowWin();
            stepList[7] = false;    
        }
    }
    private void addTargets() {
        loa.addTarget(rightHand1.transform.GetChild(1).gameObject.transform, "RH1");
        loa.addTarget(leftHand1.transform.GetChild(1).gameObject.transform, "LH1");
        loa.addTarget(rightHand2.transform.GetChild(1).gameObject.transform, "RH2");
        loa.addTarget(leftHand2.transform.GetChild(1).gameObject.transform, "LH2");
        loa.addTarget(headPyramid1.transform, "Head1");
        loa.addTarget(headPyramid2.transform, "Head2");
        loa.addTarget(headPyramid3.transform, "Head3");
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

    private bool shiftHeads() {
        // Bools used to verify that each hand reached their location; assumed not (false) until proven true
        bool head1Set = false;
        bool head2Set = false;
        bool head3Set = false;
        // Each if statement will check the distance between the current position and designated location, then move the hand if not at the spot
        if (Vector3.Distance(headPyramid1.transform.position, head1Pos) < 0.001f)
            head1Set = true;
        else
            headPyramid1.transform.position = Vector3.MoveTowards(headPyramid1.transform.position, head1Pos, shiftSpeed * Time.deltaTime);
        
        if (Vector3.Distance(headPyramid2.transform.position, head2Pos) < 0.001f)
            head2Set = true;
        else
            headPyramid2.transform.position = Vector3.MoveTowards(headPyramid2.transform.position, head2Pos, shiftSpeed * Time.deltaTime);

        if (Vector3.Distance(headPyramid3.transform.position, head3Pos) < 0.001f)
            head3Set = true;
        else
            headPyramid3.transform.position = Vector3.MoveTowards(headPyramid3.transform.position, head3Pos, shiftSpeed * Time.deltaTime);

        // Return an expression on whether all the hands are at the correct spot
        return (head1Set && head2Set && head3Set);
    }

    // Function to allow the enemy's health to return and change its name
    IEnumerator regenHealth() {
        // Set the name of the enemy
        enemyName.text = "Obama Phase 3: Virtual Insanity of God";
        // Establish the new health for the enemy
        int maxHealth = gm.setNewHealth(580, 580, 580, 580, 690, 690, 690);
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
    public void phase3() {
        stepList[0] = true;
    }
}
