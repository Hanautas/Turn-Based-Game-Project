using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;

    [TextArea(5, 10)]
    public string content;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.ShowTooltip(header, content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.HideTooltip();
    }
}