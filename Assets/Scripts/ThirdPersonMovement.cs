using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    public GameObject laser;
    public float speed = 6f;
    public float jumpAmount = 10;

    AudioSource m_AudioSource;  // EA contribution

    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private int jumpCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // get audioSource -- EA
        m_AudioSource = GetComponent<AudioSource>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue movementValue)
    {
        if(jumpCount < 1) 
        {
            rb.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);

            m_AudioSource.Play();
            jumpCount++;  
        }
    }

    private bool isGrounded()
    {
        if(transform.position.y < 0.6)
        {
            return true;
        }
        return false;
    }

    void FixedUpdate() 
    {
        if(isGrounded())
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            // rb.AddForce(movement * speed);
            rb.velocity = movement * speed;
            jumpCount = 0;
        }

        if(!(isGrounded()))
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed*15);
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = true;
        }
        else
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = false;
        }

    }
}

