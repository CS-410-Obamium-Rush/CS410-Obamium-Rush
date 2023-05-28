using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmBehavior : MonoBehaviour
{
    private float deltaTimeCount = 0;
    private Vector3 initPos;
    private Vector3 currPos;

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
    
    // Need GameObjects to direct attack positions
    public Transform targetPunch;
    public Transform targetSwipeStart;
    public Transform targetSwipeEnd;

    // States of rotation and attacks
    private bool rotator;   // Turn on and off rotation
    private bool startPunch;    // Initate the punch
    private bool retractPunch;  // Return the hand
    private bool posSwipe;      // Initate the swipe/get hand into position
    private bool useSwipe;      // Sweep across the hand
    private bool retSwipe;      // Return the hand


    


    // Use Start() to initiate the states)
    void Start()
    {
        initPos = transform.position;
        rotator = true;
        startPunch = false;
        retractPunch = false;
        posSwipe = false;
        useSwipe = false;
        retSwipe = false;
    }

    // Use Update() to manage the states
    void Update()
    {
        // Idle/rotate hands
        if (rotator) {
            deltaTimeCount += Time.deltaTime * rotSpeed;
            float x = Mathf.Cos(deltaTimeCount) * width;
            float y = Mathf.Sin(deltaTimeCount) * height;
            transform.position = new Vector3(initPos.x + x, initPos.y + y, initPos.z);
            currPos = initPos;
        }
        // Punching State
        else if (startPunch) {
            transform.position = Vector3.MoveTowards(transform.position, targetPunch.position, punchLaunchSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPunch.position) < 0.001f) {
                startPunch = false;
                retractPunch = true;
            }
        }
        // Returning Hand State (Punch)
        else if (retractPunch) {
            transform.position = Vector3.MoveTowards(transform.position, currPos, punchRetractSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, currPos) < 0.001f) {
                retractPunch = false;
                rotator = true;
            }
        }
        // Position Hand State
        else if (posSwipe) {
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
            transform.position = Vector3.MoveTowards(transform.position, currPos, swipeRetSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, currPos) < 0.001f) {
                retSwipe = false;
                rotator = true;
            }
        }

    }

    // OnTriggerEnter() Checks for detection with player (using Wall for now for testing purposes)
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Wall")) {
            Debug.Log("Wall has been hit");
        }
    }

    /* setRotators used to turn on and off rotation state*/
    public void setRotatorT() {
        rotator = true;
    }

    public void setRotatorF() {
        rotator = false;
    }

    // Use callPunch() by other script to do punch attack
    public void callPunch() {
        currPos = transform.position;
        rotator = false;
        startPunch = true;
    }

    // Use callSwipe() by other script to do swipe attack
    public void callSwipe() {
        currPos = transform.position;
        rotator = false;
        posSwipe = true;
    }

}
