using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SlopeTenthProblem : MonoBehaviour
{
    [SerializeField] TMP_InputField X1;
    [SerializeField] TMP_InputField X2;

    [SerializeField] TMP_InputField Y1;
    [SerializeField] TMP_InputField Y2;

    [SerializeField] TMP_InputField Z1;
    [SerializeField] TMP_InputField Z2;

    [SerializeField] TextMeshProUGUI FirstNumPlace;
    [SerializeField] TextMeshProUGUI XText;
    [SerializeField] TextMeshProUGUI YText;
    [SerializeField] TextMeshProUGUI ZText;

    [SerializeField] GameObject Line;
    [SerializeField] GameObject Arrow;
    [SerializeField] float XPos;
    [SerializeField] float Ypos;

    float XPosTemp = 0;

    int SlopeNue = 0;
    int SlopeDeno = 0;

    int XYNue = 0;
    int XYDeno = 0;

    int YZDeno = 0;

    int AFinalVal = 0;

    private bool Explain = false;
    public static AudioSource audioSource;
    public static string SpeakerName = "_Jenny_Eng";

    Color BackGroundColor;


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
        XPosTemp = XPos;
        AdditionVoiceSpeaker.NumPlace = "JennySound/JennyNumbers";
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

            if (Explain || X1.text.Equals("") || Y1.text.Equals("") || X2.text.Equals("") || Y2.text.Equals("") || Z1.text.Equals("") || Z1.text.Equals(""))
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
        Explain = false;
        StartCoroutine(SolveStepByStep());
    }
    public IEnumerator SolveStepByStep()
    {
        XText.text = "X ("+X1.text+ ","+X2.text+")";
        YText.text = "Y ("+Y1.text+ ","+Y2.text+")";
        ZText.text = "Z ("+Z1.text+ ","+Z2.text+")";
        Arrow.SetActive(false);
        Ypos = 450;
        XPos = -470;
        SLStaicFunctions.RemoveTexts();
        AdditionVoiceSpeaker.IsEng = true;
        yield return StartCoroutine(GetLineOneSol());
        yield return StartCoroutine(GetLineTwoSol());
        yield return StartCoroutine(GetLineThreeSol());
        yield return StartCoroutine(GetLineFourSol());
        yield return StartCoroutine(GetLineFiveSol());
        yield return StartCoroutine(GetLineSixSol());
        Explain = false;
    }
    public IEnumerator GetLineOneSol()
    {
        XPos = -470;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "first put m one equals to slope" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"Slope XY", XPos + 90, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, $"―", XPos + 185, Ypos + 50, 0, false);

        Ypos -= 100;
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
        X2.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, X2.text, XPos, Ypos, 0, false, 1);
        XPos += 50;
        X2.GetComponent<Image>().color = BackGroundColor;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 20, Ypos, 0, false, 0);


        XPos += 60;
        Y2.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain)));
        if (int.Parse(Y1.text) < 0)
            TextInstantiator.InstantiateText(FirstNumPlace, Y2.text, XPos + 30, Ypos, 0, false, 11);
        else
            TextInstantiator.InstantiateText(FirstNumPlace, Y2.text, XPos, Ypos, 0, false, 11); 
        Y2.GetComponent<Image>().color = BackGroundColor;

        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos - 50, Ypos - 30, 0), "BigLine", FirstNumPlace, Explain)));

        XPos -= 100;
        Ypos -= 50;
        X1.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X1.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, X1.text, XPos, Ypos, 0, false, 2);
        X1.GetComponent<Image>().color = BackGroundColor;

        XPos += 40;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 20, Ypos, 0, false);
        Y1.GetComponent<Image>().color = Color.red;
        XPos += 60;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        if (int.Parse(Y1.text) < 0)
            TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos + 30, Ypos, 0, false, 22);
        else
            TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos, Ypos, 0, false, 22);
        Y1.GetComponent<Image>().color = BackGroundColor;

        XPos += 100;
        Ypos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos, Ypos, 0, false);

        //----------------------------------------------------------------------------

        TextMeshProUGUI x2 = GameObject.Find("1").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI y2 = GameObject.Find("11").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI x1 = GameObject.Find("2").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI y1 = GameObject.Find("22").GetComponent<TextMeshProUGUI>();

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain)));
        x2.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain)));
        y2.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        Ypos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (int.Parse(X2.text) - int.Parse(Y2.text)).ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(X2.text) - int.Parse(Y2.text)).ToString(), XPos + 100, Ypos, 0, false);
        SlopeNue = (int.Parse(X2.text) - int.Parse(Y2.text));

        // voice for over 
        Ypos -= 50;

        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 100, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain)));

        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X1.text, Explain)));
        x1.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, y1.text, Explain)));
        y1.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (int.Parse(X1.text) - int.Parse(Y1.text)).ToString(), Explain)));


        TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(X1.text) - int.Parse(Y1.text)).ToString(), XPos + 100, Ypos, 0, false);

        SlopeDeno = (int.Parse(X1.text) - int.Parse(Y1.text));
        SlopeNue = (int.Parse(X2.text) - int.Parse(Y2.text));

        int OldNue = SlopeNue;
        FractionCalculator.SimplifyFraction(ref SlopeNue, ref SlopeDeno);

        if (OldNue != SlopeNue)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 200, Ypos + 50, 0, false);

            yield return (StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 280, Ypos+50, 0, Explain, true)));

        }

        XYNue = SlopeNue;
        XYDeno = SlopeDeno;

        Ypos -= 150;
    }
    public IEnumerator GetLineTwoSol()
    {
        XPos = -470;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "2", Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"Slope YZ", XPos + 90, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, $"―", XPos + 185, Ypos + 50, 0, false);

        Ypos -= 100;

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
        Z2.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a"+SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Z2.text, XPos, Ypos, 0, false, 3);
        XPos += 50;
        Z2.GetComponent<Image>().color = BackGroundColor;

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
        Z1.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Z1.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Z1.text, XPos, Ypos, 0, false, 4);
        Z1.GetComponent<Image>().color = BackGroundColor;

        XPos += 40;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 20, Ypos, 0, false);
        Y1.GetComponent<Image>().color = Color.red;
        XPos += 60;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos, Ypos, 0, false, 44);
        Y1.GetComponent<Image>().color = BackGroundColor;

        XPos += 100;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos, Ypos, 50, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a"+SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Z2.text, XPos+100, Ypos, 100, false, 3);


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 150, Ypos, 100, false, 0);

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text , Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Y2.text, XPos + 200, Ypos, 100, false, 0);


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos +150, Ypos +20, 0), "BigLine", FirstNumPlace, Explain)));


        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Z1.text, Explain)));
        GameObject.Find("4").GetComponent<TextMeshProUGUI>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        GameObject.Find("44").GetComponent<TextMeshProUGUI>().color = Color.red;


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        YZDeno = int.Parse(Z1.text) - int.Parse(Y1.text);

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, YZDeno.ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, YZDeno.ToString(), XPos+150, Ypos, 0, false, 33);
        Ypos -= 120;
    }
    public IEnumerator GetLineThreeSol()
    {
        XPos = -525;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "since Y Z is perpendicular to X Y" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"since", XPos + 90, Ypos, 0, false);

        XPos += 150;
        TextInstantiator.InstantiateText(FirstNumPlace, $"XY", XPos + 90, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, $"―", XPos + 90, Ypos + 50, 0, false);

        TextInstantiator.InstantiateText(FirstNumPlace, $"|", XPos + 170, Ypos + 10, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, $"―", XPos + 170, Ypos , -20, false);


        TextInstantiator.InstantiateText(FirstNumPlace, $"YZ", XPos + 250, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, $"―", XPos + 250, Ypos + 50, 0, false);
        Ypos -= 130;
    }    
    public IEnumerator GetLineFourSol()
    {
        XPos = -550;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"a", XPos + 90, Ypos+50, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"-", XPos + 130, Ypos + 50, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Y2.text, XPos + 170, Ypos + 50, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, YZDeno.ToString(), Explain)));

        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos +120, Ypos - 30, 0), "BigLine", FirstNumPlace, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, YZDeno.ToString(), XPos + 120, Ypos - 50, 0, false,88);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "=", XPos + 300, Ypos , 0, false);

        yield return (StartCoroutine(SLStaicFunctions.WriteFraction(this,FirstNumPlace,Line,Explain,-XYDeno,XYNue,XPos+400,Ypos,10,Explain,true)));
        Ypos -=200;
    }
    public IEnumerator GetLineFiveSol()
    {
        XPos = -550;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"a", XPos + 90, Ypos , 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"-", XPos + 150, Ypos, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y2.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Y2.text, XPos + 200, Ypos, 0, false,20);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"=", XPos + 300, Ypos, 0, false);

        TextMeshProUGUI ADeno = GameObject.Find("88").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI AOppositeNue = GameObject.Find("10").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI AOppositeDeno = GameObject.Find("100").GetComponent<TextMeshProUGUI>();

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, ADeno.text, Explain)));

        ADeno.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, AOppositeNue.text, Explain)));

        AOppositeNue.color = Color.red;


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        AFinalVal = int.Parse(AOppositeNue.text) * int.Parse(ADeno.text);

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, AFinalVal.ToString(), Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, AOppositeDeno.text, Explain)));

        AOppositeDeno.color = Color.red;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        Ypos -= 150;


        yield return (StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, AFinalVal, XYNue, XPos + 400, Ypos+150, 0, Explain, true)));




        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so send" + SpeakerName, Explain)));

        TextMeshProUGUI SendedValue = GameObject.Find("20").GetComponent<TextMeshProUGUI>();
        SendedValue.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SendedValue.text, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "to the right side with a negative sign" + SpeakerName, Explain)));

        Arrow.SetActive(true);


        CharacterProbs.CenterInPos(XPos + 380, Ypos + 180, ref Arrow, FirstNumPlace);
        if (!SendedValue.text[0].Equals("-"))
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "+ "+SendedValue.text.ToString(), XPos + 550, Ypos + 150, 0, false);
        }
        else
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "- " + SendedValue.text.ToString(), XPos + 550, Ypos + 150, 0, false);
        }


    }
    public IEnumerator GetLineSixSol()
    {
        XPos = -550;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"a", XPos + 90, Ypos , 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, $"=", XPos + 150, Ypos, 0, false);

        int FinalAnswer =int.Parse(GameObject.Find("20").GetComponent<TextMeshProUGUI>().text);
        Debug.Log(AFinalVal + " " + XYNue +" "+FinalAnswer);

        FinalAnswer = -FinalAnswer;


        FractionCalculator.SubtractNumberFromFraction( ref AFinalVal,ref  XYNue ,FinalAnswer);
        Debug.Log(AFinalVal + " " + XYNue);

        yield return (StartCoroutine(SLStaicFunctions.WriteFraction(this,FirstNumPlace,Line,Explain,AFinalVal,XYNue,XPos+250,Ypos,-1,Explain,true)));

    }
}

