using System;
using System.Diagnostics;
using UnityEngine;

public class ArabicEngConverter:MonoBehaviour
{
    public static string ConvertToArabicNumbers(string WesternNumber)
    {
        string arabicNumbers = WesternNumber
            .Replace("0", "٠")
            .Replace("1", "١")
            .Replace("2", "٢")
            .Replace("3", "٣")
            .Replace("4", "٤")
            .Replace("5", "٥")
            .Replace("6", "٦")
            .Replace("7", "٧")
            .Replace("8", "٨")
            .Replace("9", "٩");

        return arabicNumbers;
    }
    public static string ConvertToEngNumbers(string easternNumber)
    {
        string englishNumbers = easternNumber
            .Replace("٠", "0")
            .Replace("١", "1")
            .Replace("٢", "2")
            .Replace("٣", "3")
            .Replace("٤", "4")
            .Replace("٥", "5")
            .Replace("٦", "6")
            .Replace("٧", "7")
            .Replace("٨", "8")
            .Replace("٩", "9");

        try
        {
            if (englishNumbers[englishNumbers.Length - 1].Equals('-') && int.TryParse(englishNumbers.Substring(0, englishNumbers.Length - 1), out int TranslatedNum))
            {
                UnityEngine.Debug.Log($"{TranslatedNum}");

                englishNumbers = "-"+ TranslatedNum.ToString();
                UnityEngine.Debug.Log($"{englishNumbers}");

            }
        }
        catch (Exception e)
        {

            Console.WriteLine(e.Message);
        }

        return englishNumbers;
    }

}
