using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script allows a GameObject to float up and down; this is used for the enemy head*/

public class FloatBehavior : MonoBehaviour
{
    public float height; // Determine how far the GameObject will go
    public float freq;  // Determine how quickly the GameObject will go
    private Vector3 initPos;
    
    void Start()
    {
        initPos = transform.position;
    }

    void Update()
    {
        //Source for line: https://www.youtube.com/watch?v=kvQ-QWDWWZI
        transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * height + initPos.y, initPos.z);
    }
}
