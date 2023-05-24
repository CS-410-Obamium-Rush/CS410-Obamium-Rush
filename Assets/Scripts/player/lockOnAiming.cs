using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class lockOnAiming : MonoBehaviour
{
    // Start is called before the first frame update
    //public Transform[] transforms;

    private ArrayList transforms;

    public Transform target1;
    public Transform target2;
    public Transform target3;

    public int targetInd = 0;
    public GameObject crosshair;
    public int offsetX = 0;
    public int offsetY = 0;
    public int offsetZ = 0;
    private Transform target;
    void Start() {
        transforms = new ArrayList();
        transforms.Add(target1);
        transforms.Add(target2);
        transforms.Add(target3);
    }

    // Update is called once per frame
    void Update()
    {
        Mouse mouse = Mouse.current;
        if(mouse.rightButton.wasPressedThisFrame)
        {
            Debug.Log(targetInd);
            targetInd++;
            if(targetInd >= transforms.Count)
            {
                targetInd = 0;
            } 
        }
        target = (Transform)transforms[targetInd];
        crosshair.transform.position = target.position + new Vector3(offsetX, offsetY, offsetZ);
        transform.LookAt(target);
    }

    public void addTargets(Transform newTarget) {
        /*
        for (int i = 0; i < transforms.Length) {
            newTransforms[i] = transforms[i];
        }
        newTransforms[]
        */
        transforms.Add(newTarget);
        //transforms[transforms.length] = newTarget;
    }

}
