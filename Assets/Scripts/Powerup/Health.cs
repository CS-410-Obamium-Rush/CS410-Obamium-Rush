using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Powerup
{
    AudioSource m_AudioSource;

    void Start(){
        m_AudioSource = GetComponent<AudioSource>();
    }

    protected override void action(GameObject player) {
        m_AudioSource.Play();
        gameMonitor.playerAddHealth(100);
    }
}
