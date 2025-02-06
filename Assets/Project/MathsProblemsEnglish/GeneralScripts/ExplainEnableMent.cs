using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExplainEnableMent : MonoBehaviour
{
    public static void EnableExplain(ref TMP_InputField FrstNum, ref TMP_InputField SecNum)
    {
        GameObject Explain = GameObject.Find("Explain");
        Button ExplainBtn = Explain.GetComponent<Button>();

        try
        {
            // Attempt to parse the text from input fields to float
            bool isFirstNumValid = float.TryParse(FrstNum.text, out float Fnum);
            bool isSecNumValid = float.TryParse(SecNum.text, out float SNum);

            // Check if both input fields contain valid float values and are not empty
            if (isFirstNumValid && isSecNumValid && !string.IsNullOrEmpty(FrstNum.text) && !string.IsNullOrEmpty(SecNum.text))
            {
                ExplainBtn.interactable = true; // Enable the button
            }
            else
            {
                ExplainBtn.interactable = false; // Disable the button
            }
        }
        catch (Exception)
        {
            ExplainBtn.interactable = false; // Disable the button on any exception (e.g., parsing error)
        }
    }
}
