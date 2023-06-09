using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: EAVI

public class enemyAudioManager : MonoBehaviour{

    public static enemyAudioManager instance;

    //init audio sources
    public AudioClip[] sfx; // append all sfx for enemy you want here
    private AudioSource godSource; // main audio player

    void Awake(){
        //Debug.Log("Made enemy Instance");
        // need this instance in order to access in other scripts
        if(instance != this && instance != null){
            //Debug.Log("Destroyed enemy instance");
            Destroy(this);
        }
        
        instance = this; //make an instance
        
        godSource = gameObject.AddComponent<AudioSource>();
    }

    public void playWhoosh(){ // plays handWhoosh1
        godSource.clip = sfx[0];
        godSource.Play();
    }

    //use playScheduled to stitch the prepunch with the normal punch
    /* public void playPrepunch(){
        godSource.clip = sfx[2];
        godSource.Play();
    }*/

    public void playPunch(){
        godSource.clip = sfx[1];
        //godSource.PlayDelayed(1);
        godSource.Play();

        /*if(this == null){
            Debug.Log("instance is now null");
        }*/
    }

    public void playClap(){
        godSource.clip = sfx[2];
        godSource.PlayDelayed(1);
    }

    public void playWhirl(){
        godSource.clip = sfx[3];
        godSource.Play();
    }

    public void playShot(){
        godSource.clip = sfx[4];
        godSource.Play();
    }

    public void playProjHit(){
        godSource.clip = sfx[5];
        godSource.Play();
    }

    public void playSlam(){
        godSource.clip = sfx[6];
        godSource.Play();
    }

    public void playLazer(){
        godSource.clip = sfx[7];
        godSource.Play();
    }

    // this sfx should be towards the end of the list
    public void playHurt(){
        godSource.clip = sfx[8];
        godSource.Play();
    }
}
