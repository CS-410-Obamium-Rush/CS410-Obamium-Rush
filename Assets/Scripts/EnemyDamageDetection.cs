using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDetection : MonoBehaviour
{
    public Renderer enemyRenderer;
    public GameMonitor gm;
    private bool isFlashing = false;
    private static bool phaseTransition = false;
    private static int damageTake = 4;
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

    public void setDamage(int val) {
        damageTake = val;
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
            gm.enemyTakeDamage(damageTake,0);
        }
            
        else if (gameObject.name == "LeftHand1") {
            doFlash = true;
            gm.enemyTakeDamage(damageTake,1);
        }
            
        else if ((gameObject.name == "ObamaCube" || gameObject.name == "ObamaSphere" || gameObject.name == "ObamaPyramid1" || gameObject.name == "ObamaPyramid2" || gameObject.name == "ObamaPyramid3")) {
            if (gm.handsDefeated()) {
                doFlash = true;
                if (gameObject.name == "ObamaPyramid2")
                    gm.enemyTakeDamage(damageTake, 5);
                else if (gameObject.name == "ObamaPyramid3")
                    gm.enemyTakeDamage(damageTake, 6);
                else
                    gm.enemyTakeDamage(damageTake, 2);
            }
        }
        else if (gameObject.name == "RightHand2") {
            doFlash = true;
            gm.enemyTakeDamage(damageTake,3);
        }
        else if (gameObject.name == "LeftHand2") {
            doFlash = true;
            gm.enemyTakeDamage(damageTake,4);
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
