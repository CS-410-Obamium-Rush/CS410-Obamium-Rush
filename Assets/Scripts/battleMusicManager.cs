using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleMusicManager : MonoBehaviour {
    
    public static battleMusicManager instance;

    //init audio sources
    public AudioClip[] music; // append all music for use here
    private AudioSource godSource; // main audio player

    // standard singletone practice: use awake instead of start
    void Awake(){
        // need this instance in order to access in other scripts
        //Debug.Log("Made player instance");
        if(instance != this && instance != null){
            //Debug.Log("Destroyed player instance");
            Destroy(this);
        }
        instance = this; //make an instance
        
        godSource = gameObject.AddComponent<AudioSource>();
    }

    public void playP1(){   // mmmbn2 battle spirit
        godSource.Stop();
        godSource.clip = music[0];
        godSource.volume = 0.35f;
        godSource.Play();
    }

    public void playP2(){   // DOOM the only thing they fear is you
        godSource.Stop();
        godSource.clip = music[1];
        godSource.Play();
    }

    public void playP3(){   // KaTaMaRi DaMaShi
        godSource.Stop();
        godSource.clip = music[2];
        godSource.Play();
    }

    public void playWin(){  //Final Fantasy Tactics Win
        godSource.Stop();
        godSource.clip = music[3];
        godSource.Play();
    }

    public void playLoss(){
        godSource.Stop();
        godSource.clip = music[4];
        godSource.Play();
    }
}
