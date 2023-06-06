using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorBehavior : MonoBehaviour
{
    public GameMonitor gm;
    public GameObject baseTile;
    // The speed at which the environment will move
    public float speed = 7.5f;

    private GameObject[] obstacles;
    public GameObject[] obstaclePrefabsPhase1 = new GameObject[2];
    public GameObject[] obstaclePrefabsPhase2 = new GameObject[2];
    public GameObject[] obstaclePrefabsPhase3 = new GameObject[2];

    // This object's rigidbody component
    private Rigidbody rb;

    /* Public functions for the phase transitition of tiles */
    // Have the current tiles start to fall off the map
    public void stopActivity() {
        if (rb.useGravity == false) {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    // Adjust the current obstaclesPrefabs list
    public void resetObstacles(int phase) {
        if (phase == 2) {
            obstacles = obstaclePrefabsPhase2;
        } else if (phase == 3) {
            obstacles = obstaclePrefabsPhase3;
        }
    }

    void Start() {
        // Store Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Set obstacles to first phase
        obstacles = obstaclePrefabsPhase1;
    }

    private void OnTriggerEnter(Collider other) {
        // Filter for destroyer tag
        if(other.gameObject.tag == "gDestroy") {
            // Instantiate new floor tile some distance away
            GameObject newTile = Instantiate(gameObject, gameObject.transform.position + new Vector3(0, 0, 70), Quaternion.identity);
            // Reset name (to get rid of "(clone)" ending)
            newTile.name = gameObject.name;

            // Reset tile children (obstacles)
            foreach (Transform child in newTile.transform) {
                if (child.gameObject.name != "Road") {
                    Destroy(child.gameObject);
                }
            }

            // Place obstacles randomly
            if (Random.value < 0.4) {
                for (int i = 0; i < 4; ++i) {
                    if (Random.value < 0.4) {
                        GameObject obstacle = obstacles[i != 1 && i != 2 ? Random.Range(0, 2) : 0];
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
