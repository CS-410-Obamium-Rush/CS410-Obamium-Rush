using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{

    public int speed = 0;
    public int time = 30;
    // Gather the gameobjects and details needed to have the enemy interact with the player
    private HeadBehavior head;
    private Transform playerHitBox;
    private GameMonitor gm;
    private GameObject gmObject;

    // Start is called before the first frame update
    void Start()
    {
        gmObject = GameObject.Find("GameSettings");
        playerHitBox = GameObject.Find("MovingPlayer").transform.GetChild(0).gameObject.transform;
        gm = gmObject.GetComponent<GameMonitor>();
        head = GameObject.Find("Enemy").transform.GetChild(0).GetComponent<HeadBehavior>();
        /*
        float xRot = calcXRot();
        float yRot = calcYRot();
        Quaternion target = Quaternion.Euler(xRot, yRot, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * speed);
        */
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other) {
        // Player takes damage when the missles contact their hitbox
        if (other.gameObject.CompareTag("Player")) {
            gm.playerTakeDamage(20);
        }
    }

    void Update() {
        float xRot = calcXRot();
        float yRot = calcYRot();
        Quaternion target = Quaternion.Euler(50, yRot, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * speed);
    }

    float calcXRot() {
        Vector3 fromPlayer = playerHitBox.transform.position - transform.position;
        Vector3 downward = Vector3.down; 
        float dotVal = Vector3.Dot(fromPlayer, downward);
        float multMag = Vector3.Magnitude(fromPlayer) * Vector3.Magnitude(downward);
        return Mathf.Rad2Deg * Mathf.Acos(dotVal / multMag);
    }

    float calcYRot() {
        Vector3 fromPlayer = playerHitBox.transform.position - transform.position;
        Vector3 backward = Vector3.back * 5; 
        float dotVal = Vector3.Dot(fromPlayer, backward);
        float multMag = Vector3.Magnitude(fromPlayer) * Vector3.Magnitude(backward);
        return Mathf.Rad2Deg * Mathf.Acos(dotVal / multMag);
    }

    public void destroyLaser() {
        Destroy(gameObject);
    }
}
