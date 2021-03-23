using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionLog : MonoBehaviour
{
    public static ActionLog instance;

    public GridCombatSystemMain gridCombatSystem;
    private HealthSystem healthSystem;
    
    public Transform contentObject;
    public ScrollRect scrollRect;
    private Vector2 scrollPosition = new Vector2(0, 0);

    public GameObject textPrefab;
    private Text actionText;

    void Start()
    {
        instance = this;
        textPrefab = Resources.Load("ActionText") as GameObject;
    }

    void Update()
    {

    }

    public void OutputLog(string currentUnit, string damageDealt, string targetUnit)
    {
        GameObject textObject = Instantiate(textPrefab, transform.position, Quaternion.identity) as GameObject;
        textObject.transform.SetParent(contentObject, false);

        actionText = textObject.GetComponent<Text>();
        actionText.text = currentUnit + " dealt " + damageDealt + " damage to " + targetUnit + "!";

        StartCoroutine(SetScrollValue());
    }

    public IEnumerator SetScrollValue()
    {
        yield return null;
        scrollRect.normalizedPosition = scrollPosition;
    }
}