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

    public GameObject textPrefab;
    private Image actionPanel;
    private Text actionText;

    void Start()
    {
        instance = this;
        textPrefab = Resources.Load("ActionTextPanel") as GameObject;

        //InvokeRepeating("Advertisement", 0, 120);
    }

    public void InstantiateTextLog()
    {
        GameObject textObject = Instantiate(textPrefab, transform.position, Quaternion.identity) as GameObject;
        textObject.transform.SetParent(contentObject, false);

        actionPanel = textObject.GetComponent<Image>();
        actionText = textObject.transform.Find("Action Text").GetComponent<Text>();

        StartCoroutine(SetScrollValue());
    }

    public void OutputDamageLog(string currentUnit, string damageDealt, string targetUnit)
    {
        InstantiateTextLog();
        actionPanel.color = new Color32(100, 200, 255, 255);
        actionText.text = currentUnit + " dealt " + damageDealt + " damage to " + targetUnit + "!";
    }

    public void OutputHealLog(string currentUnit, int value)
    {
        InstantiateTextLog();
        actionPanel.color = new Color32(100, 255, 100, 255);
        actionText.text = currentUnit + " healed for " + value.ToString() + " health!";
    }

    public void OutputDeathLog(string currentUnit)
    {
        InstantiateTextLog();
        actionPanel.color = new Color32(255, 100, 100, 255);
        actionText.text = currentUnit + " is down!";
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
}