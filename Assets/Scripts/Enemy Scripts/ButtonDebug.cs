using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDebug : MonoBehaviour
{
     // Get the script for each hand
    public HandBehavior lh;
    public HandBehavior rh;

    /*
    Press number keys to activate something
    1: turn on left hand's rotation
    2: turn off left hand's rotation
    3: turn on right hand's rotation
    4: turn off hand hand's rotation
    5: use left hand's punch attack
    6: use left hand's swipe attack
    7: use right hand's punch attack
    8: use right hand's swipe attack
    */

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)) {
            lh.setRotatorT();
            Debug.Log("1 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha2)) {
            lh.setRotatorF();
            Debug.Log("2 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha3)) {
            rh.setRotatorT();
            Debug.Log("3 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha4)) {
            rh.setRotatorF();
            Debug.Log("4 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha5)) {
            lh.callPunch();
            Debug.Log("5 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha6)) {
            lh.callSwipe();
            Debug.Log("6 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha7)) {
            rh.callPunch();
            Debug.Log("7 was pressed");
        }
        if (Input.GetKey(KeyCode.Alpha8)) {
            rh.callSwipe();
            Debug.Log("8 was pressed");
        }
    }
}
