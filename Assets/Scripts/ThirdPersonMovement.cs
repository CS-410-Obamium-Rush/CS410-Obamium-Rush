using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    
    public float jumpAmount = 10;
    public float jumpForce=20;
    public float gravity = -9.81f;
    float velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        //Vector3 direction = new Vector3(horizontal, 0.0f, vertical).normalized;

        velocity += gravity * Time.deltaTime;
        if(transform.position.y < 0.1 && velocity < 0) 
        {
            velocity = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity = jumpForce;
        }  

        Vector3 direction = new Vector3(horizontal, velocity, 0.0f).normalized;

        if(direction.magnitude >= 0.1f)
        {
            controller.Move(direction * speed * Time.deltaTime);
        }     


        if(Input.GetKey(KeyCode.E))
        {
            Debug.Log("Firing");
        }

    }
}

