using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResetValues : MonoBehaviour
{
    public static void ResetAllValues(TextMeshProUGUI Line, TextMeshProUGUI FirstNumPlace, TextMeshProUGUI SecNumPlace, TextMeshProUGUI sign)
    {
        string[] allowedNames ={
                FirstNumPlace.name,
                SecNumPlace.name,
                Line.name,
                sign.name,

                "Placeholder",
                "EnPlaceholder",
                "ArPlaceholder",
                "Text",
                "Sign2",
                "Line3",
                "Line2",
                "EffectText",
                "AnswerPlace",
                "Explain",
                "Reminder",
                "Sign",
                "sign",
                "AdditionSign"
                ,"SecNumPlaceAddition"
                ,"FirstNumPlaceAdditon",
                "DivideSign",
                "TimeSign" ,
                "Line2"
            };
        TextMeshProUGUI[] textMeshPros = FindObjectsOfType<TextMeshProUGUI>();

        foreach (TextMeshProUGUI textMeshPro in textMeshPros)
        {
            // Check if the current textMeshPro's name is not in the allowedNames array
            if (!System.Array.Exists(allowedNames, name => name.Equals(textMeshPro.name)))
            {
                // Destroy the entire GameObject that the TextMeshProUGUI component is part of
                Destroy(textMeshPro.gameObject);
            }
        }
        Line.gameObject.SetActive(false);
        FirstNumPlace.text = "";
        SecNumPlace.text = "";
        LongDivisionScript.InLongDev = false;
        FirstNumPlace.gameObject.SetActive(false);
        sign.gameObject.SetActive(false);
    }
    public static void GCFDelElements()
    {
        GameObject[] CirclesAndLines = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in CirclesAndLines)
        {
            if (obj.name.Contains("Circle") && !obj.name.Equals("Circle"))
            {
                Destroy(obj);
            }
            else if (obj.name.Contains("Line") && !obj.name.Equals("Line2") && !obj.name.Equals("Line3") && !obj.name.Equals("Line"))
            {
                Destroy(obj);

            }
            else if (obj.name.Contains("Square") && !obj.name.Equals("Square"))
            {
                Destroy(obj);
            }
            else if (obj.name.Contains("InputField"))
            {
                Destroy(obj);
            }
        }


    }
}
