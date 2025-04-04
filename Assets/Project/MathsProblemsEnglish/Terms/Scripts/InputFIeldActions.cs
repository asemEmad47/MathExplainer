using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TermsFieldActions : MonoBehaviour
{
    public static float GetXPos(TMP_InputField inputField)
    {
        return CharacterProbs.GetCharPos(inputField, inputField.caretPosition).x + 30;
    }    
    public static float GetXPos(TMP_InputField inputField , int index)
    {
        return CharacterProbs.GetCharPos(inputField, index).x + 30;
    }
    public static TMP_InputField InputFieldCreator(string Name, float YPos, int TermsCounter, ref TMP_InputField inputField, ref TMP_InputField InputFieldCpy, TextMeshProUGUI FirstNumPlace, ref Dictionary<TMP_InputField, char> TmpRefrerenceChar)
    {
        if (!GameObject.Find(Name +" "+ TermsCounter+" "+inputField.caretPosition ))
        {
            TMP_InputField TmpField = Instantiate(inputField);
            TmpField.name = Name + " " + TermsCounter + " "+ inputField.caretPosition;
            TmpField.text = "";
            float Xpos = TermsFieldActions.GetXPos(inputField);
            CharacterProbs.CenterInPos(Xpos, YPos, ref TmpField, FirstNumPlace);

            Image img = TmpField.GetComponent<Image>();
            Color color = img.color; 
            color.a = 0f; 
            img.color = color; 

            TmpField.textComponent.fontSize = 56;
            TmpField.GetComponent<RectTransform>().sizeDelta = new Vector2(TmpField.GetComponent<RectTransform>().sizeDelta.x - 50, TmpField.GetComponent<RectTransform>().sizeDelta.y + 10);

            InputFieldCpy = inputField;
            inputField = TmpField;
            TmpRefrerenceChar.Add(TmpField, InputFieldCpy.text[InputFieldCpy.caretPosition-1]);
            inputField.ActivateInputField(); 
            return TmpField;
        }
        return null;
    }
    public static void WidenInputField(GameObject inputField)
    {
        RectTransform rect = inputField.GetComponent<RectTransform>();

        // Store old width
        float oldWidth = rect.sizeDelta.x;

        // Increase width while keeping the left side fixed
        float newWidth = oldWidth + 70f; // Adjust this value as needed

        // Apply new width
        rect.sizeDelta = new Vector2(newWidth, rect.sizeDelta.y);

        // Move it slightly to the right to maintain the left position
        rect.anchoredPosition += new Vector2(40f, 0);

    }
    public static List<TMP_InputField> GetAllTMPInputFields()
    {
        TMP_InputField[] inputFields = FindObjectsOfType<TMP_InputField>(); // Finds all TMP_InputField components
        return new List<TMP_InputField>(inputFields);
    }   
    public static List<GameObject> GetAllGameObjects()
    {
        GameObject[] objs = FindObjectsOfType<GameObject>(); // Finds all TMP_InputField components
        return new List<GameObject>(objs);
    }
    public static void ShiftAllFields(string text , TMP_InputField inputField , TextMeshProUGUI FirstNumPlace , bool ShiftRight = true)
    {
        List<GameObject> AllFields = GetAllGameObjects();
        foreach (GameObject obj in AllFields)
        {
            if (obj.name.Contains("Pow") || obj.name.Contains("Nue") || obj.name.Contains("Deno") || obj.name.Contains("Line"))
            {
                GameObject ShiftedObj = obj;
                int FNum = 0, SNum = 0; 
                string[] parts = obj.name.Split(' '); // Split by spaces
                if (parts.Length >= 2) // Ensure there are at least 4 parts ("Pow X Y Z")
                {
                    int.TryParse(parts[1], out FNum);
                    int.TryParse(parts[2], out SNum);
                }
                if (inputField.caretPosition <= SNum && !text.Equals(""))
                {
                    if (text.Equals("+") || text.Equals("-"))
                    {
                        obj.name = $"{parts[0]} {FNum + 1} {SNum+1}";
                    }
                    else if(!text.Equals(""))
                    {
                        obj.name = $"{parts[0]} {FNum} {SNum+1}";
                    }
                    Shift(inputField, ref ShiftedObj, FirstNumPlace, SNum + 1);

                }
                else if (text.Equals(""))
                {
                    if (ShiftRight)
                    {
                        Shift(inputField, ref ShiftedObj, FirstNumPlace, SNum + 1);

                    }
                    else
                    {
                        Shift(inputField,ref ShiftedObj, FirstNumPlace,SNum-1);
                    }
                }
            }
        }
    }
    public static void Shift(TMP_InputField inputField, ref GameObject ShiftedObj, TextMeshProUGUI FirstNumPlace , int ShiftStep)
    {
        CharacterProbs.CenterInPos(
        TermsFieldActions.GetXPos(inputField, ShiftStep),
        ShiftedObj.GetComponent<RectTransform>().anchoredPosition.y,
        ref ShiftedObj,
        FirstNumPlace
        );
    }
}
