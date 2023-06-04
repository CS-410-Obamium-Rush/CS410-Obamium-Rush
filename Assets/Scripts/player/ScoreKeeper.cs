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
    // Text variables to alter and display
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text scoreTextEnd;
    public TMP_Text timeTextEnd;

    // Time and score value
    private float totalTime = 0f;
    public int scoreVal = 0;
    
    // Minute is used to increment the time for every 60 seconds have passed
    private int minute = 0;

    // StopTimer is used to pause the time when the game comes to an end
    private bool stopTimer = false;
    public void setStopTimer() {
        stopTimer = true;
    }

    // Start sets up a place holder text for the start of the game
    void Start()
    {
        timeText.text = "Time: " + totalTime.ToString();
        scoreText.text = "Score: " + scoreVal.ToString();
    }

    // Update() does the changes in the time score
    void Update()
    {
        if (!stopTimer) 
            totalTime += Time.deltaTime;
        if (Mathf.Round(totalTime) >= 60) {
            minute++;
            totalTime = 0f;
        }
        timeText.text = "Time: " + minute.ToString("00") + ":" + totalTime.ToString("00.00");
        timeTextEnd.text = "Time: " + minute.ToString("00") + ":" + totalTime.ToString("00.00");
    }

    /*
    Public functions to adjust the score value and display information
    */
    public void addScore(int val) {
        scoreVal += val;
        scoreText.text = "Score: " + scoreVal.ToString();
        scoreTextEnd.text = "Score: " + scoreVal.ToString();
    }

    public void removeScore(int val) {
        // Do not allow a negative score value
        if (scoreVal - val >= 0)
            scoreVal -= val;
        scoreText.text = "Score: " + scoreVal.ToString();
        scoreTextEnd.text = "Score: " + scoreVal.ToString();
    }

    // gameDone() disables the text displayed throughout the game and enabled a new set of text of the same info during the win/loss canvas
    // as if the original text got relocated
    public void gameDone() {
        timeText.enabled = false;
        scoreText.enabled = false;
        timeTextEnd.enabled = true;
        scoreTextEnd.enabled = true;
    }

    
}
