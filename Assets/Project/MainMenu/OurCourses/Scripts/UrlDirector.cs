using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UrlDirector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI urlText;

    public void OnPointerClick(PointerEventData eventData)
    {

        Application.OpenURL(urlText.text);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    }
}
