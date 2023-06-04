/*
lockOnAiming: A script used to manage where the crosshair will be positioned and where the player will target
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class lockOnAiming : MonoBehaviour
{

    // ArrayList is for storing all available target transforms to shoot at
    private static ArrayList transforms;
    /* 
    Dictionary the GameMonitor to access a specific transform (regardless of index in the ArrayList) by using a string to find to find the transform
        RH1 = RightCore1
        LH1 = LeftCore1
        Head1 = ObamaSphere/ObamaCube/ObamaPyramid1
        RH2 = RightCore2
        LH2 = LeftCore2
        Head2 = ObamaPyramid2
        Head3 = ObamaPyramid3
    */
    private static Dictionary<string, Transform> targetDict;

    // Initial targets for Phase 1
    public Transform target1;
    public Transform target2;
    public Transform target3;

    // ethan's audio
    public AudioSource m_AudioSource;

    // Keep track of the target that is currently locked on
    public int targetInd = 0;
    // The Crosshair gameobject and coordinates to relocate
    public GameObject crosshair;
    public int offsetX = 0;
    public int offsetY = 0;
    public int offsetZ = 0;

    private Transform target;
    // Used for when all the targets are currently down; gets replaced once the next phase's targets are established
    public Transform placeHolder;


    // Public Functions to manipulate all the targets to shift the crosshair to

    //addTarget() appends a new target at the end of the arraylist and creates a key, value pair for the dictionary
    public void addTarget(Transform newTarget, string name) {
        transforms.Add(newTarget);
        targetDict.Add(name, newTarget);
        if ((Transform) transforms[0] == placeHolder) {
            transforms.RemoveAt(0);
        }
        if (transforms.Count == 2) {
            targetInd = 2;
            crosshair.SetActive(true); 
        }
    }
    // removeTarget() removes the specified target based on its key name (see dictionary declearion above) from both the arraylist and dictionary
    public void removeTarget(string targetName) {
        Transform target = targetDict[targetName];
        // Search through the arraylist (of no more than 7 transforms) to find then remove
        for (int i = 0; i < transforms.Count; i++) {
            if (target == (Transform) transforms[i]) {
                transforms.RemoveAt(i);
                targetDict.Remove(targetName);
                i = transforms.Count;
            }
        }
        // If the removal puts the targetInd out of bounds, then relocate to the last value of the arraylist
        if(targetInd >= transforms.Count && transforms.Count > 0)
            targetInd = transforms.Count - 1;
        // If the removal makes the arraylist empty, use the place holder as the next phase makes its apperance.
        else if (transforms.Count <= 0) {
            targetInd = 0;
            transforms.Add(placeHolder);
            crosshair.SetActive(false);
        }
    }

    // Start() sets up the initial target and defines the arraylist and dictionary
    void Start() {
        transforms = new ArrayList();
        targetDict = new Dictionary<string, Transform>();
        transforms.Add(target1);
        transforms.Add(target2);
        transforms.Add(target3);
        targetDict.Add("RH1", target1);
        targetDict.Add("LH1", target2);
        targetDict.Add("Head1", target3);
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update() allows the player to adjust the target by right-clicking or pressing P
    void Update()
    {
        // Get the right click or P button to switch targets
        Mouse mouse = Mouse.current;
        if(mouse.rightButton.wasPressedThisFrame || Input.GetButtonDown("Fire2")) {
            targetInd++;
            if(targetInd >= transforms.Count)
                targetInd = 0;
        }
        // Make sure the target doesn't go out-of-bounds
        if(targetInd >= transforms.Count) {
            targetInd = transforms.Count - 1;
        }
        // Assign the target and crosshair to the specified transform
        target = (Transform)transforms[targetInd];
        crosshair.transform.position = target.position + new Vector3(offsetX, offsetY, offsetZ);
        transform.LookAt(target);
    }
}
