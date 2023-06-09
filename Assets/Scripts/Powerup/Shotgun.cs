using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Powerup
{
    protected override void action(GameObject player) {
        ThirdPersonMovement playerScript = player.GetComponent<ThirdPersonMovement>();
        playerScript.setWeapon(ThirdPersonMovement.Weapon.Shotgun);
    }
}
