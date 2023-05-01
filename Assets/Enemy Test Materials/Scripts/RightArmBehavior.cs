using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArmBehavior : MonoBehaviour
{
    private float deltaTimeCount = 0;
    private Vector3 initPos;
    public float speed = 0;
    public float width = 0;
    public float height = 0;

    private bool rotator;
    private bool reset;
    void Start()
    {
        initPos = transform.position;
        rotator = true;
        reset = false;
    }


    void Update()
    {
        if (rotator) {
            deltaTimeCount += Time.deltaTime * speed;
            float x = Mathf.Cos(deltaTimeCount) * width;
            float y = Mathf.Sin(deltaTimeCount) * height;
            transform.position = new Vector3(initPos.x - x, initPos.y - y, initPos.z);
            reset = true;
        }
        else if (rotator == false && reset == true) {
            transform.position = initPos;
        }
    }

    public void setRotatorT() {
        rotator = true;
    }

    public void setRotatorF() {
        rotator = false;
    }
}
