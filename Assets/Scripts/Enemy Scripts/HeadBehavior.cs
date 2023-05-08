using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBehavior : MonoBehaviour
{

    public float height; // Determine how far the GameObject will go
    public float freq;  // Determine how quickly the GameObject will go

    public float speed = 0f; // How quickly
    public float range = 0f; // The total amount of area covered in degrees
    public float offset = 0f;   // The initial position to start looking (do 1/2 of range to face the screen)

    private bool idle;
    private bool startPunch;
    private bool retractPunch;

    public int punchLaunchSpeed;
    public int punchRetractSpeed;
    public int spinSpeed;

    private Transform targetPunch;

    private Vector3 initPos;
    private Vector3 initRot;

    // Files that call the attacks and use their lock systems
    public AttackPatterns lockSys;
    public ButtonDebug debugSys;

    // Start is called before the first frame update
    void Start()
    {
        idle = true;
        initRot = transform.localEulerAngles;
        initPos = transform.position;
        startPunch = false;
        retractPunch = false;
    }

    // Update is called once per frame
    void Update() {
        if (idle) {
            transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * height + initPos.y, initPos.z);
            transform.localEulerAngles = new Vector3(initRot.x, Mathf.PingPong(Time.time * speed, range) - offset, initRot.z);
        }
        else if (startPunch) {
            transform.Rotate(new Vector3(0, spinSpeed, 0) * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPunch.position, punchLaunchSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPunch.position) < 0.001f) {
                startPunch = false;
                retractPunch = true;
            }
        }
        else if (retractPunch) {
            transform.position = Vector3.MoveTowards(transform.position, initPos, punchRetractSpeed * Time.deltaTime);
            transform.Rotate(new Vector3(0, spinSpeed, 0) * Time.deltaTime);
            if (Vector3.Distance(transform.position, initPos) < 0.001f) {
                retractPunch = false;
                idle = true;
                transform.localEulerAngles = new Vector3(initRot.x, initRot.y, initRot.z);
                lockSys.unlocker();
                debugSys.unlocker();
            }
        }
    }

    public void callPunch(Transform target) {
        debugSys.locker();
        targetPunch = target;
        idle = false;
        startPunch = true;
    }
}
