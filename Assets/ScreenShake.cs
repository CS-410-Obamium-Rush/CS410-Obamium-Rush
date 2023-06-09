using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public GameMonitor gm;
    public float shakeAngle;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start() {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {        
        float strength = gm.screenShake * gm.screenShake;
        if (strength > 0) {
            transform.position = startPosition + Random.insideUnitSphere * strength * 2;
            transform.rotation = startRotation;
            transform.Rotate(
                shakeAngle * strength * Random.Range(-1, 1),
                shakeAngle * strength * Random.Range(-1, 1),
                shakeAngle * strength * Random.Range(-1, 1)
            );
            // transform.rotation = Quaternion.Euler(
            //     transform.rotation.Euler.x + shakeAngle * strength * Random.Range(-1, 1),
            //     transform.rotation.y + shakeAngle * strength * Random.Range(-1, 1),
            //     transform.rotation.z + shakeAngle * strength * Random.Range(-1, 1)
            // );
        } else {
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
    }
}
