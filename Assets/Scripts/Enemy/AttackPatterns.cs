/*
AttackPatterns: let the enemy send a random attack after a fixed period of time

Extra note: GameObjects in the scene that have an L or R refer to left and right. These are in respect to the player's perspective
with the zones. The enemy's hands refer to their respective side; the player will see the enemy's left hand on the right of the screen
since the enemy will be facing the player. This means the enemy uses the opposite hand for the side of the screen (ex. the left hand attacks
the right hnad side of the screen)

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackPatterns : MonoBehaviour
{
    // Variables for calculating when an attack should appear
    public float timeInterval; // Refers to seconds in between attacks
    private float rechargeTime; // Amount of time needed to pass to initate an attack

    // Use the targets, or zones, stored in a grouped GameObject; these work similar to waypoints
    public GameObject punchZones;
    public GameObject swipeZones;
    public GameObject clapZones;

    // Get the left and right hand and head's behavior to call attacks
    public HandBehavior lh;
    public HandBehavior rh;
    public HeadBehavior head;

    // Decide if hands are allowed to use (i.e have been defeated yet)
    private bool leftUse;
    private bool rightUse;

    // Used to set how much damage is inflicted for an attack
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
        // Unlock also allows the countdown to start again
        rechargeTime = timeInterval;
        key = true;
    }

    // Use Start() to gather how long the enemy should wait before attacking and to allow the hands to be used
    void Start() {
        rightUse = true;
        leftUse = true;
        rechargeTime = timeInterval;
    }

    // Use Update() to decrement time and initate an attack from HandBehavior
    void Update()
    {
        // To check if a valid attack is used; assume not (false) until proven true
        bool atkDone = false;
        // Decrement the time if it has not ran out yet
        if (rechargeTime > 0 && key) {
            rechargeTime = rechargeTime - Time.deltaTime;
            // Debug line to verify the countdown works or not
            //Debug.Log("Countdown:" + rechargeTime);
        }
        // If time is ran out, the enemy is able to perform an attack;
        // A key check is used here to prevent multiple usages of this elseif case simotainously
        else if (key) {
            // Lock this section; the actual attack's conclusion or if an invalid hand attack is selected (won't be used) 
            // will unlock the key
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
            
            /*
            Format:
            Check body
                Check which attack
                    Set damage amount
                    Call the attack
                    verify that a valid attack is used
            */
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
            }
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
            }
            else if (bodyUse == 2) {
                if (atkUse == 0) {
                    dmg.setDmg(20);
                    punch(2);
                    atkDone = true;
                }
                else if (atkUse == 1){
                    dmg.setDmg(10);
                    missle();
                    atkDone = true;
                }
            }
            // When a valid attack could not be used, release the key to allow another reroll for a valid attack
            if (!atkDone)
                key = true;
        }
    }
    /*
    Functions that initate the attack
        Most of them have a random number generator to decide whether to attack high or low
        0 = High
        1 = Low

        Most will also have another random number generator that veries.

    */


    // punch() calls the punch attack based on which body part it is using
    void punch(int body) {
        // Get the randomly selected zone to use
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
        // Get the randomly selected zone level to use
        Transform[] sweepTargets = getSweepTarget();
        // Handiness determines which hand to use and the zones that dictate the motion
        if (hand == 0) 
            rh.callSwipe(sweepTargets[0], sweepTargets[1]);
        else 
            lh.callSwipe(sweepTargets[1], sweepTargets[0]);
    }

    // clap() calls the hand attack where both hands clap together; only works when both hands are still active
    void clap() {
        // Generate whether to go high or low
        int levelNum = Random.Range(0,2);
        // Gather the level to get the targets (children of the level's GameObject)
        Transform clapLevel = clapZones.transform.GetChild(levelNum).gameObject.transform;
        lh.callClap(clapLevel.transform.GetChild(3).gameObject.transform, clapLevel.transform.GetChild(4).gameObject.transform, clapLevel.transform.GetChild(5).gameObject.transform);
        rh.callClap(clapLevel.transform.GetChild(0).gameObject.transform, clapLevel.transform.GetChild(1).gameObject.transform, clapLevel.transform.GetChild(2).gameObject.transform);
    }

    // missle() calls the missle attack where the enemy fires off a few projectiles that track the player and vanish when
    // they either hit the player or ground
    void missle() {
        // Generate the amount of missles to fire (1-3)
        int scenarioNum = Random.Range(1,4);
        head.callMissle(scenarioNum);
    }


    // getPunchTarget() is a helper function to generate a random area to launch the punch towards
    Transform getPunchTarget(int body) {
        // Generate the level to use;
        int levelNum = Random.Range(0, 2);
        // Generate whether to attack respective side for the hand or the center within the selected level
        int scenarioNum = Random.Range(0, 2);

        Transform punchLevel =  punchZones.transform.GetChild(levelNum).gameObject.transform;
        // Each hand can only punch on their respective side
        // (Ex. the enemy left hand will not go diagonal to attack the left side of the screen)
        /* 
        sceenarioNum:
            Hands: dictate whether to attack their respective side (== 0) or the center (== 1)
            Head: dictate whether to attack the left (== 0), right (== 1), or center (== 2) of the screen
        */

        if (scenarioNum == 0 && body != 2) {
            return punchLevel.transform.GetChild(body).gameObject.transform;
        }
        else if (scenarioNum == 1 && body != 2) {
            return punchLevel.transform.GetChild(2).gameObject.transform;
        }
        // Body is the head, so a reroll is needed to decide which zone to attack at
        else {
            scenarioNum = Random.Range(0, 3);
            return punchLevel.transform.GetChild(scenarioNum).gameObject.transform;
        }
    }

    // getSweepTarget() is a helper function to generate the randomized level to sweep across
    Transform[] getSweepTarget() {
        Transform[] retArray = new Transform[2];
        // Generate a random number to determine the attack's level
        int levelNum = Random.Range(0,2);
        Transform swipeLevel = swipeZones.transform.GetChild(levelNum).gameObject.transform;
        retArray[0] = swipeLevel.transform.GetChild(0).gameObject.transform;
        retArray[1] = swipeLevel.transform.GetChild(1).gameObject.transform;
        return retArray;
    }

    /* 
    Functions used by the Game Monitor to disable hand use and let their HandBehavior know that
    the GameObject needs to indicate defeat
    */
    public void disableLeft() {
        leftUse = false;
        lh.setDefeat();
    }

    public void disableRight() {
        rightUse = false;
        rh.setDefeat();
    }
}

