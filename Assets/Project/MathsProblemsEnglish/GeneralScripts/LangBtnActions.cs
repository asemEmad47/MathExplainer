using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LangBtnActions : MonoBehaviour
{
    public static void LangBtnClick(ref bool IsEng, ref string SpeakerName, ref AudioClip[] loop)
    {
        if (IsEng)
        {
            SpeakerName = "_Heba_Egy";
            loop = Resources.LoadAll<AudioClip>("ArabLoop");
            ChangeLang(false);
            IsEng = false;
            AdditionVoiceSpeaker.NumPlace = "EgyNums";
        }
        else
        {

            SpeakerName = "_Sonya_Eng";
            loop = Resources.LoadAll<AudioClip>("EngLoop");
            ChangeLang(true);
            AdditionVoiceSpeaker.NumPlace = "EngNums";
            IsEng = true;
        }
    }
    public static void ChangeLang(bool IsEng)
    {
        TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>();
        if (textMeshProObjects.Length > 0)
        {
            foreach (TextMeshProUGUI textMeshPro in textMeshProObjects)
            {
                if (textMeshPro.name.Equals("EnPlaceholder"))
                {
                    textMeshPro.GetComponent<TextMeshProUGUI>().enabled = IsEng;
                }
                else if (textMeshPro.name.Equals("ArPlaceholder"))
                {
                    textMeshPro.GetComponent<TextMeshProUGUI>().enabled = !IsEng;
                }
                textMeshPro.alpha = 1;

            }
        }

    }
}
