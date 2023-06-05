using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorBehavior : MonoBehaviour
{
    public GameMonitor gm;
    public GameObject baseTile;
    // The speed at which the environment will move
    public float speed = 7.5f;

    public List<GameObject> obstaclePrefabs;
    public List<GameObject> obstacleReserves;

    // This object's rigidbody component
    private Rigidbody rb;

    /* Public functions for the phase transitition of tiles */
    // Have the current tiles start to fall off the map
    public void stopActivity() {
        if (rb.useGravity == false) {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        if (rb.useGravity == true)
            rb.AddForce(Physics.gravity * (rb.mass * rb.mass));
    
    }

    // Adjust the current obstaclesPrefabs list
    public void resetObstacles(int phase) {
        for (int i = obstaclePrefabs.Count - 1; i >= 0; i--)
            obstaclePrefabs.RemoveAt(i);
        if (phase == 2) {
            obstaclePrefabs.Add(obstacleReserves[0]);
            obstaclePrefabs.Add(obstacleReserves[1]);
        }
        else if (phase == 3) {
            obstaclePrefabs.Add(obstacleReserves[2]);
            obstaclePrefabs.Add(obstacleReserves[3]);
        }
    }

    void Start() {
        // Store Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        // Filter for destroyer tag
        if(other.gameObject.tag == "gDestroy") {
            // Instantiate new floor tile some distance away
            GameObject newTile = Instantiate(gameObject, gameObject.transform.position + new Vector3(0, 0, 70), Quaternion.identity);
            // Reset name (to get rid of "(clone)" ending)
            newTile.name = gameObject.name;

            foreach (Transform child in newTile.transform) {
                if (child.gameObject.name != "Road") {
                    Destroy(child.gameObject);
                }
            }

            if (Random.value < 0.4) {
                for (int i = 0; i < 4; ++i) {
                    if (Random.value < 0.4) {
                        GameObject obstacle = obstaclePrefabs[i != 1 && i != 2 ? Random.Range(0, obstaclePrefabs.Count) : 0];
                        GameObject temp = Instantiate(obstacle);
                        temp.transform.SetParent(newTile.transform, false);
                        temp.transform.Translate(new Vector3((i * 6) - 9, 0, 0), Space.Self);
                        
                        ObstacleDamageDealer tempScript = temp.GetComponent<ObstacleDamageDealer>();
                        tempScript.gm = gm;
                    }
                }
            }

            // Destroy this object
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        // Calculate velocity of the tile
        Vector3 constForward = Vector3.back * speed * Time.deltaTime;
        // Move the tile using the velocity
        // Added line for gravity
        if (rb.useGravity == false)
            rb.MovePosition(rb.position + constForward);
    }
}
