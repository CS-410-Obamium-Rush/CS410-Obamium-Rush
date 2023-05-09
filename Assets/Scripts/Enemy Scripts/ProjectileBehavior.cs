using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    public HeadBehavior head;
    public int speed = 0;
    public Transform player;
    public GameMonitor gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
            gm.playerTakeDamage(10);
            head.countMissle();
            Debug.Log("Player has been hit");
        }
    
    }
}
