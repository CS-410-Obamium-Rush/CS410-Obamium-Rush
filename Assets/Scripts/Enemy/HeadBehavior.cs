/*
HandBehavior: Manage the different states that the enemy head can be in and is responsible 
for performing the attacks it can do. Alters the GameObject's transform and animations to fit the attack.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBehavior : MonoBehaviour
{
    // Head transform characteristics to use and alter
    private Vector3 initPos;
    private Vector3 initRot;
    public float height; // Determine how far the GameObject will go
    public float freq;  // Determine how quickly the GameObject will go
    public float speed = 0f; // How quickly
    public float range = 0f; // The total amount of area covered in degrees
    public float offset = 0f;   // The initial position to start looking (do 1/2 of range to face the screen)
    public float laserTime = 0f;

    // ethan's audio stuff
    public enemyAudioManager enemysfx; // need to figure out why the fuck this doesn't work
    AudioSource m_AudioSource;

    // Different States
    private bool idle;
    private bool startPunch;
    private bool retractPunch;
    private bool startMissle;
    private bool startLaser;
    
    // Punch Attack Speeds
    public int punchLaunchSpeed;
    public int punchRetractSpeed;
    public int spinSpeed;

    // Target zones
    private Transform targetPunch;


    // For performing the missle attack
    private int missleAmt;  // Amount of missles for the current attack
    public Transform missleSpawner;
    public GameObject misslePrefab;
    private int missleGone; // Amount of missles already used for the current attack
    private bool doOnce = false;

    // For the laser attack
    private LaserBehavior laserCode;
    public GameObject laserPrefab;
    GameObject laser;

    // Files that call the attacks and use their lock systems
    public AttackPatterns lockSys;
    public ButtonDebug debugSys;

    // Use Start() to establish initial characteristics and audio
    void Start()
    {
        idle = true;
        initRot = transform.localEulerAngles;
        initPos = transform.position;
        startPunch = false;
        retractPunch = false;
        missleGone = 0;

        // get audiosource
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Use Update() to manage the states
    /*
        State Structure:

        Check State
            Set the rotation if needed
            Use Vector3.MoveTowards to move hand to selected zone
            (Except of Missle) One the hand reaches the designated zone
                Disable its current state and move to the next one
    */
    void Update() {
        // Idle
        if (idle) {
            transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * height + initPos.y, initPos.z);
            transform.localEulerAngles = new Vector3(initRot.x, Mathf.PingPong(Time.time * speed, range) - offset, initRot.z);
        }
        // Using punch
        else if (startPunch) {
            transform.Rotate(new Vector3(0, spinSpeed, 0) * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPunch.position, punchLaunchSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPunch.position) < 0.001f) {
                startPunch = false;
                retractPunch = true;
            }
        }
        // Retracting the punch
        else if (retractPunch) {
            transform.Rotate(new Vector3(0, spinSpeed, 0) * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, initPos, punchRetractSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initPos) < 0.001f) {
                retractPunch = false;
                idle = true;
                transform.localEulerAngles = new Vector3(initRot.x, initRot.y, initRot.z);
                lockSys.unlocker();
                debugSys.unlocker();
            }
        }
        // Create the missle
        else if (startMissle) {
            // Create the amount of missles only once
            if (doOnce) {
                StartCoroutine(spawn(missleAmt));
                doOnce = false;
            }
            // Act as if in idle state until all the missles are gone
            transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * height + initPos.y, initPos.z);
            transform.localEulerAngles = new Vector3(initRot.x, Mathf.PingPong(Time.time * speed, range) - offset, initRot.z);
        }
        else if (startLaser) {
            if (doOnce) {
                laser = Instantiate(laserPrefab, new Vector3(missleSpawner.position.x, missleSpawner.position.y, missleSpawner.position.z), Quaternion.identity);
                laserCode = laser.GetComponent<LaserBehavior>();
                StartCoroutine(fireLaser());
                doOnce = false;
            }
        }



    }

    // spawn() creates the missles in the scene with a few second delay in between each creation
    IEnumerator spawn(int missleAmt) {
        for (int i = 0; i < missleAmt; i++) {
            Instantiate(misslePrefab, new Vector3(missleSpawner.position.x, missleSpawner.position.y, missleSpawner.position.z), Quaternion.identity);
            yield return new WaitForSeconds(1.75f);
        }
        
    }

    IEnumerator fireLaser() {
        yield return new WaitForSeconds(laserTime);
        laserCode.destroyLaser();
        lockSys.unlocker();
        debugSys.unlocker();
        idle = true;
        startLaser = false;
    }


    /*
    Public functions to initate an attack and changes in the state
    Structure:

        Play Audio
        Place a lock on the ButtonDebug code
        Get the attack target zone(s) if needed
        Change the state to the start of the attack
    */

    public void callPunch(Transform target) {
        enemyAudioManager.instance.playWhirl();
        debugSys.locker();
        targetPunch = target;
        idle = false;
        startPunch = true;
    }

    public void callMissle(int amt) {
        enemyAudioManager.instance.playShot();
        debugSys.locker();
        doOnce = true;
        missleAmt = amt;
        idle = false;
        startMissle = true;
    }

    public void callLaser() {
        debugSys.locker();
        doOnce = true;
        idle = false;
        startLaser = true;
    }



    // countMissle() used for collision detections to indicate the new removal of a missle;
    // Will also be responsible for freeing up the attack state
    public void countMissle(){
        enemyAudioManager.instance.playProjHit();
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
