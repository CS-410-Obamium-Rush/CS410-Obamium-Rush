/*
ButtonDebug: Used to test out the attack movement of the Enemy GameObject. This allows a button press of 1-8 on the keyboard
to perform a certain action. Use the escape key to quit the application at anytime.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonDebug : MonoBehaviour
{
    // Get the script for each hand
    public HandBehavior lh;
    public HandBehavior rh;
    public HeadBehavior head;

    // Get the GameObject to target; non-clap attacks need a specific zone to attack as opposed to the level
    public Transform punchH;
    public Transform punchL;
    public Transform punchR;
    public Transform swipeL1;
    public Transform swipeL2;
    public Transform swipeR1;
    public Transform swipeR2;
    public Transform clapLevel;
    public Transform slamSideL;
    public Transform slamSideR;
    public Transform expandH;



    // Get other Scripts to inflict player damage and alter player health
    public DamageDealer dmg;
    public GameMonitor gm;
    
    /* 
    Used to limit attacks to be one at a time
    The idea is to implement a lock-like system where an inititated attack will hold onto the lock
    and release the lock after the attack is done. These are public functions for the actual attacks 
    found in HandBehavior to decide the lock and unlock.

    This operates slightly different to AttackPatterns's lock system as their locker() function is used at different times.
    */
    private bool key = true;
    public void locker() {
        key = false;
    }

    public void unlocker() {
        key = true;
    }
    
    /*
    Press number keys to activate something
    Esc: Quit Application
    All attacks set to deal 10 health

    1: Restore player's health (Amount set to 100)
    2: use head's missle attack (Amount set to 2)
    3: use head's punch attack
    4: use both hand's clap attack
    5: use left hand's punch attack
    6: use left hand's swipe attack
    7: use right hand's punch attack
    8: use right hand's swipe attack
    */
    

    void Update() {
        if (Input.GetKey("escape")) {
            Application.Quit();
            Debug.Log("Esc was pressed");
        }
        
        if (Input.GetKey(KeyCode.Alpha1)) {
            gm.playerAddHealth(100); 
            Debug.Log("1 was pressed");
        }

        if (Input.GetKey(KeyCode.Alpha2) && key) {
            dmg.setDmg(10);
            head.callMissle(2);
            Debug.Log("2 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha3) && key) {
            dmg.setDmg(10);
            head.callPunch(punchH);
            Debug.Log("3 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha4) && key) {
            dmg.setDmg(10);
            lh.callClap(clapLevel.transform.GetChild(3).gameObject.transform, clapLevel.transform.GetChild(4).gameObject.transform, clapLevel.transform.GetChild(5).gameObject.transform);
            rh.callClap(clapLevel.transform.GetChild(0).gameObject.transform, clapLevel.transform.GetChild(1).gameObject.transform, clapLevel.transform.GetChild(2).gameObject.transform);
            Debug.Log("4 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha5) && key) {
            dmg.setDmg(10);
            lh.callPunch(punchL);
            Debug.Log("5 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha6) && key) {
            dmg.setDmg(10);
            lh.callSwipe(swipeL1, swipeL2);
            Debug.Log("6 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha7) && key) {
            dmg.setDmg(10);
            rh.callPunch(punchR);
            Debug.Log("7 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha8) && key) {
            dmg.setDmg(10);
            rh.callSwipe(swipeR1, swipeR2);
            Debug.Log("8 was pressed");
        }

        if (Input.GetKey(KeyCode.Alpha9) && key) {
            head.callLaser();
            Debug.Log("9 was pressed");
        }

        if (Input.GetKey(KeyCode.F1) && key) {
            dmg.setDmg(10);
            lh.callSlam(slamSideL.transform.GetChild(0).gameObject.transform, slamSideL.transform.GetChild(1).gameObject.transform);
            Debug.Log("F1 was pressed");
        }
        if (Input.GetKey(KeyCode.F2) && key) {
            dmg.setDmg(10);
            rh.callSlam(slamSideR.transform.GetChild(0).gameObject.transform, slamSideR.transform.GetChild(1).gameObject.transform);
            Debug.Log("F2 was pressed");
        }
        if (Input.GetKey(KeyCode.F3) && key) {
            dmg.setDmg(10);
            head.callExpand(expandH);
            Debug.Log("F3 was pressed");
        }

    }
}
