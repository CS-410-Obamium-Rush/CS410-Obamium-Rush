using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{

    public float speed = 0.0f;
    public int time = 30;
    // Gather the gameobjects and details needed to have the enemy interact with the player
    private HeadBehavior head;
    private Transform playerHitBox;
    private GameMonitor gm;
    private GameObject gmObject;
    private bool contact = false;
    private bool invincible = false;
    private GameObject spawnPoint;
    private bool getPosition = true;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GameObject.Find("ProjectileSpawner");
        gmObject = GameObject.Find("GameSettings");
        playerHitBox = GameObject.Find("MovingPlayer").transform.GetChild(0).gameObject.transform;
        gm = gmObject.GetComponent<GameMonitor>();
        head = GameObject.Find("Enemy").transform.GetChild(0).GetComponent<HeadBehavior>();
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
        if (getPosition) {
            transform.position = Vector3.MoveTowards(transform.position, spawnPoint.transform.position, 30 * Time.deltaTime);
            if (transform.position == spawnPoint.transform.position)
                getPosition = false;
        }
        else {
            
        }
        Vector3 fromPlayer = transform.position - playerHitBox.transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, fromPlayer, Time.deltaTime * speed, 0.0f);
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
}
