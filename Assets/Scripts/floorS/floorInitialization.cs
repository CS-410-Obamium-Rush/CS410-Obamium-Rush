using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorInitialization : MonoBehaviour {
    // The number of tiles to have loaded at any given time
    public int tileCount = 7;
    // The floor tile prefab to be used
    public GameObject floortile;

    void Start(){
        for (int i = 0; i < tileCount; ++i) {
            // Create the floor tiles at increments of 10 meters
            GameObject temp = Instantiate(floortile, Vector3.forward * (i * 10 - 10), Quaternion.identity);
            // Reset name (to get rid of "(clone)" ending)
            temp.name = floortile.name;
        }
    }
}
