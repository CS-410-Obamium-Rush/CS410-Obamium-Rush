using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//author: EAVI
/* this is the audio initializing script for the player
designed to help add multiple sfx to the player, like jumps, shots, and more
*/

public class playerAudioManager : MonoBehaviour{
    
    public static playerAudioManager instance;

    //init audio sources
    public AudioClip[] sfx; // append all sfx for player you want here
    private AudioSource godSource; // main audio player

    // standard singletone practice: use awake instead of start
    void Awake(){
        // need this instance in order to access in other scripts
        //Debug.Log("Made player instance");
        if(instance != this){
            //Debug.Log("Destroyed player instance");
            Destroy(this);
        }
        instance = this; //make an instance
        
        godSource = gameObject.AddComponent<AudioSource>();
    }

    public void playJump(){
        godSource.clip = sfx[0];
        godSource.Play();
    }

    public void playShoot(){
        godSource.clip = sfx[1];
        godSource.Play();
    }

    // this should be one of the last sfx in the player sfx list
    public void playHurt(){
        godSource.clip = sfx[2];
        godSource.Play();
    }
}
