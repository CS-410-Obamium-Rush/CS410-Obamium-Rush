using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotator : MonoBehaviour
{
    private Quaternion initRot;
    public float speed = 0f;
    public float range = 0f;
    public float offset = 0f;


    Vector3 from;
    Vector3 to;
    void Start()
    {
        initRot = transform.rotation;
        //to = new Vector3(0.0F, -1F * range, 0.0F);
        //from = new Vector3(0.0F, range, 0.0F);
    }

    // Update is called once per frame
    void Update()
    {

        //Quaternion fromRot = Quaternion.Euler(from);
        //Quaternion toRot = Quaternion.Euler(to);

        //float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * speed));
        //transform.localRotation = Quaternion.Lerp(fromRot, toRot, lerp);

        transform.localEulerAngles = new Vector3(initRot.x, Mathf.PingPong(Time.time * speed, range) - offset, initRot.z);

    }
}
