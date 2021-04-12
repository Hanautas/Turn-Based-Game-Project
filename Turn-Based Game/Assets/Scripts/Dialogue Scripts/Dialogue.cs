using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    [Header ("Text Options")]
    public string[] names;

    [TextArea(5, 10)]
    public string[] sentences;

    [Header ("Text Colors - (Alpha: 255)")]
    public Color[] textColors;
    public Font[] textFonts;

    [Header ("Time Options - (Default: 0.075)")]
    public float[] delays;

    [Header ("Character Sprites")]
    public Sprite[] sprites;

    [Header ("Sound Options")]
    public AudioClip[] sounds;
}