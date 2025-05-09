using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SolvingTwoEqs : MonoBehaviour
{

    [SerializeField] private GameObject ENInfoText;
    [SerializeField] private GameObject ARInfoText;

    [SerializeField] private TextMeshProUGUI FirstNumPlace;


    private TMP_InputField X1;
    private TMP_InputField Y1;
    private TMP_InputField Res1;

    private TMP_InputField X2;
    private TMP_InputField Y2;
    private TMP_InputField Res2;

    private TextMeshProUGUI X1Txt;
    private TextMeshProUGUI Y1Txt;
    private TextMeshProUGUI Res1Txt;

    private TextMeshProUGUI X2Txt;
    private TextMeshProUGUI Y2Txt;
    private TextMeshProUGUI Res2Txt;

    private TextMeshProUGUI FirstMultiplier;
    private TextMeshProUGUI SecondMultiplier;

    [SerializeField] private Button LangButton;

    [SerializeField] private TMP_FontAsset Autmon;
    [SerializeField] private TMP_FontAsset Amari;

    [SerializeField] private Button ExplainBTN;
    [SerializeField] private Button SolveBTN;

    [SerializeField] private TextMeshProUGUI Line;
    [SerializeField] private GameObject DivisionLine;
    [SerializeField] private GameObject Arrow;


    public static string FirstNum;
    public static string SecNum;
    public static string ThirdNum;
    public static string FourthNum;
    public static string FifthNum;
    public static string SixthNum;

    private bool Arab = false;
    private bool Explain;
    private bool InExplain;
    private string SpeakerName;
    private string VoicesPlace;
    private string NumbersPlace;

    List<TMP_InputField> FieldsList;
    private string XSymbol;
    private string YSymbol;

    private float XPos = 0;
    private float XPosSpacing = 1;
    private float YPos = 0;
    private string Y1Sign = "";
    private string Y2Sign = "";
    private bool IsAdding;

    private int FinalYValNue = 0;
    private int FinalYValDeno = 1;

    private int FinalXValNue = 0;
    private int FinalXValDeno = 1;

    private int ResAfterLineFour = 0;
    private Quaternion ArrowRotation;

    private Button PauseBtn;
    private Button ResumeBtn;

    void Start()
    {
        PauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        ResumeBtn = GameObject.Find("Resume").GetComponent<Button>();
        ArrowRotation = Quaternion.Euler(157, 180, 0.5f);
        X1 = GameObject.Find("ENX1").GetComponent<TMP_InputField>();
        X2 = GameObject.Find("ENX2").GetComponent<TMP_InputField>();
        Y1 = GameObject.Find("ENY1").GetComponent<TMP_InputField>();
        Y2 = GameObject.Find("ENY2").GetComponent<TMP_InputField>();
        Res1 = GameObject.Find("ENResult1").GetComponent<TMP_InputField>();
        Res2 = GameObject.Find("ENResult2").GetComponent<TMP_InputField>();

        FieldsList = new List<TMP_InputField> { X1, Y1, Res1, X2, Y2, Res2 };
        PauseBtn.onClick.AddListener(PauseScript.Pause);
        ResumeBtn.onClick.AddListener(PauseScript.Resume);
        LangButton.onClick.AddListener(ChangeLang);
        SolveBTN.onClick.AddListener(SolveProblem);
        ExplainBTN.onClick.AddListener(ExplainProblem);

        if (FirstNum != null)
        {
            X1.text = FirstNum.ToString();
        }
        if (SecNum != null)
        {
            Y2.text = SecNum.ToString();
        }
        if (ThirdNum != null)
        {
            Res1.text = ThirdNum.ToString();
        }
        if (FourthNum != null)
        {
            X2.text = FourthNum.ToString();
        }
        if (FifthNum != null)
        {
            Y2.text = FifthNum.ToString();
        }
        if (SixthNum != null)
        {
            Res2.text = SixthNum.ToString();
        }
        VoicesPlace = "JennySound";
        NumbersPlace = "JennySound/Numbers";
        SpeakerName = "_Jenny_Eng";
        SLStaicFunctions.SpeakerName = SpeakerName;
        XPos = -400;
        YPos = 750;

        XSymbol = "x";
        YSymbol = "y";
    }

    void Update()
    {
        InputFieldsActions.InitializePlaceholders(X1);
        InputFieldsActions.InitializePlaceholders(X2);
        InputFieldsActions.InitializePlaceholders(Y1);
        InputFieldsActions.InitializePlaceholders(Y1);
        InputFieldsActions.InitializePlaceholders(Res1);
        InputFieldsActions.InitializePlaceholders(Res2);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ExplainEnableMent.EnableExplain(ref FieldsList);
        ExplainEnableMent.DisableExplain(Explain);
        PauseScript.ControlPause();
    }


    public void SolveProblem()
    {
        Explain = false;
        StartCoroutine(SolveStepByStep());
    }
    public void ExplainProblem()
    {
        Explain = true;
        StartCoroutine(SolveStepByStep());

    }

    public IEnumerator SolveStepByStep()
    {

        InExplain = true;
        YPos = 750;
        ResetValues.ResetAllValues();
        Arrow.SetActive(false);
        Line.gameObject.SetActive(false);

        yield return StartCoroutine(GetFirstLineSol());
        yield return StartCoroutine(GetSecondLineSol());
        yield return StartCoroutine(GetThirdLineSol());
        yield return StartCoroutine(GetFourthLineSol());
        if (IsAdding)
        {
            yield return StartCoroutine(GetAdditionFifthLineSol());
            yield return StartCoroutine(ByReplacingStep(true));

            yield return StartCoroutine(GetAdditionSixthLineSol());
            yield return StartCoroutine(GetAdditionSeventhLineSol());
            yield return StartCoroutine(GetAdditionEightthLineSol());
            yield return StartCoroutine(GetAdditionNinethLineSol());

        }
        else
        {
            yield return StartCoroutine(GetSubtractionFifthLineSol());
            yield return StartCoroutine(ByReplacingStep(false));
            yield return StartCoroutine(GetSubtractionSixthLineSol());
            yield return StartCoroutine(GetSubtractionSeventhLineSol());
            yield return StartCoroutine(GetSubtractionEightthLineSol());
            yield return StartCoroutine(GetSubtractionNinethLineSol());

        }
        yield return StartCoroutine(GetSolutionSet());
        Explain = false;
        PauseBtn.GetComponent<Image>().enabled = false;
        ResumeBtn.GetComponent<Image>().enabled = false;
    }
    public void ChangeLang()
    {
        if (!Arab)
        {
            Arab = true;
            IntitlizeFields();
            TranslateToAr();
        }
        else
        {
            Arab = false;
            IntitlizeFields();
            TranslateToEng();
        }
    }
    public IEnumerator GetFirstLineSol()
    {
        AdditionVoiceSpeaker.NumPlace = NumbersPlace;
        AdditionVoiceSpeaker.VoiceClipsPlace = VoicesPlace;
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;


        if (int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y1.text)) >= 0)
        {
            Y1Sign = "+";
        }

        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "First" + SpeakerName, Explain));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "write the first equation" + SpeakerName, Explain));

        X1Txt = TextInstantiator.InstantiateText(FirstNumPlace, X1.text + XSymbol, XPos, YPos, 0, false, 9, 0, Arab);

        Y1Txt = TextInstantiator.InstantiateText(FirstNumPlace, Y1Sign + Y1.text + YSymbol,
             XPos + 150 * XPosSpacing
            , YPos, 0, false, 10, 0, Arab);

        TextInstantiator.InstantiateText(FirstNumPlace, "="
            , XPos + 280 * XPosSpacing
            , YPos, 0, false, 11, 0, Arab);

        Res1Txt = TextInstantiator.InstantiateText(FirstNumPlace, Res1.text, XPos + 425 * XPosSpacing, YPos, 0, false, 12, 0, Arab);

        YPos -= 80;
    }
    public IEnumerator GetSecondLineSol()
    {
        if (int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y2.text)) >= 0)
        {
            Y2Sign = "+";
        }

        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "write the second equation" + SpeakerName, Explain));

        X2Txt = TextInstantiator.InstantiateText(FirstNumPlace, X2.text + XSymbol, XPos, YPos, 0, false, 9, 0, Arab);

        Y2Txt = TextInstantiator.InstantiateText(FirstNumPlace, Y2Sign + Y2.text + YSymbol,
             XPos + 150 * XPosSpacing
            , YPos, 0, false, 10, 0, Arab);

        TextInstantiator.InstantiateText(FirstNumPlace, "="
            , XPos + 280 * XPosSpacing
            , YPos, 0, false, 11, 0, Arab);

        Res2Txt = TextInstantiator.InstantiateText(FirstNumPlace, Res2.text, XPos + 425 * XPosSpacing, YPos, 0, false, 12, 0, Arab);


        ColoringScript.ColorAllTextsWith("9", Color.red);
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x under x" + SpeakerName, Explain));
        yield return new WaitForSeconds(1f);
        ColoringScript.ColorAllTextsWith("9", Color.black);

        ColoringScript.ColorAllTextsWith("10", Color.red);
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y under y" + SpeakerName, Explain));
        yield return new WaitForSeconds(1f);
        ColoringScript.ColorAllTextsWith("10", Color.black);

        ColoringScript.ColorAllTextsWith("11", Color.red);
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal under equal" + SpeakerName, Explain));
        yield return new WaitForSeconds(1f);
        ColoringScript.ColorAllTextsWith("11", Color.black);

        ColoringScript.ColorAllTextsWith("12", Color.red);
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "free term under free term" + SpeakerName, Explain));
        yield return new WaitForSeconds(1f);
        ColoringScript.ColorAllTextsWith("12", Color.black);
    }
    public IEnumerator GetThirdLineSol()
    {
        TwoEqsStrategySpecifier TwoEqsStrategySpecifier = new(Y1.text, Y2.text);
        IsAdding = TwoEqsStrategySpecifier.IsAdding();

        ColoringScript.ColorAllTextsWith("10", Color.red);

        if (!IsAdding)
        {
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "since y1 plus y2 not equals zero" + SpeakerName, Explain));
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so use subtraction method" + SpeakerName, Explain));

            ColoringScript.ColorAllTextsWith("10", Color.black);


            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain));


            if (Math.Abs(int.Parse(ArabicEngConverter.ConvertToEngNumbers(X1.text))) != 1 || Math.Abs(int.Parse(ArabicEngConverter.ConvertToEngNumbers(X2.text))) != 1) {

                X2Txt.color = Color.red;
                yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "multiply the first equation by" + SpeakerName, Explain));

                yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain);

                if (!Arab)
                    TextInstantiator.InstantiateText(FirstNumPlace, "×", XPos + 600 * XPosSpacing, YPos, 60, false, -1, 0, Arab);
                else
                {
                    TextInstantiator.InstantiateText(FirstNumPlace, "×", XPos + 600 * XPosSpacing, YPos, 30, false, -1, 0, Arab);

                }
                TextInstantiator.InstantiateText(FirstNumPlace, "(", XPos + 650 * XPosSpacing, YPos, 80, false, -1, 0, Arab);

                FirstMultiplier = TextInstantiator.InstantiateText(FirstNumPlace, X2.text, XPos + 700 * XPosSpacing, YPos, 80, false, -1, 0, Arab);

                TextInstantiator.InstantiateText(FirstNumPlace, ")", XPos + 750 * XPosSpacing, YPos, 80, false, -1, 0, Arab);


                X2Txt.color = Color.black;


                yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain));


                yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "multiply the second equation by" + SpeakerName, Explain));
                X1Txt.color = Color.red;
                yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, X1.text, Explain);


                if (!Arab)
                    TextInstantiator.InstantiateText(FirstNumPlace, "×", XPos + 600 * XPosSpacing, YPos, 0, false, -1, 0, Arab);
                else
                {
                    TextInstantiator.InstantiateText(FirstNumPlace, "×", XPos + 600 * XPosSpacing, YPos, -30, false, -1, 0, Arab);

                }
                TextInstantiator.InstantiateText(FirstNumPlace, "(", XPos + 650 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

                SecondMultiplier = TextInstantiator.InstantiateText(FirstNumPlace, X1.text, XPos + 700 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

                TextInstantiator.InstantiateText(FirstNumPlace, ")", XPos + 750 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

                X1Txt.color = Color.black;
                yield return StartCoroutine(MutliplyTwoEqs());
                X1Txt = GameObject.Find("13").GetComponent<TextMeshProUGUI>();
                Y1Txt = GameObject.Find("14").GetComponent<TextMeshProUGUI>();
                Res1Txt = GameObject.Find("15").GetComponent<TextMeshProUGUI>();

                X2Txt = GameObject.Find("16").GetComponent<TextMeshProUGUI>();
                Y2Txt = GameObject.Find("17").GetComponent<TextMeshProUGUI>();
                Res2Txt = GameObject.Find("18").GetComponent<TextMeshProUGUI>();


            }
        }
        else
        {
            if (int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y1.text)) + int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y2.text)) == 0)
            {
                yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "since y1 plus y2 equals zero" + SpeakerName, Explain));

            }
            ColoringScript.ColorAllTextsWith("10", Color.black);
            Y1Txt.color = Color.black;
            Y2Txt.color = Color.black;

            if (Math.Abs(int.Parse(ArabicEngConverter.ConvertToEngNumbers(X1.text))) != 1 || Math.Abs(int.Parse(ArabicEngConverter.ConvertToEngNumbers(X2.text))) != 1)
            {
                if (int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y2.text)) > int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y1.text)))
                {
                    Y1Txt.color = Color.red;
                    yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain);
                    yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "could be" + SpeakerName, Explain));
                    Y1Txt.color = Color.red;

                    yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain);

                    yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain));
                    yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "multiply the first equation by" + SpeakerName, Explain));
                    yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain);

                    TextInstantiator.InstantiateText(FirstNumPlace, "×", XPos + 600 * XPosSpacing, YPos, 80, false, -1, 0, Arab);


                    TextInstantiator.InstantiateText(FirstNumPlace, "(", XPos + 650 * XPosSpacing, YPos, 80, false, -1, 0, Arab);

                    FirstMultiplier = TextInstantiator.InstantiateText(FirstNumPlace, Y2.text, XPos + 700 * XPosSpacing, YPos, 80, false, -1, 0, Arab);

                    TextInstantiator.InstantiateText(FirstNumPlace, ")", XPos + 750 * XPosSpacing, YPos, 80, false, -1, 0, Arab);
                    Y2Txt.color = Color.black;
                    Y1Txt.color = Color.black;



                }

                else if (int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y2.text)) < int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y1.text)))
                {
                    yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain));

                    Y2Txt.color = Color.red;

                    yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain);
                    yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "could be" + SpeakerName, Explain));
                    Y1Txt.color = Color.red;

                    yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain);
                    yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain));

                    yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "multiply the second equation by" + SpeakerName, Explain));
                    Y1Txt.color = Color.red;
                    yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain);



                    TextInstantiator.InstantiateText(FirstNumPlace, "×", XPos + 600 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

                    TextInstantiator.InstantiateText(FirstNumPlace, "(", XPos + 650 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

                    SecondMultiplier = TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos + 700 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

                    TextInstantiator.InstantiateText(FirstNumPlace, ")", XPos + 750 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

                    Y1Txt.color = Color.black;
                    Y2Txt.color = Color.black;
                }

                yield return StartCoroutine(AdditionMultiplication());
                X1Txt = GameObject.Find("13").GetComponent<TextMeshProUGUI>();
                Y1Txt = GameObject.Find("14").GetComponent<TextMeshProUGUI>();
                Res1Txt = GameObject.Find("15").GetComponent<TextMeshProUGUI>();

                X2Txt = GameObject.Find("16").GetComponent<TextMeshProUGUI>();
                Y2Txt = GameObject.Find("17").GetComponent<TextMeshProUGUI>();
                Res2Txt = GameObject.Find("18").GetComponent<TextMeshProUGUI>();
                yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so use addition method" + SpeakerName, Explain));

            }
        }
        YPos -= 120;

    }
    public IEnumerator GetFourthLineSol()
    {
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "now" + SpeakerName, Explain));
        if (IsAdding)
        {
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "start adding" + SpeakerName, Explain));
            TextInstantiator.InstantiateText(FirstNumPlace, "+", XPos - 50 * XPosSpacing, YPos, 150, false, 0, 0, Arab);

            Line.gameObject.SetActive(true);
            CharacterProbs.CenterInPos(XPos + (170 * XPosSpacing), YPos + 70, ref Line, FirstNumPlace);
            // First Eq
            yield return AddTwoNumbers(X1Txt, X2Txt, XPos, YPos, XSymbol, true, 19);
            yield return AddTwoNumbers(Y1Txt, Y2Txt,
                XPos + 180 * XPosSpacing, YPos, YSymbol, false, 20);

            TextInstantiator.InstantiateText(FirstNumPlace, "="
                , XPos + 300 * XPosSpacing
                , YPos, 0, false, -1, 0, Arab);

            yield return AddTwoNumbers(Res1Txt, Res2Txt,
            XPos + 425 * XPosSpacing, YPos, "", true, 21);
        }
        else
        {
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "start subtracting" + SpeakerName, Explain));
            TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos - 50 * XPosSpacing, YPos, 150, false, 0, 0, Arab);

            Line.gameObject.SetActive(true);
            CharacterProbs.CenterInPos(XPos + (170 * XPosSpacing), YPos + 70, ref Line, FirstNumPlace);


            yield return SubtractingTwoNumbers(X1Txt, X2Txt, XPos, YPos, XSymbol, true, 19);
            yield return SubtractingTwoNumbers(Y1Txt, Y2Txt,
                XPos + 180 * XPosSpacing, YPos, YSymbol, false, 20);

            TextInstantiator.InstantiateText(FirstNumPlace, "="
                , XPos + 300 * XPosSpacing
                , YPos, 0, false, -1, 0, Arab);

            yield return SubtractingTwoNumbers(Res1Txt, Res2Txt,
            XPos + 425 * XPosSpacing, YPos, "", true, 21);
        }
        YPos -= 80;
    }
    public IEnumerator GetSubtractionFifthLineSol()
    {
        FinalYValNue = int.Parse(ArabicEngConverter.ConvertToEngNumbers(ArabicEngConverter.ConvertToEngNumbers(GameObject.Find("20").GetComponent<TextMeshProUGUI>().text)));

        ResAfterLineFour = int.Parse(ArabicEngConverter.ConvertToEngNumbers(GameObject.Find("21").GetComponent<TextMeshProUGUI>().text));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain));
        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, FinalYValNue.ToString(), Explain);
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, ResAfterLineFour.ToString(), Explain);

        Y1Txt = TextInstantiator.InstantiateText(FirstNumPlace, FinalYValNue + YSymbol,
        XPos + 190 * XPosSpacing
        , YPos, 0, false, -1, 0, Arab);

        TextInstantiator.InstantiateText(FirstNumPlace, "="
            , XPos + 305 * XPosSpacing
            , YPos, 0, false, -1, 0, Arab);

        Res1Txt = TextInstantiator.InstantiateText(FirstNumPlace, ResAfterLineFour.ToString(), XPos + 440 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "now" + SpeakerName, Explain));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "divide both sides by" + SpeakerName, Explain));

        Y1Txt.color = Color.red;
        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, FinalYValNue.ToString(), Explain);


        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));


        TextInstantiator.InstantiateText(FirstNumPlace, YSymbol + " = "
        , XPos + 220 * XPosSpacing
        , YPos, -100, false, -1, 0, Arab);

        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, ResAfterLineFour, int.Parse(ArabicEngConverter.ConvertToEngNumbers(FinalYValNue.ToString())), XPos + 400 * XPosSpacing, YPos - 100, 22, Explain, true, Arab);
        FinalYValDeno = ResAfterLineFour;

        (FinalYValDeno, FinalYValNue) = (FinalYValNue, FinalYValDeno);

        int gcf = MathTricks.GetGCF(FinalYValNue, FinalYValDeno);

        FinalYValNue /= gcf;
        FinalYValDeno /= gcf;

        if (gcf != 1)
        {
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));


            TextInstantiator.InstantiateText(FirstNumPlace, " = "
            , XPos + 500 * XPosSpacing
            , YPos, -100, false, -1, 0, Arab);

            yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, int.Parse(ArabicEngConverter.ConvertToEngNumbers(FinalYValNue.ToString())), int.Parse(ArabicEngConverter.ConvertToEngNumbers(FinalYValDeno.ToString())), XPos + 600 * XPosSpacing, YPos - 100, 22, Explain, true, Arab);

        }
        if (FinalYValNue % FinalYValDeno != 0)
        {
            YPos -= 270;
        }
        else
            YPos -= 200;
    }

    public IEnumerator ByReplacingStep(bool IsX)
    {
        TextMeshProUGUI SecEqX = FindingScript.GetFirstGameObjectWithName("9").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI SecEqY = FindingScript.GetFirstGameObjectWithName("10").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI SecEqEqual = FindingScript.GetFirstGameObjectWithName("11").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI SecEqRes = FindingScript.GetFirstGameObjectWithName("12").GetComponent<TextMeshProUGUI>();

        yield return SLStaicFunctions.PlayByAddress(this, "by replacing" + SpeakerName, Explain);
        if (!IsX)
            yield return SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain);
        else
        {
            yield return SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain);
        }
        yield return SLStaicFunctions.PlayByAddress(this, "in the second equation" + SpeakerName, Explain);

        SecEqX.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        SecEqX.color = Color.black;

        SecEqY.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        SecEqY.color = Color.black;

        SecEqEqual.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        SecEqEqual.color = Color.black;

        SecEqRes.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        SecEqRes.color = Color.black;
    }
    public IEnumerator GetSubtractionSixthLineSol()
    {
        yield return SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain);

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain);

        yield return SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain);


        X2Txt = TextInstantiator.InstantiateText(FirstNumPlace, X2.text + XSymbol, XPos, YPos, 0, false, -1, 0, Arab);

        if (Y2Sign.Length == 1)
        {
            yield return SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain);

        }
        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain);

        yield return SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain);

        Y2Txt = TextInstantiator.InstantiateText(FirstNumPlace, Y2Sign + Y2.text + " × ",
             XPos + 150 * XPosSpacing
            , YPos, 0, false, 23, 0, Arab);

        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalYValNue, FinalYValDeno, XPos + 300 * XPosSpacing, YPos, 24, Explain, true, Arab);

        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);

        TextInstantiator.InstantiateText(FirstNumPlace, "="
            , XPos + 400 * XPosSpacing
            , YPos, 0, false, -1, 0, Arab);


        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Res2.text, Explain);

        Res2Txt = TextInstantiator.InstantiateText(FirstNumPlace, Res2.text, XPos + 455 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

        if (FinalYValNue % FinalYValDeno != 0)
        {
            YPos -= 200;
        }
        else
            YPos -= 120;
    }
    public IEnumerator GetSubtractionSeventhLineSol()
    {

        yield return SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain);

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain);

        GameObject.Find("23").GetComponent<TextMeshProUGUI>().color = Color.red;

        yield return SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain);

        yield return SLStaicFunctions.PlayFraction(this, FinalYValNue.ToString(), FinalYValDeno.ToString(), Explain);

        try
        {
            GameObject.Find("576").GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        GameObject.Find("24").GetComponent<TextMeshProUGUI>().color = Color.red;


        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);

        FinalXValNue = FinalYValNue;
        FinalXValDeno = FinalYValDeno;

        FractionCalculator.MultiplyFractionByInteger(ref FinalXValNue, ref FinalXValDeno, int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y2.text)));


        yield return SLStaicFunctions.PlayFraction(this, FinalXValNue.ToString(), FinalXValDeno.ToString(), Explain);


        if ((float)FinalXValNue / FinalXValDeno >= 0)
        {
            Y2Sign = "+";
        }
        else
        {
            Y2Sign = "";
        }

        X2Txt = TextInstantiator.InstantiateText(FirstNumPlace, X2.text + XSymbol + " " + Y2Sign, XPos + 15 * XPosSpacing, YPos, 0, false, -1, 0, Arab);


        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalXValNue, FinalXValDeno, XPos + 170 * XPosSpacing, YPos, 25, Explain, true, Arab);


        Res2Txt = TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 270 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

        Res2Txt = TextInstantiator.InstantiateText(FirstNumPlace, Res2.text, XPos + 320 * XPosSpacing, YPos, 0, false, 26, 0, Arab);


        yield return SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayByAddress(this, "send" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayFraction(this, FinalXValNue.ToString(), FinalYValDeno.ToString(), Explain);


        try
        {
            GameObject.Find("625").GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        GameObject.Find("25").GetComponent<TextMeshProUGUI>().color = Color.red;

        yield return SLStaicFunctions.PlayByAddress(this, "to the right side with a negative sign" + SpeakerName, Explain);
        FinalXValNue = -FinalXValNue;

        Arrow.SetActive(true);
        if (!Arab)
        {
            CharacterProbs.CenterInPos(1 * XPosSpacing, YPos + 50, ref Arrow, FirstNumPlace);
        }
        else
        {
            CharacterProbs.CenterInPos(45 * XPosSpacing, YPos + 30, ref Arrow, FirstNumPlace);
        }
        Arrow.transform.rotation = ArrowRotation;

        if (FinalXValNue >= 0 && FinalXValDeno >= 0)
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "+", XPos + 550 * XPosSpacing, YPos, 0, false, -1, 0, Arab);
            yield return SLStaicFunctions.PlayByAddress(this, "becomes" + SpeakerName, Explain);

            yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalXValNue, FinalXValDeno, XPos + 650 * XPosSpacing, YPos, 27, Explain, true, Arab);

        }
        else
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 550 * XPosSpacing, YPos, 0, false, -1, 0, Arab);
            yield return SLStaicFunctions.PlayByAddress(this, "becomes" + SpeakerName, Explain);

            yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, Math.Abs(FinalXValNue), Math.Abs(FinalXValDeno), XPos + 650 * XPosSpacing, YPos, 27, Explain, true, Arab);

        }


        if (FinalXValNue % FinalXValDeno != 0)
        {
            YPos -= 200;
        }
        else
            YPos -= 120;
    }
    public IEnumerator GetSubtractionEightthLineSol()
    {

        yield return SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain);
        yield return SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);

        X2Txt = TextInstantiator.InstantiateText(FirstNumPlace, X2.text + XSymbol, XPos, YPos, 0, false, 28, 0, Arab);


        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Res2Txt.text, Explain);
        Res2Txt = GameObject.Find("26").GetComponent<TextMeshProUGUI>();
        Res2Txt.color = Color.red;

        if ((float)FinalXValNue / FinalXValDeno >= 0)
        {
            yield return SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain);

        }
        else
        {
            FinalXValNue = Math.Abs(FinalXValNue);
            FinalXValDeno = Math.Abs(FinalXValDeno);
            yield return SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain);

        }


        yield return SLStaicFunctions.PlayFraction(this, FinalXValNue.ToString(), FinalXValDeno.ToString(), Explain);
        GameObject.Find("27").GetComponent<TextMeshProUGUI>().color = Color.red;
        try
        {
            GameObject.Find("729").GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        catch (Exception e)
        {

            Debug.Log(e.Message);
        }

        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);
        FractionCalculator.SubtractFractionFromNumber(int.Parse(ArabicEngConverter.ConvertToEngNumbers(Res2Txt.text)), ref FinalXValNue, ref FinalXValDeno);

        X1Txt = TextInstantiator.InstantiateText(FirstNumPlace, "=", XPos + 120 * XPosSpacing, YPos, 0, false, 28, 0, Arab);

        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalXValNue, FinalXValDeno, XPos + 220 * XPosSpacing, YPos, 29, Explain, true, Arab);

        if (FinalXValNue % FinalXValDeno != 0)
        {
            YPos -= 200;
        }
        else
            YPos -= 120;

    }
    public IEnumerator GetSubtractionNinethLineSol()
    {

        yield return SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);
        TextInstantiator.InstantiateText(FirstNumPlace, XSymbol + " = ", XPos, YPos, 0, false, 28, 0, Arab);
        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalXValNue, FinalXValDeno, XPos + 120 * XPosSpacing, YPos, 29, Explain, true, Arab);
        yield return SLStaicFunctions.PlayByAddress(this, "divide" + SpeakerName, Explain);
        TextInstantiator.InstantiateText(FirstNumPlace, "÷", XPos + 250 * XPosSpacing, YPos, 0, false, 28, 0, Arab);
        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain);
        TextInstantiator.InstantiateText(FirstNumPlace, ArabicEngConverter.ConvertToEngNumbers(X2.text), XPos + 300 * XPosSpacing, YPos, 0, false, 28, 0, Arab);

        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);
        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 350 * XPosSpacing, YPos, 0, false, 28, 0, Arab);

        FinalXValDeno *= int.Parse(X2.text);

        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalXValNue, FinalXValDeno, XPos + 450 * XPosSpacing, YPos, 29, Explain, true, Arab);

        int gcf = MathTricks.GetGCF(FinalXValNue, FinalXValDeno);

        FinalXValNue /= gcf;
        FinalXValDeno /= gcf;

        if (gcf != 1)
        {
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));


            TextInstantiator.InstantiateText(FirstNumPlace, " = "
            , XPos + 550 * XPosSpacing
            , YPos, 0, false, -1, 0, Arab);

            yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, int.Parse(ArabicEngConverter.ConvertToEngNumbers(FinalXValNue.ToString())), int.Parse(ArabicEngConverter.ConvertToEngNumbers(FinalXValDeno.ToString())), XPos + 650 * XPosSpacing, YPos, 22, Explain, true, Arab);

        }
        YPos -= 270;
    }


    public IEnumerator GetSolutionSet()
    {
        yield return SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayByAddress(this, "the solution set is" + SpeakerName, Explain);

        if (Arab)
            TextInstantiator.InstantiateText(FirstNumPlace, "م.ح", XPos, YPos, 0, false, -1, 0, Arab);
        else
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "S.S", XPos, YPos, 0, false, -1, 0, Arab);

        }
        TextInstantiator.InstantiateText(FirstNumPlace, "{(", XPos + 100 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalXValNue, FinalXValDeno, XPos + 200 * XPosSpacing, YPos, -1, Explain, true, Arab);

        yield return SLStaicFunctions.PlayByAddress(this, "and" + SpeakerName, Explain);

        TextInstantiator.InstantiateText(FirstNumPlace, ",", XPos + 300 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalYValNue, FinalYValDeno, XPos + 400 * XPosSpacing, YPos, -1, Explain, true, Arab);

        TextInstantiator.InstantiateText(FirstNumPlace, ")}", XPos + 500 * XPosSpacing, YPos, 0, false, -1, 0, Arab);


    }


    public IEnumerator GetAdditionFifthLineSol()
    {
        FinalXValNue = int.Parse(ArabicEngConverter.ConvertToEngNumbers(ArabicEngConverter.ConvertToEngNumbers(GameObject.Find("19").GetComponent<TextMeshProUGUI>().text)));

        ResAfterLineFour = int.Parse(ArabicEngConverter.ConvertToEngNumbers(GameObject.Find("21").GetComponent<TextMeshProUGUI>().text));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain));
        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, FinalXValNue.ToString(), Explain);
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, ResAfterLineFour.ToString(), Explain);

        Y1Txt = TextInstantiator.InstantiateText(FirstNumPlace, FinalXValNue + XSymbol,
        XPos
        , YPos, 0, false, -1, 0, Arab);

        TextInstantiator.InstantiateText(FirstNumPlace, "="
            , XPos + 100 * XPosSpacing
            , YPos, 0, false, -1, 0, Arab);

        Res1Txt = TextInstantiator.InstantiateText(FirstNumPlace, ResAfterLineFour.ToString(), XPos + 200 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "now" + SpeakerName, Explain));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "divide both sides by" + SpeakerName, Explain));

        Y1Txt.color = Color.red;
        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, FinalXValNue.ToString(), Explain);


        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain));
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));


        TextInstantiator.InstantiateText(FirstNumPlace, XSymbol + " = "
        , XPos
        , YPos, -100, false, -1, 0, Arab);

        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, ResAfterLineFour, int.Parse(ArabicEngConverter.ConvertToEngNumbers(FinalXValNue.ToString())), XPos + 180 * XPosSpacing, YPos - 100, 22, Explain, true, Arab);
        FinalXValDeno = ResAfterLineFour;

        (FinalXValDeno, FinalXValNue) = (FinalXValNue, FinalXValDeno);

        if (FinalXValNue % FinalXValDeno != 0)
        {
            YPos -= 270;
        }
        else
            YPos -= 200;
    }

    public IEnumerator GetAdditionSixthLineSol()
    {
        yield return SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain);

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain);


        yield return SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain);

        X2Txt = TextInstantiator.InstantiateText(FirstNumPlace, X2.text + " × ",
             XPos
            , YPos, 0, false, 23, 0, Arab);


        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalXValNue, FinalXValDeno, XPos + 150 * XPosSpacing, YPos, 24, Explain, true, Arab);


        if (Y2Sign.Length == 1)
        {
            yield return SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain);

        }
        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain);

        yield return SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain);


        Y2Txt = TextInstantiator.InstantiateText(FirstNumPlace, Y2Sign + Y2.text + YSymbol, XPos + 300 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);

        TextInstantiator.InstantiateText(FirstNumPlace, "="
            , XPos + 400 * XPosSpacing
            , YPos, 0, false, -1, 0, Arab);


        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Res2.text, Explain);

        Res2Txt = TextInstantiator.InstantiateText(FirstNumPlace, Res2.text, XPos + 455 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

        if (FinalXValNue % FinalXValDeno != 0)
        {
            YPos -= 200;
        }
        else
            YPos -= 150;
    }

    public IEnumerator GetAdditionSeventhLineSol()
    {

        yield return SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain);

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain);

        GameObject.Find("23").GetComponent<TextMeshProUGUI>().color = Color.red;

        yield return SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain);

        yield return SLStaicFunctions.PlayFraction(this, FinalXValNue.ToString(), FinalXValDeno.ToString(), Explain);

        try
        {
            GameObject.Find("576").GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        GameObject.Find("24").GetComponent<TextMeshProUGUI>().color = Color.red;


        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);

        FinalYValNue = FinalXValNue;
        FinalYValDeno = FinalXValDeno;
        FractionCalculator.MultiplyFractionByInteger(ref FinalYValNue, ref FinalYValDeno, int.Parse(ArabicEngConverter.ConvertToEngNumbers(X2.text)));


        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalYValNue, FinalYValDeno, XPos, YPos, 25, Explain, true, Arab);

        Y2Txt = TextInstantiator.InstantiateText(FirstNumPlace, Y2Sign + Y2.text + YSymbol, XPos + 150 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

        Res2Txt = TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 250 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

        Res2Txt = TextInstantiator.InstantiateText(FirstNumPlace, Res2.text, XPos + 350 * XPosSpacing, YPos, 0, false, 26, 0, Arab);


        yield return SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayByAddress(this, "send" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayFraction(this, FinalYValNue.ToString(), FinalYValDeno.ToString(), Explain);


        try
        {
            GameObject.Find("625").GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        GameObject.Find("25").GetComponent<TextMeshProUGUI>().color = Color.red;

        yield return SLStaicFunctions.PlayByAddress(this, "to the right side with a negative sign" + SpeakerName, Explain);
        FinalYValNue = -FinalYValNue;

        Arrow.SetActive(true);

        CharacterProbs.CenterInPos(XPos + 245 * XPosSpacing, YPos + 35, ref Arrow, FirstNumPlace);

        Arrow.transform.rotation = ArrowRotation;

        TextInstantiator.InstantiateText(FirstNumPlace, "+", XPos + 420 * XPosSpacing, YPos, 0, false, -1, 0, Arab);

        yield return SLStaicFunctions.PlayByAddress(this, "becomes" + SpeakerName, Explain);

        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalYValNue, FinalYValDeno, XPos + 500 * XPosSpacing, YPos, 27, Explain, true, Arab);


        if (FinalYValNue % FinalYValDeno != 0)
        {
            YPos -= 200;
        }
        else
            YPos -= 150;
    }

    public IEnumerator GetAdditionEightthLineSol()
    {

        yield return SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain);
        yield return SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);

        X2Txt = TextInstantiator.InstantiateText(FirstNumPlace, Y2.text + YSymbol, XPos, YPos, 0, false, 28, 0, Arab);


        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Res2Txt.text, Explain);
        Res2Txt = GameObject.Find("26").GetComponent<TextMeshProUGUI>();
        Res2Txt.color = Color.red;

        if ((float)FinalYValNue / FinalYValDeno >= 0)
        {
            yield return SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain);

        }
        else
        {
            FinalYValNue = Math.Abs(FinalYValNue);
            FinalYValDeno = Math.Abs(FinalYValDeno);
            yield return SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain);

        }


        yield return SLStaicFunctions.PlayFraction(this, FinalYValNue.ToString(), FinalYValDeno.ToString(), Explain);
        GameObject.Find("27").GetComponent<TextMeshProUGUI>().color = Color.red;
        try
        {
            GameObject.Find("729").GetComponent<TextMeshProUGUI>().color = Color.red;

        }
        catch (Exception e)
        {

            Debug.Log(e.Message);
        }

        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);
        FractionCalculator.SubtractFractionFromNumber(int.Parse(ArabicEngConverter.ConvertToEngNumbers(Res2Txt.text)), ref FinalYValNue, ref FinalYValDeno);

        X1Txt = TextInstantiator.InstantiateText(FirstNumPlace, "=", XPos + 150 * XPosSpacing, YPos, 0, false, 28, 0, Arab);

        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalYValNue, FinalYValDeno, XPos + 250 * XPosSpacing, YPos, 29, Explain, true, Arab);

        if (FinalYValNue % FinalYValDeno != 0)
        {
            YPos -= 200;
        }
        else
            YPos -= 150;

    }

    public IEnumerator GetAdditionNinethLineSol()
    {

        yield return SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain);
        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);

        if(!Arab)
            TextInstantiator.InstantiateText(FirstNumPlace, YSymbol + " = ", XPos, YPos, 0, false, 28, 0, Arab);
        else
            TextInstantiator.InstantiateText(FirstNumPlace, YSymbol + " = ", XPos, YPos, -30, false, 28, 0, Arab);

        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalYValNue, FinalYValDeno, XPos + 120 * XPosSpacing, YPos, 29, Explain, true, Arab);
        yield return SLStaicFunctions.PlayByAddress(this, "divide" + SpeakerName, Explain);

        if(!Arab)
            TextInstantiator.InstantiateText(FirstNumPlace, "÷", XPos + 230 * XPosSpacing, YPos, 0, false, 28, 0, Arab);
        else
            TextInstantiator.InstantiateText(FirstNumPlace, "÷", XPos + 230 * XPosSpacing, YPos, -30, false, 28, 0, Arab);

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain);
        TextInstantiator.InstantiateText(FirstNumPlace, ArabicEngConverter.ConvertToEngNumbers(Y2.text), XPos + 300 * XPosSpacing, YPos, 0, false, 28, 0, Arab);

        yield return SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain);


        if (!Arab)
            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 350 * XPosSpacing, YPos, 0, false, 28, 0, Arab);
        else
            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 350 * XPosSpacing, YPos, -30, false, 28, 0, Arab);


        FinalYValDeno *= int.Parse(Y2.text);

        yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, FinalYValNue, FinalYValDeno, XPos + 450 * XPosSpacing, YPos, 29, Explain, true, Arab);

        int gcf = MathTricks.GetGCF(FinalYValNue, FinalYValDeno);

        FinalYValNue /= gcf;
        FinalYValDeno /= gcf;

        if (gcf != 1)
        {
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));


            TextInstantiator.InstantiateText(FirstNumPlace, " = "
            , XPos + 550 * XPosSpacing
            , YPos, 0, false, -1, 0, Arab);

            yield return SLStaicFunctions.WriteFraction(this, FirstNumPlace, DivisionLine, Explain, int.Parse(ArabicEngConverter.ConvertToEngNumbers(FinalYValNue.ToString())), int.Parse(ArabicEngConverter.ConvertToEngNumbers(FinalYValDeno.ToString())), XPos + 650 * XPosSpacing, YPos, 22, Explain, true, Arab);

        }
        YPos -= 150;

    }

    public IEnumerator MutliplyTwoEqs()
    {
        YPos -= 120;

        // First Eq
        yield return MultiplyTwoNumbers(FirstMultiplier,X1Txt,XPos, YPos, XSymbol,true,13);
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain));

        yield return MultiplyTwoNumbers(FirstMultiplier,Y1Txt,
            XPos + 150 * XPosSpacing , YPos, YSymbol , false , 14);
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain));

        TextInstantiator.InstantiateText(FirstNumPlace, "="
            , XPos + 280 * XPosSpacing 
            , YPos, 0, false, -1, 0, Arab);

        yield return MultiplyTwoNumbers(FirstMultiplier, Res1Txt,
        XPos + 425 * XPosSpacing , YPos , "" , true , 15);
        YPos-=80;

        //SecEq
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain));

        yield return MultiplyTwoNumbers(SecondMultiplier, X2Txt, XPos, YPos, XSymbol , true , 16);
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain));

        yield return MultiplyTwoNumbers(SecondMultiplier, Y2Txt, XPos + 150 * XPosSpacing , YPos, YSymbol, false , 17);
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain));

        TextInstantiator.InstantiateText(FirstNumPlace, "="
            , XPos + 230 * XPosSpacing + XPosSpacing  * 50 
            , YPos, 0, false, -1, 0, Arab);

        yield return MultiplyTwoNumbers(SecondMultiplier, Res2Txt,
        XPos + 425 * XPosSpacing , YPos, "" , true,18);
    }

    public IEnumerator AdditionMultiplication()
    {
        YPos -= 120;

        // First Eq
        if(int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y2.text)) % int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y1.text)) == 0){
            yield return MultiplyTwoNumbers(FirstMultiplier, X1Txt, XPos, YPos, XSymbol, true, 13);
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain));

            yield return MultiplyTwoNumbers(FirstMultiplier, Y1Txt,
                XPos + 150 * XPosSpacing, YPos, YSymbol, false, 14);
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain));

            TextInstantiator.InstantiateText(FirstNumPlace, "="
                , XPos + 280 * XPosSpacing
                , YPos, 0, false, -1, 0, Arab);

            yield return MultiplyTwoNumbers(FirstMultiplier, Res1Txt,
            XPos + 425 * XPosSpacing, YPos, "", true, 15);
            YPos -= 80;

            TextInstantiator.InstantiateText(FirstNumPlace, X2.text + XSymbol, XPos, YPos, 0, false, 16, 0, Arab);

            TextInstantiator.InstantiateText(FirstNumPlace, Y2Sign + Y2.text + YSymbol,
                 XPos + 150 * XPosSpacing
                , YPos, 0, false, 17, 0, Arab);

            TextInstantiator.InstantiateText(FirstNumPlace, "="
                , XPos + 280 * XPosSpacing
                , YPos, 0, false, -1, 0, Arab);

            TextInstantiator.InstantiateText(FirstNumPlace, Res2.text, XPos + 425 * XPosSpacing, YPos, 0, false, 18, 0, Arab);
        }


        //SecEq
        else if (int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y1.text)) % int.Parse(ArabicEngConverter.ConvertToEngNumbers(Y2.text)) == 0)
        {
            TextInstantiator.InstantiateText(FirstNumPlace, X1.text + XSymbol, XPos, YPos, 0, false, 13, 0, Arab);

            TextInstantiator.InstantiateText(FirstNumPlace, Y1Sign + Y1.text + YSymbol,
                 XPos + 150 * XPosSpacing
                , YPos, 0, false, 14, 0, Arab);

            TextInstantiator.InstantiateText(FirstNumPlace, "="
                , XPos + 280 * XPosSpacing
                , YPos, 0, false, -1, 0, Arab);

            TextInstantiator.InstantiateText(FirstNumPlace, Res1.text, XPos + 425 * XPosSpacing, YPos, 0, false, 15, 0, Arab);

            YPos -= 80;
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain));

            yield return MultiplyTwoNumbers(SecondMultiplier, X2Txt, XPos, YPos, XSymbol, true, 16);
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain));

            yield return MultiplyTwoNumbers(SecondMultiplier, Y2Txt, XPos + 150 * XPosSpacing, YPos, YSymbol, false, 17);
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain));

            TextInstantiator.InstantiateText(FirstNumPlace, "="
                , XPos + 230 * XPosSpacing + XPosSpacing * 50
                , YPos, 0, false, -1, 0, Arab);

            yield return MultiplyTwoNumbers(SecondMultiplier, Res2Txt,
            XPos + 425 * XPosSpacing, YPos, "", true, 18);

        }

    }
    public IEnumerator MultiplyTwoNumbers(TextMeshProUGUI FNum, TextMeshProUGUI SNum , float NewXpos , float NewYPos , string Symbol, bool IsFirst , int counter)
    {

        string FirstMultiplier = PrepareMultiplier(FNum);
        string SecMultiplier = PrepareMultiplier(SNum);

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, FirstMultiplier, Explain);
        FNum.color = Color.red;
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain));

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, SecMultiplier, Explain);
        SNum.color = Color.red;

        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, (int.Parse(ArabicEngConverter.ConvertToEngNumbers(FirstMultiplier)) * int.Parse(ArabicEngConverter.ConvertToEngNumbers(SecMultiplier))).ToString(), Explain);

        if(!IsFirst && (int.Parse(ArabicEngConverter.ConvertToEngNumbers(FirstMultiplier)) * int.Parse(ArabicEngConverter.ConvertToEngNumbers(SecMultiplier)) >= 0))
        {
            if (!Arab)
                TextInstantiator.InstantiateText(FirstNumPlace, "+", NewXpos - 50 * XPosSpacing, NewYPos, 0, false, 0, 0, Arab);
            else
                TextInstantiator.InstantiateText(FirstNumPlace, "+", NewXpos - 50 * XPosSpacing, NewYPos, -20, false, 0, 0, Arab);
        }
        if (!Arab)
        {
            TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(ArabicEngConverter.ConvertToEngNumbers(FirstMultiplier)) * int.Parse(ArabicEngConverter.ConvertToEngNumbers(SecMultiplier))).ToString(), NewXpos, NewYPos, 0, false, counter, 0, Arab);

        }
        else
        {
            TextInstantiator.InstantiateText(FirstNumPlace, ArabicEngConverter.ReverseString((int.Parse(ArabicEngConverter.ConvertToEngNumbers(FirstMultiplier)) * int.Parse(ArabicEngConverter.ConvertToEngNumbers(SecMultiplier))).ToString()), NewXpos, NewYPos, 0, false, counter, 0, Arab);

        }


        TextInstantiator.InstantiateText(FirstNumPlace, Symbol, NewXpos + (int.Parse(ArabicEngConverter.ConvertToEngNumbers(FirstMultiplier)) * int.Parse(ArabicEngConverter.ConvertToEngNumbers(SecMultiplier))).ToString().Length * 10 * XPosSpacing + 40 * XPosSpacing, NewYPos, 0, false, 0, 0, Arab);

        SNum.color = Color.black;
        FNum.color = Color.black;
    }    
    public IEnumerator AddTwoNumbers(TextMeshProUGUI FNum, TextMeshProUGUI SNum , float NewXpos , float NewYPos , string Symbol, bool IsFirst , int counter)
    {

        string FirstMultiplier = PrepareMultiplier(FNum);
        string SecMultiplier = PrepareMultiplier(SNum);

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, FirstMultiplier, Explain);
        FNum.color = Color.red;
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain));

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, SecMultiplier, Explain);
        SNum.color = Color.red;

        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, (int.Parse(ArabicEngConverter.ConvertToEngNumbers(FirstMultiplier)) + int.Parse(ArabicEngConverter.ConvertToEngNumbers(SecMultiplier))).ToString(), Explain);

        if ((int.Parse(ArabicEngConverter.ConvertToEngNumbers(FirstMultiplier)) + int.Parse(ArabicEngConverter.ConvertToEngNumbers(SecMultiplier)) !=0))
        {
            if (!IsFirst && (int.Parse(ArabicEngConverter.ConvertToEngNumbers(FirstMultiplier)) + int.Parse(ArabicEngConverter.ConvertToEngNumbers(SecMultiplier)) >= 0))
            {
                if (!Arab)
                    TextInstantiator.InstantiateText(FirstNumPlace, "+", NewXpos - 50 * XPosSpacing, NewYPos, 0, false, 0, 0, Arab);
                else
                    TextInstantiator.InstantiateText(FirstNumPlace, "+", NewXpos - 50 * XPosSpacing, NewYPos, -20, false, 0, 0, Arab);
            }
            TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(ArabicEngConverter.ConvertToEngNumbers(FirstMultiplier)) + int.Parse(ArabicEngConverter.ConvertToEngNumbers(SecMultiplier))).ToString(), NewXpos, NewYPos, 0, false, counter, 0, Arab);

            TextInstantiator.InstantiateText(FirstNumPlace, Symbol, NewXpos + (int.Parse(ArabicEngConverter.ConvertToEngNumbers(FirstMultiplier)) + int.Parse(ArabicEngConverter.ConvertToEngNumbers(SecMultiplier))).ToString().Length * 10 * XPosSpacing + 40 * XPosSpacing, NewYPos, 0, false, 0, 0, Arab);
        }

        SNum.color = Color.black;
        FNum.color = Color.black;
    }   
    public IEnumerator SubtractingTwoNumbers(TextMeshProUGUI FNum, TextMeshProUGUI SNum , float NewXpos , float NewYPos , string Symbol, bool IsFirst , int counter)
    {

        int FirstMultiplier = int.Parse(ArabicEngConverter.ConvertToEngNumbers(FNum.text));
        int SecMultiplier = int.Parse(ArabicEngConverter.ConvertToEngNumbers(SNum.text));
        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, FirstMultiplier.ToString(), Explain);
        FNum.color = Color.red;
        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain));

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, SecMultiplier.ToString(), Explain);
        SNum.color = Color.red;

        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain));

        yield return SLStaicFunctions.PlayVoiceNumberAndWait(this, (FirstMultiplier - SecMultiplier).ToString(), Explain);

        if((FirstMultiplier - SecMultiplier) != 0)
        {

            if (!IsFirst && (FirstMultiplier - SecMultiplier) >= 0)
            {
                if (!Arab)
                    TextInstantiator.InstantiateText(FirstNumPlace, "+", NewXpos - 50 * XPosSpacing, NewYPos, 0, false, 0, 0, Arab);
                else
                    TextInstantiator.InstantiateText(FirstNumPlace, "+", NewXpos - 50 * XPosSpacing, NewYPos, -20, false, 0, 0, Arab);
            }
            TextInstantiator.InstantiateText(FirstNumPlace, (FirstMultiplier - SecMultiplier).ToString(), NewXpos, NewYPos, 0, false, counter, 0, Arab);

            TextInstantiator.InstantiateText(
                FirstNumPlace,
                Symbol,
                NewXpos + ((FirstMultiplier - SecMultiplier).ToString().Length * 10 * XPosSpacing) + (40 * XPosSpacing),
                NewYPos,
                0,
                false,
                0,
                0,
                Arab
            );

        }
        SNum.color = Color.black;
        FNum.color = Color.black;
    }
    public string PrepareMultiplier(TextMeshProUGUI Multiplier)
    {
        string PreparedMultiplier = ArabicEngConverter.ConvertToEngNumbers(Multiplier.text.Trim().TrimEnd());
        if (Arab)
        {
            PreparedMultiplier = new string(PreparedMultiplier.Reverse().ToArray());
        }
        if (!int.TryParse(PreparedMultiplier[PreparedMultiplier.Length - 1].ToString(), out _))
        {
            PreparedMultiplier = PreparedMultiplier[..^1];
        }
        if (PreparedMultiplier[0] == '+')
        {
            PreparedMultiplier = PreparedMultiplier[1..];
            PreparedMultiplier = PreparedMultiplier.Trim().TrimEnd();
        }
        return PreparedMultiplier;
    }
    public void IntitlizeFields()
    {
        if (!Arab)
        {
            ENInfoText.gameObject.SetActive(true);
            ARInfoText.gameObject.SetActive(false);
            X1 = GameObject.Find("ENX1").GetComponent<TMP_InputField>();
            X2 = GameObject.Find("ENX2").GetComponent<TMP_InputField>();
            Y1 = GameObject.Find("ENY1").GetComponent<TMP_InputField>();
            Y2 = GameObject.Find("ENY2").GetComponent<TMP_InputField>();
            Res1 = GameObject.Find("ENResult1").GetComponent<TMP_InputField>();
            Res2 = GameObject.Find("ENResult2").GetComponent<TMP_InputField>();
        }
        else
        {
            ENInfoText.gameObject.SetActive(false);
            ARInfoText.gameObject.SetActive(true);
            X1 = GameObject.Find("ARX1").GetComponent<TMP_InputField>();
            X2 = GameObject.Find("ARX2").GetComponent<TMP_InputField>();
            Y1 = GameObject.Find("ARY1").GetComponent<TMP_InputField>();
            Y2 = GameObject.Find("ARY2").GetComponent<TMP_InputField>();
            Res1 = GameObject.Find("ARResult1").GetComponent<TMP_InputField>();
            Res2 = GameObject.Find("ARResult2").GetComponent<TMP_InputField>();
        }
        FieldsList = new List<TMP_InputField> { X1, Y1, Res1, X2, Y2, Res2 };
        AdditionVoiceSpeaker.NumPlace = NumbersPlace;
        AdditionVoiceSpeaker.VoiceClipsPlace = VoicesPlace;
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        SLStaicFunctions.SpeakerName = SpeakerName;
    }
    public void TranslateToEng()
    {
        TextMeshProUGUI textComponent = LangButton.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.font = Amari;
        textComponent.text = "ﻲﺑﺮﻋ";

        TextMeshProUGUI SolveBtnTxt = SolveBTN.GetComponentInChildren<TextMeshProUGUI>();
        SolveBtnTxt.font = Autmon;

        TextMeshProUGUI ExplainBtnTxt = ExplainBTN.GetComponentInChildren<TextMeshProUGUI>();
        ExplainBtnTxt.font = Autmon;

        TextMeshProUGUI FirstNumPlaceTxt = FirstNumPlace.GetComponent<TextMeshProUGUI>();
        FirstNumPlace.font = Autmon;

        FirstNumPlace.fontSize = 72f;

        SolveBtnTxt.text = "Solve";
        ExplainBtnTxt.text = "Explain";

        VoicesPlace = "JennySound";
        NumbersPlace = "JennySound/Numbers";
        SpeakerName = "_Jenny_Eng";
        SLStaicFunctions.SpeakerName = SpeakerName;
        XPosSpacing = 1;
        XPos = -400;
        YPos = 750;

        XSymbol = "x";
        YSymbol = "y";
        ArrowRotation = Quaternion.Euler(157, 180, 0.5f);

    }
    public void TranslateToAr()
    {
        TextMeshProUGUI textComponent = LangButton.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.font = Autmon;
        textComponent.text = "Eng";

        TextMeshProUGUI SolveBtnTxt = SolveBTN.GetComponentInChildren<TextMeshProUGUI>();
        SolveBtnTxt.font = Amari;

        TextMeshProUGUI ExplainBtnTxt = ExplainBTN.GetComponentInChildren<TextMeshProUGUI>();
        ExplainBtnTxt.font = Amari;        
        
        TextMeshProUGUI FirstNumPlaceTxt = FirstNumPlace.GetComponent<TextMeshProUGUI>();
        FirstNumPlace.font = Amari;

        FirstNumPlace.fontSize = 65f;

        SolveBtnTxt.text = "ﻞﺣ";
        ExplainBtnTxt.text = "ﺡﺮﺷﺍ";

        VoicesPlace = "ShakirSound";
        NumbersPlace = "ShakirSound/Numbers";
        SpeakerName = "_Shakir_arab";
        SLStaicFunctions.SpeakerName = SpeakerName;

        XPos = 350;
        XPosSpacing = -1;
        YPos = 750;

        XSymbol = "س";
        YSymbol = "ص";
        ArrowRotation = Quaternion.Euler(157, 0, 0.5f);
    }
}