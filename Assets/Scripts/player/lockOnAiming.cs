using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class lockOnAiming : MonoBehaviour
{
    // Start is called before the first frame update
    //public Transform[] transforms;

    private static ArrayList transforms;
    private static Dictionary<string, Transform> targetDict;

    public Transform target1;
    public Transform target2;
    public Transform target3;

    // ethan's audio
    public AudioSource m_AudioSource;

    public int targetInd = 0;
    public GameObject crosshair;
    public int offsetX = 0;
    public int offsetY = 0;
    public int offsetZ = 0;
    private Transform target;
    public Transform placeHolder;

    

    // Public Functions to manipulate all the targets to shift the crosshair to
    public void addTarget(Transform newTarget, string name) {
        transforms.Add(newTarget);
        targetDict.Add(name, newTarget);
        if ((Transform) transforms[0] == placeHolder)
            transforms.RemoveAt(0); 
        if (transforms.Count == 2) {
            targetInd = 2;
            crosshair.SetActive(true);
        }
    }

    public void removeTarget(string targetName) {
        Transform target = targetDict[targetName];
        for (int i = 0; i < transforms.Count; i++) {
            if (target == (Transform) transforms[i]) {
                transforms.RemoveAt(i);
                targetDict.Remove(targetName);
                i = transforms.Count;
            }
        }
        if(targetInd >= transforms.Count && transforms.Count > 0)
            targetInd = transforms.Count - 1;
        else if (transforms.Count <= 0) {
            targetInd = 0;
            transforms.Add(placeHolder);
            crosshair.SetActive(false);
        }
    }

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

    // Update is called once per frame
    void Update()
    {

        // Get the right click or P button to switch targets
        Mouse mouse = Mouse.current;
        if(mouse.rightButton.wasPressedThisFrame || Input.GetButtonDown("Fire2")) {
            targetInd++;
            if(targetInd >= transforms.Count)
                targetInd = 0;
        }
        // Get the target itself
        if(targetInd >= transforms.Count)
            targetInd = transforms.Count - 1;
        target = (Transform)transforms[targetInd];
        crosshair.transform.position = target.position + new Vector3(offsetX, offsetY, offsetZ);
        transform.LookAt(target);
    }
}
