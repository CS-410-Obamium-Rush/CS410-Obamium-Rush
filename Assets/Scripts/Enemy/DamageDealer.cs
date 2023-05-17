using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public GameMonitor gm;
    public static int dmgAmt = 0;

    private static bool invincible = false;
    private GameObject player;
    private Renderer playerRenderer;
    Color initColor;

    public void setDmg(int amt) {
        dmgAmt = amt;
    }
    void start() {
        player = GameObject.Find("PlayerSprite");
        playerRenderer = player.GetComponent<Renderer>();
        initColor = playerRenderer.material.color;
    }

    void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag("Player") && !invincible) {
            invincible = true;
            StartCoroutine(flashDamageColor());
            //Debug.Log("Player has been hit: -" + dmgAmt);
            gm.playerTakeDamage(dmgAmt);
        }
    }

    IEnumerator flashDamageColor()
    {
        /*for (int i = 0; i < 2; i++) {
            Debug.Log("Used");
            playerRenderer.material.SetColor("_Color", Color.red);
            yield return new WaitForSeconds(0.075f);
            playerRenderer.material.SetColor("_Color", initColor);
            yield return new WaitForSeconds(0.075f);
        }*/
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
}
