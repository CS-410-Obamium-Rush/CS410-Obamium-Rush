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
    // Get the left and right hand's behavior to call attacks
    public HandBehavior lh;
    public HandBehavior rh;

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
        key = true;
    }

    // Use Start() to gather how long the enemy should wait before attacking
    void Start()
    {
        rechargeTime = timeInterval;
    }

    // Use Update() to decrement time and initate an attack from HandBehavior
    void Update()
    {
        // Decrement the time if it has not ran out yet
        if (rechargeTime > 0 && key) {
            rechargeTime = rechargeTime - Time.deltaTime;
        }
        // If time is ran out, the enemy is able to perform an attack;
        // A key check is used here to prevent multiple usages of this elseif case simotainously
        else if (key) {
            // Lock this section; the actual attack's conclusion will unlock the key
            locker();
            /* Generate which hand to use and which attack to use
            Hand
                0 = Right
                1 = Left
            Attack
                0 = Punch
                1 = Swipe/Sweep
            */
            int atkUse = Random.Range(0,2);
            int handUse = Random.Range(0,2);
            // Right Hand then attack call
            if (handUse == 0) {
                if (atkUse == 0) {
                    punch(0);
                }
                else if (atkUse == 1) {
                    sweep(0);
                }
            }
            // Hand Left Hand then attack call
            if (handUse == 1) {
                if (atkUse == 0) {
                    punch(1);
                }
                else if (atkUse == 1) {
                    sweep(1);
                }
            }
            // Reset the charge time after attack has been initated
            rechargeTime = timeInterval;
        }
    }

    // punch() calls the punch attack based on which hand it is using
    void punch(int hand) {
        Transform target = getPunchTarget(hand);
        // Handiness determines which hand to use
        if (hand == 0) {
            rh.callPunch(target);
        }
        else {
            lh.callPunch(target);
        }
    }

    // swipe() calls the swipe attack based on which hand it is using
    void sweep(int hand) {
        Transform[] sweepTargets = getSweepTarget();
        // Handiness determines which hand to use and the zones that dictate the motion
        if (hand == 0) {
            rh.callSwipe(sweepTargets[0], sweepTargets[1]);
        }
        else {
            lh.callSwipe(sweepTargets[1], sweepTargets[0]);
        }
    }

    // getPunchTarget() is a helper function to generate a random area to launch the punch towards
    Transform getPunchTarget(int hand) {
        // Generate the location to use;
        int scenarioNum = Random.Range(0,4);
        // Each hand can only punch on their respective side (Scenarios 0 and 1) 
        // (Ex. the left hand will not go diagonal to attack the right side of the screen)
        // So two checks are made in an expression, but both hands are allowed to attack the center of the path (Scenarios 2 and 3)

        if (hand == 0 && scenarioNum == 0) {
            // Top Side of Screen
            return punchZones.transform.GetChild(0).gameObject.transform;
        }
        else if (hand == 1 && scenarioNum == 0) {
            // Top Side of Screen
            return punchZones.transform.GetChild(1).gameObject.transform;;
        }
        else if (hand == 0 && scenarioNum == 1) {
            // Bottom Side of Screen
            return punchZones.transform.GetChild(2).gameObject.transform;
        }
        else if (hand == 1 && scenarioNum == 1) {
            // Bottom Side of Screen
            return punchZones.transform.GetChild(3).gameObject.transform;
        }
        else if (scenarioNum == 2) {
            // Top Center
            return punchZones.transform.GetChild(4).gameObject.transform;
        }
        else {//(scenarioNum == 3) 
            // Bottom Center
            return punchZones.transform.GetChild(5).gameObject.transform;
        }
    }

    // getSweepTarget() is a helper function to generate the randomized level to sweep across
    Transform[] getSweepTarget() {
        Transform[] retArray = new Transform[2];
        // Generate a random number to determine the attack's elevation
        int scenarioNum = Random.Range(0,3);
        // High
        if (scenarioNum == 0) {
            retArray[0] = swipeZones.transform.GetChild(0).gameObject.transform;
            retArray[1] = swipeZones.transform.GetChild(1).gameObject.transform;
        }
        // Middle
        else if (scenarioNum == 1) {
            retArray[0] = swipeZones.transform.GetChild(2).gameObject.transform;
            retArray[1] = swipeZones.transform.GetChild(3).gameObject.transform;
        }
        // Low
        else if (scenarioNum == 2) {
            retArray[0] = swipeZones.transform.GetChild(4).gameObject.transform;;
            retArray[1] = swipeZones.transform.GetChild(5).gameObject.transform;
        }
        return retArray;
    }


}

