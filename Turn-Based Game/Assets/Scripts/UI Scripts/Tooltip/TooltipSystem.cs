using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;

    public Tooltip tooltip;

    public void Awake()
    {
        instance = this;
    }

    public static void ShowTooltip(string header = "", string content = "")
    {
        instance.tooltip.gameObject.SetActive(true);
        instance.tooltip.SetText(header, content);
        instance.tooltip.animator.SetBool("Show Tooltip", true);
    }

    public static void HideTooltip()
    {
        instance.tooltip.gameObject.SetActive(false);
    }
}