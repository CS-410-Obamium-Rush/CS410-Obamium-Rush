using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPhase : MonoBehaviour
{
    public AttackPatterns atkPat;
    public EnemyDamageDetection enDamDet;
    public GameObject rightHand1;
    public GameObject leftHand1;
    public GameObject rightHand2;
    public GameObject leftHand2;
    public GameObject headSphere;
    public GameObject headCube;


    private HandBehavior lh1;
    private HandBehavior rh1;
    private HandBehavior lh2;
    private HandBehavior rh2;

    private HeadBehavior hs;


    private bool[] stepList;


    // Start is called before the first frame update
    void Start()
    {
        stepList = new bool[10];
        for (int i = 0; i < 10; i++) {
            stepList[i] = false;
        }
        rh1 = rightHand1.GetComponent<HandBehavior>();
        lh1 = leftHand1.GetComponent<HandBehavior>();
        rh2 = rightHand2.GetComponent<HandBehavior>();
        lh2 = leftHand2.GetComponent<HandBehavior>();
        hs = headSphere.GetComponent<HeadBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stepList[0]) {
            enDamDet.setPhaseTransition(true);
            atkPat.setPhaseTransition(true);
            stepList[0] = false;
            stepList[1] = true;
        }
        else if (stepList[1]) {
            headSphere.transform.Rotate(new Vector3(0, 100, 0) * Time.deltaTime);
            Vector3 updateScale = headSphere.transform.localScale;
            if (updateScale.x != 1f)
                updateScale.x -= 0.2f * Time.deltaTime;
            if (updateScale.y != 1f)
                updateScale.y -= 0.2f * Time.deltaTime;
            if (updateScale.z != 1f)
                updateScale.z -= 0.2f * Time.deltaTime;
            if (updateScale.x == 1f && updateScale.y == 1f && updateScale.z == 1f && headSphere.transform.localEulerAngles == new Vector3(0, 0, 0)) {
                stepList[1] = false;
                stepList[0] = true;
            }
            headSphere.transform.localScale = updateScale;
        }
        else if (stepList[2]) {
            headSphere.SetActive(false);
            headCube.SetActive(true);
            Vector3 updateScale = headCube.transform.localScale;
            headCube.transform.Rotate(new Vector3(0, 100, 0) * Time.deltaTime);
            if (updateScale.x != 15f)
                updateScale.x += 1f * Time.deltaTime;
            if (updateScale.y != 15f)
                updateScale.y -= 1f * Time.deltaTime;
            if (updateScale.z != 15f)
                updateScale.z -= 1f * Time.deltaTime;
            if (updateScale.x == 15f && updateScale.y == 15f && updateScale.z == 15f && headCube.transform.localEulerAngles == new Vector3(0, 0, 0)) {
                stepList[2] = false;
                stepList[3] = true;
            }
            headCube.transform.localScale = updateScale;
        }      
    }
    public void phase2() {
        stepList[0] = true;
    }
}
