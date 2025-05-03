using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FinalAnswerScript : MonoBehaviour
{
    private TextMeshProUGUI FirstNumPlace;
    private TextMeshProUGUI SecNumPlace;
    private TextMeshProUGUI Line2;
    private TextMeshProUGUI Sign2;
    private TextMeshProUGUI AdditionLine;
    private TextMeshProUGUI FirstNumPlaceAddition;
    private TextMeshProUGUI SecNumPlaceAddition;
    private TMP_FontAsset AutmnFont;
    private TMP_FontAsset Amari;

    private GameObject Line;
    private GameObject Circle;
    private GameObject Square;
    private GameObject TwoDigitsMultiplication;
    private string SpeakerName;
    private bool Explain;
    private bool Stop;
    private List<string> FinalAnswer;
    private List<float> FirstNumList;
    private List<float> SecNumList ;
    
    public string FnumTxt;
    public string SNumTxt;
    public static string result;


    GCFListOperaions gCFListOperaions;

    public void Awake()
    {
        gCFListOperaions = gameObject.AddComponent<GCFListOperaions>();
        FinalAnswer = new List<string>();

    }

    public void SetComponents(TextMeshProUGUI FirstNumPlace, TextMeshProUGUI SecNumPlace, TextMeshProUGUI Line2, TextMeshProUGUI AdditionLine, TextMeshProUGUI Sign2, GameObject Circle, GameObject Square, GameObject Line, TextMeshProUGUI FirstNumPlaceAddition, TextMeshProUGUI SecNumPlaceAddition , string Fnumtxt , string Snumtxt , List<float> FirstNumList , List<float> SecNumList , bool Explain , string SpeakerName , TMP_FontAsset Autmn , TMP_FontAsset Amari)
    {
        this.FirstNumPlace = FirstNumPlace;
        this.SecNumPlace = SecNumPlace;
        this.Line2 = Line2;
        this.AdditionLine = AdditionLine;
        this.Sign2 = Sign2;
        this.Circle = Circle;
        this.Square = Square;
        this.Line = Line;
        this.FirstNumPlaceAddition = FirstNumPlaceAddition;
        this.SecNumPlaceAddition = SecNumPlaceAddition;
        this.SNumTxt = Snumtxt;
        this.FnumTxt = Fnumtxt;
        this.FirstNumList = FirstNumList;
        this.SecNumList = SecNumList;
        this.Explain = Explain;
        this.SpeakerName = SpeakerName;
        this.AutmnFont = Autmn;
        this.Amari = Amari;
    }

    public void SetTDM(GameObject TDM)
    {
        this.TwoDigitsMultiplication = TDM;
    }
    public void GetGCFFinalAnswer()
    {
        int FirstListIterator = 0, SecListIterator = 0;

        while (FirstListIterator < FirstNumList.Count && SecListIterator < SecNumList.Count)
        {
            if (FirstNumList[FirstListIterator] == SecNumList[SecListIterator])
            {
                FinalAnswer.Add(FirstNumList[FirstListIterator].ToString());
                FirstListIterator++;
                SecListIterator++;
            }
            else if (FirstNumList[FirstListIterator] < SecNumList[SecListIterator])
            {
                FirstListIterator++;
            }
            else
            {
                SecListIterator++;
            }
        }
    }
    public void GetLCMFinalAnswer()
    {
        int FirstListIterator = 0, SecListIterator = 0;

        while (FirstListIterator < FirstNumList.Count && SecListIterator < SecNumList.Count)
        {
            if (FirstNumList[FirstListIterator] == SecNumList[SecListIterator])
            {
                FinalAnswer.Add(FirstNumList[FirstListIterator].ToString());
                FirstListIterator++;
                SecListIterator++;
            }
            else if (FirstNumList[FirstListIterator] < SecNumList[SecListIterator])
            {
                FinalAnswer.Add(FirstNumList[FirstListIterator].ToString());
                FirstListIterator++;
            }
            else
            {
                FinalAnswer.Add(SecNumList[SecListIterator].ToString());
                SecListIterator++;
            }
        }
        while (FirstListIterator < FirstNumList.Count)
        {
            FinalAnswer.Add(FirstNumList[FirstListIterator].ToString());
            FirstListIterator++;
        }

        while (SecListIterator < SecNumList.Count)
        {
            FinalAnswer.Add(SecNumList[SecListIterator].ToString());
            SecListIterator++;
        }
    }
    public IEnumerator GCF_LCM_FinalAnswer()
    {
        gCFListOperaions.SetComponents(FirstNumPlace, SecNumPlace, Line2, AdditionLine, Sign2, Circle, Square, Line, FirstNumPlaceAddition, SecNumPlaceAddition , FirstNumList , SecNumList , Explain , SpeakerName);
        yield return StartCoroutine(gCFListOperaions.WriteList(FnumTxt, FirstNumList));
        PrimeFactors.CurrentY -= 70;

        yield return StartCoroutine(gCFListOperaions.WriteList(SNumTxt, SecNumList, true));

        if (PlayerPrefs.GetString("type").Equals("GCF"))
        {
            GetGCFFinalAnswer();

        }
        else
        {
            GetLCMFinalAnswer();
        }

        yield return StartCoroutine(SquareScript.SquarePairs(FirstNumList , SecNumList ,Square, FirstNumPlace));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "we have here" + SpeakerName, Explain));

        int counter =GCFListOperaions.CompareListsAndCount(FirstNumList, SecNumList);

        yield return StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, counter.ToString(), Explain));


        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "columns" + SpeakerName, Explain));



        if (PlayerPrefs.GetString("type").Equals("GCF"))
        {
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "Look at" + SpeakerName, Explain));

            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "the full columns" + SpeakerName, Explain));


        }
        else
        {
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "take one number" + SpeakerName, Explain));
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "from each column" + SpeakerName, Explain));

        }

        yield return StartCoroutine(WriteFinalAnswer());

    }

    public IEnumerator WriteFinalAnswer()
    {
        float XVal = PrimeFactors.XOffset - 570;
        PrimeFactors.CurrentY -= 100;
        TextMeshProUGUI GCfPlace = new TextMeshProUGUI();
        if (PlayerPrefs.GetString("type").Equals("GCF"))
        {
            if(AdditionVoiceSpeaker.IsEng)
                TextInstantiator.InstantiateText(FirstNumPlace, ("G.C.F = ").ToString(), XVal, PrimeFactors.CurrentY - 110, 0, true, -9999);
            else
            {
                FirstNumPlace.font = Amari;
                TextInstantiator.InstantiateText(FirstNumPlace, ("م . ع . أ = ").ToString(), 280, PrimeFactors.CurrentY - 110, 0, true, -9999, -1, true);
            }

            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so the greatest common factor equals" + SpeakerName, Explain));

        }
        else
        {
            if(AdditionVoiceSpeaker.IsEng)
                TextInstantiator.InstantiateText(FirstNumPlace, ("L.C.M = ").ToString(), XVal, PrimeFactors.CurrentY - 110, 0, true, -9999);
            else
            {
                FirstNumPlace.font = Amari;
                TextInstantiator.InstantiateText(FirstNumPlace, ("م . م . أ = ").ToString(), 280, PrimeFactors.CurrentY - 110, 0, true, -9999, -1, true);

            }

            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "least common multiple" + SpeakerName, Explain));

        }
        FirstNumPlace.font = AutmnFont;
        GCfPlace = GameObject.Find("-9999").GetComponent<TextMeshProUGUI>();
        if (AdditionVoiceSpeaker.IsEng)
            XVal += 140;
        else{
            XVal -= 30;
        }
        Stop = true;
        int finalAnswer = 0;
        if (FinalAnswer.Count > 1)
        {
            if (!GCFScript.IsEng)
            {
                PrimeFactors.CurrentY -= 200;
            }
            for (int i = 0; i < FinalAnswer.Count; i++)
            {
                yield return StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FinalAnswer[i].ToString(), Explain));
                TextInstantiator.InstantiateText(FirstNumPlace, (FinalAnswer[i]).ToString(), XVal + 60, PrimeFactors.CurrentY - 110, 0, true, i);
                XVal += 70;

                if (i != FinalAnswer.Count - 1)
                {
                    yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain));
                    TextInstantiator.InstantiateText(FirstNumPlace, "×", XVal + 60, PrimeFactors.CurrentY - 110, 0, true, -99);

                }
                XVal += 70;
            }
            yield return new WaitForSeconds(2f);

            gCFListOperaions.SetTDM(TwoDigitsMultiplication);
            gCFListOperaions.SetFinalAnswer(FinalAnswer);
            yield return StartCoroutine(gCFListOperaions.MulitplyList());
            finalAnswer = gCFListOperaions.GetFinalAnswerStr();

            if(AdditionVoiceSpeaker.IsEng)
                TextInstantiator.InstantiateText(FirstNumPlace, "= " + finalAnswer.ToString(), PrimeFactors.XOffset - 380, GCfPlace.GetComponent<RectTransform>().anchoredPosition.y - 75, 0, true, -123);
            else
                TextInstantiator.InstantiateText(FirstNumPlace,  finalAnswer.ToString() + " =", 60, GCfPlace.GetComponent<RectTransform>().anchoredPosition.y - 95, 0, true, -123);

            result = finalAnswer.ToString();
        }
        else
        {
            if (FinalAnswer.Count == 0)
            {
                finalAnswer = 0;

            }
            else
            {
                finalAnswer = int.Parse(FinalAnswer[0].ToString());
            }
            yield return StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, finalAnswer.ToString(), Explain));
            TextInstantiator.InstantiateText(FirstNumPlace, finalAnswer.ToString(), XVal + 90, PrimeFactors.CurrentY - 110, 0, true, -99);
            result = finalAnswer.ToString();
        }
    }
}
