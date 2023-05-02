using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmBehavior : MonoBehaviour
{
    private float deltaTimeCount = 0;
    private Vector3 initPos;
    public float rotSpeed = 0;
    public float punchLaunchSpeed = 0;
    public float punchRetractSpeed = 0;

    public float swipePosSpeed = 0;
    public float swipeUseSpeed = 0;
    public float swipeRetSpeed = 0;



    public float width = 0;
    public float height = 0;

    private bool rotator;
    public Transform targetPunch;
    public Transform targetSwipeStart;
    public Transform targetSwipeEnd;


    private bool startPunch;
    private bool retractPunch;

    private bool posSwipe;
    private bool useSwipe;
    private bool retSwipe;


    private Vector3 currPos;



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

    void Update()
    {
        if (rotator) {
            deltaTimeCount += Time.deltaTime * rotSpeed;
            float x = Mathf.Cos(deltaTimeCount) * width;
            float y = Mathf.Sin(deltaTimeCount) * height;
            transform.position = new Vector3(initPos.x + x, initPos.y + y, initPos.z);
            currPos = initPos;
        }
        else if (startPunch) {
            transform.position = Vector3.MoveTowards(transform.position, targetPunch.position, punchLaunchSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPunch.position) < 0.001f) {
                startPunch = false;
                retractPunch = true;
            }
        }
        else if (retractPunch) {
            transform.position = Vector3.MoveTowards(transform.position, currPos, punchRetractSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, currPos) < 0.001f) {
                retractPunch = false;
                rotator = true;
            }
        }
        else if (posSwipe) {
            transform.position = Vector3.MoveTowards(transform.position, targetSwipeStart.position, swipePosSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetSwipeStart.position) < 0.001f) {
                posSwipe = false;
                useSwipe = true;
            }
        }
        else if (useSwipe) {
            transform.position = Vector3.MoveTowards(transform.position, targetSwipeEnd.position, swipeUseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetSwipeEnd.position) < 0.001f) {
                useSwipe = false;
                retSwipe = true;
            }
        }
        else if (retSwipe) {
            transform.position = Vector3.MoveTowards(transform.position, currPos, swipeRetSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, currPos) < 0.001f) {
                retSwipe = false;
                rotator = true;
            }
        }


    }

    public void setRotatorT() {
        rotator = true;
    }

    public void setRotatorF() {
        rotator = false;
    }

    public void callPunch() {
        currPos = transform.position;
        rotator = false;
        startPunch = true;
    }

    public void callSwipe() {
        currPos = transform.position;
        rotator = false;
        posSwipe = true;
    }

}
