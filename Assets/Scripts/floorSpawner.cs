using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//author: EAVI
// Co-author: Joey Le

public class floorSpawner : MonoBehaviour {
    private int tileCount;
    public GameObject floorTile;
    public GameObject spawnPoint;
    Vector3 nextSpawnPoint;
    private int count = 0;

    public void spawnFloor () {
        /* how to spawn an object in unity
        first param is item we're spawning
        second is where we're spawning it
        third is orientation, which is the identity of a Quaternion
        */
        // make a temp object
        GameObject temp = Instantiate(floorTile, transform.position, Quaternion.identity);

        // get the next spawnpoint
        //nextSpawnPoint = temp.transform.GetChild(1).transform.position;
    }

    /* public void spawnTile(){
        // other.transform.position = lastSpawnPos.position + Vector3(0, 0, 85.0f);
        // lastSpawnPos = other.transform;
        // GameObject temp = Instantiate(floorTile, Vector3(0,0,85.0f), Quaternion.identity);
    } */

    // Start is called before the first frame update
    void Start(){
        // spawn 5 pieces of floor
        // NOTE: the floor actually starts behind the player
        /*uint i;
        for(i = 0; i < 5; i++) {
            GameObject temp = Instantiate(floorTile, nextSpawnPoint, Quaternion.identity);
        }*/

        /* 
        ! I did some adjustments with the floor tile prefab and calucalted their widths and lengths;
        I use the width of z = 10 to seperate their spawn points apart in regards to where the groundDespawnFloor is (z = -13)
        Using z = 10 and starting from -13, I work my way up to z = 47 and this is where floor tiles will spawn at.
        x = 0 for all the floor tile and calulations. 
        */

        //! These variables names are terrible, feel free to change
        //! These create the spawn locations of the first tiles at the start of the game

        // use a for loop to set tiles
        // keep legacy names?
        Vector3 spawnFrontist = new Vector3(0,0,-13);
        Vector3 spawnFront = new Vector3(0,0,-3);
        Vector3 spawnBack = new Vector3(0,0,7);
        Vector3 spawnBacker = new Vector3(0,0,17);
        Vector3 spawnBackerer = new Vector3(0,0,27);
        Vector3 spawnBackererer = new Vector3(0,0,37);
        Vector3 spawnBackerererer = new Vector3(0,0,47); // causes generation here? Joey thinks so
        //! These created the floor tiles for the start of the game; the very far back one,
        //! Vector3 spawnBackerererer = new Vector3(0,0,47), will start the generation of new tiles when leaving the box collider
        //! Associated for the floorSpawner game object
        GameObject tempFrontist = Instantiate(floorTile, spawnFrontist, Quaternion.identity);
        GameObject tempFront = Instantiate(floorTile, spawnFront, Quaternion.identity);
        GameObject tempBack = Instantiate(floorTile, spawnBack, Quaternion.identity);
        GameObject tempBacker = Instantiate(floorTile, spawnBacker, Quaternion.identity);
        GameObject tempBackerer = Instantiate(floorTile, spawnBackerer, Quaternion.identity);
        GameObject tempBackererer = Instantiate(floorTile, spawnBackererer, Quaternion.identity);
        GameObject tempBackerererer = Instantiate(floorTile, spawnBackerererer, Quaternion.identity);
        //nextSpawnPoint = tempFrontist.transform.GetChild(1).transform.position;

    }
    //! This involves changing the structure so that the floor spawner is spawning the floors rather than
    //! basicFloorBehavior relying on the destruction of the tiles
    private void OnTriggerExit(Collider other){
        Debug.Log("Tile Created: " + count++);
        spawnFloor();
    }


}
