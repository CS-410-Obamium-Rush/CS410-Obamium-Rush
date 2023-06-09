/*
HandBehavior: Manage the different states that each enemy hand can be in and is responsible 
for performing the attacks it can do. Alters the GameObject's transform and animations to fit the attack.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    
    // Need the aimator for the state
    public Animator animator;
    public bool inPhase1;
    
    private Vector3 initPos;
    private Vector3 initRot;
    // Files that call the attacks and use their lock systems
    public AttackPatterns lockSys;
    public ButtonDebug debugSys;

    // Ethan's audio stuff
    public enemyAudioManager enemysfx; // need to figure out why the fuck this doesn't work
    AudioSource m_AudioSource;

    // Game monitor
    public GameMonitor gm;

    // Speed Values
    private float deltaTimeCount = 0;
    public float rotSpeed = 0;
    public float nonAtkSpeed = 0;
    public float punchLaunchSpeed = 0;
    public float swipeUseSpeed = 0;
    public float clapUseSpeed = 0;
    public float slamUseSpeed = 0;

    // Rotation Paramters
    public float width = 0;
    public float height = 0;
    public bool isRight = false;
    private int dir = -1;

    // Need GameObjects to direct attack positions
    private Transform targetPunch;
    private Transform targetSwipeStart;
    private Transform targetSwipeEnd;

    private Transform targetClapStart;
    private Transform targetClapEnd;
    private Transform targetClapBack;

    private Transform targetSlamStart;
    private Transform targetSlamEnd;

    // States of rotation and attacks
    private bool idle;       
    private bool startPunch;    // Initate the punch
    private bool retractPunch;  // Return the hand
    private bool posSwipe;      // Initate the swipe/get hand into position
    private bool useSwipe;      // Sweep across the hand
    private bool retSwipe;      // Return the hand

    private bool posClap;      
    private bool useClap;
    private bool backClap;
    private bool retClap;    
    private bool posSlam;
    private bool useSlam;
    private bool retSlam;
    private bool defeated;

    // Used to spawn shockwaves from a Slam attack
    public ShockwaveSpawn shockwaveUse;

    /* Public functions used for a phase change by altering the state of the hands */
    public void setDefeat(){
        defeated = true;
    }

    public void setIdle(bool val){
        idle = val;
    }
    public void setPause() {
        idle = false;
        defeated = false;
        doRot(initRot.x, initRot.y, initRot.z);
    }
    public void setResume() {
        initPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        idle = true;
        defeated = false;
    }

    // Public Function to get the hands to their original positions during a phase change; returns a bool to indicate they reached
    // their location yet or not
    public bool resetHand(float resetSpeed) {
        transform.position = Vector3.MoveTowards(transform.position, initPos, resetSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, initPos) < 0.001f) {
            doRot(initRot.x, initRot.y, initRot.z);
            return true;
        }
        else
            return false;
    }

    // Public function to adjust attack speeds, rotation, and idle movement respectivelly for the third phase
    public void setAtkSpeeds(int pSpd, int swSpd, int cSpd, int slSpd) {
        punchLaunchSpeed = pSpd;
        swipeUseSpeed = swSpd;
        clapUseSpeed = cSpd;
        slamUseSpeed = slSpd;
    }
    public void setRot(float x, float y, float z) {
        doRot(x, y, z);
        initRot = transform.localEulerAngles;
    }
    public void setMove(float w, float h) {
        width = w;
        height = h;
    }
    


    // Use Start() to initiate the states
    void Start()
    {
        initRot = transform.localEulerAngles;
        initPos = transform.position;
        if (inPhase1) 
            idle = true;
        else
            idle = false;
        defeated = false;
        startPunch = false;
        retractPunch = false;

        posSwipe = false;
        useSwipe = false;
        retSwipe = false;

        posClap = false;
        useClap = false;
        retClap = false;

        posSlam = false;
        useSlam = false;
        retSlam = false;

        //get audiosource
        m_AudioSource = GetComponent<AudioSource>();

        if (isRight) {
            dir = 1;
        }
    }

    // Use Update() to manage the states
    void Update()
    {
        // Idle/rotate hands
        if (idle) {
            animator.SetBool("staticState", false);
            // Do not have the hand move around once it has lost all its health
            if (defeated) {
                animator.SetBool("staticState", true);
                doRot(225, initRot.y, initRot.z);
            } else {
                deltaTimeCount += Time.deltaTime * rotSpeed;
                // Spin the hand in a circular motion; direction is dictated by hand so one goes clockwise while the other goes counterclockwise
                float x = Mathf.Cos(deltaTimeCount) * width * dir;
                float y = Mathf.Sin(deltaTimeCount) * height * dir;
                transform.position = new Vector3(initPos.x + x, initPos.y + y, initPos.z);
            }
            
        }
        /*
        State Structure:

        Check State
            Change animation condition variables if needed
            Set the rotation if needed
            Use Vector3.MoveTowards to move hand to selected zone
            One the hand reaches the designated zone
                Disable its current state and move to the next one
        */
        // Punching State
        else if (startPunch) {
            doRot(270f, 0f, initRot.z);
            animator.SetBool("punchState", true);
            transform.position = Vector3.MoveTowards(transform.position, targetPunch.position, punchLaunchSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPunch.position) < 0.001f) {
                gm.tryPowerup(transform.position);
                gm.addScreenShake(0.2f);
                startPunch = false;
                retractPunch = true;
            }

        }
        // Returning Hand State (Punch)
        else if (retractPunch) {
            animator.SetBool("punchState", false);
            transform.position = Vector3.MoveTowards(transform.position, initPos, nonAtkSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initPos) < 0.001f) {
                retractPunch = false;
                idle = true;
                doRot(initRot.x, initRot.y, initRot.z);
                lockSys.unlocker();
                debugSys.unlocker();
            }
        }
        // Position Hand State
        else if (posSwipe) {
            animator.SetBool("flatState", true);
            doRot(initRot.x, -90 * dir, initRot.z);
            transform.position = Vector3.MoveTowards(transform.position, targetSwipeStart.position, nonAtkSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetSwipeStart.position) < 0.001f) {
                posSwipe = false;
                useSwipe = true;
            }
        }
        // Sweep State
        else if (useSwipe) {
            transform.position = Vector3.MoveTowards(transform.position, targetSwipeEnd.position, swipeUseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetSwipeEnd.position) < 0.001f) {
                gm.tryPowerup(transform.position);
                useSwipe = false;
                retSwipe = true;
            }
        }
        // Returning the Hand State (Sweep)
        else if (retSwipe) {
            animator.SetBool("flatState", false);
            doRot(initRot.x, initRot.y, initRot.z);
            transform.position = Vector3.MoveTowards(transform.position, initPos, nonAtkSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initPos) < 0.001f) {
                retSwipe = false;
                idle = true;
                lockSys.unlocker();
                debugSys.unlocker();
            }
        }

        // Position the Hand State (Clap)
        else if (posClap) {
            animator.SetBool("flatState", true);
            doRot(initRot.x, -90 * dir, initRot.z);
            transform.position = Vector3.MoveTowards(transform.position, targetClapStart.position, nonAtkSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetClapStart.position) < 0.001f) {
                posClap= false;
                useClap = true;
            }
        }

        // Clap State
        else if (useClap) {
            transform.position = Vector3.MoveTowards(transform.position, targetClapEnd.position, clapUseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetClapEnd.position) < 0.001f) {
                gm.tryPowerup(transform.position);
                gm.addScreenShake(0.4f);
                useClap = false;
                backClap = true;
            }
        }
        // Recoil the Hands after Clap
        else if (backClap) {
            transform.position = Vector3.MoveTowards(transform.position, targetClapBack.position, nonAtkSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetClapBack.position) < 0.001f) {
                backClap = false;
                retClap = true;
            }
        }
        // Returning the Hand State (Clap)
        else if (retClap) {
            animator.SetBool("flatState", false);
            doRot(initRot.x, initRot.y, initRot.z);
            transform.position = Vector3.MoveTowards(transform.position, initPos, nonAtkSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initPos) < 0.001f) {
                retClap = false;
                idle = true;
                lockSys.unlocker();
                debugSys.unlocker();
            }
        }
        else if (posSlam) {
            animator.SetBool("punchState", true);
            doRot(initRot.x, -90 * dir, 90 * dir);
            transform.position = Vector3.MoveTowards(transform.position, targetSlamStart.position, nonAtkSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetSlamStart.position) < 0.001f) {
                shockwaveUse.setActive(true);
                posSlam = false;
                useSlam = true;
            }
        }
        else if (useSlam) {
            transform.position = Vector3.MoveTowards(transform.position, targetSlamEnd.position, slamUseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetSlamEnd.position) < 0.001f) {
                gm.tryPowerup(transform.position);
                gm.addScreenShake(0.4f);
                shockwaveUse.setActive(false);
                useSlam = false;
                retSlam = true;
            }
        }
        else if (retSlam) {
            transform.position = Vector3.MoveTowards(transform.position, initPos, nonAtkSpeed * Time.deltaTime);
            animator.SetBool("punchState", false);
            if (Vector3.Distance(transform.position, initPos) < 0.001f) {
                retSlam = false;
                idle = true;
                doRot(initRot.x, initRot.y, initRot.z);
                lockSys.unlocker();
                debugSys.unlocker();
            }
        }
    }

    // Helper Function: doRot() sets the rototation of objects hands during attacks based on the inspector Euler angles
    private void doRot(float x, float y, float z) {
        transform.localEulerAngles = new Vector3(x, y, z);
    }

    /*
    Public functions to initate an attack and changes in the state
    Structure:

        Play Audio
        Turn off idle animation
        Place a lock on the ButtonDebug code
        Get the attack target zone(s)
        Change the state to the start of the attack
    */


    // Use callPunch() by other script to do punch attack
    public void callPunch(Transform target) {
        /*if(enemysfx != null){
            Debug.Log("enemysfx is not null");
        }*/
        // ethan's sfx
        //enemysfx.playPunch();
        // enemyAudioManager.instance.playPrepunch();
        enemyAudioManager.instance.playPunch();
        animator.SetBool("staticState", true);
        debugSys.locker();
        targetPunch = target;
        idle = false;
        startPunch = true;
    }

    // Use callSwipe() by other script to do swipe attack
    public void callSwipe(Transform targetStart, Transform targetEnd) {
        /*if(enemysfx != null){
            Debug.Log("enemysfx is not null");
        }*/
        // play the movement sfx here
        // will try to put it in a more interactive spot
        // currently breaks hand movement when I place this in bool statements -- EAVI
        //enemysfx.playWhoosh();
        enemyAudioManager.instance.playWhoosh();
        animator.SetBool("staticState", true);
        debugSys.locker();
        targetSwipeStart = targetStart;
        targetSwipeEnd = targetEnd;
        idle = false;
        posSwipe = true;
    }

    // Use callClap() by other script to do clap attack
    public void callClap(Transform targetStart, Transform targetEnd, Transform targetBack) {
        enemyAudioManager.instance.playClap();
        animator.SetBool("staticState", true);
        debugSys.locker();
        targetClapStart = targetStart;
        targetClapEnd = targetEnd;
        targetClapBack = targetBack;
        idle = false;
        posClap = true;
    }

    public void callSlam(Transform targetStart, Transform targetEnd) {
        enemyAudioManager.instance.playSlam();
        animator.SetBool("staticState", true);
        debugSys.locker();
        targetSlamStart = targetStart;
        targetSlamEnd = targetEnd;
        idle = false;
        posSlam = true;
    }
}
