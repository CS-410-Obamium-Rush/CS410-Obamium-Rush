using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Powerup
{
    protected override void action(GameObject player) {
        gameMonitor.playerAddHealth(100);
    }
}
