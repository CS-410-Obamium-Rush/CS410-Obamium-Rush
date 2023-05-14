using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script allows the enemy head GameObject to rotate its head back and forth*/
public class EnemyRotator : MonoBehaviour
{
    private Quaternion initRot;
    public float speed = 0f; // How quickly
    public float range = 0f; // The total amount of area covered in degrees
    public float offset = 0f;   // The initial position to start looking (do 1/2 of range to face the screen)

    void Start()
    {
        initRot = transform.rotation;
    }


    void Update()
    {
        transform.localEulerAngles = new Vector3(initRot.x, Mathf.PingPong(Time.time * speed, range) - offset, initRot.z);
    }
}
