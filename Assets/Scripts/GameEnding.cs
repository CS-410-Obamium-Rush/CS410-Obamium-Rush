/*
GameEnding: Used to display the victory or game over screen when the enemy or player run out of health.

This is structed based on the Haunted House tutorial done for Assignment 2; this may be greatly adjusted in future builds
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public NextPhase np;
    // For the length of the ending screen
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    float m_Timer;
    // Bools to determine if the player has won or lost
    private bool playerWin = false;
    private bool playerLost = false;
    // Get the canvas containing the win and lost image
    public CanvasGroup winCanvas;
    public CanvasGroup loseCanvas;
    private bool doOnce = true;
    // Update() displays canvas for when player wins or lost
    void Update()
    {
        if (playerWin) {
            if(doOnce) {
                np.phase2();
                doOnce = false;
            }
        }
            
            //EndGame(winCanvas, false);
        else if (playerLost)
            EndGame(loseCanvas, true);
    }

    // Displays the victory or game over image for a certain amount of time before either quitting the app (when player wins)
    // or restarting the level (when the player loses)
    void EndGame (CanvasGroup imageCanvas, bool doRestart) {
        m_Timer += Time.deltaTime;
        imageCanvas.alpha = m_Timer / fadeDuration;
        if(m_Timer > fadeDuration + displayImageDuration) {
            if (doRestart) {
                 SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else {
                Application.Quit();
            }
        }
    }

    public void callEndWin() {
        EndGame(winCanvas, true);
    }

    // Used by the Game Monitor to determine that the player won or lost.
    public void setWin() {
        playerWin = true;
    }

    public void setLost() {
        playerLost = true;
    }
}
