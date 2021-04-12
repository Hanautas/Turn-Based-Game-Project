using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Source: https://forum.unity.com/threads/fps-counter.505495/
public class FPSCounter : MonoBehaviour
{
 
    public float timer, refresh, avgFramerate;
    private string display = "FPS: {0}";
    private Text m_Text;
 
    private void Start()
    {
        m_Text = GetComponent<Text>();
    }
 
 
    private void Update()
    {
        //Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;
 
        if(timer <= 0) avgFramerate = (int) (1f / timelapse);
        m_Text.text = string.Format(display,avgFramerate.ToString());
    }
}