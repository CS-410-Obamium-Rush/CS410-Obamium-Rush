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

    void Start(){
        // need this instance in order to access in other scripts
        if(instance != this){
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
}
