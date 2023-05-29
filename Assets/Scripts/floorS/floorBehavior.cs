using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorBehavior : MonoBehaviour
{
    // The speed at which the environment will move
    public float speed = 7.5f;

    // This object's rigidbody component
    private Rigidbody rb;

    void Start() {
        // Store Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        // Filter for destroyer tag
        if(other.gameObject.tag == "gDestroy") {
            // Instantiate new floor tile some distance away
            GameObject temp = Instantiate(gameObject, gameObject.transform.position + new Vector3(0, 0, 70), Quaternion.identity);
            // Reset name (to get rid of "(clone)" ending)
            temp.name = gameObject.name;
            // Destroy this object
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        // Calculate velocity of the tile
        Vector3 constForward = Vector3.back * speed * Time.deltaTime;
        // Move the tile using the velocity
        rb.MovePosition(rb.position + constForward);
    }
}
