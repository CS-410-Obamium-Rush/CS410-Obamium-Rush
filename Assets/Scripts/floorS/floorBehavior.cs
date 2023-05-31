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
        rb.MovePosition(rb.position + constForward);
    }
}
