using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameMonitor gm;
    public GameEnding ge;
    public lockOnAiming loa;

    private HandBehavior lh1;
    private HandBehavior rh1;
    private HandBehavior lh2;
    private HandBehavior rh2;
    public Image enemyBar;
    private HeadBehavior hc;


    private bool[] stepList;
    Vector3 rh1Pos; 
    Vector3 rh2Pos; 
    Vector3 lh1Pos;
    Vector3 lh2Pos;

    private bool doOnce = true;

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
        hc = headCube.GetComponent<HeadBehavior>();
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
            headSphere.transform.Rotate(new Vector3(0, 1000, 0) * Time.deltaTime);
            Vector3 updateScale = headSphere.transform.localScale;
            if (updateScale.x != 1f)
                updateScale.x -= 4f * Time.deltaTime;
            if (updateScale.y != 1f)
                updateScale.y -= 4f * Time.deltaTime;
            if (updateScale.z != 1f)
                updateScale.z -= 4f * Time.deltaTime;
            headSphere.transform.localScale = updateScale;
            if (updateScale.x <= 1f && updateScale.y <= 1f && updateScale.z <= 1f) {
                stepList[1] = false;
                stepList[2] = true;
            }
        }
        else if (stepList[2]) {
            headSphere.SetActive(false);
            headCube.SetActive(true);
            hc.setIdle(false);
            Vector3 updateScale = headCube.transform.localScale;
            headCube.transform.Rotate(new Vector3(0, 100, 0) * Time.deltaTime);
            if (updateScale.x != 15f)
                updateScale.x += 2f * Time.deltaTime;
            if (updateScale.y != 15f)
                updateScale.y += 2f * Time.deltaTime;
            if (updateScale.z != 15f)
                updateScale.z += 2f * Time.deltaTime;
            headCube.transform.localScale = updateScale;
            if (updateScale.x >= 14f && updateScale.y >= 14f && updateScale.z >= 14f) {
                headCube.transform.localEulerAngles = new Vector3(0, 90, 0);
                stepList[2] = false;
                stepList[3] = true;
                rh1.setPause();
                lh1.setPause();
                rh2.setPause();
                lh2.setPause();
            }
        }
        else if (stepList[3]) {
            if (rh1.resetHand() && lh1.resetHand()) {
                stepList[3] = false;
                stepList[4] = true;
            }
        }
        else if (stepList[4]) {
            rightHand2.SetActive(true);
            leftHand2.SetActive(true);
            
            rh1Pos = new Vector3 (rightHand1.transform.position.x, rightHand1.transform.position.y + 8f, rightHand1.transform.position.z);
            rh2Pos = new Vector3 (rightHand2.transform.position.x, rightHand2.transform.position.y - 8f, rightHand2.transform.position.z);
            lh1Pos = new Vector3 (leftHand1.transform.position.x, leftHand1.transform.position.y + 8f, leftHand1.transform.position.z);
            lh2Pos = new Vector3 (leftHand2.transform.position.x, leftHand2.transform.position.y - 8f, leftHand2.transform.position.z);
            stepList[4] = false;
            stepList[5] = true;
        }
        else if (stepList[5]) {
            if (shiftHands()) {
                stepList[5] = false;
                stepList[6] = true;
            }
        }
        else if (stepList[6]) {
            atkPat.setAmt(4,5);
            
            if (doOnce) {
                atkPat.setHeadBehavior(hc);
                hc.setResume();
                rh1.setResume();
                rh2.setResume();
                lh1.setResume();
                lh2.setResume(); 
                loa.addTargets(rightHand2.transform.GetChild(1).gameObject.transform);
                loa.addTargets(leftHand2.transform.GetChild(1).gameObject.transform);
                StartCoroutine(regenHealth());
                doOnce = false;
            }
                
        }
        else if (stepList[7]) {
            stepList[7] = false;
            stepList[8] = true;     
        }
        else if (stepList[8]) {
            ge.callEndWin();     
        }
             
    }
    private bool shiftHands() {
        bool rh1Set = false;
        bool rh2Set = false;
        bool lh1Set = false;
        bool lh2Set = false;
        if (Vector3.Distance(rightHand1.transform.position, rh1Pos) < 0.001f)
            rh1Set = true;
        else
            rightHand1.transform.position = Vector3.MoveTowards(rightHand1.transform.position, rh1Pos, 1.5f * Time.deltaTime);

        if (Vector3.Distance(rightHand2.transform.position, rh2Pos) < 0.001f)
            rh2Set = true;
        else
            rightHand2.transform.position = Vector3.MoveTowards(rightHand2.transform.position, rh2Pos, 1.5f * Time.deltaTime);

        if (Vector3.Distance(leftHand1.transform.position, lh1Pos) < 0.001f)
            lh1Set = true;
        else
            leftHand1.transform.position = Vector3.MoveTowards(leftHand1.transform.position, lh1Pos, 1.5f * Time.deltaTime);

        if (Vector3.Distance(leftHand2.transform.position, lh2Pos) < 0.001f)
            lh2Set = true;
        else
            leftHand2.transform.position = Vector3.MoveTowards(leftHand2.transform.position, lh2Pos, 1.5f * Time.deltaTime);
        return (rh1Set && rh2Set && lh1Set && lh2Set);
    }

    IEnumerator regenHealth() {
        int maxHealth = gm.setNewHealth(400, 400, 400, 400, 600);
        int healthRecord = 0;
        int step = 10;
        for (int i = 0; healthRecord < maxHealth; i += step) {
            healthRecord += step;
            enemyBar.fillAmount = (float) healthRecord / maxHealth;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(60.00f);
        stepList[6] = false;
        stepList[7] = true;
    }

    public void phase2() {
        stepList[0] = true;
    }
}
