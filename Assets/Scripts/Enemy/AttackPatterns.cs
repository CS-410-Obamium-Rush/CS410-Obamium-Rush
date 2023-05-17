using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
This script will let the enemy send a random attack after a fixed period of time
*/
public class AttackPatterns : MonoBehaviour
{
    // Variables for calculating when an attack should appear
    public float timeInterval; // Refers to seconds in between attacks
    private float rechargeTime; // Amount of time needed to pass to initate an attack

    // Use the targets, or zones, stored in a grouped GameObject
    public GameObject punchZones;
    public GameObject swipeZones;
    public GameObject clapZones;

    // Get the left and right hand and head's behavior to call attacks
    public HandBehavior lh;
    public HandBehavior rh;
    public HeadBehavior head;

    // Decide if hands are allowed to use
    private bool leftUse;
    private bool rightUse;

    
    public DamageDealer dmg;
    /* 
    Using a lock system to prevent multiple attacks occuring at once; only one attack at a time
    The idea is to implement a lock-like system where an inititated attack will hold onto the lock
    and release the lock after the attack is done. These are public functions for the actual attacks 
    found in HandBehavior to decide the lock and unlock when appropiate.
    */
    
    private bool key = true;

    public void locker() {
        key = false;
    }

    public void unlocker() {
        //Debug.Log("Change Time");
        rechargeTime = timeInterval;
        key = true;
    }


    // Use Start() to gather how long the enemy should wait before attacking
    void Start()
    {
        rightUse = true;
        leftUse = true;
        rechargeTime = timeInterval;
    }

    // Use Update() to decrement time and initate an attack from HandBehavior
    void Update()
    {
        bool atkDone = false;
        // Decrement the time if it has not ran out yet
        if (rechargeTime > 0 && key) {
            rechargeTime = rechargeTime - Time.deltaTime;
            //Debug.Log("Countdown:" + rechargeTime);
        }
        // If time is ran out, the enemy is able to perform an attack;
        // A key check is used here to prevent multiple usages of this elseif case simotainously
        else if (key) {
            
            // Lock this section; the actual attack's conclusion will unlock the key
            locker();
            
            /* Generate which hand to use and which attack to use
            Body
                0 = Right
                1 = Left
                2 = Head

            Attack (Hand)
                0 = Punch
                1 = Swipe/Sweep
                2 = Clap

            Attack (Head)
                0 = Punch
                1/2 = Missle
            */
            int atkUse = Random.Range(0,3);
            int bodyUse = Random.Range(0,3);
            // Right Hand then attack call
            if (bodyUse == 0 && rightUse) {
                if (atkUse == 0) {
                    dmg.setDmg(15);
                    punch(0);
                    atkDone = true;
                }
                else if (atkUse == 1) {
                    dmg.setDmg(10);
                    sweep(0);
                    atkDone = true;
                }
                else if (atkUse == 2 && leftUse) {
                    dmg.setDmg(5);
                    clap();
                    atkDone = true;
                }
                // Reset the charge time after attack has been initated
            }
            // Hand Left Hand then attack call
            else if (bodyUse == 1 && leftUse) {
                if (atkUse == 0) {
                    dmg.setDmg(15);
                    punch(1);
                    atkDone = true;
                }
                else if (atkUse == 1) {
                    dmg.setDmg(10);
                    sweep(1);
                    atkDone = true;
                }
                else if (atkUse == 2 && rightUse) {
                    dmg.setDmg(5);
                    clap();
                    atkDone = true;
                }
                // Reset the charge time after attack has been initated
            }
            else if (bodyUse == 2) {
                if (atkUse == 0) {
                    dmg.setDmg(20);
                    punch(2);
                    atkDone = true;
                }
                else if (atkUse == 1){
                    dmg.setDmg(10);
                    //Debug.Log("Missles Shot");
                    missle();
                    atkDone = true;
                }
            }
            if (!atkDone)
                key = true;
        }
    }

    // punch() calls the punch attack based on which hand it is using
    void punch(int body) {
        Transform target = getPunchTarget(body);
        // Handiness determines which hand to use
        if (body == 0) 
            rh.callPunch(target);
        else if (body == 1) 
            lh.callPunch(target);
        else 
            head.callPunch(target);
    }


    // swipe() calls the swipe attack based on which hand it is using
    void sweep(int hand) {
        Transform[] sweepTargets = getSweepTarget();
        // Handiness determines which hand to use and the zones that dictate the motion
        if (hand == 0) 
            rh.callSwipe(sweepTargets[0], sweepTargets[1]);
        else 
            lh.callSwipe(sweepTargets[1], sweepTargets[0]);
    }

    void clap() {
        int levelNum = Random.Range(0,2);
        Transform clapLevel = clapZones.transform.GetChild(levelNum).gameObject.transform;
        lh.callClap(clapLevel.transform.GetChild(3).gameObject.transform, clapLevel.transform.GetChild(4).gameObject.transform, clapLevel.transform.GetChild(5).gameObject.transform);
        rh.callClap(clapLevel.transform.GetChild(0).gameObject.transform, clapLevel.transform.GetChild(1).gameObject.transform, clapLevel.transform.GetChild(2).gameObject.transform);
    }

    void missle() {
        int scenarioNum = Random.Range(1,4);
        head.callMissle(scenarioNum);
    }


    // getPunchTarget() is a helper function to generate a random area to launch the punch towards
    Transform getPunchTarget(int body) {
        // Generate the location to use;
        int levelNum = Random.Range(0, 2);
        int scenarioNum = Random.Range(0, 2);

        Transform punchLevel =  punchZones.transform.GetChild(levelNum).gameObject.transform;
        // Each hand can only punch on their respective side (Scenarios 0 and 1) 
        // (Ex. the left hand will not go diagonal to attack the right side of the screen)
        // So two checks are made in an expression, but both hands are allowed to attack the center of the path (Scenarios 2 and 3)

        if (scenarioNum == 0 && body != 2) {
            // Top Left Side of Screen
            return punchLevel.transform.GetChild(body).gameObject.transform;
        }
        else if (scenarioNum == 1 && body != 2) {
            // Top Right Side of Screen
            return punchLevel.transform.GetChild(2).gameObject.transform;
        }
        else {
            scenarioNum = Random.Range(0, 3);
            return punchLevel.transform.GetChild(scenarioNum).gameObject.transform;
        }
    }

    // getSweepTarget() is a helper function to generate the randomized level to sweep across
    Transform[] getSweepTarget() {
        Transform[] retArray = new Transform[2];
        // Generate a random number to determine the attack's elevation
        int levelNum = Random.Range(0,2);
        Transform swipeLevel = swipeZones.transform.GetChild(levelNum).gameObject.transform;
        retArray[0] = swipeLevel.transform.GetChild(0).gameObject.transform;
        retArray[1] = swipeLevel.transform.GetChild(1).gameObject.transform;
        return retArray;
    }

    // Functions used by the Game Monitor to disable hand use
    public void disableLeft() {
        leftUse = false;
        lh.setDefeat();
    }

    public void disableRight() {
        rightUse = false;
        rh.setDefeat();
    }

    


}

