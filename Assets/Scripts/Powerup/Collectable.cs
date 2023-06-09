using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Powerup {
    AudioSource m_AudioSource;

    void Start(){
        m_AudioSource = GetComponent<AudioSource>();
    }

    protected override void action(GameObject player) {
        m_AudioSource.Play();
        scoreKeeper.addScore(15000);
    }
}
