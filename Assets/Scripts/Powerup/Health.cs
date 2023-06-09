using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Powerup
{
    protected override void action(GameObject player) {
        pickupAudioSource.Play();
        scoreKeeper.addScore(1000);
        gameMonitor.playerAddHealth(100);
    }
}
