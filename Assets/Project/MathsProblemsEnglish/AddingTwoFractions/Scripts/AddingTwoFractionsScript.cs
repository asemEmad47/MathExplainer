using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AddingTwoFractionsScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FDeno;
    [SerializeField] private TMP_InputField SDeno;

    [SerializeField] private TMP_InputField FNue;
    [SerializeField] private TMP_InputField SNue;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI SecNumPlace;
    [SerializeField] private TextMeshProUGUI Line2;
    [SerializeField] private TextMeshProUGUI AdditionLine;
    [SerializeField] private TextMeshProUGUI Sign2;
    [SerializeField] private GameObject Circle;
    [SerializeField] private GameObject Square;
    [SerializeField] private GameObject Line;
    [SerializeField] private ScrollRect scrollRect; // Reference to the ScrollRect component
    [SerializeField] private Button LangBtn;
    [SerializeField] private GameObject TwoDigitsMultiplication;
    [SerializeField] private GameObject LCM;
    [SerializeField] private TextMeshProUGUI FirstNumPlaceAddition;
    [SerializeField] private TextMeshProUGUI SecNumPlaceAddition;
    [SerializeField] private bool IsExplain = true;

    public GameObject PmfObj;

    private AudioClip[] loop;
    public static AudioSource audioSource;


    private bool Explain = false;
    public static string SpeakerName = "_Jenny_Eng";
    public static bool IsEng = true;


    public static string FirstNumber = "";
    public static string SecNumber = "";

    public static string ThirdNumber = "";
    public static string FourthNumber = "";


    public void Start()
    {
        FNue.text = FirstNumber;
        FDeno.text = SecNumber;

        SNue.text = ThirdNumber;
        SDeno.text = FourthNumber;


        UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

        InputFieldsActions.InitializePlaceholders(FNue);
        InputFieldsActions.InitializePlaceholders(SDeno);
        InputFieldsActions.InitializePlaceholders(SNue);
        InputFieldsActions.InitializePlaceholders(SDeno);

    }

    // Update is called once per frame
    void Update()
    {
        FNue.onValidateInput = ValidateInput;
        SNue.onValidateInput = ValidateInput;
        FDeno.onValidateInput = ValidateInput;
        SDeno.onValidateInput = ValidateInput;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ExplainEnableMent.EnableExplain(ref FNue, ref FDeno);
        ExplainEnableMent.EnableExplain(ref SDeno, ref SDeno);
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";
        LangBtn.interactable = false;
        if (IsEng)
        {
            SpeakerName = "_Jenny_Eng";
        }
        else
        {
            SpeakerName = "_Jenny_Eng";

        }
        if (Explain)
        {
            GameObject ExplainBtn = GameObject.Find("Explain");
            UnityEngine.UI.Button button = ExplainBtn.GetComponent<UnityEngine.UI.Button>();
            button.interactable = false;
        }



    }
    public void SolveBtnAction()
    {
        Explain = false;
        StartCoroutine(StaretExplaining());
    }  
    public void ExplainBtnAction()
    {
        Explain = true;
        StartCoroutine(StaretExplaining());
    }

    public IEnumerator StaretExplaining()
    {
        ResetValues.ResetAllValues(Line2, FirstNumPlace, SecNumPlace, Sign2);
        PlayerPrefs.SetString("type", "LCM");
        ResetValues.GCFDelElements();
        Line2.gameObject.SetActive(false);

        UnityEngine.Color color = FNue.GetComponent<UnityEngine.UI.Image>().color; 
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "Look at the two denominator" + SpeakerName, IsExplain)));
        FDeno.GetComponent<UnityEngine.UI.Image>().color = Color.red;
        SDeno.GetComponent<UnityEngine.UI.Image>().color = Color.red;

        float YPos = 0;
        string LCMRes = "";
        if (!FDeno.text.Equals(SDeno.text))
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "they are not equal" + SpeakerName, IsExplain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so lets use lcm" + SpeakerName, IsExplain)));
            FDeno.GetComponent<UnityEngine.UI.Image>().color = color;
            SDeno.GetComponent<UnityEngine.UI.Image>().color = color;

            GCFScript LCMObj = LCM.GetComponent<GCFScript>();
            GCFScript.Explain = Explain;
            PrimeFactors.Explain = Explain;
            yield return (StartCoroutine(LCMObj.StartExplaining()));

            YPos = PrimeFactors.CurrentY - 300;
            LCMRes = FinalAnswerScript.result;

            yield return (StartCoroutine(WriteTwoNumbers(YPos, FNue.text, FDeno.text, SNue.text, SDeno.text)));


            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "now the two denominators must be" + SpeakerName, IsExplain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, LCMRes.ToString(), IsExplain)));

            TextMeshProUGUI LcmResNum = GameObject.Find("-123").GetComponent<TextMeshProUGUI>();
            LcmResNum.color = Color.red;
        }
        else
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "the are like denominators" + SpeakerName, IsExplain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so start adding" + SpeakerName, IsExplain)));
            FDeno.GetComponent<UnityEngine.UI.Image>().color = color;
            SDeno.GetComponent<UnityEngine.UI.Image>().color = color;
            LCMRes = "1";
            YPos += 3500;
        }
        float XPos = 0 , YposTewmp = YPos;
        if (!LCMRes.Equals("1")&& int.Parse(FDeno.text) != int.Parse(LCMRes))
        {
            yield return (StartCoroutine(MutliPlyNumbers(true, LCMRes, YPos)));
        }
        else
        {

            YposTewmp = YPos;
            YPos += 300;
            XPos = PrimeFactors.XOffset - 680;
            TextInstantiator.InstantiateText(FirstNumPlace, (((int.Parse(FNue.text)).ToString())), XPos, YPos - 600, 0, true, 500);

            YPos -= 50;
            TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos, YPos - 600, 0, true, -99);
            YPos -= 50;
            TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(FDeno.text)).ToString(), XPos, YPos - 600, 0, true);
            XPos += 200;
            YPos += 100;
            TextInstantiator.InstantiateText(FirstNumPlace, "+", XPos, YPos - 650, 0, true, -99);
            YPos = YposTewmp;
        }
        if (!LCMRes.Equals("1") && int.Parse(SDeno.text) != int.Parse(LCMRes))
        {
            yield return (StartCoroutine(MutliPlyNumbers(false, LCMRes, YPos)));

        }
        else
        {
            XPos = PrimeFactors.XOffset-225 ;

            YPos += 300;
            TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(SNue.text)).ToString(), XPos - 50, YPos - 600, 0, true, 501);
            YPos -= 50;
            TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos - 50, YPos - 600, 0, true, -99);
            YPos -= 50;
            TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(SDeno.text)).ToString(), XPos - 50, YPos - 600, 0, true, (int)XPos + 3);
            YPos -= 200;
        }
        int MutlipliedNum = int.Parse(LCMRes);

        int FNumNewNum = int.Parse(FNue.text) * (MutlipliedNum / int.Parse(FDeno.text));

        int SNumNewNum = int.Parse(SNue.text) * (MutlipliedNum / int.Parse(SDeno.text));

        if (LCMRes.Equals("1"))
        {
            FNumNewNum = int.Parse(FNue.text);
            SNumNewNum = int.Parse(SNue.text);
            LCMRes = FDeno.text;
            MutlipliedNum = int.Parse(FDeno.text);
        }
        YPos -= 200;

        if(!FDeno.text.Equals(SDeno.text))
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "now they are like denominators" + SpeakerName, IsExplain)));

        XPos = PrimeFactors.XOffset + 200;

        YPos -= 150;
        TextInstantiator.InstantiateText(FirstNumPlace, "= ", XPos - 230, YPos, 0, true, -99);

        XPos -= 150;
        TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos, YPos, 0, true, -99);
        YPos -= 50;
        TextInstantiator.InstantiateText(FirstNumPlace, (MutlipliedNum).ToString(), XPos, YPos, 0, true, -444);
        TextMeshProUGUI FNueRes = new TextMeshProUGUI() , SNueRes = new TextMeshProUGUI(); 
        try
        {
            FNueRes = GameObject.Find((500).ToString()).GetComponent<TextMeshProUGUI>();

        }
        catch (Exception)
        {
            FNueRes.text = FNue.text;
        }

        try
        {
            SNueRes = GameObject.Find((501).ToString()).GetComponent<TextMeshProUGUI>();

        }
        catch (Exception)
        {
            SNueRes.text = SNue.text;
        }

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FNueRes.text.ToString(), IsExplain)));
        FNueRes.color = Color.red;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, IsExplain)));


        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SNueRes.text.ToString(), IsExplain)));
        SNueRes.color = Color.red;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, IsExplain)));

        int finalRes = FNumNewNum + SNumNewNum;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, finalRes.ToString(), IsExplain)));

        TextInstantiator.InstantiateText(FirstNumPlace, (finalRes).ToString(), XPos, YPos + 100, 0, true, 123);
        TextMeshProUGUI NonSimplifiedNue = GameObject.Find((123).ToString()).GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI NonSimplifiedDeno = GameObject.Find((-444).ToString()).GetComponent<TextMeshProUGUI>();



        for (int i = Math.Min(finalRes, int.Parse(LCMRes)); i >= 2; i--)
        {
            if (finalRes % i == 0 && int.Parse(LCMRes) % i == 0)
            {

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "this number can be simplified" + SpeakerName, IsExplain)));
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "divide" + SpeakerName, IsExplain)));
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "by" + SpeakerName, IsExplain)));
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, i.ToString(), IsExplain)));
                TextInstantiator.InstantiateText(FirstNumPlace, "÷ " + (i).ToString(), XPos + 150, YPos, 0, true, 444);
                TextInstantiator.InstantiateText(FirstNumPlace, "÷ " + (i).ToString(), XPos + 150, YPos + 100, 0, true, 555);

                XPos = PrimeFactors.XOffset - 30;

                TextMeshProUGUI NueDivisor = GameObject.Find((555).ToString()).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI DenoDivisor = GameObject.Find((444).ToString()).GetComponent<TextMeshProUGUI>();


                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, IsExplain)));
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, NonSimplifiedNue.text.ToString(), IsExplain)));


                NonSimplifiedNue.color = Color.red;
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "divide" + SpeakerName, IsExplain)));
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, i.ToString(), IsExplain)));
                NueDivisor.color = Color.red;
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, IsExplain)));

                TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos-15, YPos-170, 0, true);
                XPos += 70;
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (finalRes / i).ToString(), IsExplain)));

                YPos -= 150;
                TextInstantiator.InstantiateText(FirstNumPlace, (finalRes / i).ToString(), XPos, YPos, 0, true, 123);

                YPos -= 50;
                TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos, YPos, 0, true, -99);
                YPos -= 50;



                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, NonSimplifiedDeno.text.ToString(), IsExplain)));


                NonSimplifiedDeno.color = Color.red;
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "divide" + SpeakerName, IsExplain)));
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, i.ToString(), IsExplain)));
                DenoDivisor.color = Color.red;
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, IsExplain)));


                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (int.Parse(LCMRes) / i).ToString(), IsExplain)));

                TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(LCMRes) / i).ToString(), XPos, YPos, 0, true, 123);

                if((int.Parse(LCMRes) / i) == 1)
                {
                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, IsExplain)));
                    TextInstantiator.InstantiateText(FirstNumPlace, "=", XPos + 150, YPos + 50, 0, true);


                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (finalRes / i).ToString(), IsExplain)));
                    TextInstantiator.InstantiateText(FirstNumPlace, (finalRes / i).ToString(), XPos + 250, YPos+50, 0, true);

                }
                break;
            }
        }

    }

    public IEnumerator MutliPlyNumbers(bool IsFirstNumber, string LCMRes, float YPos)
    {
        int MutlipliedNum = int.Parse(LCMRes);
        if (IsFirstNumber)
            MutlipliedNum /= int.Parse(FDeno.text);
        else
            MutlipliedNum /= int.Parse(SDeno.text);


        if (IsFirstNumber)
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "look at the first denominator" + SpeakerName, IsExplain)));
        else
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "look at the second denominator" + SpeakerName, IsExplain)));

        }

        float XPos = PrimeFactors.XOffset - 680;

        if (IsFirstNumber)
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "multiply the first denominator and the first numerator by" + SpeakerName, IsExplain)));
        else
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "multiply the second denominator and the second numerator by" + SpeakerName, IsExplain)));
            XPos += 400;
        }

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, MutlipliedNum.ToString(), IsExplain)));

        YPos += 100;
        XPos += 100;

        YPos -= 100;
        TextInstantiator.InstantiateText(FirstNumPlace, "× " + (MutlipliedNum).ToString(), XPos, YPos, 0, true, -3);
        YPos -= 100;
        TextInstantiator.InstantiateText(FirstNumPlace, "× " + (MutlipliedNum).ToString(), XPos, YPos, 0, true, -4);
        YPos += 400;

        TextMeshProUGUI Nue;
        TextMeshProUGUI Deno;
        if (IsFirstNumber)
        {
            Nue = GameObject.Find((PrimeFactors.XOffset - 680).ToString()).GetComponent<TextMeshProUGUI>();

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FNue.text.ToString(), IsExplain)));
            Nue.color = Color.red;
            Nue.name = "-99";
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, IsExplain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, MutlipliedNum.ToString(), IsExplain)));
            Deno = GameObject.Find((-3).ToString()).GetComponent<TextMeshProUGUI>();
            Deno.color = Color.red;
            Deno.name = "-99";
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, IsExplain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (MutlipliedNum * int.Parse(FNue.text)).ToString(), IsExplain)));


            XPos = PrimeFactors.XOffset - 680;
            TextInstantiator.InstantiateText(FirstNumPlace, (((MutlipliedNum * int.Parse(FNue.text)).ToString())), XPos, YPos - 600, 0, true, 500);

            YPos -= 50;
            TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos, YPos - 600, 0, true, -99);
            YPos -= 50;


            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FDeno.text.ToString(), IsExplain)));

            Deno = GameObject.Find((PrimeFactors.XOffset - 678).ToString()).GetComponent<TextMeshProUGUI>();
            Deno.color = Color.red;
            Deno.name = "-99";

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, IsExplain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, MutlipliedNum.ToString(), IsExplain)));
            Deno = GameObject.Find((-4).ToString()).GetComponent<TextMeshProUGUI>();
            Deno.color = Color.red;
            Deno.name = "-99";

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, IsExplain)));


            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,
                (MutlipliedNum * int.Parse(FDeno.text)).ToString(), IsExplain)
                ));

            TextInstantiator.InstantiateText(FirstNumPlace, (MutlipliedNum * int.Parse(FDeno.text)).ToString(), XPos, YPos - 600, 0, true);

            XPos += 200;

            YPos += 100;

            TextInstantiator.InstantiateText(FirstNumPlace, "+", XPos, YPos - 650, 0, true, -99);

        }
        else
        {


            Nue = GameObject.Find((PrimeFactors.XOffset - 280).ToString()).GetComponent<TextMeshProUGUI>();
            Nue.name = "-99";

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SNue.text.ToString(), IsExplain)));
            Nue.color = Color.red;
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, IsExplain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, MutlipliedNum.ToString(), IsExplain)));
            Deno = GameObject.Find((-3).ToString()).GetComponent<TextMeshProUGUI>();
            Deno.color = Color.red;
            Deno.name = "-99";

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, IsExplain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (MutlipliedNum * int.Parse(SNue.text)).ToString(), IsExplain)));


            TextInstantiator.InstantiateText(FirstNumPlace, (MutlipliedNum * int.Parse(SNue.text)).ToString(), XPos - 50, YPos - 600, 0, true, 501);
            YPos -= 50;
            TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos - 50, YPos - 600, 0, true, -99);
            YPos -= 50;

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SDeno.text.ToString(), IsExplain)));

            Deno = GameObject.Find((PrimeFactors.XOffset - 277).ToString()).GetComponent<TextMeshProUGUI>();
            Deno.color = Color.red;
            Deno.name = "-99";

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, IsExplain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, MutlipliedNum.ToString(), IsExplain)));
            Deno = GameObject.Find((-4).ToString()).GetComponent<TextMeshProUGUI>();
            Deno.color = Color.red;
            Deno.name = "-99";

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, IsExplain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (MutlipliedNum * int.Parse(SDeno.text)).ToString(), IsExplain)));

            TextInstantiator.InstantiateText(FirstNumPlace, (MutlipliedNum * int.Parse(SDeno.text)).ToString(), XPos - 50, YPos - 600, 0, true, (int)XPos + 3);

        }

    }

    public IEnumerator WriteTwoNumbers(float YPos, string FNumNue, string FNumDeno, string SNumNue, string SNumDeno)
    {
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "now write the first number" + SpeakerName, IsExplain)));

        float XPos = PrimeFactors.XOffset - 680;

        TextInstantiator.InstantiateText(FirstNumPlace, (FNumNue), XPos, YPos, 0, true, (int)XPos);

        YPos -= 50;
        TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos, YPos, 0, true, -99);
        YPos -= 50;
        TextInstantiator.InstantiateText(FirstNumPlace, (FNumDeno), XPos, YPos, 0, true, (int)XPos + 2);

        XPos += 200;

        YPos += 100;

        TextInstantiator.InstantiateText(FirstNumPlace, "+", XPos, YPos - 50, 0, true, -99);

        XPos += 200;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then write the second number" + SpeakerName, IsExplain)));


        try
        {
            TextMeshProUGUI FNueRes = GameObject.Find(((int)XPos).ToString()).GetComponent<TextMeshProUGUI>();
            FNueRes.name = "-99";
        }
        catch (Exception e)
        {

            Debug.Log(e);
        }
        TextInstantiator.InstantiateText(FirstNumPlace, (SNumNue), XPos, YPos, 0, true, (int)XPos);
        YPos -= 50;
        TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos, YPos, 0, true, -99);
        YPos -= 50;
        TextInstantiator.InstantiateText(FirstNumPlace, (SNumDeno), XPos, YPos, 0, true, (int)XPos + 3);
    }

    public static char ValidateInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar) & text.Length < 2)
        {
            return addedChar;
        }
        return '\0';
    }
}
