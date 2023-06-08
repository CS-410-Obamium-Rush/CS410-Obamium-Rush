using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public Material phase1Skybox = null;
    public Material phase3Skybox = null;

    private float speed = 0.2f;
    private float alpha = 1f;
    private Quaternion start = Quaternion.Euler(0, 0, 0);
    private Quaternion goal = Quaternion.Euler(180, 0, 0);

    void Start() {
        RenderSettings.skybox = phase1Skybox;
    }

    public void phase2() {
        speed = 0.1f;
        start = Quaternion.Euler(0, 0, 0);
        goal = Quaternion.Euler(180, 0, 0);
        alpha = 0;
    }

    public void phase3() {
        if (phase3Skybox != null) {
            RenderSettings.skybox = phase3Skybox;
        }
        speed = 0.3f;
        start = Quaternion.Euler(180, 0, 0);
        goal = Quaternion.Euler(0, 0, 0);
        alpha = 0;
    }

    void Update()
    {
        if (alpha < 1) {
            alpha += speed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(start, goal, alpha);
        }
    }
}
