using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorTile : MonoBehaviour{

    floorSpawner groundMaker;

    // Start is called before the first frame update
    void Start(){
        // only have one of these so it shouldn't cause any issues
        groundMaker = GameObject.FindObjectOfType<floorSpawner>();
    }

    private void OnTriggerExit(Collider other){
        // make new tiles
        groundMaker.spawnFloor();

        // erase old tiles 2 seconds after leaving them
        Destroy(gameObject, 2);
    }
}
