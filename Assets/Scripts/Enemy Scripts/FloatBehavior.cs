using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatBehavior : MonoBehaviour
{
    public float height;
    public float freq;
    private Vector3 initPos;
    
    void Start()
    {
        initPos = transform.position;
    }

    void Update()
    {
        Vector3 moveUp = new Vector3(0, 50, 0);
        Vector3 moveDown = new Vector3(0, -100, 0);
        transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * height + initPos.y, initPos.z);
        //https://www.youtube.com/watch?v=kvQ-QWDWWZI
    }
}
