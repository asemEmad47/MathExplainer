using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColoringScript : MonoBehaviour
{
    public static void ColorAllTextsWith(string StrName , Color color)
    {
        List<TextMeshProUGUI> ALlTexts = new List<TextMeshProUGUI>(Resources.FindObjectsOfTypeAll<TextMeshProUGUI>());
        foreach (TextMeshProUGUI txt in ALlTexts)
        {
            if (txt.name == StrName) {
                txt.color = color;
            }
        }
    }
    public static void ColorThemAll(string name, Color color)
    {
        TextMeshProUGUI[] allTextMeshes = GameObject.FindObjectsOfType<TextMeshProUGUI>();

        foreach (TextMeshProUGUI textMesh in allTextMeshes)
        {
            if (textMesh.name.Equals(name))
            {
                textMesh.color = color; 
            }
        }

    }
}
