using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Slider volume;
    public GameObject pauseMenu;

    private bool paused = false;

    public void setVolume(float volume) {
        AudioListener.volume = volume;
    }

    public void play() {
        pauseMenu.SetActive(false);
        paused = false;
        Time.timeScale = 1f;
    }

    void Start() {
        volume.value = AudioListener.volume;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (paused) {
                play();
            } else {
                pauseMenu.SetActive(true);
                paused = true;
                Time.timeScale = 0f;
            }
        }
    }
}
