using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatterns : MonoBehaviour
{
    
    public float timeInterval;
    private float rechargeTime;
    public GameObject punchZones;
    public GameObject swipeZones;


    public HandBehavior lh;
    public HandBehavior rh;

    private bool key = true;
    private bool key2 = true;

    public void locker() {
        key = false;
    }

    public void unlocker() {
        key = true;
    }

    public void locker2() {
        key2 = false;
    }

    public void unlocker2() {
        key2 = true;
    }


    void Start()
    {
        rechargeTime = timeInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (rechargeTime > 0 && key2) {
            rechargeTime = rechargeTime - Time.deltaTime;
        }
        else if (key2) {
            locker2();
            Debug.Log("Initiate attack");
            int atkUse = Random.Range(0,2);
            print("Attack Use = " + atkUse);
            int handUse = Random.Range(0,2);
            print("Hand Use = " + handUse);
            if (handUse == 0) {
                if (atkUse == 0) {
                    punch(0);
                }
                else if (atkUse == 1) {
                    sweep(0);
                }
            }
            /* Left Hand */
            if (handUse == 1) {
                if (atkUse == 0) {
                    punch(1);
                }
                else if (atkUse == 1) {
                    sweep(1);
                }
            }
            rechargeTime = timeInterval;
        }
    }

    void punch(int hand) {
        Transform target = getPunchTarget(hand);
        if (hand == 0) {
            rh.callPunch(target);
        }
        else {
            lh.callPunch(target);
        }
    }

    void sweep(int hand) {
        Transform[] sweepTargets = getSweepTarget();
        if (hand == 0) {
            rh.callSwipe(sweepTargets[0], sweepTargets[1]);
        }
        else {
            lh.callSwipe(sweepTargets[1], sweepTargets[0]);
        }
    }

    Transform getPunchTarget(int hand) {
        int scenarioNum = Random.Range(0,4);
        print("Punch Use = " + scenarioNum);
        if (hand == 0 && scenarioNum == 0) {
            return punchZones.transform.GetChild(0).gameObject.transform;
        }
        else if (hand == 1 && scenarioNum == 0) {
            return punchZones.transform.GetChild(1).gameObject.transform;;
        }
        else if (hand == 0 && scenarioNum == 1) {
            return punchZones.transform.GetChild(2).gameObject.transform;
        }
        else if (hand == 1 && scenarioNum == 1) {
            return punchZones.transform.GetChild(3).gameObject.transform;
        }
        else if (scenarioNum == 2) {
            return punchZones.transform.GetChild(4).gameObject.transform;
        }
        else {//(scenarioNum == 3) {
            return punchZones.transform.GetChild(5).gameObject.transform;
        }
    }

    Transform[] getSweepTarget() {
        Transform[] retArray = new Transform[2];
        int scenarioNum = Random.Range(0,3);
        print("Swipe Use = " + scenarioNum);
        if (scenarioNum == 0) {
            retArray[0] = swipeZones.transform.GetChild(0).gameObject.transform;
            retArray[1] = swipeZones.transform.GetChild(1).gameObject.transform;
        }
        else if (scenarioNum == 1) {
            retArray[0] = swipeZones.transform.GetChild(2).gameObject.transform;
            retArray[1] = swipeZones.transform.GetChild(3).gameObject.transform;
        }
        else if (scenarioNum == 2) {
            retArray[0] = swipeZones.transform.GetChild(4).gameObject.transform;;
            retArray[1] = swipeZones.transform.GetChild(5).gameObject.transform;
        }
        return retArray;
    }


}

