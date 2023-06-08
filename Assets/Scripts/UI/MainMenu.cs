using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Slider volume;

    public string _newGameLevel;
    private string levelToLoad;

    public void startGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_newGameLevel);
    }

    public void setVolume(float volume) {
        AudioListener.volume = volume;
    }
}
