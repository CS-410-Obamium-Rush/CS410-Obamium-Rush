/*
GameEnding: Used to display the victory or game over screen or to trigger the next enemy phase when the enemy or player run out of health.

This is structed based on the Haunted House tutorial done for Assignment 2, but has received modifications to incorporate additional
enemy phases
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    // For the length of the ending screen
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    float m_Timer;
    // Bools to determine if the player has won, needs to do the next phase, or hase lost
    private bool playerWin = false;
    private bool doPhase2 = false;
    private bool doPhase3 = false;

    private bool playerLost = false;
    // Get the canvas containing the win and lost image
    public CanvasGroup winCanvas;
    public CanvasGroup loseCanvas;

    // Get the images associated to each health bar
    public Canvas allHealth;

    // Trigger the next phase with their public functions
    public TwoTransition twoTrans;
    public ThreeTransition threeTrans;

    /* Public Functions */

    // Used for debugging phase changing scripts
    public void callEndWin() {
        EndGame(winCanvas, true);
    }

    // Used by the Game Monitor to start the next part of the game
    public void setPhase2() {
        doPhase2 = true;
    }
    public void setPhase3() {
        doPhase3 = true;
    }
    public void setWin() {
        playerWin = true;
    }
    public void setLost() {
        playerLost = true;
    }



    // Update() displays canvas for when player wins or lost
    void Update()
    {
        // Put Victory image if all phasesa are defeated
        if (playerWin) 
            EndGame(winCanvas, false);
        // Trigger phase 2 if enemy has lost all their health once
        else if (doPhase2) {
            twoTrans.phase2();
            doPhase2 = false;
        }
        else if (doPhase3) {
            threeTrans.phase3();
            doPhase3 = false;
        }

        // Put Defeat image is player loses all their health
        else if (playerLost)
            EndGame(loseCanvas, true);
    }

    // Displays the victory or game over image for a certain amount of time before either quitting the app (when player wins)
    // or restarting the level (when the player loses)
    void EndGame (CanvasGroup imageCanvas, bool doRestart) {
        allHealth.enabled = false;

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

    
}
