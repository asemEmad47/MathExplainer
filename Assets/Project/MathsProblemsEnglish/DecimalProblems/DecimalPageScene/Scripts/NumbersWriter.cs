using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class NumbersWriter : MonoBehaviour
{
    public static IEnumerator WriteEachNumberInOrder(TMP_InputField number, bool IsRight, TextMeshProUGUI NumPlace, bool Explain, MonoBehaviour monoBehaviour)
    {
        string WritedNubmer = "";
        if (IsRight)
        {
            WritedNubmer = number.text.Substring(number.text.IndexOf('.') + 1, number.text.Length - number.text.IndexOf('.') - 1);
            for (int i = 0; i < WritedNubmer.Length; i++)
            {
                if (char.IsDigit(WritedNubmer[i]))
                {
                    yield return monoBehaviour.StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, WritedNubmer[i].ToString(), Explain));
                    NumPlace.text = "   " + NumPlace.text;
                    if (i == 0)
                        NumPlace.text += " " + WritedNubmer[i] + " ";
                    else
                        NumPlace.text += WritedNubmer[i] + " ";
                }
            }
        }
        else
        {
            WritedNubmer = number.text.Substring(0, number.text.IndexOf('.'));
            StringBuilder temp = new StringBuilder(NumPlace.text);
            int CurrentSpace = NumPlace.text.IndexOf('.') - 2;
            for (int i = WritedNubmer.Length - 1; i >= 0; i--)
            {
                yield return monoBehaviour.StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, WritedNubmer[i].ToString(), Explain));

                temp = new StringBuilder(NumPlace.text);
                if (CurrentSpace < 0)
                {
                    temp.Insert(0, WritedNubmer[i]);
                }
                else
                {
                    temp[CurrentSpace] = WritedNubmer[i];
                }
                CurrentSpace -= 2;
                NumPlace.text = temp.ToString();
            }
        }
    }

    public static IEnumerator WriteNumber(bool IsRight, string SpeakerName, TMP_InputField FrstNum, TMP_InputField SecNum, TextMeshProUGUI FirstNumPlace, TextMeshProUGUI SecNumPlace, bool Explain, GameObject RightArrow, GameObject LeftArrow, MonoBehaviour monoBehaviour)
    {
        if (IsRight)
            yield return monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "write the first number " + SpeakerName, Explain));
        else
            yield return monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "write the second number " + SpeakerName, Explain));

        yield return monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "starting from the decimal point " + SpeakerName, Explain));
        yield return monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "go right " + SpeakerName, Explain));

        RightArrow.SetActive(true);

        yield return monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "and write each number in order " + SpeakerName, Explain));

        if (IsRight)
            yield return monoBehaviour.StartCoroutine(WriteEachNumberInOrder(FrstNum, true, FirstNumPlace, Explain, monoBehaviour));
        else
            yield return monoBehaviour.StartCoroutine(WriteEachNumberInOrder(SecNum, true, SecNumPlace, Explain, monoBehaviour));

        RightArrow.SetActive(false);

        yield return monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "go left " + SpeakerName, Explain));

        LeftArrow.SetActive(true);

        if (IsRight)
            yield return monoBehaviour.StartCoroutine(WriteEachNumberInOrder(FrstNum, false, FirstNumPlace, Explain, monoBehaviour));
        else
            yield return monoBehaviour.StartCoroutine(WriteEachNumberInOrder(SecNum, false, SecNumPlace, Explain, monoBehaviour));

        LeftArrow.SetActive(false);
    }
}
