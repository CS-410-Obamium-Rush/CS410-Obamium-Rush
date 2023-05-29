using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{

    public float speed = 0.0f;
    public int time = 30;
    // Gather the gameobjects and details needed to have the enemy interact with the player
    public Transform playerHitBox;
    public GameMonitor gm;
    private bool contact = false;
    private bool invincible = false;
    private bool longEnough = false;


    // Start is called before the first frame update
    void Start()
    {
        transform.localEulerAngles = new Vector3(-69, 0, 0);
        StartCoroutine(spawnLaser());
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other) {
        // Player takes damage when the missles contact their hitbox
        if (other.gameObject.CompareTag("Player")) {
            contact = true;
        }
    }

    void OnTriggerExit(Collider other) {
        // Player takes damage when the missles contact their hitbox
        if (other.gameObject.CompareTag("Player")) {
            contact = false;
        }
    }

    void Update() {
        float rotSpeed = Time.deltaTime * speed;
        Vector3 fromPlayer = transform.position - playerHitBox.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, fromPlayer, rotSpeed, 0.0f);
        if (longEnough) {
            newDirection = Vector3.RotateTowards(transform.forward, fromPlayer, rotSpeed, 0.0f);
        } else {
            newDirection = Vector3.RotateTowards(transform.forward, fromPlayer, rotSpeed * 5, 0.0f);
        }
        transform.rotation = Quaternion.LookRotation(newDirection);
        if (contact && !invincible) {
            gm.playerTakeDamage(5);
            invincible = true;
            StartCoroutine(flashDamageColor());
        }
            
    }

    public void destroyLaser() {
        Destroy(gameObject);
    }

    IEnumerator flashDamageColor()
    {
        // Wait 1 second before the player can take damage again;
        // Planning to have the player sprite flashing in the near future
        yield return new WaitForSeconds(2f);
        invincible = false;
    }

    IEnumerator spawnLaser()
    {
        int children = transform.childCount;
        
        for (int i = 1; i < children; ++i) {
            transform.GetChild(i).gameObject.SetActive(true);
            if (i > 16)
                longEnough = true;
            yield return new WaitForSeconds(.05f);
        }
    }
}
    
