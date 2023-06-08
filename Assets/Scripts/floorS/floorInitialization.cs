using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorInitialization : MonoBehaviour {
    // The number of tiles to have loaded at any given time
    public int tileCount = 7;
    
    // The floor tile prefab to be used
    private GameObject floortile;
    public GameObject floortilePhase1;
    public GameObject floortilePhase2;
    public GameObject floortilePhase3;

    public GameMonitor gm;

    public void placeNewTiles(int phase) {
        floorBehavior.resetObstacles(phase);
        for (int i = 0; i < tileCount; ++i) {
            // Create the floor tiles at increments of 10 meters
            GameObject floortile;
            if (phase == 2)
                floortile = floortilePhase2;
            else if (phase == 3)
                floortile = floortilePhase3;
            else // phase == 1
                floortile = floortilePhase1;

            GameObject tile = Instantiate(floortile, new Vector3(0, 0, (i * 10)), Quaternion.identity);
            // Reset name (to get rid of "(clone)" ending)
            tile.name = floortile.name;

            floorBehavior tileScript = tile.GetComponent<floorBehavior>();
            tileScript.gm = gm;
            // Set the spawn distance according to the number of tiles
            tileScript.spawnDistance = tileCount * 10;
        }
    }

    void Start() {
        placeNewTiles(1);
    }
}
