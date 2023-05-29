using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDetection : MonoBehaviour
{
    public Renderer enemyRenderer;
    public GameMonitor gm;
    private bool isFlashing = false;
    private static bool phaseTransition = false;

    // ethan's audio stuff
    //public enemyAudioManager enemysfx; // need to figure out why the fuck this doesn't work
    //AudioSource m_AudioSource;
    
    Color initColor;

    public void setPhaseTransition(bool val) {
        phaseTransition = val;
    }
    // Start is called before the first frame update
    void Start()
    {
       initColor = enemyRenderer.material.color;

       //m_AudioSource = GetComponent<AudioSource>();
    }


    void OnParticleCollision(GameObject other) 
    {
        if (phaseTransition) {
            return;
        }

        bool doFlash = false;
        //enemyAudioManager.instance.playHurt();  // plays the playerShooting sfx for now

        if (gameObject.name == "RightHand1") {
            doFlash = true;
            gm.enemyTakeDamage(5,0);
        }
            
        else if (gameObject.name == "LeftHand1") {
            doFlash = true;
            gm.enemyTakeDamage(5,1);
        }
            
        else if ((gameObject.name == "ObamaCube" || gameObject.name == "ObamaSphere")) {
            if (gm.handsDefeated()) {
                doFlash = true;
                gm.enemyTakeDamage(5,2);
            }
        }
        else if (gameObject.name == "RightHand2") {
            doFlash = true;
            gm.enemyTakeDamage(5,3);
        }
        else if (gameObject.name == "LeftHand2") {
            doFlash = true;
            gm.enemyTakeDamage(5,4);
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
