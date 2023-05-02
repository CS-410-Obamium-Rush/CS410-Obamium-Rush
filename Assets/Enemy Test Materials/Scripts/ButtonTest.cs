using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    public LeftArmBehavior lh;
    public LeftArmBehavior rh;

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
