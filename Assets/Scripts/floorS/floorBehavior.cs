using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorBehavior : MonoBehaviour
{
    public GameMonitor gm;
    public GameObject baseTile;
    // The speed at which the environment will move
    public static float speed = 7.5f;

    public GameObject[] baseObstaclePrefabsPhase1 = new GameObject[2];
    public GameObject[] baseObstaclePrefabsPhase2 = new GameObject[2];
    public GameObject[] baseObstaclePrefabsPhase3 = new GameObject[2];

    private static GameObject[] obstacles;
    private static GameObject[] obstaclePrefabsPhase1 = new GameObject[2];
    private static GameObject[] obstaclePrefabsPhase2 = new GameObject[2];
    private static GameObject[] obstaclePrefabsPhase3 = new GameObject[2];

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
            cl.enabled = false;
        }
    }

    // Adjust the current obstaclesPrefabs list
    public static void resetObstacles(int phase) {
        if (phase == 2) {
            obstacles = obstaclePrefabsPhase2;
        } else if (phase == 3) {
            obstacles = obstaclePrefabsPhase3;
        }
    }

    void Start() {
        // Store Rigidbody component
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();

        // Initialize static fields
        obstaclePrefabsPhase1 = baseObstaclePrefabsPhase1;
        obstaclePrefabsPhase2 = baseObstaclePrefabsPhase2;
        obstaclePrefabsPhase3 = baseObstaclePrefabsPhase3;

        // Set current obstacle set to first phase
        obstacles = obstaclePrefabsPhase1;
    }

    private void OnTriggerEnter(Collider other) {
        // Filter for destroyer tag
        if(other.gameObject.tag == "gDestroy") {
            doReset = true;
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

        if (doReset) {
            stopActivity();
        }
    }
}
