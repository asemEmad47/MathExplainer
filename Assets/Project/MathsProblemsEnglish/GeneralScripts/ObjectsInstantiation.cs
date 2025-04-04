using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectsInstantiation : MonoBehaviour
{
    public static GameObject InstantiateOBJ(float Xpos, float Ypos, GameObject OBJ,TextMeshProUGUI FirstNumPlace )
    {
        GameObject Obj = GameObject.Instantiate(OBJ);
        Obj.SetActive(true);
        CharacterProbs.CenterInPos(Xpos, Ypos, ref Obj, FirstNumPlace);
        return Obj;
    }
    public static void InstantiateCircle(float Xpos, float Ypos , GameObject Circle , ref int NumOfCircles , TextMeshProUGUI FirstNumPlace)
    {
        GameObject circle = GameObject.Instantiate(Circle);
        circle.SetActive(true);
        CharacterProbs.CenterInPos(Xpos, Ypos, ref circle, FirstNumPlace);
        circle.name = "Circle" + NumOfCircles;
        NumOfCircles++;
    }   

    public static void InstantiateLine(float Xpos, float Ypos, bool IsLeft , GameObject Line, TextMeshProUGUI FirstNumPlace)
    {
        Ypos -= 30;
        GameObject line = GameObject.Instantiate(Line);
        line.SetActive(true);
        CharacterProbs.CenterInPos(Xpos, Ypos, ref line, FirstNumPlace);

        if (IsLeft)
            line.transform.rotation = Quaternion.Euler(0, 0, -30);
        else
            line.transform.rotation = Quaternion.Euler(0, 0, 30);
    }
    public static TextMeshProUGUI InstantiateText(string txt , ref int NumOfCircles ,TMP_InputField Number , TextMeshProUGUI FirstNumPlace)
    {
        GameObject newTextObject = new GameObject("text" + NumOfCircles);

        // Add a TextMeshProUGUI component to the object
        TextMeshProUGUI newTextMesh = newTextObject.AddComponent<TextMeshProUGUI>();

        // Set the parent to the correct canvas object
        newTextMesh.transform.SetParent(Number.transform, false);  // false ensures it inherits the parent Canvas settings

        // Customize text properties
        newTextMesh.text = txt;
        newTextMesh.fontSize = 52;
        newTextMesh.color = Color.white;  // Change to white for better visibility over dark circles
        newTextMesh.alignment = TextAlignmentOptions.Center;

        // Set sorting order to make sure it's drawn on top of other UI elements
        Canvas textCanvas = newTextMesh.gameObject.AddComponent<Canvas>();
        textCanvas.overrideSorting = true;
        textCanvas.sortingOrder = 10;  // Adjust the sorting order to be above other layers (e.g., circles)

        CharacterProbs.CenterInPos(
            GameObject.Find("Circle" + (NumOfCircles - 1)).GetComponent<RectTransform>().anchoredPosition.x,
            GameObject.Find("Circle" + (NumOfCircles - 1)).GetComponent<RectTransform>().anchoredPosition.y,
            ref newTextMesh,
            FirstNumPlace
        );

        // Ensure it's rendered on top of other UI elements in the same canvas
        newTextMesh.GetComponent<RectTransform>().SetAsLastSibling();

        return newTextMesh;
    }
}
