using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: EAVI

// this connects with the NextPhase script in the gameSettings object

public class enemyNextPhaseAudioManager : MonoBehaviour
{
    public static enemyNextPhaseAudioManager instance;

    //init audio sources
    public AudioClip[] sfx; // append all sfx for enemy you want here
    private AudioSource godSource; // main audio player

    void Awake(){
        if(instance != this && instance != null){
            Destroy(this);
        }
        
        instance = this; //make an instance
        
        godSource = gameObject.AddComponent<AudioSource>();
    }

    public void playDefeat(){
        godSource.clip = sfx[0];
        godSource.Play();
    }

    public void playRise(){

    }

    public void playSplit(){

    }

    public void playRegen(){

    }
}
