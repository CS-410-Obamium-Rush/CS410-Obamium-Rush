using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDetection : MonoBehaviour
{
    private Renderer enemyRenderer;
    private bool isFlashing = false;
    // Start is called before the first frame update
    void Start()
    {
       enemyRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
 
    }


    void OnParticleCollision(GameObject other) 
    {
        if(isFlashing == false) {
            StartCoroutine(flashDamageColor());
        }
    }

    IEnumerator flashDamageColor()
    {
        isFlashing = true;
        enemyRenderer.material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(0.5f);
        enemyRenderer.material.SetColor("_Color", Color.white);
        isFlashing = false;
    }
}
