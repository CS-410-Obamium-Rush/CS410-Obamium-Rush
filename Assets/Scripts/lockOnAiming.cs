using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockOnAiming : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
