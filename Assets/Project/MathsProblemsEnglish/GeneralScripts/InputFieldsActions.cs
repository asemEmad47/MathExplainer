using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldsActions : MonoBehaviour
{
    public static char ValidateEqsInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar) &&  !(text.Length == 0 && addedChar == '0'))
        {
            return addedChar;
        }
        else
        {
            if (text.Length == 0 && addedChar == '-') {
                return addedChar;
            }
            return '\0';
        }
    }
    public static char ValidateBasicObsInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar) && !(text.Length == 0 && addedChar == '0'))
        {
            return addedChar;
        }
        else
        {
            return '\0';
        }
    }    
    public static char ValidateDecimalObsInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar))
        {
            return addedChar;
        }
        else
        {
            if (text.Length != 0 && addedChar == '.')
            {
                return addedChar;
            }
            else if(addedChar == '.' && !text.Contains('.'))
            {
                return addedChar;
            }
            return '\0';
        }
    }
    public static void InitializePlaceholders(TMP_InputField inputField)
    {
        TextMeshProUGUI enPlaceholder;
        TextMeshProUGUI arPlaceholder;
        // Find the "Text Area" child from the input field's hierarchy
        Transform textArea = inputField.transform.Find("Text Area");

        if (textArea != null)
        {
            // Find the "EnPlaceholder" and "ArPlaceholder" under the "Text Area"
            try
            {
                enPlaceholder = textArea.Find("EnPlaceholder").GetComponent<TextMeshProUGUI>();
                arPlaceholder = textArea.Find("ArPlaceholder").GetComponent<TextMeshProUGUI>();
                // Add listener to the onSelect event of the input field
                inputField.onSelect.AddListener((string input) => OnInputFieldSelected(enPlaceholder, arPlaceholder));


                inputField.onDeselect.AddListener((string input) => OnInputFieldDeselected(inputField, enPlaceholder, arPlaceholder, AdditionVoiceSpeaker.IsEng));
            }
            catch (System.Exception )
            {

            }

;

            // Set input type to numeric to display numeric keyboard on Android
            inputField.keyboardType = TouchScreenKeyboardType.PhonePad;
            inputField.shouldHideMobileInput = true;
        }
    }

    private static void OnInputFieldSelected(TextMeshProUGUI enPlaceholder, TextMeshProUGUI arPlaceholder)
    {

        // Disable both placeholders
        if (enPlaceholder != null)
        {
            enPlaceholder.enabled = false; // Try disabling it
            enPlaceholder.alpha = 0f;      // Set the alpha to 0 to ensure it is invisible
        }

        if (arPlaceholder != null)
        {
            arPlaceholder.enabled = false;
            arPlaceholder.alpha = 0f;      // Set the alpha to 0
        }

        // Force the canvas to update
        Canvas.ForceUpdateCanvases();

    }

    // When the input field is deselected
    private static void OnInputFieldDeselected(TMP_InputField inputField, TextMeshProUGUI enPlaceholder, TextMeshProUGUI arPlaceholder, bool isEng)
    {
        // Re-enable placeholders if input field is empty
        if (string.IsNullOrEmpty(inputField.text))
        {
            if (inputField.text.Length == 0 && isEng)
            {
                enPlaceholder.enabled = true;
                arPlaceholder.enabled = false;
                enPlaceholder.alpha = 0.5f;
            }
            else if (inputField.text.Length == 0 && !isEng)
            {
                arPlaceholder.enabled = true;
                enPlaceholder.enabled = false;
                arPlaceholder.alpha = 0.5f;
            }
        }
    }
}
