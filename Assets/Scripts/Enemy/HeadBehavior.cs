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
    private bool startMissle;
    

    public int punchLaunchSpeed;
    public int punchRetractSpeed;
    public int spinSpeed;

    private Transform targetPunch;
    private Transform targetMissle;
    private int missleAmt;
    public Transform missleSpawner;
    public GameObject misslePrefab;
    private int missleGone;
    private bool doOnce = false;

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
        missleGone = 0;
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
        else if (startMissle) {
            if (doOnce) {
                StartCoroutine(spawn(missleAmt));
                doOnce = false;
            }
            transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * height + initPos.y, initPos.z);
            transform.localEulerAngles = new Vector3(initRot.x, Mathf.PingPong(Time.time * speed, range) - offset, initRot.z);
        }
    }
    IEnumerator spawn(int missleAmt) {
        for (int i = 0; i < missleAmt; i++) {
            Instantiate(misslePrefab, new Vector3(missleSpawner.position.x, missleSpawner.position.y, missleSpawner.position.z), Quaternion.identity);
            yield return new WaitForSeconds(2.5f);
        }
        
    }

    public void callPunch(Transform target) {
        debugSys.locker();
        targetPunch = target;
        idle = false;
        startPunch = true;
    }

    public void callMissle(int amt) {
        debugSys.locker();
        doOnce = true;
        //targetMissle = target;
        missleAmt = amt;
        idle = false;
        startMissle = true;
    }

    public void countMissle(){
        missleGone += 1;
        if (missleGone >= missleAmt) {
            startMissle = false;
            idle = true;
            missleGone = 0;
            lockSys.unlocker();
            debugSys.unlocker();
         }
    }
}
