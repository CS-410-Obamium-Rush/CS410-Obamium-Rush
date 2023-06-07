using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorBehavior : MonoBehaviour
{
    public GameMonitor gm;
    public GameObject baseTile;

    // The speed at which the environment will move
    public float speed = 7.5f;

    // Long obstacles only spawn and the edges
    public GameObject[] shortObstacles;
    public GameObject[] longObstacles;

    // The total number of obstacles that can be selected from
    private int totalObstacles = 0;

    // This object's rigidbody component
    private Rigidbody rb;
    private Collider cl;

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

    void Start() {
        // Store Rigidbody component
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();

        // Inititialize totalObstacles to the total number of possible obstacles
        totalObstacles = shortObstacles.Length + longObstacles.Length;
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
                        // Select a new obstacle, making sure to avoid placing long obstacles in the middle
                        GameObject obstacle;
                        if (i == 1 || i == 2) {
                            int obstacleIndex = Random.Range(0, shortObstacles.Length);
                            obstacle = shortObstacles[obstacleIndex];
                        } else {
                            int obstacleIndex = Random.Range(0, totalObstacles);
                            // Index into shortObstacles or longObstacles as if they were a single combined array
                            if (obstacleIndex >= shortObstacles.Length)
                                obstacle = longObstacles[obstacleIndex % shortObstacles.Length];
                            else
                                obstacle = shortObstacles[obstacleIndex];
                        }

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
        }
        else if (other.gameObject.tag == "gDestroyBelow") {
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
