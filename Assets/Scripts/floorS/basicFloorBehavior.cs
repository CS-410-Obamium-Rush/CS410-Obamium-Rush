using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicFloorBehavior : MonoBehaviour
{
    public float speed = 7.5f;
    private Rigidbody rb;
    private static int count = 0;
    private GameObject fs;
    private floorSpawner floorCounter;

    /*
    ! I did create a new prefab that combines both gameObject elements of the original ground prefab.
    This new one is named NewGround. The code should still work with the original prefab in theory.

    ! An issue I ran was the deletion of 1 prefab instance caused the destruction of two tiles; I found out that
    it was because there was the box collider on the despawn Game Object and one on the floor tile. This caused
    the collision check to occur twice because of 2 box colliders, so I took off the box collider of the prefab.
    */

    // Start is called before the first frame update
    void Start(){
        // only have one of these so it shouldn't cause any issues
        //groundMaker = GameObject.FindObjectOfType<floorSpawner>();

        //! I had trouble figuring out what FindObjectOfType does, and with the change in floorSpawner;
        //! The next spawn point became pointless as it was always the same and instead assigned to
        //! the floorSpawner gameObject
        //! I also made the rigidBody a private variable
        rb = GetComponent<Rigidbody>();
    }

    // Allows gameobject to despawn then create a new floor
    private void OnTriggerExit(Collider other){
        // check tag if it's the destroyer tag
        if(other.gameObject.tag == "gDestroy"){
            // erase old tiles 2 seconds after leaving them
            //! I removed the 2 second deletion to create a consistent timing between creating and destroying tiles
            Destroy(gameObject); // Made this instance
            // make new tiles
            //! This is just a debug 
            Debug.Log("Tile Destroy: " + count++);
            //groundMaker.spawnFloor();
        }
    }

    // this should move the tiles
    private void FixedUpdate(){
        Vector3 constForward = Vector3.back * speed * Time.deltaTime;
        rb.MovePosition(rb.position + constForward);
        //transform.position = constForward;
        if (Input.GetKey("escape")) {
            Application.Quit();
            Debug.Log("1 was pressed");
        }
    }
}
