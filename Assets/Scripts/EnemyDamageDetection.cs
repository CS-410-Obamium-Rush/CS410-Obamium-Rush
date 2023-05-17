using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDetection : MonoBehaviour
{
    private Renderer enemyRenderer;
    public GameMonitor gm;
    private bool isFlashing = false;

    // ethan's audio stuff
    //public enemyAudioManager enemysfx; // need to figure out why the fuck this doesn't work
    //AudioSource m_AudioSource;
    
    Color initColor;

    // Start is called before the first frame update
    void Start()
    {
       enemyRenderer = GetComponent<Renderer>();
       initColor = GetComponent<Renderer>().material.color;

       //m_AudioSource = GetComponent<AudioSource>();
    }

    void OnParticleCollision(GameObject other) 
    {
        bool doFlash = false;

        //enemyAudioManager.instance.playHurt();  // plays the playerShooting sfx for now

        if (this.gameObject.name == "RightCore") {
            doFlash = true;
            gm.enemyTakeDamage(5,0);
        }
            
        else if (this.gameObject.name == "LeftCore") {
            doFlash = true;
            gm.enemyTakeDamage(5,1);
        }
            
        else if (this.gameObject.name == "obama_sphere") {
            if (gm.handsDefeated()) {
                doFlash = true;
                gm.enemyTakeDamage(5,2);
            }
        }

        if(isFlashing == false && doFlash == true) {
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
