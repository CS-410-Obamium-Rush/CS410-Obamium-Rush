using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmBehavior : MonoBehaviour
{
    private float deltaTimeCount = 0;
    private Vector3 initPos;
    public float rotSpeed = 0;
    public float atkSpeed = 0;
    public float retractSpeed = 0;

    public float width = 0;
    public float height = 0;

    private bool rotator;
    private bool reset;
    public Transform target;
    private bool startAtk;
    private bool retractAtk;

    void Start()
    {
        initPos = transform.position;
        rotator = true;
        reset = false;
        startAtk = false;
        retractAtk = false;
    }

    void Update()
    {
        if (rotator) {
            print("rotate");
            deltaTimeCount += Time.deltaTime * rotSpeed;
            float x = Mathf.Cos(deltaTimeCount) * width;
            float y = Mathf.Sin(deltaTimeCount) * height;
            transform.position = new Vector3(initPos.x + x, initPos.y + y, initPos.z);
            reset = true;
        }
        else if (startAtk) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, atkSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.position) < 0.001f) {
                startAtk = false;
                retractAtk = true;
            }
        }
        else if (retractAtk) {
            transform.position = Vector3.MoveTowards(transform.position, initPos, retractSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initPos) < 0.001f) {
                print("start rotate again");
                retractAtk = false;
                rotator = true;
            }
        }
        /*else if (rotator == false && reset == true && startAtk == false && retractAtk == false) {
            transform.position = initPos;
        }*/
    }

    public void setRotatorT() {
        rotator = true;
    }

    public void setRotatorF() {
        rotator = false;
    }

    public void callAtk() {
        rotator = false;
        startAtk = true;
    }
}
