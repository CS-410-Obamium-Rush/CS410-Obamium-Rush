using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    private HeadBehavior head;
    public int speed = 0;
    private Transform movPlayer;
    private GameMonitor gm;
    private GameObject playerGO;
    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("Player");
        movPlayer = playerGO.transform.GetChild(1).gameObject.transform;
        gm = playerGO.GetComponent<GameMonitor>();
        head = GameObject.Find("Enemy").transform.GetChild(0).GetComponent<HeadBehavior>();
        transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = new Vector3(movPlayer.position.x, 0, movPlayer.position.z);
        transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            gm.playerTakeDamage(10);
            //Debug.Log("Player has been hit");
        }
        /*
        else if (other.gameObject.CompareTag("Damage")) {
            //Debug.Log("Projectile has been hit");
        }
        else if (other.gameObject.CompareTag("Ground")) {
            Debug.Log("Ground has been hit");
        }
        */
        head.countMissle();
        Destroy(gameObject);
    }
}
