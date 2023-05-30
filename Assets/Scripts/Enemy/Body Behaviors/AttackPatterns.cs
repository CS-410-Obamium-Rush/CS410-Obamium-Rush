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
    public GameObject slamZones;

    // Get the left and right hand and head's behavior to call attacks
    public HandBehavior lh1;
    public HandBehavior rh1;
    public HandBehavior lh2;
    public HandBehavior rh2;
    public HeadBehavior head;

    // Decide if hands are allowed to use (i.e have been defeated yet)
    private bool leftUse1;
    private bool rightUse1;
    private bool leftUse2;
    private bool rightUse2;
    private bool headUse;

    // Used to set how much damage is inflicted for an attack
    public DamageDealer dmg;

    // Used to determine how many body parts and attacks the enemy has at their disposal
    private int atkAmt;
    private int bodyAmt;

    // Used to disable the attacks during phase transitions
    private bool phaseTransition = false;
    
    /* Public functions used for the phase transitions */
    // Determine new amount of body parts or attacks
    public void setAmt(int newAtkAmt, int newBodyAmt) {
        atkAmt = newAtkAmt;
        bodyAmt = newBodyAmt;
    }

    // Determine the recharge rate of attacks
    public void setTimeInterval(float newTime) {
        timeInterval = newTime;
    }
    // Used to disable or enable enemy attacks from occuring
    public void setPhaseTransition(bool val) {
        phaseTransition = val;
    }
    // Replace the current HeadBehavior instance to the new head's HeadBehavior instance
    public void setHeadBehavior(HeadBehavior newHead) {
        head = newHead.GetComponent<HeadBehavior>();
    }
    // Enable all hand usage for the next phase
    public void activateAllHands() {
        headUse = true;
        rightUse1 = true;
        rightUse2 = true;
        leftUse1 = true;
        leftUse2 = true;
    }
    /* 
    Using a lock system to prevent multiple attacks occuring at once; only one attack at a time
    The idea is to implement a lock-like system where an inititated attack will hold onto the lock
    and release the lock after the attack is done. These are public functions for the actual attacks 
    found in HandBehavior to decide the lock and unlock when appropiate.
    */
    private bool key = true;
    public bool getKey() {
        return key;
    }
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
        rightUse1 = true;
        leftUse1 = true;
        headUse = true;
        atkAmt = 2;
        bodyAmt = 3;
        rechargeTime = timeInterval;
    }

    // Use Update() to decrement time and initate an attack from HandBehavior
    void Update()
    {
        // Decrement the time if it has not ran out yet
        if (rechargeTime > 0 && key && !phaseTransition) {
            rechargeTime = rechargeTime - Time.deltaTime;
            // Debug line to verify the countdown works or not
            // Debug.Log("Countdown:" + rechargeTime);
        }
        
        // If time is ran out, the enemy is able to perform an attack;
        // A key check is used here to prevent multiple usages of this elseif case simotainously
        else if (key && !phaseTransition) {
            // Lock this section; the actual attack's conclusion or if an invalid hand attack is selected (won't be used) 
            // will unlock the key
            locker();
            
            /* Generate which hand to use and which attack to use
            Body
                0 = Right (Phase 1)
                1 = Left (Phase 1)
                2 = Head
                3 = Right (Phase 2)
                4 = Left (Phase 2)
            Attack (Hand)
                0 = Punch
                1 = Swipe/Sweep
                2 = Clap
                3 = Slam Shockwave
            Attack (Head)
                0 = Punch
                1 = Missle
                2 = Laser
            */
            int atkUse = Random.Range(0,atkAmt);
            int bodyUse = Random.Range(0,bodyAmt);

            // When a valid attack could not be used, release the key to allow another reroll for a valid attack
            if (! callAtk(atkUse, bodyUse)) {
                key = true;
            }

                
        }
    }

    /*

    callAtk: used to activate an attack based on which attack id and body part; returns a bool to determine if an attack was actually
    pulled off or not.

    Format:
        Check body
            Check which attack
                Set damage amount
                Call the attack
                verify that a valid attack is used
    */

    private bool callAtk(int atkUse, int bodyInput) {
        // Use atkDone to indicate
        bool atkDone = false;
        if (bodyInput == 2 && headUse) {
            if (atkUse == 0) {
                dmg.setDmg(20);
                //Debug.Log("Punch");
                punch(bodyInput);
                atkDone = true;
            }
            else if (atkUse == 1) {
                //Debug.Log("Missle");
                dmg.setDmg(10);
                missle();
                atkDone = true;
            }
            else if (atkUse == 2 || atkUse == 3){
                //Debug.Log("Laser");
                laser();
                atkDone = true;
            }
        }
        else {
            if (atkUse == 0) {
                dmg.setDmg(15);
                if (bodyInput == 0 && rightUse1) {
                    punch(bodyInput);
                    atkDone = true;
                }
                else if (bodyInput == 1 && leftUse1) {
                    punch(bodyInput);
                    atkDone = true;
                }
                else if (bodyInput == 3 && rightUse2) {
                    punch(bodyInput);
                    atkDone = true;
                }
                else if (bodyInput == 4 && leftUse2) {
                    punch(bodyInput);
                    atkDone = true;
                }
            }
            else if (atkUse == 1) {
                dmg.setDmg(10);
                if (bodyInput == 0 && rightUse1) {
                    sweep(bodyInput);
                    atkDone = true;
                }
                else if (bodyInput == 1 && leftUse1) {
                    sweep(bodyInput);
                    atkDone = true;
                }
                else if (bodyInput == 3 && rightUse2) {
                    sweep(bodyInput);
                    atkDone = true;
                }
                else if (bodyInput == 4 && leftUse2) {
                    sweep(bodyInput);
                    atkDone = true;
                }
            }
            else if (atkUse == 2) {
                dmg.setDmg(5);
                if ((bodyInput == 0 || bodyInput == 1) && (rightUse1 && leftUse1)) {
                    clap(0);
                    atkDone = true;
                }
                else if ((bodyInput == 3 || bodyInput == 4) && (rightUse2 && leftUse2)) {
                    clap(1);
                    atkDone = true;
                }
            }
            else if (atkUse == 3) {
                dmg.setDmg(10);
                if (bodyInput == 0 && rightUse1) {
                    slam(bodyInput);
                    atkDone = true;
                }
                else if (bodyInput == 1 && leftUse1) {
                    slam(bodyInput);
                    atkDone = true;
                }
                else if (bodyInput == 3 && rightUse2) {
                    slam(bodyInput);
                    atkDone = true;
                }
                else if (bodyInput == 4 && leftUse2) {
                    slam(bodyInput);
                    atkDone = true;
                }
            }
        }
        return atkDone;
    }

    /*
    Functions that initate the attack
        Most of them have a random number generator to decide whether to attack high or low
        0 = High
        1 = Low

        Most will also have another random number generator that varies.

    */
    // punch() calls the punch attack based on which body part it is using
    void punch(int body) {
        // Get the randomly selected zone to use
        Transform target = getPunchTarget(body);
        // Handiness determines which hand to use
        if (body == 0) 
            rh1.callPunch(target);
        else if (body == 1) 
            lh1.callPunch(target);
        else if (body == 2)
            head.callPunch(target);
        else if (body == 3) 
            rh2.callPunch(target);
        else if (body == 4) 
            lh2.callPunch(target);
    }


    // swipe() calls the swipe attack based on which hand it is using
    void sweep(int hand) {
        // Get the randomly selected zone level to use
        Transform[] sweepTargets = getSweepTarget();
        // Handiness determines which hand to use and the zones that dictate the motion
        if (hand == 0) 
            rh1.callSwipe(sweepTargets[0], sweepTargets[1]);
        else if (hand == 1) 
            lh1.callSwipe(sweepTargets[1], sweepTargets[0]);
        else if (hand == 3) 
            rh2.callSwipe(sweepTargets[0], sweepTargets[1]);
        else if (hand == 4) 
            lh2.callSwipe(sweepTargets[1], sweepTargets[0]);
        
    }

    // clap() calls the hand attack where both hands clap together; only works when both hands are still active
    // handGroup involves which pair of hands to use
    void clap(int handGroup) {
        // Generate whether to go high or low
        int levelNum = Random.Range(0,2);
        // Gather the level to get the targets (children of the level's GameObject)
        Transform clapLevel = clapZones.transform.GetChild(levelNum).gameObject.transform;
        if (handGroup == 0) {
            lh1.callClap(clapLevel.transform.GetChild(3).gameObject.transform, clapLevel.transform.GetChild(4).gameObject.transform, clapLevel.transform.GetChild(5).gameObject.transform);
            rh1.callClap(clapLevel.transform.GetChild(0).gameObject.transform, clapLevel.transform.GetChild(1).gameObject.transform, clapLevel.transform.GetChild(2).gameObject.transform);
        }
        else {
            lh2.callClap(clapLevel.transform.GetChild(3).gameObject.transform, clapLevel.transform.GetChild(4).gameObject.transform, clapLevel.transform.GetChild(5).gameObject.transform);
            rh2.callClap(clapLevel.transform.GetChild(0).gameObject.transform, clapLevel.transform.GetChild(1).gameObject.transform, clapLevel.transform.GetChild(2).gameObject.transform);
        }
    }

    // missle() calls the missle attack where the enemy fires off a few projectiles that track the player and vanish when
    // they either hit the player or ground
    void missle() {
        // Generate the amount of missles to fire (1-3)
        int scenarioNum = Random.Range(1,4);
        head.callMissle(scenarioNum);
    }

    void laser() {
        //Debug.Log("Fire Laser");
        head.callLaser();
    }

    void slam(int hand) {
        Transform[] slamTargets = getSlamTarget();
        if (hand == 0) 
            rh1.callSlam(slamTargets[0], slamTargets[1]);
        else if (hand == 1) 
            lh1.callSlam(slamTargets[0], slamTargets[1]);
        else if (hand == 3) 
            rh2.callSlam(slamTargets[0], slamTargets[1]);
        else if (hand == 4) 
            lh2.callSlam(slamTargets[0], slamTargets[1]);
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
            if (body > 2)
                return punchLevel.transform.GetChild(body - 3).gameObject.transform;
            else
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

    // getSlamTarget() is a helper function to generate the randomized level to sweep across
    Transform[] getSlamTarget() {
        Transform[] retArray = new Transform[2];
        // Generate a random number to determine the attack's level
        int scenarioNum = Random.Range(0, 3);
        Transform slamSide = slamZones.transform.GetChild(scenarioNum).gameObject.transform;
        retArray[0] = slamSide.transform.GetChild(0).gameObject.transform;
        retArray[1] = slamSide.transform.GetChild(1).gameObject.transform;
        return retArray;
    }

    /* 
    Functions used by the Game Monitor to disable hand use and let their HandBehavior know that
    the GameObject needs to indicate defeat
    */
    public void disableBody(int body) {
        if (body == 0) {
            rightUse1 = false;
            rh1.setDefeat();
        }
        else if (body == 1) {
            leftUse1 = false;
            lh1.setDefeat();
        }
        else if (body == 2) {
            headUse = false;
            head.setDefeat();
        }
        else if (body == 3) {
            rightUse2 = false;
            rh2.setDefeat();
        }
        else if (body == 4) {
            leftUse2 = false;
            lh2.setDefeat();
        }
    }
}

