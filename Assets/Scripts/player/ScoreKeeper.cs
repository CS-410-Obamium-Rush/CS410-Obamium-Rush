/*
ScoreKeeper: A script used by other scripts to monitor the player's score and time
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timeText;
    private float totalTime = 0f;
    public int scoreVal = 0;
    private int minute = 0;
    private bool stopTimer = false;

    public void setStopTimer() {
        stopTimer = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        timeText.text = "Time: " + Mathf.Round(totalTime).ToString();
        scoreText.text = "Score: " + scoreVal.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTimer) 
            totalTime += Time.deltaTime;
        if (Mathf.Round(totalTime) >= 60) {
            minute++;
            totalTime = 0f;
        }
        timeText.text = "Time: " + minute.ToString("00") + ":" + totalTime.ToString("00.00");
    }
}
