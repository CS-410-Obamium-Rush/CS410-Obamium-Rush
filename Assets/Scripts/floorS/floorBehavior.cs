using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorBehavior : MonoBehaviour
{
    public GameMonitor gm;
    public GameObject baseTile;

    // The speed at which the environment will move
    public float speed = 7.5f;

    // The chance that a tile will have any obstacles on it
    public float chanceTileObstacle = 0.4f;
    // The chance that an obstacle will spawn in each lane
    public float chanceObstacle = 0.4f;

    // The distance at which the new tile will be spawned
    [HideInInspector]
    public float spawnDistance = 70f;

    // Long obstacles only spawn and the edges
    public GameObject[] shortObstacles;
    public GameObject[] longObstacles;

    // The total number of obstacles that can be selected from
    private int totalObstacles = 0;

    // This object's rigidbody component
    private Rigidbody rb;
    private Collider cl;

    // Set to true to start the removal of the current phase's floor tiles
    public static bool doReset = false; 

    /* Public functions for the phase transitition of tiles */
    // Have the current tiles start to fall off the map
    public void stopActivity() {
        if (rb.useGravity == false) {
            rb.isKinematic = false;
            rb.useGravity = true;
            cl.isTrigger = true;
        }
    }

    // Adjust the current obstaclesPrefabs list
    public static void resetObstacles(int phase) {
        doReset = false;
    }

    void Awake() {
        // Store Rigidbody component
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();

        // Inititialize totalObstacles to the total number of possible obstacles
        totalObstacles = shortObstacles.Length + longObstacles.Length;
    }

    private GameObject getObstacle(int index) {
        GameObject obstacle;
        if (index >= shortObstacles.Length) {
            obstacle = longObstacles[index % shortObstacles.Length];
        } else {
            obstacle = shortObstacles[index];
        }
        return obstacle;
    }

    private void OnTriggerEnter(Collider other) {
        // Filter for destroyer tag
        if(other.gameObject.tag == "gDestroy") {
            // Instantiate new floor tile some distance away
            GameObject newTile = Instantiate(gameObject, gameObject.transform.position + new Vector3(0, 0, spawnDistance), Quaternion.identity);
            // Reset name (to get rid of "(clone)" ending)
            newTile.name = gameObject.name;

            // Fixes lingering tile bug
            floorBehavior tileScript = newTile.GetComponent<floorBehavior>();
            if (tileScript.enabled == false) {
                Destroy(newTile);
                Destroy(gameObject);
                return;
            } 

            // Reset tile children (obstacles)
            foreach (Transform child in newTile.transform) {
                if (child.gameObject.name != "Road") {
                    Destroy(child.gameObject);
                }
            }

            // Place obstacles randomly
            if (Random.value < chanceTileObstacle) {
                // Split play area into four lanes
                for (int i = 0; i < 4; ++i) {
                    if (Random.value < chanceObstacle) {
                        // Select a new obstacle, making sure to avoid placing long obstacles in the middle
                        int obstacleIndex = Random.Range(0, i == 1 || i == 2 ? shortObstacles.Length : totalObstacles);
                        GameObject obstacle = getObstacle(obstacleIndex);

                        // Instantiate and initialize the selected obstacle
                        GameObject newObstacle = Instantiate(obstacle);
                        newObstacle.transform.SetParent(newTile.transform, false);
                        newObstacle.transform.Translate(new Vector3((i * 6) - 9, 0, 0), Space.Self);
                        
                        // Initialize the new obstacle's script
                        ObstacleDamageDealer obstacleScript = newObstacle.GetComponent<ObstacleDamageDealer>();
                        obstacleScript.gm = gm;
                    }
                }
            }

            // Destroy this object
            Destroy(gameObject);
        } else if (other.gameObject.tag == "gDestroyBelow") {
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

        // Apply the doReset request
        if (doReset) {
            stopActivity();
        }
    }
}
