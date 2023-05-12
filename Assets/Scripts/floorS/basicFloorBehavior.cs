using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicFloorBehavior : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody rb;

    floorSpawner groundMaker;

    // Start is called before the first frame update
    void Start(){
        // only have one of these so it shouldn't cause any issues
        groundMaker = GameObject.FindObjectOfType<floorSpawner>();
    }

    private void OnTriggerEnter(Collider other){
        
        // check tag if it's the destroyer tag

        if(other.gameObject.tag == "gDestroy"){
            // erase old tiles 2 seconds after leaving them
            Destroy(gameObject, 2);
            // make new tiles

            groundMaker.spawnFloor();
        }
    }

    // this should move the tiles
    private void FixedUpdate(){
        Vector3 constForward = Vector3.back * speed * Time.deltaTime;
        rb.MovePosition(rb.position + constForward);
    }
}
