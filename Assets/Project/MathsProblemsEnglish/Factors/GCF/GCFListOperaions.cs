using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GCFListOperaions : MonoBehaviour
{
    private List<float> FirstNumList;
    private List<float> SecNumList;
    private TextMeshProUGUI FirstNumPlace;
    private string SpeakerName;
    private bool Explain;
    private GameObject TwoDigitsMultiplication;
    private Button LangBtn;
    private TextMeshProUGUI SecNumPlace;
    private TextMeshProUGUI Line2;
    private TextMeshProUGUI AdditionLine;
    private TextMeshProUGUI Sign2;
    private GameObject Circle;
    private GameObject Square;
    private GameObject Line;
    private TextMeshProUGUI FirstNumPlaceAddition;
    private TextMeshProUGUI SecNumPlaceAddition;

    List<string> FinalAnswer = new List<string>();
    private int finalAnswer = 1;

    public void SetComponents(TextMeshProUGUI FirstNumPlace, TextMeshProUGUI SecNumPlace, TextMeshProUGUI Line2, TextMeshProUGUI AdditionSign, TextMeshProUGUI Sign2, GameObject Circle, GameObject Square, GameObject Line, TextMeshProUGUI FirstNumPlaceAddition, TextMeshProUGUI SecNumPlaceAddition, List<float> FirstNumList, List<float> SecNumList , bool Explain, string SpeakerName)
    {
        this.FirstNumPlace = FirstNumPlace;
        this.SecNumPlace = SecNumPlace;
        this.Line2 = Line2;
        this.AdditionLine = AdditionSign;
        this.Sign2 = Sign2;
        this.Circle = Circle;
        this.Square = Square;
        this.Line = Line;
        this.FirstNumPlaceAddition = FirstNumPlaceAddition;
        this.SecNumPlaceAddition = SecNumPlaceAddition;
        this.FirstNumList = FirstNumList;
        this.SecNumList = SecNumList;
        this.Explain = Explain;
        this.SpeakerName = SpeakerName;
    }
    
    public void SetFinalAnswer(List<string> FinalAnswer)
    {
        this.FinalAnswer = FinalAnswer;
    }     
    public int GetFinalAnswerStr()
    {
        return finalAnswer;
    } 
    public void SetTDM(GameObject TDM)
    {
        this.TwoDigitsMultiplication = TDM;
    }
    public IEnumerator MulitplyList()
    {
        for (int i = 0; i < FinalAnswer.Count; i++)
        {
            Debug.Log("in gcf " + AdditionVoiceSpeaker.IsEng);
            yield return StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FinalAnswer[i].ToString(), Explain));

            TextMeshProUGUI CurrentNUmber = GameObject.Find(i.ToString()).GetComponent<TextMeshProUGUI>();
            CurrentNUmber.color = Color.red;
            CurrentNUmber.name = "-99";
            if (finalAnswer > 12 || int.Parse(FinalAnswer[i].ToString()) > 12)
            {
                TDMComponents TdmCmp = gameObject.AddComponent<TDMComponents>();
                TdmCmp.SetComponents(FirstNumPlace,SecNumPlace,Line2,AdditionLine,Sign2,FirstNumPlaceAddition,SecNumPlaceAddition);
                TdmCmp.CreateTDMComponents(finalAnswer, i, FinalAnswer);
                TextMeshProUGUI FirstNumPlaceCpy = GameObject.Find("FirstNumPlace" + i).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI SecNumPlaceCpy = GameObject.Find("SecNumPlace" + i).GetComponent<TextMeshProUGUI>();

                TMP_InputField FirstInputField = GameObject.Find("FrstNumInputField" + i).GetComponent<TMP_InputField>();
                TMP_InputField SecInputField = GameObject.Find("SecNumInputField" + i).GetComponent<TMP_InputField>();

                TwoDigitsMultiplicationScript TDM = TwoDigitsMultiplication.GetComponent<TwoDigitsMultiplicationScript>();
                TDM.SetComponenets(
                  FirstInputField
                , SecInputField
                , FirstNumPlaceCpy
                , SecNumPlaceCpy
                , GameObject.Find("Line2" + i).GetComponent<TextMeshProUGUI>()
                , GameObject.Find("Sign2" + i).GetComponent<TextMeshProUGUI>()
                , null
                , GameObject.Find("FirstNumPlaceAddition" + i).GetComponent<TextMeshProUGUI>()
                , GameObject.Find("SecNumPlaceAddition" + i).GetComponent<TextMeshProUGUI>()
                , GameObject.Find("AdditionLine" + i).GetComponent<TextMeshProUGUI>()
                );

                TwoDigitsMultiplicationScript.Explain = Explain;
                TwoDigitsMultiplicationScript.IsCalledFromOutSide = true;

                GameObject.Find("FirstNumPlaceAddition" + i).SetActive(false);
                GameObject.Find("SecNumPlaceAddition" + i).SetActive(false);
                GameObject.Find("AdditionLine" + i).SetActive(false);
                GameObject.Find("FirstNumPlace" + i).SetActive(false);
                PrimeFactors.CurrentY -= 700;

                FirstNumPlaceCpy.text = finalAnswer.ToString();
                SecNumPlaceCpy.text = FinalAnswer[i].ToString();

                FirstInputField.text = FirstNumPlaceCpy.text;
                SecInputField.text = SecNumPlaceCpy.text;

                yield return StartCoroutine(TDM.solve());

                if (AdditionVoiceSpeaker.IsEng)
                {
                    SpeakerName = "_Jenny_Eng";
                    SLStaicFunctions.SpeakerName = SpeakerName;
                    AdditionVoiceSpeaker.SpeakerName = SpeakerName;
                    AdditionVoiceSpeaker.NumPlace = "JennySound/Numbers";
                    AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";

                }
                else
                {
                    SpeakerName = "_Shakir_arab";
                    SLStaicFunctions.SpeakerName = SpeakerName;
                    AdditionVoiceSpeaker.SpeakerName = SpeakerName;
                    AdditionVoiceSpeaker.NumPlace = "ShakirSound/Numbers";
                    AdditionVoiceSpeaker.VoiceClipsPlace = "ShakirSound";
                }
            }
            finalAnswer *= int.Parse(FinalAnswer[i].ToString());

            if (i != 0)
            {

                yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));

                yield return StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, finalAnswer.ToString(), Explain));
            }
            if (i != FinalAnswer.Count - 1)
            {
                yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain));

            }
        }
    }

    public static int CompareListsAndCount(List<float> list1, List<float> list2)
    {
        int count = 0;
        int cursor1 = 0;
        int cursor2 = 0;

        while (cursor1 < list1.Count && cursor2 < list2.Count)
        {
            float value1 = list1[cursor1];
            float value2 = list2[cursor2];
            if (value1 == value2)
            {
                // Both numbers match
                cursor1++;
                cursor2++;
            }
            else
            {
                // Move the cursor of the smaller value and add 2 to the count
                if (value1 < value2)
                {
                    cursor1++;
                }
                else
                {
                    cursor2++;
                }
            }
            count += 1;

        }
        if (cursor1 < list1.Count)
            count += (list1.Count - cursor1);
        if (cursor2 < list2.Count)
            count += (list2.Count - cursor2);
        return count;
    }

    public IEnumerator WriteList(string number, List<float> Numbers, bool IsSecond = false)
    {
        FirstNumList.Sort();
        SecNumList.Sort();
        float XVal = PrimeFactors.XOffset - 600;
        yield return StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, number, Explain));
        TextInstantiator.InstantiateText(FirstNumPlace, (number).ToString() + "=", XVal, PrimeFactors.CurrentY, 0, true, -99);
        XVal += 40;

        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));
        XVal += 20;
        int Counter = 0;
        for (int i = 0; i < Numbers.Count; i++)
        {
            if (!IsSecond)
            {
                while (Counter < SecNumList.Count && SecNumList[Counter] < FirstNumList[i])
                {
                    XVal += 180;
                    Counter++;
                }

            }
            else
            {
                while (Counter < FirstNumList.Count && FirstNumList[Counter] < SecNumList[i])
                {
                    XVal += 180;
                    Counter++;
                }

            }
            yield return StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Numbers[i].ToString(), Explain));
            TextInstantiator.InstantiateText(FirstNumPlace, (Numbers[i]).ToString(), XVal + 60, PrimeFactors.CurrentY, 0, true, -99);
            XVal += 90;

            if (i != Numbers.Count - 1)
            {
                yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain));
                TextInstantiator.InstantiateText(FirstNumPlace, "×", XVal + 60, PrimeFactors.CurrentY, 0, true, -99);

            }
            XVal += 90;
            Counter++;
        }
    }
}
