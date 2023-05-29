using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Author: Peter
//Co-author: EAVI

public class ThirdPersonMovement : MonoBehaviour
{
    public GameObject bullet;
    public GameObject shotgun;
    public float speed = 6f;
    public float jumpAmount = 10;

    // EA contribution
    public playerAudioManager playersfx; // need to figure out why the fuck this works
    AudioSource m_AudioSource;

    public enum Weapon {Bullet, Shotgun};

    private GameObject selected;
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
        selected = bullet;
    }

    void OnMove(InputValue movementValue)
    {
        //playersfx.playWalk();
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue movementValue)
    {
        if(jumpCount < 1) 
        {
            rb.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);

            playersfx.playJump();
            //playerAudioManager.instance.playJump();
            jumpCount++;  
        }
    }

    public void setWeapon(Weapon newWeapon) {
        var emissionModule = selected.GetComponent<ParticleSystem>().emission;
        emissionModule.enabled = false;
        switch (newWeapon) {
            case Weapon.Shotgun:
                selected = shotgun;
                playersfx.playSwitchShoggun();
                StartCoroutine(powerupTimer(5f));
                break;
            default:
                break;
        }
    }

    IEnumerator powerupTimer(float time) {
        yield return new WaitForSeconds(time);
        var emissionModule = selected.GetComponent<ParticleSystem>().emission;
        emissionModule.enabled = false;
        selected = bullet;
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

    void Update()
    {
        // EA QoL would like to make this while for the sfx
        if(Input.GetMouseButton(0))
        {
            //playersfx.playShoot();    //ethan wants to fix this since it overlaps with the jumping sfx
            //playerAudioManager.instance.playShoot();

            var emissionModule = selected.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = true;
        }
        // DO NOT REMOVE THIS ELSE
        // YOU WILL START AN INFINITE LOOP -- EAVI
        else
        {
            var emissionModule = selected.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = false;
        }
    }
}
