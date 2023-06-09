using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Powerup
{
    protected override void action(GameObject player) {
        pickupAudioSource.Play();
        scoreKeeper.addScore(15000);
    }
}
