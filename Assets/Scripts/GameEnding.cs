using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    // Start is called before the first frame update
    float m_Timer;
    private bool playerWin = false;
    private bool playerLost = false;

    public CanvasGroup winCanvas;
    public CanvasGroup loseCanvas;

    void Update()
    {
        if (playerWin)
            EndGame(winCanvas, false);
        else if (playerLost)
            EndGame(loseCanvas, true);
    }

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

    public void setWin() {
        playerWin = true;
    }

    public void setLost() {
        playerLost = true;
    }
}
