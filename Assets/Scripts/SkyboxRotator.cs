using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{

    public float speed = 2f;
    private float alpha = 1f;

    public void rotate() {
        alpha = 0;
    }

    void Update()
    {
        if (alpha < 1) {
            alpha += speed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(180, 0, 0), alpha);
        }
    }
}
