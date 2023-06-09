using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Powerup
{
    protected override void action(GameObject player) {
        scoreKeeper.addScore(15000);
    }
}
