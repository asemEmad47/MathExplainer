using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecProbelm : MonoBehaviour
{

    [SerializeField] TMP_InputField X1;
    [SerializeField] TMP_InputField X2;
    [SerializeField] TMP_InputField Y1;
    [SerializeField] TMP_InputField Y2;
    [SerializeField] TextMeshProUGUI FirstNumPlace;
    [SerializeField] GameObject Line;
    [SerializeField] GameObject Arrow;
    [SerializeField] GameObject FirstProblemObj;

    [SerializeField] float XPos;
    [SerializeField] float Ypos;
    private bool Explain = false;
    public static AudioSource audioSource;
    public static string SpeakerName = "_Jenny_Eng";
    private float XTemp = 0;

    private int SlopeNue = 0;
    private int SlopeDeno = 0;

    Color BackGroundColor;
    public static string X1val = "";
    public static string Y1val = "";

    public static string X2val = "";
    public static string Y2val = "";


    private Button PauseBtn;
    private Button ResumeBtn;
    private void OnDisable()
    {
        XTemp = XPos;
        XPos = -470;
        Ypos = 420;
        SLStaicFunctions.RemoveTexts();
        Arrow.SetActive(false);

    }
    public void Start()
    {
        PauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        ResumeBtn = GameObject.Find("Resume").GetComponent<Button>();
        PauseBtn.onClick.AddListener(PauseScript.Pause);
        ResumeBtn.onClick.AddListener(PauseScript.Resume);
        SLStaicFunctions.RemoveTexts();
        if (!X1val.Equals(""))
        {
            X1.text = X1val;
        }
        if (!Y1val.Equals(""))
        {
            Y1.text = Y1val;
        }
        if (!X2val.Equals(""))
        {
            X2.text = X2val;
        }
        if (!Y2val.Equals(""))
        {
            Y2.text = Y2val;
        }
        AdditionVoiceSpeaker.NumPlace = "JennySound/Numbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        SLStaicFunctions.SpeakerName = SpeakerName;

        X1.onValidateInput = InputFieldsActions.ValidateEqsInput;
        X2.onValidateInput = InputFieldsActions.ValidateEqsInput;


        Y1.onValidateInput = InputFieldsActions.ValidateEqsInput;
        Y2.onValidateInput = InputFieldsActions.ValidateEqsInput;

        BackGroundColor = Y1.GetComponent<Image>().color;

    }
    public void Solve()
    {

        XTemp = XPos;
        XPos = -470;
        Ypos = 420;
        Explain = false;
        StartCoroutine(SolveStepByStep());
    }

    private void Update()
    {
        PauseScript.ControlPause();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Button SolveBtn = GameObject.Find("Solve").GetComponent<Button>();
        Button ExplainBtn = GameObject.Find("Explain").GetComponent<Button>();

        if (Explain || X1.text.Equals("") || X2.text.Equals("")  || Y1.text.Equals("") || Y2.text.Equals(""))
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
    public void explain()
    {
        Explain = true;
        StartCoroutine(SolveStepByStep());
    }
    public IEnumerator SolveStepByStep()
    {
        XTemp = XPos;
        XPos = -470;
        Ypos = 420;
        SLStaicFunctions.RemoveTexts();
        Arrow.SetActive(false);
        yield return StartCoroutine(GetLineOneSol());
        FirstProblem.IsCalledFromOutSide = true;
        FirstProblem firstProblem = FirstProblemObj.GetComponent<FirstProblem>();
        firstProblem.SetComponents(X1, Y1, FirstNumPlace, Line, Arrow, -470, Ypos, SlopeNue, SlopeDeno, Explain, BackGroundColor);
        yield return StartCoroutine(firstProblem.SolveStepByStep());
        Explain = false;
    }
    public IEnumerator GetLineOneSol()
    {
        // voice for first put m equals to
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "first put m one equals to slope" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "m = ", XPos, Ypos, 0, false);
        XPos += 150;
        Ypos += 50;


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
        if(int.Parse(Y1.text) < 0)
            XPos += 30;
        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos, Ypos, 0, false, 11);
        Y1.GetComponent<Image>().color = BackGroundColor;

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
            XPos += 30;
        TextInstantiator.InstantiateText(FirstNumPlace, X1.text, XPos, Ypos, 0, false, 22);
        X1.GetComponent<Image>().color = BackGroundColor;

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
        if (OldNue != SlopeNue && SlopeDeno != 1)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 180, Ypos + 50, 0, false);

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, SlopeNue.ToString(), XPos + 280, Ypos + 100, 0, false);
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 280, Ypos + 30, 0), "SmallLine", FirstNumPlace, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeDeno.ToString(), Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, SlopeDeno.ToString(), XPos + 280, Ypos, 0, false);

        }
        else if (SlopeDeno == 1)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 180, Ypos + 50, 0, false);

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, SlopeNue.ToString(), XPos + 250, Ypos + 50, 0, false);

        }
        Ypos -= 100;

    }

}
