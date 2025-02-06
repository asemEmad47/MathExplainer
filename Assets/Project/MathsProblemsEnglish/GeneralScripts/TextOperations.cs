using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextOperations : MonoBehaviour
{
    public static void MakeTextRed(string name, string FNum)
    {
        GameObject textGameObject = GameObject.Find((name).ToString());
        TextMeshProUGUI myText = textGameObject.GetComponent<TextMeshProUGUI>();
        myText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum}</color>";
    }
}
