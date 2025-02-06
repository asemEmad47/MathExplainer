using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollControler : MonoBehaviour , IDragHandler
{
    [SerializeField] private ScrollRect scrollRect;

    public static float Ypos = 0;
    public void OnDrag(PointerEventData eventData)
    {

        if (scrollRect == null) return;


        // Prevent scrolling if at the top or bottom
        if (scrollRect.vertical)
        {
            float newY = scrollRect.verticalNormalizedPosition + eventData.scrollDelta.y * scrollRect.scrollSensitivity * 0.1f;

            if (newY <= Ypos && Ypos != 0) // At top or bottom
            {
                scrollRect.verticalNormalizedPosition = Ypos;
            }
        }
    }

}
