using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayerController : MonoBehaviour{

    public float speed = 5;
    public Rigidbody rb;

    // no need for start

    private void FixedUpdate(){

        Vector3 constForward = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + constForward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
