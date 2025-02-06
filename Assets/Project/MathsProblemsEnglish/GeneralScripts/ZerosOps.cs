using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZerosOps : MonoBehaviour
{
    public static void FillWithZeros(TMP_InputField FrstNum, TMP_InputField SecNum, TextMeshProUGUI FirstNumPlace, TextMeshProUGUI SecNumPlace, string symb)
    {
        int LenDifference = Mathf.Abs(FrstNum.text.Length - SecNum.text.Length);
        string tempstr = "";
        for (int i = 0; i < LenDifference; i++)
        {
            tempstr += symb;
        }
        if (FrstNum.text.Length > SecNum.text.Length)
        {
            tempstr += SecNum.text;
            SecNumPlace.text = tempstr;
            FirstNumPlace.text = FrstNum.text;
        }
        else
        {
            tempstr += FrstNum.text;
            FirstNumPlace.text = tempstr;
            SecNumPlace.text = SecNum.text;
        }
        string TempFrstNum = "";
        string TempSecNum = "";
        for (int i = 0; i < FirstNumPlace.text.Length; i++)
        {
            TempFrstNum += FirstNumPlace.text[i] + " ";
        }
        for (int i = 0; i < SecNumPlace.text.Length; i++)
        {
            TempSecNum += SecNumPlace.text[i] + " ";
        }
        FirstNumPlace.text = TempFrstNum;
        SecNumPlace.text = TempSecNum;
    }
    public static IEnumerator RemoveUselessZeros(TextMeshProUGUI FirstNumPlace, MonoBehaviour monoBehaviour, string SpeakerName, bool Explain)
    {
        TextMeshProUGUI[] textMeshProObjects = GameObject.FindObjectsOfType<TextMeshProUGUI>();

        if (textMeshProObjects.Length == 0)
            yield break;

        bool firstTimeInLoop = true;

        foreach (TextMeshProUGUI textMeshPro in textMeshProObjects)
        {
            if (textMeshPro == null)
                continue;

            // Check if the text is a number and should be removed
            if (int.TryParse(textMeshPro.name, out int number) && number != -9999 && number != 999 && textMeshPro.text == "0")
            {
                if (firstTimeInLoop)
                {
                    firstTimeInLoop = false;
                    yield return monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "Remove useless zeros " + SpeakerName, Explain));
                }

                textMeshPro.color = Color.grey;
                yield return new WaitForSeconds(0.5f);

                if (textMeshPro != null)
                    GameObject.Destroy(textMeshPro.gameObject);
            }
            else
            {
                break;
            }
        }

        // Handle decimal numbers
        if (FirstNumPlace != null && FirstNumPlace.text.Contains('.'))
        {
            for (int i = FirstNumPlace.text.Length - 2; i >= 0; i -= 2)
            {
                TextMeshProUGUI textMeshPro = null;

                try
                {
                    string objName = (-100 + i).ToString();
                    GameObject foundObj = GameObject.Find(objName);

                    if (foundObj != null)
                        textMeshPro = foundObj.GetComponent<TextMeshProUGUI>();
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error finding object: {e.Message}");
                    continue;
                }

                if (textMeshPro == null || !int.TryParse(textMeshPro.name, out int number) || textMeshPro.text != "0")
                    break;

                textMeshPro.color = Color.grey;
                yield return new WaitForSeconds(0.2f);

                try
                {
                    if (textMeshPro != null)
                        GameObject.Destroy(textMeshPro.gameObject);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error destroying object: {e.Message}");
                }
            }
        }
    }
    public static string GetNumberWithoutZeros(string number, int index)
    {
        string numberString = "";
        for (int i = index; i >= 0; i -= 2)
        {
            numberString += number[i];
        }
        return numberString;
    }

    public static IEnumerator DecimalRemoveUselessZeros(int LongestInt , string SpeakerName , bool Explain , MonoBehaviour monoBehaviour)
    {
        TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>();

        int counter = 0;
        bool IsFirstTime = true;
        if (textMeshProObjects.Length > 0)
        {
            foreach (TextMeshProUGUI textMeshPro in textMeshProObjects)
            {
                // Check if the text is a number and greater than or equal to 100
                if (int.TryParse(textMeshPro.name, out int number) && number >= 100 && textMeshPro.text == "0" && counter < LongestInt - 1)
                {
                    if (IsFirstTime)
                    {
                        yield return monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "Remove useless zeros " + SpeakerName, Explain));
                        IsFirstTime = false;
                        yield return new WaitForSeconds(0.5f);
                    }
                    textMeshPro.color = Color.grey;
                    yield return new WaitForSeconds(0.5f);

                    Destroy(textMeshPro.gameObject);
                    counter++;
                }
                else
                {
                    break;
                }
            }
        }

    }

    public static void DecimalFillWithZeros(TMP_InputField FrstNum , TMP_InputField SecNum , int LongestInt , TextMeshProUGUI FirstNumPlace,TextMeshProUGUI SecNumPlace)
    {
        // Parse inputs from the TMP_InputFields
        if (float.TryParse(FrstNum.text, out float firstNumber) && float.TryParse(SecNum.text, out float secondNumber))
        {
            // Convert the numbers to strings
            string firstStr = firstNumber.ToString();
            string secondStr = secondNumber.ToString();

            // Split into integer and decimal parts
            string[] firstParts = firstStr.Split('.');
            string[] secondParts = secondStr.Split('.');

            string firstInteger = firstParts[0];
            string firstDecimal = firstParts.Length > 1 ? firstParts[1] : "";

            string secondInteger = secondParts[0];
            string secondDecimal = secondParts.Length > 1 ? secondParts[1] : "";

            // Make integer parts equal in length by padding with leading zeros
            int maxIntegerLength = Mathf.Max(firstInteger.Length, secondInteger.Length);
            LongestInt = maxIntegerLength;

            firstInteger = firstInteger.PadLeft(maxIntegerLength, '0');
            secondInteger = secondInteger.PadLeft(maxIntegerLength, '0');

            // Make decimal parts equal in length by padding with trailing zeros
            int maxDecimalLength = Mathf.Max(firstDecimal.Length, secondDecimal.Length);
            firstDecimal = firstDecimal.PadRight(maxDecimalLength, '0');
            secondDecimal = secondDecimal.PadRight(maxDecimalLength, '0');

            // Format both integer and decimal parts with spaces
            if (firstDecimal.Equals(""))
            {
                firstDecimal = "0";
            }
            if (secondDecimal.Equals(""))
            {
                secondDecimal = "0";
            }

            string formattedFirstNum = FormatNumberWithSpaces(firstInteger, firstDecimal, ".");
            string formattedSecondNum = FormatNumberWithSpaces(secondInteger, secondDecimal, ".");

            // Update the TextMeshProUGUI fields with formatted numbers

            FirstNumPlace.text = formattedFirstNum;
            SecNumPlace.text = formattedSecondNum;
        }
        else
        {
            Debug.LogError("Invalid input: Please ensure both input fields contain valid float numbers.");
        }
    }
    public static string FormatNumberWithSpaces(string integerPart, string decimalPart, string separator)
    {
        // Add space between digits of the integer part
        string formattedInteger = string.Join(" ", integerPart.ToCharArray());

        // Add space between digits of the decimal part
        string formattedDecimal = string.Join(" ", decimalPart.ToCharArray());

        // Return the formatted number with separator between integer and decimal parts
        return formattedInteger + " " + separator + " " + formattedDecimal;
    }
}
