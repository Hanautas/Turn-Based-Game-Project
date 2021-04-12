using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Animator transition;
    public float transitionTime = 1f;
    public string sceneToLoad;

    public void NewGame()
    {
        StartCoroutine(LoadLevel(sceneToLoad));
    }

    public IEnumerator LoadLevel(string name)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(name);
    }

    public void LoadGame()
    {
        // Load a saved game
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetDialogueSFX()
    {
        
    }
}