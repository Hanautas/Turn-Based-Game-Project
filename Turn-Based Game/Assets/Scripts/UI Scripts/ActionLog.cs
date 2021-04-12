using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionLog : MonoBehaviour
{
    public static ActionLog instance;
    
    public Transform contentObject;
    public ScrollRect scrollRect;
    private Vector2 scrollPosition = new Vector2(0, 0);

    private GameObject textPrefab;
    private Image actionPanel;
    private Text actionText;

    public AudioClip soundEffect;
    public AudioSource audioSource;

    void Start()
    {
        instance = this;
        textPrefab = Resources.Load("ActionTextPanel") as GameObject;

        //InvokeRepeating("Advertisement", 0, 120);
        Tips();
    }

    public void InstantiateTextLog()
    {
        GameObject textObject = Instantiate(textPrefab, transform.position, Quaternion.identity) as GameObject;
        textObject.transform.SetParent(contentObject, false);

        actionPanel = textObject.GetComponent<Image>();
        actionText = textObject.transform.Find("Action Text").GetComponent<Text>();

        StartCoroutine(SetScrollValue());

        audioSource.PlayOneShot(soundEffect, 1f);
    }

    public void OutputDamageLog(string currentUnit, string damageDealt, string targetUnit)
    {
        InstantiateTextLog();
        actionPanel.color = new Color32(100, 200, 255, 255);
        actionText.text = "<b>" + currentUnit + "</b> dealt " + damageDealt + " damage to <b>" + targetUnit + "!</b>";
    }

    public void OutputHealLog(string currentUnit, int value)
    {
        InstantiateTextLog();
        actionPanel.color = new Color32(100, 255, 100, 255);
        actionText.text = "<b>" + currentUnit + "</b> healed for " + value.ToString() + " health!";
    }

    public void OutputDeathLog(string currentUnit)
    {
        InstantiateTextLog();
        actionPanel.color = new Color32(255, 100, 100, 255);
        actionText.text = "<b>" + currentUnit + "</b> is down!";
    }

    public void OutputCombatLine(string currentUnit, string combatLine)
    {
        InstantiateTextLog();
        actionPanel.color = new Color32(225, 225, 225, 255);
        actionText.text = "<b>" + currentUnit + ":</b> '" + combatLine + "'";
    }

    public IEnumerator SetScrollValue()
    {
        yield return new WaitForSeconds(0.1f);
        scrollRect.normalizedPosition = scrollPosition;
    }

    public void Advertisement()
    {
        InstantiateTextLog();
        actionPanel.color = new Color32(225, 225, 225, 255);
        actionText.text = "This game is a work in progress.";
    }

    public void Tips()
    {
        InstantiateTextLog();
        actionPanel.color = new Color32(225, 225, 225, 255);
        actionText.text = "This is the action log.\nDamage, healing, deaths, etc. will be displayed here.";
    }
}