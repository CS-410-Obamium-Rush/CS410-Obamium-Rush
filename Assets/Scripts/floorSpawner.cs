using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//author: EAVI

public class floorSpawner : MonoBehaviour {

    public GameObject floorTile;
    Vector3 nextSpawnPoint;

    public void spawnFloor () {
        /* how to spawn an object in unity
        first param is item we're spawning
        second is where we're spawning it
        third is orientation, which is the identity of a Quaternion
        */
        // make a temp object
        GameObject temp = Instantiate(floorTile, nextSpawnPoint, Quaternion.identity);

        // get the next spawnpoint
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;
    }

    // Start is called before the first frame update
    void Start(){

        // spawn 5 pieces of floor
        // NOTE: the floor actually starts behind the player
        uint i;
        for(i = 0; i < 5; i++) {
            spawnFloor();
        }
            
    }
}
