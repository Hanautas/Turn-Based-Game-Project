using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public AudioSource musicSource;
    public static bool isPaused;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        musicSource.Play();
        isPaused = false;
        Time.timeScale = 1f;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        musicSource.Pause();
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void MainMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}