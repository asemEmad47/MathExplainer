using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextInstantiator : MonoBehaviour
{
    public static TextMeshProUGUI InstantiateText(TextMeshProUGUI FirstNumPlace, string txt, float XPos, float YPos, float Distance, bool IsResult, int counter = -1, int yPos = 0 , bool IsArabic = false)
    {

        // Create a new GameObject for the text
        GameObject newTextObject = new GameObject(counter.ToString());
        TextMeshProUGUI newTextMesh = newTextObject.AddComponent<TextMeshProUGUI>();

        if (IsArabic)
        {
            if (txt.Equals("="))
            {
                YPos -= 25;
            }
            else
            {
                if(txt.Equals("("))
                {
                    txt = ")";
                    YPos -= 10;
                }
                else if(txt.Equals(")"))
                {
                    txt = "(";
                    YPos -= 10;
                }              
                
                if(txt.Equals("{"))
                {
                    txt = "}";
                    YPos -= 10;
                }
                else if(txt.Equals("}"))
                {
                    txt = "{";
                    YPos -= 10;
                }

            }
            txt = ArabicEngConverter.ConvertToArabicNumbers(txt);
            txt = new string(txt.Reverse().ToArray());
        }
        newTextMesh.text = txt;
        newTextMesh.font = FirstNumPlace.font;
        newTextMesh.fontSize = FirstNumPlace.fontSize;
        newTextMesh.color = Color.black;
        newTextMesh.alignment = FirstNumPlace.alignment;
        newTextMesh.fontStyle = FontStyles.Bold;

        // Set the new object as a child of the same parent
        newTextObject.transform.SetParent(FirstNumPlace.transform.parent);

        // Ensure the new Text object is in the correct position within the Canvas
        RectTransform newTextRect = newTextObject.GetComponent<RectTransform>();
        newTextRect.localScale = Vector3.one; // Ensure it maintains the correct scale

        // Set the anchored position relative to the parent's anchor
        newTextRect.GetComponent<RectTransform>().anchoredPosition = new Vector2(XPos, YPos + Distance);

        // Disable raycasting for this TextMeshPro object
        newTextMesh.raycastTarget = false;

        // Adjust font size if this is the result text
        if (IsResult)
        {
            newTextMesh.fontSize = 85f;
        }

        if (txt.Length >= 5)
        {
            // Calculate the width increment based on the length difference
            int lengthDifference = txt.Length - 5;
            float extraWidth = lengthDifference * 20f; // Increase width by 10 units for each character over 54

            // Adjust the width of the RectTransform
            newTextRect.sizeDelta = new Vector2(newTextRect.sizeDelta.x + extraWidth, newTextRect.sizeDelta.y);
            newTextMesh.enableWordWrapping = false;
        }
        return newTextMesh;
    }
}
