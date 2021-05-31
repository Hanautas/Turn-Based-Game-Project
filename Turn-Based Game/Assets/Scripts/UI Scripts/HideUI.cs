using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    private GameObject canvasUI;

    void Start()
    {
        canvasUI = this.gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (canvasUI.activeSelf)
            {
                canvasUI.SetActive(false);
            }
            else if (!canvasUI.activeSelf)
            {
                canvasUI.SetActive(true);
            }
        }
    }
}