using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArmBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private float deltaTimeCount = 0;
    private Vector3 initPos;
    public float speed = 0;
    public float width = 0;
    public float height = 0;
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTimeCount += Time.deltaTime * speed;
        float x = Mathf.Cos(deltaTimeCount) * width;
        float y = Mathf.Sin(deltaTimeCount) * height;
        transform.position = new Vector3(initPos.x - x, initPos.y - y, initPos.z);
    }
}
