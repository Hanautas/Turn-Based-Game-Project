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
    }

    void Update()
    {

    }

    public void OutputDamageLog(string currentUnit, string damageDealt, string targetUnit)
    {
        GameObject textObject = Instantiate(textPrefab, transform.position, Quaternion.identity) as GameObject;
        textObject.transform.SetParent(contentObject, false);

        actionPanel = textObject.GetComponent<Image>();
        actionPanel.color = new Color32(100, 200, 255, 255);

        actionText = textObject.transform.Find("Action Text").GetComponent<Text>();
        actionText.text = currentUnit + " dealt " + damageDealt + " damage to " + targetUnit + "!";

        StartCoroutine(SetScrollValue());
    }

    public void OutputDeathLog(string currentUnit)
    {
        GameObject textObject = Instantiate(textPrefab, transform.position, Quaternion.identity) as GameObject;
        textObject.transform.SetParent(contentObject, false);

        actionPanel = textObject.GetComponent<Image>();
        actionPanel.color = new Color32(255, 100, 100, 255);

        actionText = textObject.transform.Find("Action Text").GetComponent<Text>();
        actionText.text = currentUnit + " is down!";

        StartCoroutine(SetScrollValue());
    }

    public IEnumerator SetScrollValue()
    {
        yield return null;
        scrollRect.normalizedPosition = scrollPosition;
    }
}