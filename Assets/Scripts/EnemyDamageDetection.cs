using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDetection : MonoBehaviour
{
    private Renderer enemyRenderer;
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
       enemyRenderer = GetComponent<Renderer>();
       initColor = GetComponent<Renderer>().material.color;

       //m_AudioSource = GetComponent<AudioSource>();
    }

    void OnParticleCollision(GameObject other) 
    {
        bool doFlash = false;

        //enemyAudioManager.instance.playHurt();  // plays the playerShooting sfx for now

        if (this.transform.parent.gameObject.name == "RightHand1" && !phaseTransition) {
            doFlash = true;
            gm.enemyTakeDamage(900,0);
        }
            
        else if (this.transform.parent.gameObject.name == "LeftHand1"  && !phaseTransition) {
            doFlash = true;
            gm.enemyTakeDamage(900,1);
        }
            
        else if ((this.gameObject.name == "obama_sphere" || this.gameObject.name == "Obama Cube")  && !phaseTransition) {
            if (gm.handsDefeated()) {
                doFlash = true;
                gm.enemyTakeDamage(900,2);
            }
        }
        else if (this.transform.parent.gameObject.name == "RightHand2"  && !phaseTransition) {
            doFlash = true;
            gm.enemyTakeDamage(900,3);
        }

        else if (this.transform.parent.gameObject.name == "LeftHand2"  && !phaseTransition) {
            doFlash = true;
            gm.enemyTakeDamage(900,4);
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
