using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    private HeadBehavior head;
    public int speed = 0;
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
        transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = new Vector3(playerHitBox.position.x, 0, playerHitBox.position.z);
        transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            gm.playerTakeDamage(10);
            head.countMissle();
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Ground")) {
            head.countMissle();
            Destroy(gameObject);
        }

    }
}
