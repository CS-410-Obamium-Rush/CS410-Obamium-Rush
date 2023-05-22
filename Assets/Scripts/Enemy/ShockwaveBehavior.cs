using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveBehavior : MonoBehaviour
{
    public float speed = 0f;
    private GameMonitor gm;
    private GameObject gmObject;
    private bool invincible = false;
    // Start is called before the first frame update
    
    void Start() {
        gmObject = GameObject.Find("GameSettings");
        gm = gmObject.GetComponent<GameMonitor>();
        StartCoroutine(destroyShockWave());
    }

    // Update is called once per frame
    void Update() {
        // Source: https://stackoverflow.com/questions/48750990/how-does-this-time-deltatime-dependent-transform-scale-script-work 
        Vector3 updateScale = transform.localScale;
        updateScale.x += speed * Time.deltaTime;
        updateScale.z += speed * Time.deltaTime;
        transform.localScale = updateScale;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !invincible) {
            gm.playerTakeDamage(15);
            invincible = true;
            StartCoroutine(flashDamageColor());
        }
    }

    IEnumerator flashDamageColor()
    {
        yield return new WaitForSeconds(2f);
        invincible = false;
    }

    IEnumerator destroyShockWave() {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

}
