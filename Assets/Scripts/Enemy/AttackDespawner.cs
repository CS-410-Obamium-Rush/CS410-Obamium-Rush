using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDespawner : MonoBehaviour
{
    void OnTriggerExit(Collider other) {
        // Player takes damage when the missles contact their hitbox
        if (other.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }
}
