using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EightsProblem : MonoBehaviour
{
    [SerializeField] TMP_InputField X1;
    [SerializeField] TMP_InputField Y1;

    [SerializeField] TMP_InputField SlopeNuemerator;
    [SerializeField] TMP_InputField SlopeDenominator;

    [SerializeField] TextMeshProUGUI FirstNumPlace;
    [SerializeField] GameObject Line;
    [SerializeField] GameObject Arrow;
    [SerializeField] float XPos;
    [SerializeField] float Ypos;
    [SerializeField] GameObject FirstProblemObj;

    private bool Explain = false;
    public static AudioSource audioSource;
    public static string SpeakerName = "_Jenny_Eng";
    public static bool IsCalledFromOutSide = false;
    private float XTemp = 0;

    private int SlopeNue = 0;
    private int SlopeDeno = 0;


    Color BackGroundColor;


    public static string Xval = "";
    public static string Yval = "";

    public static string FNue = "";
    public static string FDeno = "";

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

        SLStaicFunctions.RemoveTexts();
        if (!Xval.Equals(""))
        {
            X1.text = Xval;
        }
        if (!Yval.Equals(""))
        {
            Y1.text = Yval;
        }
        if (!FNue.Equals(""))
        {
            SlopeNuemerator.text = FNue;
        }
        if (!FDeno.Equals(""))
        {
            SlopeDenominator.text = FDeno;
        }
        AdditionVoiceSpeaker.NumPlace = "JennySound/JennyNumbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        SLStaicFunctions.SpeakerName = SpeakerName;

        X1.onValidateInput = InputFieldsActions.ValidateEqsInput;
        SlopeNuemerator.onValidateInput = InputFieldsActions.ValidateEqsInput;

        Y1.onValidateInput = InputFieldsActions.ValidateEqsInput;
        SlopeDenominator.onValidateInput = InputFieldsActions.ValidateEqsInput;

        BackGroundColor = Y1.GetComponent<Image>().color;

    }

    public void Solve()
    {

        Explain = false;
        StartCoroutine(SolveStepByStep());
    }
    private void Update()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Button SolveBtn = GameObject.Find("Solve").GetComponent<Button>();
        Button ExplainBtn = GameObject.Find("Explain").GetComponent<Button>();

        if (Explain || X1.text.Equals("") || Y1.text.Equals("") || SlopeNuemerator.text.Equals("") || SlopeNuemerator.text.Equals("") )
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
        Ypos -= 50;
        Arrow.SetActive(false);
        SlopeNue = int.Parse(SlopeNuemerator.text);
        SlopeDeno = int.Parse(SlopeDenominator.text);
        yield return StartCoroutine(GetLineThreeSol());


        yield return StartCoroutine(GetLineFourSol());
        FirstProblem.IsCalledFromOutSide = true;
        FirstProblem firstProblem = FirstProblemObj.GetComponent<FirstProblem>();
        firstProblem.SetComponents(X1, Y1, FirstNumPlace, Line, Arrow, -470, Ypos, SlopeNue, SlopeDeno, Explain, BackGroundColor);
        yield return StartCoroutine(firstProblem.SolveStepByStep());
        Explain = false;

    }
    public IEnumerator GetLineThreeSol()
    {

        XPos = XTemp;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "first" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "2", Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "m<sub>2</sub> = ", XPos + 10, Ypos, 0, false);

        SlopeNuemerator.GetComponent<Image>().color = Color.red;
        SlopeDenominator.GetComponent<Image>().color = Color.red;
        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 200, Ypos, -1, Explain));

        SlopeNuemerator.GetComponent<Image>().color = BackGroundColor;
        SlopeDenominator.GetComponent<Image>().color = BackGroundColor;
        Ypos -= 150;
    }

    public IEnumerator GetLineFourSol()
    {
        XPos = XTemp;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "Since L1 is perpendicular to L2" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "since L<sub>1</sub> | L<sub>2</sub><line-height=35%>\n        --</line-height=35%>", XPos + 115, Ypos, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then m1 equals minus 1 over m2" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "then", XPos + 400, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, "m<sub>1</sub> = ", XPos + 600, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, "-1", XPos + 750, Ypos + 50, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 760, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "m<sub>2</sub>", XPos + 750, Ypos - 50, 0, false);
        //--------------------------------------------
        Ypos -= 125;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "m<sub>2</sub> = ", XPos, Ypos, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "-" + SlopeDeno.ToString(), Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-" + SlopeDeno.ToString(), XPos + 150, Ypos + 50, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 160, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));
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
