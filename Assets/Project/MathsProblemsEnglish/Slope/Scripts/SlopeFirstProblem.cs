using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlopeFirstProblem : MonoBehaviour
{
    [SerializeField] TMP_InputField X1;
    [SerializeField] TMP_InputField Y1;

    [SerializeField] TMP_InputField X2;
    [SerializeField] TMP_InputField Y2;

    [SerializeField] TMP_InputField angel;

    [SerializeField] TextMeshProUGUI FirstNumPlace;
    [SerializeField] GameObject Line;
    [SerializeField] float XPos;
    [SerializeField] float Ypos;

    float XPosTemp = 0;

    int SlopeNue = 0;
    int SlopeDeno = 0;
    private bool Explain = false;
    public static AudioSource audioSource;
    public static string SpeakerName = "_Jenny_Eng";

    Color BackGroundColor;

    public static string X1Val ="";
    public static string Y1Val ="";

    public static string X2Val ="";
    public static string Y2Val ="";

    public static string AngleVal ="";

    private void OnDisable()
    {
        XPosTemp = XPos;
        SLStaicFunctions.RemoveTexts();
    }
    private void Start()
    {
        if (!X1Val.Equals(""))
        {
            X1.text = X1Val;
        }
        if (!Y1Val.Equals(""))
        {
            Y1.text = Y1Val;
        }
        if (!X2Val.Equals(""))
        {
            X2.text = X2Val;
        }
        if (!Y2Val.Equals(""))
        {
            Y2.text = Y2Val;
        }
        if (!AngleVal.Equals(""))
        {
            angel.text = AngleVal;
        }
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

        Explain = false;
    }
    public IEnumerator GetLineOneSol()
    {
        // voice for first put m equals to
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "first put m one equals to slope" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>1</sub> = ", XPos, Ypos, 0, false);
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
        if (int.Parse(Y1.text) < 0)
            TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos + 30, Ypos, 0, false, 11);
        else
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
            TextInstantiator.InstantiateText(FirstNumPlace, X1.text, XPos + 30, Ypos, 0, false, 22);
        else
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
        if (OldNue != SlopeNue)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 180, Ypos + 50, 0, false);

            if(SlopeNue != SlopeDeno)
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

                TextInstantiator.InstantiateText(FirstNumPlace, SlopeNue.ToString(), XPos + 280, Ypos+50 , 0, false);
            }

        }
        Ypos -= 100;

    }
    public IEnumerator GetLineTwoSol()
    {
        XPos = XPosTemp;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "2", Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "tan" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, angel.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>2</sub> = Tan " + angel.text + "°", XPos + 140, Ypos, 0, false);


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "use your calculator and get the value of tan" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, angel.text, Explain)));


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        float TanAngel = Mathf.Tan(int.Parse(angel.text) * Mathf.Deg2Rad);
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, TanAngel.ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, " = "+TanAngel.ToString(), XPos+400 , Ypos , 0, false);
        Ypos -= 100;

        if((float)SlopeNue/SlopeDeno ==TanAngel)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "since m one equals m two" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain)));
            TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>2</sub> = m<sub>1</sub>", XPos + 45, Ypos , 0, false);
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so the two straight lines are parallel"+SpeakerName, Explain)));
            Ypos -= 100;
            TextInstantiator.InstantiateText(FirstNumPlace, $"L<sub>2</sub> // L<sub>1</sub>", XPos + 45, Ypos, 0, false);

        }
        else
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "since m one not equals m two" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain)));
            TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>2</sub> \u2260 m<sub>1</sub>", XPos + 50, Ypos, 0, false);
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so the two straight lines are not parallel" + SpeakerName, Explain)));
            Ypos -= 100;
            TextInstantiator.InstantiateText(FirstNumPlace, $"L<sub>2</sub> and L<sub>1</sub> are not parallel!", XPos+350, Ypos, 0, false);
        }
    }
}
