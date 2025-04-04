using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeventhSlopeProblem : MonoBehaviour
{
    [SerializeField] TMP_InputField X1;
    [SerializeField] TMP_InputField Y1;

    [SerializeField] TMP_InputField X2;
    [SerializeField] TMP_InputField Y2;

    [SerializeField] TMP_InputField X3;
    [SerializeField] TMP_InputField Y3;

    [SerializeField] TextMeshProUGUI FirstNumPlace;
    [SerializeField] GameObject Line;
    [SerializeField] float XPos;
    [SerializeField] float Ypos;

    float XPosTemp = 0;

    int SlopeNue = 0;
    int SlopeDeno = 0;

    int ABNue = 0;
    int ABDeno = 0;    
    
    int BCNue = 0;
    int BCDeno = 0;
    private bool Explain = false;
    public static AudioSource audioSource;
    public static string SpeakerName = "_Jenny_Eng";

    Color BackGroundColor;

    public static string A1 = "";
    public static string B1 = "";
    public static string C1 = "";

    public static string A2 = "";
    public static string B2 = "";
    public static string C2 = "";

    private Button PauseBtn;
    private Button ResumeBtn;
    private void OnDisable()
    {
        XPosTemp = XPos;
        SLStaicFunctions.RemoveTexts();
    }
    private void Start()
    {
        PauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        ResumeBtn = GameObject.Find("Resume").GetComponent<Button>();
        PauseBtn.onClick.AddListener(PauseScript.Pause);
        ResumeBtn.onClick.AddListener(PauseScript.Resume);
        if (!A1.Equals(""))
        {
            X1.text = A1;
        }
        if (!B1.Equals(""))
        {
            Y1.text = B1;

        }
        if (!C1.Equals(""))
        {
            X2.text = C1;

        }
        if (!A2.Equals(""))
        {
            Y2.text = A2;

        }
        if (!B2.Equals(""))
        {
            X3.text = B2;

        }
        if (!C2.Equals(""))
        {
            Y3.text = C2;

        }
        XPosTemp = XPos;
        AdditionVoiceSpeaker.NumPlace = "JennySound/Numbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        SLStaicFunctions.SpeakerName = SpeakerName;
        BackGroundColor = Y1.GetComponent<Image>().color;
        X1.onValidateInput = InputFieldsActions.ValidateEqsInput;
        Y1.onValidateInput = InputFieldsActions.ValidateEqsInput;

    }
    private void Update()
    {
        PauseScript.ControlPause();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Button SolveBtn = GameObject.Find("Solve").GetComponent<Button>();
        Button ExplainBtn = GameObject.Find("Explain").GetComponent<Button>();

        if (X1 != null)
        {

            if (Explain || X1.text.Equals("") || Y1.text.Equals(""))
            {
                SolveBtn.interactable = false;
                ExplainBtn.interactable = false;
            }
            else
            {
                SolveBtn.interactable = true;
                ExplainBtn.interactable = true;

            }
        }
    }
    public void explain()
    {
        Explain = true;
        StartCoroutine(SolveStepByStep());
    }
    public void Solve()
    {
        Ypos = 340;
        Explain = false;
        StartCoroutine(SolveStepByStep());
    }
    public IEnumerator SolveStepByStep()
    {

        SLStaicFunctions.RemoveTexts();
        AdditionVoiceSpeaker.IsEng = true;
        yield return StartCoroutine(GetLineOneSol());
        yield return StartCoroutine(GetLineTwoSol());
        yield return StartCoroutine(GetLineThreeSol());

        Explain = false;
    }
    public IEnumerator GetLineOneSol()
    {
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "first put m one equals to slope" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"Slope AB", XPos + 90, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, $"―", XPos + 185, Ypos + 50, 0, false);

        XPos = -390;
        Ypos -= 100;
        TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>1</sub> = ", XPos-50, Ypos-50, 0, false);
        XPos  +=100;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y2" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"y<sub>2</sub>", XPos, Ypos, 0, false);
        XPos += 50;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos, Ypos, 0, false, 0);

        XPos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y1" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"y<sub>1</sub>", XPos, Ypos, 0, false);

        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos - 50, Ypos - 30, 0), "BigLine", FirstNumPlace, Explain)));

        XPos -= 100;
        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x2" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"x<sub>2</sub>", XPos, Ypos, 0, false);

        XPos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos, Ypos, 0, false);

        XPos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x1" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"x<sub>1</sub>", XPos, Ypos, 0, false);

        XPos += 100;
        Ypos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos, Ypos, 0, false);

        //----------------------------------------------------------------------------

        Ypos += 50;
        XPos += 100;
        Y2.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Y2.text, XPos, Ypos, 0, false, 1);
        XPos += 50;
        Y2.GetComponent<Image>().color = BackGroundColor;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 20, Ypos, 0, false, 0);


        XPos += 60;
        Y1.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        if (int.Parse(Y1.text) < 0)
            TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos + 30, Ypos, 0, false, 11);
        else
            TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos, Ypos, 0, false, 11); Y1.GetComponent<Image>().color = BackGroundColor;

        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos - 50, Ypos - 30, 0), "BigLine", FirstNumPlace, Explain)));

        XPos -= 100;
        Ypos -= 50;
        X2.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, X2.text, XPos, Ypos, 0, false, 2);
        X2.GetComponent<Image>().color = BackGroundColor;

        XPos += 40;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 20, Ypos, 0, false);
        X1.GetComponent<Image>().color = Color.red;
        XPos += 60;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X1.text, Explain)));
        if (int.Parse(X1.text) < 0)
            TextInstantiator.InstantiateText(FirstNumPlace, X1.text, XPos + 30, Ypos, 0, false, 22);
        else
            TextInstantiator.InstantiateText(FirstNumPlace, X1.text, XPos, Ypos, 0, false, 22); X1.GetComponent<Image>().color = BackGroundColor;

        XPos += 100;
        Ypos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos, Ypos, 0, false);

        //----------------------------------------------------------------------------

        TextMeshProUGUI y2 = GameObject.Find("1").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI y1 = GameObject.Find("11").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI x2 = GameObject.Find("2").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI x1 = GameObject.Find("22").GetComponent<TextMeshProUGUI>();

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain)));
        y2.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        y1.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        Ypos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (int.Parse(Y2.text) - int.Parse(Y1.text)).ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(Y2.text) - int.Parse(Y1.text)).ToString(), XPos + 100, Ypos, 0, false);
        SlopeNue = (int.Parse(Y2.text) - int.Parse(Y1.text));

        // voice for over 
        Ypos -= 50;

        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 100, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain)));

        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain)));
        x2.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X1.text, Explain)));
        x1.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (int.Parse(X2.text) - int.Parse(X1.text)).ToString(), Explain)));


        TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(X2.text) - int.Parse(X1.text)).ToString(), XPos + 100, Ypos, 0, false);
        SlopeDeno = (int.Parse(X2.text) - int.Parse(X1.text));

        int OldNue = SlopeNue;

        FractionCalculator.SimplifyFraction(ref SlopeNue, ref SlopeDeno);
        if (SlopeNue > 0 && SlopeDeno < 0)
        {
            SlopeNue = -SlopeNue;
            SlopeDeno = -SlopeDeno;
        }
        if (OldNue != SlopeNue)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 180, Ypos + 50, 0, false);

            if (SlopeNue != SlopeDeno)
            {

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));

                TextInstantiator.InstantiateText(FirstNumPlace, SlopeNue.ToString(), XPos + 280, Ypos + 100, 0, false);
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 280, Ypos + 30, 0), "SmallLine", FirstNumPlace, Explain)));
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeDeno.ToString(), Explain)));
                TextInstantiator.InstantiateText(FirstNumPlace, SlopeDeno.ToString(), XPos + 280, Ypos, 0, false);


            }
            else
            {
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));

                TextInstantiator.InstantiateText(FirstNumPlace, SlopeNue.ToString(), XPos + 280, Ypos + 50, 0, false);
            }

        }

        ABNue = SlopeNue;
        ABDeno = SlopeDeno;

        Ypos -= 175;
    }
    public IEnumerator GetLineTwoSol()
    {
        XPos = -470;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "2", Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"Slope BC", XPos + 90, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, $"―", XPos + 185, Ypos + 50, 0, false);

        XPos = -390;
        Ypos -= 100;
        TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>2</sub> = ", XPos-50, Ypos-50, 0, false);
        XPos  +=100;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y2" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"y<sub>2</sub>", XPos, Ypos, 0, false);
        XPos += 50;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos, Ypos, 0, false, 0);

        XPos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y1" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"y<sub>1</sub>", XPos, Ypos, 0, false);

        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos - 50, Ypos - 30, 0), "BigLine", FirstNumPlace, Explain)));

        XPos -= 100;
        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x2" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"x<sub>2</sub>", XPos, Ypos, 0, false);

        XPos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos, Ypos, 0, false);

        XPos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x1" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"x<sub>1</sub>", XPos, Ypos, 0, false);

        XPos += 100;
        Ypos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos, Ypos, 0, false);

        //----------------------------------------------------------------------------

        Ypos += 50;
        XPos += 100;
        Y3.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y3.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Y3.text, XPos, Ypos, 0, false, 3);
        XPos += 50;
        Y3.GetComponent<Image>().color = BackGroundColor;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 20, Ypos, 0, false, 0);


        XPos += 60;
        Y2.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Y2.text, XPos, Ypos, 0, false, 33);
        Y2.GetComponent<Image>().color = BackGroundColor;

        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos - 50, Ypos - 30, 0), "BigLine", FirstNumPlace, Explain)));

        XPos -= 100;
        Ypos -= 50;
        X3.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X3.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, X3.text, XPos, Ypos, 0, false, 4);
        X3.GetComponent<Image>().color = BackGroundColor;

        XPos += 40;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 20, Ypos, 0, false);
        X2.GetComponent<Image>().color = Color.red;
        XPos += 60;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, X2.text, XPos, Ypos, 0, false, 44);
        X2.GetComponent<Image>().color = BackGroundColor;

        XPos += 100;
        Ypos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos, Ypos, 0, false);

        //----------------------------------------------------------------------------

        TextMeshProUGUI y3 = GameObject.Find("3").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI y2 = GameObject.Find("33").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI x3 = GameObject.Find("4").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI x2 = GameObject.Find("44").GetComponent<TextMeshProUGUI>();

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y3.text, Explain)));
        y3.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain)));
        y2.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        Ypos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (int.Parse(Y3.text) - int.Parse(Y2.text)).ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(Y3.text) - int.Parse(Y2.text)).ToString(), XPos + 100, Ypos, 0, false);
        SlopeNue = (int.Parse(Y3.text) - int.Parse(Y2.text));

        // voice for over 
        Ypos -= 50;

        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 100, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain)));

        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X3.text, Explain)));
        x3.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain)));
        x2.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (int.Parse(X3.text) - int.Parse(X2.text)).ToString(), Explain)));


        TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(X3.text) - int.Parse(X2.text)).ToString(), XPos + 100, Ypos, 0, false);
        SlopeDeno = (int.Parse(X3.text) - int.Parse(X2.text));

        int OldNue = SlopeNue;

        FractionCalculator.SimplifyFraction(ref SlopeNue, ref SlopeDeno);
        if (SlopeNue > 0 && SlopeDeno < 0)
        {
            SlopeNue = -SlopeNue;
            SlopeDeno = -SlopeDeno;
        }
        if (OldNue != SlopeNue)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 180, Ypos + 50, 0, false);

            if (SlopeNue != SlopeDeno)
            {

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));

                TextInstantiator.InstantiateText(FirstNumPlace, SlopeNue.ToString(), XPos + 280, Ypos + 100, 0, false);
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 280, Ypos + 30, 0), "SmallLine", FirstNumPlace, Explain)));
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeDeno.ToString(), Explain)));
                TextInstantiator.InstantiateText(FirstNumPlace, SlopeDeno.ToString(), XPos + 280, Ypos, 0, false);


            }
            else
            {
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));

                TextInstantiator.InstantiateText(FirstNumPlace, SlopeNue.ToString(), XPos + 280, Ypos + 50, 0, false);
            }

        }

        BCNue = SlopeNue;
        BCDeno = SlopeDeno;

        Ypos -= 100;
    }
    public IEnumerator GetLineThreeSol()
    {
        XPos = -500;
        if(ABNue == BCNue && ABDeno == BCDeno)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "since m one equals m two" + SpeakerName, Explain)));
            TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>1</sub> = ", XPos + 50, Ypos, 0, false);
            TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>2</sub> ", XPos + 200, Ypos, 0, false);
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "are collinear" + SpeakerName, Explain)));
            TextInstantiator.InstantiateText(FirstNumPlace, "A , B , C are collinear", XPos + 650, Ypos, 0, false);

        }
        else{

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "since m one not equals m two" + SpeakerName, Explain)));
            TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>1</sub> \u2260 ", XPos + 50, Ypos, 0, false);
            TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>2</sub>", XPos + 160, Ypos, 0, false);
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "are not collinear" + SpeakerName, Explain)));
            TextInstantiator.InstantiateText(FirstNumPlace, "A , B , C are not collinear", XPos + 620, Ypos, 0, false);

        }
    }
}
