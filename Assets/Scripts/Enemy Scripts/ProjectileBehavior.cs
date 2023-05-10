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
        transform.position = Vector3.MoveTowards(transform.position, movPlayer.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            gm.playerTakeDamage(10);
            Debug.Log("Player has been hit");
        }
        else if (other.gameObject.CompareTag("Damage")) {
            Debug.Log("Projectile has been hit");
        }
        head.countMissle();
        Destroy(gameObject);
    }
}
