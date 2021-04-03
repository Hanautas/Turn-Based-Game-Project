using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelButton : MonoBehaviour
{
    public GameObject panelObject;
    public List<GameObject> otherObjects;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && panelObject.activeSelf == true)
        {
            panelObject.SetActive(false);
        }
    }

    public void OpenPanel()
    {
        if (panelObject.activeSelf == false)
        {
            panelObject.SetActive(true);

            foreach (GameObject obj in otherObjects)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            panelObject.SetActive(false);
        }
    }
}
