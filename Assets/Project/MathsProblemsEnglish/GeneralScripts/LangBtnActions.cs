using TMPro;
using UnityEngine;

public class LangBtnActions : MonoBehaviour
{
    public static void LangBtnClick(ref bool IsEng, ref string SpeakerName, ref AudioClip[] loop , string ArabSpekerName = "_Heba_Egy", string ArabNumPlace = "EgyNums" , string EngSpekerName = "_Sonya_Eng", string EngNumPlace = "EngNums" , string ArabVoicePlaces = "AdditionTerms/AdditionSound", string EngVoicePlaces = "AdditionTerms/AdditionSound")
    {
        if (IsEng)
        {
            SpeakerName = ArabSpekerName;
            loop = Resources.LoadAll<AudioClip>("ArabLoop");
            ChangeLang(false);
            IsEng = false;
            AdditionVoiceSpeaker.IsEng = false;
            AdditionVoiceSpeaker.NumPlace = ArabNumPlace;
            AdditionVoiceSpeaker.VoiceClipsPlace = ArabVoicePlaces;
            AdditionVoiceSpeaker.SpeakerName = ArabSpekerName;
            SLStaicFunctions.SpeakerName = SpeakerName;
        }
        else
        {

            SpeakerName = EngSpekerName;
            loop = Resources.LoadAll<AudioClip>("EngLoop");
            ChangeLang(true);
            AdditionVoiceSpeaker.NumPlace = EngNumPlace;
            IsEng = true;
            AdditionVoiceSpeaker.IsEng = true;
            AdditionVoiceSpeaker.VoiceClipsPlace = EngVoicePlaces;
            AdditionVoiceSpeaker.SpeakerName = EngSpekerName;
            SLStaicFunctions.SpeakerName = SpeakerName;

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
