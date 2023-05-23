using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class lockOnAiming : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] transforms;
    public Transform target;
    public int targetInd = 0;

    // Update is called once per frame
    void Update()
    {
        Mouse mouse = Mouse.current;
        if(mouse.rightButton.wasPressedThisFrame)
        {
            targetInd++;
            if(targetInd >= transforms.Length)
            {
                targetInd = 0;
            } 
        }
        target = transforms[targetInd];
        transform.LookAt(target);
    }

}
