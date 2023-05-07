using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
This script is used for the different states that each enemy hand can be in and is responsible 
for performing the attacks it can do
*/
public class HandBehavior : MonoBehaviour
{
    
    Animator animator;
    private bool isAttacking;

    private float deltaTimeCount = 0;
    private Vector3 initPos;
    private Vector3 initRot;
    // Files that call the attacks and use their lock systems
    public AttackPatterns lockSys;
    public ButtonDebug debugSys;

    // Speed Values
    public float rotSpeed = 0;
    public float punchLaunchSpeed = 0;
    public float punchRetractSpeed = 0;
    public float swipePosSpeed = 0;
    public float swipeUseSpeed = 0;
    public float swipeRetSpeed = 0;

    // Rotation Paramters
    public float width = 0;
    public float height = 0;
    public bool isRight = false;
    private int dir = -1;

    
    // Need GameObjects to direct attack positions
    private Transform targetPunch;
    private Transform targetSwipeStart;
    private Transform targetSwipeEnd;
    
    // States of rotation and attacks
    private bool rotator;   // Turn on and off rotation
    private bool startPunch;    // Initate the punch
    private bool retractPunch;  // Return the hand
    private bool posSwipe;      // Initate the swipe/get hand into position
    private bool useSwipe;      // Sweep across the hand
    private bool retSwipe;      // Return the hand

    // Use Start() to initiate the states
    void Start()
    {
        animator = GetComponent<Animator>();
        initRot = transform.localEulerAngles;
        isAttacking = false;
        initPos = transform.position;
        rotator = true;
        startPunch = false;
        retractPunch = false;
        posSwipe = false;
        useSwipe = false;
        retSwipe = false;
        if (isRight) {
            dir = 1;
        }
    }

    // Use Update() to manage the states
    void Update()
    {
        // Idle/rotate hands
        if (rotator) {
            if (isAttacking == true) {
                isAttacking = false;
                animator.SetBool("isAttacking", isAttacking);
            }
            deltaTimeCount += Time.deltaTime * rotSpeed;
            // Spin the hand in a circular motion; direction is dictated by hand so one goes clockwise while the other goes counterclockwise
            float x = Mathf.Cos(deltaTimeCount) * width * dir;
            float y = Mathf.Sin(deltaTimeCount) * height * dir;
            transform.position = new Vector3(initPos.x + x, initPos.y + y, initPos.z);
        }
        // Punching State
        else if (startPunch) {
            doRot(0f, initRot.y, initRot.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPunch.position, punchLaunchSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPunch.position) < 0.001f) {
                startPunch = false;
                retractPunch = true;
            }

        }
        // Returning Hand State (Punch)
        else if (retractPunch) {
            transform.position = Vector3.MoveTowards(transform.position, initPos, punchRetractSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initPos) < 0.001f) {
                retractPunch = false;
                rotator = true;
                lockSys.unlocker();
                debugSys.unlocker();
            }
            doRot(initRot.x, initRot.y, initRot.z);
        }
        // Position Hand State
        else if (posSwipe) {
            if (isRight) 
                doRot(initRot.x, initRot.y, -90);
            else
                doRot(0, initRot.y, 90);
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
            doRot(initRot.x, initRot.y, initRot.z);
            transform.position = Vector3.MoveTowards(transform.position, initPos, swipeRetSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initPos) < 0.001f) {
                retSwipe = false;
                rotator = true;
                lockSys.unlocker();
                debugSys.unlocker();
            }
        }
    }

    // Use doRot() to set the rototation of objects hands during attacks based on the inspector Euler angles
    private void doRot(float x, float y, float z) {
        /*
        Scrap code for smooth transition rotation
        Vector3 currRot = transform.localEulerAngles;
        Vector3 newRot = new Vector3(x, y, z);
        Vector3 transRot = Vector3.Lerp(currRot, newRot, Time.deltaTime * atkRotSpeed);
        transform.eulerAngles = transRot; 
        */
        transform.localEulerAngles = new Vector3(x, y, z);
    }


    // OnTriggerEnter() Checks for detection with player (using Wall for now for testing purposes)
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Projectile")) {
            Debug.Log("Wall has been hit");
        }
    }

    /* setRotators used to turn on and off rotation state; mainly for ButtonDebug*/
    public void setRotatorT() {
        rotator = true;
    }

    public void setRotatorF() {
        rotator = false;
    }

    // Use callPunch() by other script to do punch attack
    public void callPunch(Transform target) {
        if (isAttacking == false) {
            isAttacking = true;
            animator.SetBool("isAttacking", isAttacking);
        }
        debugSys.locker();
        targetPunch = target;
        rotator = false;
        startPunch = true;
    }

    // Use callSwipe() by other script to do swipe attack
    public void callSwipe(Transform targetStart, Transform targetEnd) {
        if (isAttacking == false) {
            isAttacking = true;
            animator.SetBool("isAttacking", isAttacking);
        }
        debugSys.locker();
        targetSwipeStart = targetStart;
        targetSwipeEnd = targetEnd;
        rotator = false;
        posSwipe = true;
    }
    static public void callClap(Transform target) {
        
    }

}
