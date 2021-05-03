using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text headerField;
    public Text contentField;
    public LayoutElement layoutElement;
    public Animator animator;
    public int characterWrapLimit;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = position;
    }

    public void SetText(string header = "", string content = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        if (string.IsNullOrEmpty(content))
        {
            contentField.gameObject.SetActive(false);
        }
        else
        {
            contentField.gameObject.SetActive(true);
            contentField.text = content;
        }

        int headerLength = headerField.text.Length;
        int contentlength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentlength > characterWrapLimit) ? true : false;
    }
}