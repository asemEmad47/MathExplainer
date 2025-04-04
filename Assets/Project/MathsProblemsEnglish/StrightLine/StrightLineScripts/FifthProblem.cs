using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FifthProblem : MonoBehaviour
{
    [SerializeField] TMP_InputField X1;
    [SerializeField] TMP_InputField Y1;
    [SerializeField] TMP_InputField a;
    [SerializeField] TMP_InputField b;
    [SerializeField] TMP_InputField c;
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


    public static string Xval = "";
    public static string Yval = "";

    public static string A = "";
    public static string B = "";
    public static string C = "";


    private Button PauseBtn;
    private Button ResumeBtn;
    private void OnDisable()
    {
        XTemp = XPos;
        XPos = -470;
        Ypos = 600;
        SLStaicFunctions.RemoveTexts();

    }
    private void Start()
    {
        PauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        ResumeBtn = GameObject.Find("Resume").GetComponent<Button>();
        PauseBtn.onClick.AddListener(PauseScript.Pause);
        ResumeBtn.onClick.AddListener(PauseScript.Resume);

        SLStaicFunctions.RemoveTexts();
        if (!Xval.Equals(""))
        {
            X1.text = Xval;
        }
        if (!Yval.Equals(""))
        {
            Y1.text = Yval;
        }
        if (!A.Equals(""))
        {
            a.text = A;
        }
        if (!B.Equals(""))
        {
            b.text = B;
        }
        if (!C.Equals(""))
        {
            c.text = C;
        }
        AdditionVoiceSpeaker.NumPlace = "JennySound/Numbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        SLStaicFunctions.SpeakerName = SpeakerName;

        X1.onValidateInput = InputFieldsActions.ValidateEqsInput;
        Y1.onValidateInput = InputFieldsActions.ValidateEqsInput;
        a.onValidateInput = InputFieldsActions.ValidateEqsInput;
        b.onValidateInput = InputFieldsActions.ValidateEqsInput;
        c.onValidateInput = InputFieldsActions.ValidateEqsInput;

        BackGroundColor = Y1.GetComponent<Image>().color;

    }

    public void Solve()
    {
        Explain = false;
        StartCoroutine(SolveStepByStep());
    }
    private void Update()
    {
        PauseScript.ControlPause();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Button SolveBtn = GameObject.Find("Solve").GetComponent<Button>();
        Button ExplainBtn = GameObject.Find("Explain").GetComponent<Button>();

        if (Explain || X1.text.Equals("") || Y1.text.Equals("") || a.text.Equals("") || b.text.Equals("") || c.text.Equals(""))
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
        Ypos = 600;
        SLStaicFunctions.RemoveTexts();
        Arrow.SetActive(false);
        yield return StartCoroutine(GetLineOneSol());
        yield return StartCoroutine(GetLineTwoSol());
        FirstProblem.IsCalledFromOutSide = true;
        FirstProblem firstProblem = FirstProblemObj.GetComponent<FirstProblem>();
        firstProblem.SetComponents(X1, Y1, FirstNumPlace, Line, Arrow, -470, Ypos, SlopeNue, SlopeDeno, Explain, BackGroundColor);
        yield return StartCoroutine(firstProblem.SolveStepByStep());
        Explain = false;
    }
    public IEnumerator GetLineOneSol()
    {
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "first put m equals to" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "m = ", XPos, Ypos, 0, false);
        XPos += 150;

        a.GetComponent<Image>().color = Color.red;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"-a", XPos, Ypos + 50, 0, false);
        a.GetComponent<Image>().color = BackGroundColor;



        b.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos - 10, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "b" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"b", XPos, Ypos - 50, 0, false);
        b.GetComponent<Image>().color = BackGroundColor;

        //----------------------------------------------------------------------------------------------------------
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 80, Ypos, 0, false);
        SlopeNue = -int.Parse(a.text);
        SlopeDeno = int.Parse(b.text);

        a.GetComponent<Image>().color = Color.red;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, a.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-" + a.text, XPos + 180, Ypos + 50, 0, false);
        a.GetComponent<Image>().color = BackGroundColor;



        b.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 190, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, b.text, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, b.text, XPos + 190, Ypos - 50, 0, false);
        b.GetComponent<Image>().color = BackGroundColor;


        int OldNue = SlopeNue;
        FractionCalculator.SimplifyFraction(ref SlopeNue, ref SlopeDeno);

        if ((OldNue != SlopeNue && SlopeNue != -OldNue) || SlopeDeno == 1)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 300, Ypos, 0, false);

            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 400, Ypos, -1, Explain));

        }
        Ypos -= 150;
    }
    public IEnumerator GetLineTwoSol()
    {
        XPos = XTemp;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "Since L1 is perpendicular to L2" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "since L<sub>1</sub> | L<sub>2</sub><line-height=35%>\n        --</line-height=35%>", XPos + 115, Ypos, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then m1 equals minus 1 over m2" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "then", XPos + 400, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, "m<sub>1</sub> = ", XPos + 600, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, "-1", XPos + 750, Ypos+50, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 760, Ypos-20 , 0), "SmallLine", FirstNumPlace, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "m<sub>2</sub>", XPos + 750, Ypos - 50, 0, false);
        //--------------------------------------------
        Ypos -= 125;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "m<sub>2</sub> = ", XPos , Ypos, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "-"+SlopeDeno.ToString() , Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-"+SlopeDeno.ToString(), XPos + 150, Ypos + 50, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 160, Ypos-20, 0), "SmallLine", FirstNumPlace, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,  SlopeNue.ToString(), Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, SlopeNue.ToString(), XPos + 150, Ypos - 50, 0, false);

        (SlopeDeno, SlopeNue) = (-SlopeNue, SlopeDeno);

        int OldNue = SlopeNue;

        FractionCalculator.SimplifyFraction(ref SlopeNue, ref SlopeDeno);

        if ((OldNue != SlopeNue && SlopeNue != -OldNue) || SlopeDeno == 1)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 250, Ypos, 0, false);

            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 350, Ypos, -1, Explain));

        }
        Ypos -= 150;
        yield return null;
    }


}
