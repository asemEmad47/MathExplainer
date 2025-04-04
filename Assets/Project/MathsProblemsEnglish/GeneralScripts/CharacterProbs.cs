using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProbs : MonoBehaviour
{
    public static Vector2 GetCharPoos(TextMeshProUGUI FirstNumPlace, TMP_CharacterInfo charInfo, int i)
    {
        TMP_TextInfo textInfo2 = FirstNumPlace.GetTextInfo(FirstNumPlace.text);

        // Get character information
        charInfo = textInfo2.characterInfo[i];

        Vector3 charLocalPosition = charInfo.bottomLeft;

        // Convert local position to world space
        Vector3 worldPosition = FirstNumPlace.transform.TransformPoint(charLocalPosition);

        // Adjust position if necessary based on canvas scale and offsets
        RectTransform canvasRect = FirstNumPlace.canvas.GetComponent<RectTransform>();
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);

        // Set the correct position using the canvas scaling
        Vector2 uiPosition = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
        );
        return uiPosition;
    }
    public static Vector2 GetCharPos(TMP_InputField inputField, int charIndex)
    {
        // Ensure the input field and its text component exist
        if (inputField == null || inputField.textComponent == null)
        {
            Debug.LogError("InputField or its text component is null!");
            return Vector2.zero;
        }

        TMP_Text textComponent = inputField.textComponent;

        // Get text info
        TMP_TextInfo textInfo = textComponent.textInfo;

        // Ensure index is within range
        if (charIndex < 0 || charIndex >= textInfo.characterCount)
        {
            Debug.LogWarning("Character index out of range!");
            return Vector2.zero;
        }

        // Get character info
        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

        // Get character local position
        Vector3 charLocalPosition = (charInfo.bottomLeft + charInfo.topRight) / 2;

        // Convert local position to world position
        Vector3 worldPosition = textComponent.transform.TransformPoint(charLocalPosition);

        // Adjust position based on canvas
        RectTransform canvasRect = inputField.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);

        // Convert viewport to UI position
        Vector2 uiPosition = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
        );

        return uiPosition;
    }
    public static void CenterInPos(float XPos, float YPos, ref TextMeshProUGUI obj, TextMeshProUGUI FirstNumPlace)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Set the parent without keeping world position
            obj.transform.SetParent(FirstNumPlace.transform.parent, false);

            // Reset the local scale
            rectTransform.localScale = Vector3.one;

            // Convert XPos and YPos to local space of the parent RectTransform
            rectTransform.anchoredPosition = new Vector2(XPos, YPos);

            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);  // Bottom-left corner of the anchors
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);  // Top-right corner of the anchors

            // Optionally, set the pivot to the center as well
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        else
        {
            Debug.LogWarning("No RectTransform found on the TextMeshProUGUI.");
        }
    }
    public static void CenterInPos(float XPos, float YPos, ref GameObject obj, TextMeshProUGUI FirstNumPlace , int Zvalue = 1000)
    {
        if (obj.TryGetComponent<RectTransform>(out var rectTransform))
        {
            // For RectTransform
            obj.transform.SetParent(FirstNumPlace.transform.parent, false);

            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition = new Vector2(XPos, YPos);

            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        else if (obj.TryGetComponent<Transform>(out var transform))
        {
            // For regular Transform
            obj.transform.SetParent(FirstNumPlace.transform.parent, false);

            // Convert UI local position (XPos, YPos) to world space
            RectTransform parentRectTransform = FirstNumPlace.transform.parent.GetComponent<RectTransform>();
            if (parentRectTransform != null)
            {
                Vector3 worldPosition = parentRectTransform.TransformPoint(new Vector3(XPos, YPos, Zvalue));
                transform.position = worldPosition;
            }
            else
            {
                Debug.LogWarning("Parent is not a RectTransform. Using world position directly.");
                transform.position = FirstNumPlace.transform.position + new Vector3(XPos, YPos, Zvalue);
            }
        }
        else
        {
            Debug.LogWarning("The object does not have a RectTransform or Transform component.");
        }
    }    
    public static void CenterInPos(float XPos, float YPos, ref TMP_InputField obj, TextMeshProUGUI FirstNumPlace , int Zvalue = 1000)
    {
        if (obj.TryGetComponent<RectTransform>(out var rectTransform))
        {
            // For RectTransform
            obj.transform.SetParent(FirstNumPlace.transform.parent, false);

            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition = new Vector2(XPos, YPos);

            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        else if (obj.TryGetComponent<Transform>(out var transform))
        {
            // For regular Transform
            obj.transform.SetParent(FirstNumPlace.transform.parent, false);

            // Convert UI local position (XPos, YPos) to world space
            RectTransform parentRectTransform = FirstNumPlace.transform.parent.GetComponent<RectTransform>();
            if (parentRectTransform != null)
            {
                Vector3 worldPosition = parentRectTransform.TransformPoint(new Vector3(XPos, YPos, Zvalue));
                transform.position = worldPosition;
            }
            else
            {
                Debug.LogWarning("Parent is not a RectTransform. Using world position directly.");
                transform.position = FirstNumPlace.transform.position + new Vector3(XPos, YPos, Zvalue);
            }
        }
        else
        {
            Debug.LogWarning("The object does not have a RectTransform or Transform component.");
        }
    }

    public static void CenterInPos(float XPos, float YPos, ref ToggleGroup obj, TextMeshProUGUI FirstNumPlace)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Set the parent without keeping world position
            obj.transform.SetParent(FirstNumPlace.transform.parent, false);


            // Convert XPos and YPos to local space of the parent RectTransform
            rectTransform.anchoredPosition = new Vector2(XPos, YPos);

            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);  // Bottom-left corner of the anchors
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);  // Top-right corner of the anchors

            // Optionally, set the pivot to the center as well
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        else
        {
            Debug.LogWarning("No RectTransform found on the TextMeshProUGUI.");
        }
    }
}
