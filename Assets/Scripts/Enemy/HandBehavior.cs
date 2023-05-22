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

    
    private Vector3 initPos;
    private Vector3 initRot;
    // Files that call the attacks and use their lock systems
    public AttackPatterns lockSys;
    public ButtonDebug debugSys;

    // Ethan's audio stuff
    public enemyAudioManager enemysfx; // need to figure out why the fuck this doesn't work
    AudioSource m_AudioSource;

    // Speed Values
    private float deltaTimeCount = 0;
    public float rotSpeed = 0;
    public float punchLaunchSpeed = 0;
    public float punchRetractSpeed = 0;
    public float swipePosSpeed = 0;
    public float swipeUseSpeed = 0;
    public float swipeRetSpeed = 0;

    public float clapPosSpeed = 0;
    public float clapUseSpeed = 0;
    public float clapBackSpeed = 0;
    public float clapRetSpeed = 0;

    public float slamPosSpeed = 0;
    public float slamUseSpeed = 0;
    public float slamRetSpeed = 0;

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

    public ShockwaveSpawn shockwaveUse;

    public void setDefeat(){
        defeated = true;
    }

    public void setNextPhase(){
        defeated = false;
    }


    // Use Start() to initiate the states
    void Start()
    {
        initRot = transform.localEulerAngles;
        initPos = transform.position;
        idle = true;
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
            animator.SetBool("idleState", true);
            // Do not have the hand move around once it has lost all its health
            if (defeated) 
                doRot(225, initRot.y, initRot.z);
            else {
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
            animator.SetBool("punchState", true);
            doRot(270f, initRot.y, initRot.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPunch.position, punchLaunchSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPunch.position) < 0.001f) {
                startPunch = false;
                retractPunch = true;
            }

        }
        // Returning Hand State (Punch)
        else if (retractPunch) {
            animator.SetBool("punchState", false);
            transform.position = Vector3.MoveTowards(transform.position, initPos, punchRetractSpeed * Time.deltaTime);
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
            transform.position = Vector3.MoveTowards(transform.position, targetSwipeStart.position, swipePosSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetSwipeStart.position) < 0.001f) {
                posSwipe = false;
                useSwipe = true;
            }
        }
        // Sweep State
        else if (useSwipe) {
            transform.position = Vector3.MoveTowards(transform.position, targetSwipeEnd.position, swipeUseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetSwipeEnd.position) < 0.001f) {
                useSwipe = false;
                retSwipe = true;
            }
        }
        // Returning the Hand State (Sweep)
        else if (retSwipe) {
            animator.SetBool("flatState", false);
            doRot(initRot.x, initRot.y, initRot.z);
            transform.position = Vector3.MoveTowards(transform.position, initPos, swipeRetSpeed * Time.deltaTime);
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
            transform.position = Vector3.MoveTowards(transform.position, targetClapStart.position, clapPosSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetClapStart.position) < 0.001f) {
                posClap= false;
                useClap = true;
            }
        }

        // Clap State
        else if (useClap) {
            transform.position = Vector3.MoveTowards(transform.position, targetClapEnd.position, clapUseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetClapEnd.position) < 0.001f) {
                useClap = false;
                backClap = true;
            }
        }
        // Recoil the Hands after Clap
        else if (backClap) {
            transform.position = Vector3.MoveTowards(transform.position, targetClapBack.position, clapBackSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetClapBack.position) < 0.001f) {
                backClap = false;
                retClap = true;
            }
        }
        // Returning the Hand State (Clap)
        else if (retClap) {
            animator.SetBool("flatState", false);
            doRot(initRot.x, initRot.y, initRot.z);
            transform.position = Vector3.MoveTowards(transform.position, initPos, clapRetSpeed * Time.deltaTime);
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
            transform.position = Vector3.MoveTowards(transform.position, targetSlamStart.position, slamPosSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetSlamStart.position) < 0.001f) {
                shockwaveUse.setActive(true);
                posSlam = false;
                useSlam = true;
            }
        }
        else if (useSlam) {
            transform.position = Vector3.MoveTowards(transform.position, targetSlamEnd.position, slamUseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetSlamEnd.position) < 0.001f) {
                shockwaveUse.setActive(false);
                useSlam = false;
                retSlam = true;
            }
        }
        else if (retSlam) {
            transform.position = Vector3.MoveTowards(transform.position, initPos, slamRetSpeed * Time.deltaTime);
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
        animator.SetBool("idleState", false);
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
        animator.SetBool("idleState", false);
        debugSys.locker();
        targetSwipeStart = targetStart;
        targetSwipeEnd = targetEnd;
        idle = false;
        posSwipe = true;
    }

    // Use callClap() by other script to do clap attack
    public void callClap(Transform targetStart, Transform targetEnd, Transform targetBack) {
        enemyAudioManager.instance.playClap();
        animator.SetBool("idleState", false);
        debugSys.locker();
        targetClapStart = targetStart;
        targetClapEnd = targetEnd;
        targetClapBack = targetBack;
        idle = false;
        posClap = true;
    }

    public void callSlam(Transform targetStart, Transform targetEnd) {
        animator.SetBool("idleState", false);
        debugSys.locker();
        targetSlamStart = targetStart;
        targetSlamEnd = targetEnd;
        idle = false;
        posSlam = true;
    }

}
