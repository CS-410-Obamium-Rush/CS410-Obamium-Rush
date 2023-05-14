using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
This script is used to test out the attack movement of the Enemy GameObject. This allows a button press of 1-8 on the keyboard
to perform a certain action.
*/
public class ButtonDebug : MonoBehaviour
{
    // Get the script for each hand
    public HandBehavior lh;
    public HandBehavior rh;
    public HeadBehavior head;

    // Get the GameObject to target
    public Transform punchH;
    public Transform punchL;
    public Transform punchR;
    public Transform swipeL1;
    public Transform swipeL2;
    public Transform swipeR1;
    public Transform swipeR2;

    public Transform clapLevel;
    
    





    /* Used to limit attacks to be one at a time
    The idea is to implement a lock-like system where an inititated attack will hold onto the lock
    and release the lock after the attack is done. These are public functions for the actual attacks 
    found in HandBehavior to decide the lock and unlock.

    This operates slightly different to AttackPatterns's lock system as their locker() function is used at different times.
    
    */
    private static bool key = true;
    public void locker() {
        key = false;
    }

    public void unlocker() {
        key = true;
    }
    
    /*
    Press number keys to activate something
    1: Quit Application
    2: turn off left hand's rotation
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
            Debug.Log("1 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha2) && key) {
            head.callMissle(2);
            Debug.Log("2 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha3) && key) {
            head.callPunch(punchH);
            Debug.Log("3 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha4) && key) {
            lh.callClap(clapLevel.transform.GetChild(3).gameObject.transform, clapLevel.transform.GetChild(4).gameObject.transform, clapLevel.transform.GetChild(5).gameObject.transform);
            rh.callClap(clapLevel.transform.GetChild(0).gameObject.transform, clapLevel.transform.GetChild(1).gameObject.transform, clapLevel.transform.GetChild(2).gameObject.transform);
            Debug.Log("4 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha5) && key) {
            lh.callPunch(punchL);
            Debug.Log("5 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha6) && key) {
            lh.callSwipe(swipeL1, swipeL2);
            Debug.Log("6 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha7) && key) {
            rh.callPunch(punchR);
            Debug.Log("7 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha8) && key) {
            rh.callSwipe(swipeR1, swipeR2);
            Debug.Log("8 was pressed");
        }
    }
}
