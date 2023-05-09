using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDetection : MonoBehaviour
{
    private Renderer enemyRenderer;
    private bool isFlashing = false;
    Color initColor;

    // Start is called before the first frame update
    void Start()
    {
       enemyRenderer = GetComponent<Renderer>();
       initColor = GetComponent<Renderer>().material.color;
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
        enemyRenderer.material.SetColor("_Color", initColor);
        isFlashing = false;
    }
}
