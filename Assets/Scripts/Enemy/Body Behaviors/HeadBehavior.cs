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

    // Game monitor
    public GameMonitor gm;

    // Different States
    private bool idle;
    private bool startPunch;
    private bool retractPunch;
    private bool startMissle;
    private bool startLaser;
    private bool startExpand;
    private bool retExpand;
    private bool defeated;
    
    // Punch Attack Speeds
    public int punchLaunchSpeed;
    public int nonAtkSpeed;
    public int spinSpeed;
    public float shrinkSpeed;
    public float expandSpeed;

    // Target zones
    private Transform targetPunch;
    private Transform targetExpand;

    // For performing the missle attack
    private int missleAmt;  // Amount of missles for the current attack
    public Transform projectileSpawner;
    public GameObject misslePrefab;
    private int missleGone; // Amount of missles already used for the current attack
    private bool doOnce = false;
    public Transform playerHitBox;

    // For the laser attack
    private LaserBehavior laserScript;
    public GameObject laserPrefab;
    GameObject laser;

    // For the expand attack
    private Vector3 initScale;
    // The cube head is scaled significantly different than the other types of head, so needs an additional value to factor in when scaling
    public bool cubeScale;
    private float cubeFactor;
    public float reduceScaleVal;
    private float smallSize = 0f;

    // Files that call the attacks and use their lock systems
    public AttackPatterns lockSys;
    public ButtonDebug debugSys;

    /* Public functions used to manage a change in phase */
    public void setIdle(bool val) {
        idle = val;
    }

    public void setResume() {
        idle = true;
        defeated = false;
        initRot = new Vector3(initRot.x, 0, initRot.z);
        initPos = transform.position;
    }


    public void setDefeat() {
        defeated = true;
    }

    // Use Start() to establish initial characteristics and audio
    void Start()
    {
        defeated = false;
        idle = true;
        initRot = transform.localEulerAngles;
        initPos = transform.position;
        startPunch = false;
        retractPunch = false;
        startMissle = false;
        startLaser = false;
        startExpand = false;
        retExpand = false;
        laser = null;
        if (cubeScale) 
            cubeFactor = 11f;
        else
            cubeFactor = 1f;

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
        Vector3 newPos = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * height + initPos.y, initPos.z);
        Vector3 newAngle = new Vector3(initRot.x, Mathf.PingPong(Time.time * speed, range) - offset + initRot.y, initRot.z);
        // Idle
        if (idle) {
            if (!defeated) {
                transform.position = newPos;
                transform.localEulerAngles = newAngle;
            }
        }
        // Using punch
        else if (startPunch) {
            transform.Rotate(new Vector3(0, spinSpeed, 0) * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPunch.position, punchLaunchSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPunch.position) < 0.001f) {
                gm.tryPowerup(transform.position);
                startPunch = false;
                retractPunch = true;
            }
        }
        // Retracting the punch
        else if (retractPunch) {
            transform.Rotate(new Vector3(0, spinSpeed, 0) * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, initPos, nonAtkSpeed * Time.deltaTime);
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
            transform.localEulerAngles = new Vector3(initRot.x, Mathf.PingPong(Time.time * speed, range) - offset + initRot.y, initRot.z);
        }
        else if (startLaser) {
            if (doOnce) {
                /*if (laser != null)
                    laserScript.destroyLaser(); */
                laser = Instantiate(laserPrefab, new Vector3(projectileSpawner.position.x, projectileSpawner.position.y, projectileSpawner.position.z), Quaternion.identity);
                laserScript = laser.GetComponent<LaserBehavior>();
                laserScript.playerHitBox = playerHitBox;
                laserScript.gm = gm;
                StartCoroutine(fireLaser());
                doOnce = false;
            }
        }
        // Initiate the expand attack
        else if (startExpand) {
            // Need a one-time calculation of how small the shrink should be and what the initial scale is
            if (doOnce) {
                initScale = transform.localScale;
                smallSize = initScale.x / reduceScaleVal;
                doOnce = false;
            }
            // Then shrink the head to prepare for the attack
            if (transform.localScale.x > smallSize || transform.localScale.y > smallSize || transform.localScale.z > smallSize) {
                Vector3 updateScale = transform.localScale;
                updateScale.x -= cubeFactor * shrinkSpeed * Time.deltaTime;
                updateScale.y -= cubeFactor * shrinkSpeed * Time.deltaTime;
                updateScale.z -= cubeFactor * shrinkSpeed * Time.deltaTime;
                // if the shrink causes a negative, then have the scale be extremely small to prevent a warning message
                if (updateScale.x <= 0.0f || updateScale.y <= 0.0f || updateScale.z <= 0.0f) {
                    updateScale.x = 0.01f;
                    updateScale.y = 0.01f;
                    updateScale.z = 0.01f;
                }
                transform.localScale = updateScale;
            }
            // Once the shrink is complete, have the head move to the desired expand zone to initiate the next state
            else {
                transform.position = Vector3.MoveTowards(transform.position, targetExpand.position, punchLaunchSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, targetExpand.position) < 0.001f) {
                    startExpand = false;
                    retExpand = true;
                    // For use in retExpand state
                    doOnce = true;
                }
            }
        }
        // Initiate the state where the head expands to attack then retracts back
        else if (retExpand) {
            // Have the head expand first, dealing damage when it touches the player
            if (transform.localScale.x < initScale.x || transform.localScale.y < initScale.y || transform.localScale.z < initScale.z) {
                Vector3 updateScale = transform.localScale;
                updateScale.x += cubeFactor * expandSpeed * Time.deltaTime;
                updateScale.y += cubeFactor * expandSpeed * Time.deltaTime;
                updateScale.z += cubeFactor * expandSpeed * Time.deltaTime;
                transform.localScale = updateScale;
            }
            // Once the head expands back to its normal size, have it return to go back into the idle state
            else {
                if (doOnce) {
                    gm.tryPowerup(transform.position);
                    doOnce = false;
                }
                transform.position = Vector3.MoveTowards(transform.position, initPos, nonAtkSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, initPos) < 0.001f) {
                    retExpand = false;
                    idle = true;
                    lockSys.unlocker();
                    debugSys.unlocker();
                }
            }
        }
    }

    // spawn() creates the missles in the scene with a few second delay in between each creation
    IEnumerator spawn(int missleAmt) {
        for (int i = 0; i < missleAmt; i++) {
            GameObject missile = Instantiate(misslePrefab, new Vector3(projectileSpawner.position.x, projectileSpawner.position.y, projectileSpawner.position.z), Quaternion.identity);
            ProjectileBehavior missileScript = missile.GetComponent<ProjectileBehavior>();
            missileScript.head = this;
            missileScript.headObject = gameObject;
            missileScript.playerHitBox = playerHitBox;
            missileScript.gm = gm;
            yield return new WaitForSeconds(1.75f);
        }
        
    }

    IEnumerator fireLaser() {
        yield return new WaitForSeconds(laserTime);
        laserScript.destroyLaser();
        laser = null;
        lockSys.unlocker();
        debugSys.unlocker();
        startLaser = false;
        idle = true;
    }


    /*
    Public functions to initate an attack and changes in the state
    Structure:

        Play Audio
        Place a lock on the ButtonDebug code
        Get the attack target zone(s) if needed
        Change the state to be the start of the attack
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
        enemyAudioManager.instance.playLazer();
        debugSys.locker();
        doOnce = true;
        idle = false;
        startLaser = true;
    }
    public void callExpand(Transform target) {
        debugSys.locker();
        targetExpand = target;
        doOnce = true;
        idle = false;
        startExpand = true;
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
