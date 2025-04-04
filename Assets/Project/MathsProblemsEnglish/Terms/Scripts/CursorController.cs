using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public static void GoRight(ref TMP_InputField inputField ,ref TMP_InputField InputFieldCpy)
    {
        if (InputFieldCpy != null && inputField.caretPosition + 1 > inputField.text.Length)
        {
            SwitchCursor(ref inputField, ref InputFieldCpy);
        }
        else
        {
            inputField.caretPosition = Mathf.Min(inputField.text.Length, inputField.caretPosition + 1);
            while (inputField.caretPosition < inputField.text.Length && inputField.text[inputField.caretPosition] == ' ')
            {
                inputField.caretPosition = Mathf.Min(inputField.text.Length, inputField.caretPosition + 1);
            }
        }
        inputField.ActivateInputField();
    }
    public static void GoLeft(ref TMP_InputField inputField,ref TMP_InputField InputFieldCpy)
    {
        if (InputFieldCpy != null && inputField.caretPosition - 1 < 0)
        {
            SwitchCursor(ref inputField, ref InputFieldCpy);
        }
        else
        {
            inputField.caretPosition = Mathf.Max(0, inputField.caretPosition - 1);
            while (inputField.caretPosition > 0 && inputField.text[inputField.caretPosition] == ' ')
            {
                inputField.caretPosition = Mathf.Max(0, inputField.caretPosition - 1);
            }
        }
        inputField.ActivateInputField();
    }
    public static void UpDown(ref TMP_InputField inputField, ref TMP_InputField InputFieldCpy , string FieldName , string FieldName2 = "")
    {
        if (InputFieldCpy != null)
        {
            inputField = InputFieldCpy;
            InputFieldCpy = null;
        }
        List<TMP_InputField> AllFields = TermsFieldActions.GetAllTMPInputFields();
        TMP_InputField ClosestField = null;
        int ClosestNueIndex = inputField.text.Length;
        foreach (TMP_InputField Field in AllFields)
        {
            if (Field.name.Contains(FieldName) || (!FieldName2.Equals("") && Field.name.Contains(FieldName2)))
            {
                string[] parts = Field.name.Split(' '); // Split by spaces
                int.TryParse(parts[2], out int PowerPlace);
                if (Math.Abs(PowerPlace-inputField.caretPosition) < ClosestNueIndex)
                {
                    ClosestNueIndex = Math.Abs(PowerPlace - inputField.caretPosition);
                    inputField.caretPosition = PowerPlace;
                    ClosestField = Field;
                }
            }
        }
        if (ClosestField != null)
        {
            InputFieldCpy = inputField;
            inputField = ClosestField;
            inputField.caretPosition = 0;
        }
        inputField.ActivateInputField();
    }
    public static void SwitchCursor(ref TMP_InputField inputField, ref TMP_InputField InputFieldCpy)
    {
        inputField.DeactivateInputField();
        inputField = InputFieldCpy;
        InputFieldCpy = null;
        inputField.ActivateInputField();
    }
}