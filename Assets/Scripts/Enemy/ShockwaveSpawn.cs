using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveSpawn : MonoBehaviour {
    public static bool active = false;
    public GameObject shockwavePrefab;

    public void setActive(bool use) {
        active = use;
    }
    

    void OnTriggerEnter(Collider other) {
        // Player takes damage when the missles contact their hitbox
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0;
        if (active) {
            if (other.gameObject.CompareTag("Right Hand") || other.gameObject.CompareTag("Left Hand") || other.gameObject.CompareTag("Head")) {
                Instantiate(shockwavePrefab, spawnPos, Quaternion.identity);
                active = false;
            }
        }
        
    }
}
