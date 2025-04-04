using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class FifthSlopeProblem : MonoBehaviour
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
    [SerializeField] float YposTemp;

    float XPosTemp = 0;

    int SlopeNue = 0;
    int SlopeDeno = 0;
    private bool Explain = false;
    public static AudioSource audioSource;
    public static string SpeakerName = "_Jenny_Eng";

    Color BackGroundColor;
    public static string X1Val = "";
    public static string Y1Val = "";

    public static string X2Val = "";
    public static string Y2Val = "";

    public static string AngleVal = "";

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
        Explain = false;
        StartCoroutine(SolveStepByStep());
    }
    public IEnumerator SolveStepByStep()
    {
        Ypos = 140;
        XPos = -470;
        SLStaicFunctions.RemoveTexts();
        AdditionVoiceSpeaker.IsEng = true;
        yield return StartCoroutine(GetLineOneSol());
        yield return StartCoroutine(GetLineTwoSol());
        yield return StartCoroutine(GetParallelSol());
        yield return StartCoroutine(GetPerpedicularSol());
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
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));

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
        TextInstantiator.InstantiateText(FirstNumPlace, X2.text, XPos, Ypos, 0, false, 33);
        X2.GetComponent<Image>().color = BackGroundColor;

        XPos += 40;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 20, Ypos, 0, false);
        X1.GetComponent<Image>().color = Color.red;
        XPos += 60;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X1.text, Explain)));
        if (int.Parse(X1.text) < 0)
            TextInstantiator.InstantiateText(FirstNumPlace, X1.text, XPos + 30, Ypos, 0, false, 44);
        else
            TextInstantiator.InstantiateText(FirstNumPlace, X1.text, XPos, Ypos, 0, false, 44); X1.GetComponent<Image>().color = BackGroundColor;

        XPos += 100;
        Ypos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos, Ypos, 0, false);

        //----------------------------------------------------------------------------

        SlopeDeno = int.Parse(X2.text) - int.Parse(X1.text);

        Y2.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, Y2.text, XPos + 100, Ypos + 50, 0, false, 1);
        Y2.GetComponent<Image>().color = BackGroundColor;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 165, Ypos + 50, 0, false, 0);


        Y1.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos + 220, Ypos + 50, 0, false, 11);
        Y1.GetComponent<Image>().color = BackGroundColor;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 165, Ypos - 30, 0), "BigLine", FirstNumPlace, Explain)));


        TextMeshProUGUI FirstX = GameObject.Find("33").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI SecX = GameObject.Find("44").GetComponent<TextMeshProUGUI>();


        FirstX.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X2.text, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));

        FirstX.color = BackGroundColor;

        SecX.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X1.text, Explain)));
        SecX.color = BackGroundColor;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeDeno.ToString(), Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, SlopeDeno.ToString(), XPos + 165, Ypos - 50, 0, false, 2);

        Ypos -= 175;

    }
    public IEnumerator GetLineTwoSol()
    {
        XPos = XPosTemp;
        XPos = XPosTemp;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "2", Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "tan" + SpeakerName, Explain)));
        angel.GetComponent<Image>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, angel.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>2</sub> = Tan " + angel.text + "°", XPos + 150, Ypos, 0, false);

        angel.GetComponent<Image>().color = BackGroundColor;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "use your calculator and get the value of tan" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, angel.text, Explain)));


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        float TanAngel = Mathf.Tan(int.Parse(angel.text) * Mathf.Deg2Rad);
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, TanAngel.ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, " = " + TanAngel.ToString(), XPos + 400, Ypos, 0, false);
        Ypos -= 100;
        Ypos -= 75;

    }
    public IEnumerator GetPerpedicularSol()
    {
        XPos = 50;
        Ypos = YposTemp;
        float TanAngel = Mathf.Tan(int.Parse(angel.text) * Mathf.Deg2Rad);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "Since L1 is perpendicular to L2" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"L<sub>2</sub> | L<sub>1</sub>", XPos+80 , Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, $"―", XPos +85, Ypos - 40, 0, false);

        XPos += 250;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "1", Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $", m<sub>1</sub>=", XPos + 10, Ypos, 0, false);


        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "-1", Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "2", Explain)));


        TextInstantiator.InstantiateText(FirstNumPlace, "-1", XPos + 150, Ypos + 50, 0, false);
        yield return StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos+150, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));
        TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>2</sub>", XPos + 150, Ypos - 50, 0, false);

        //--------------------------------------------------------------------
        Ypos -= 175;
        XPos = 50;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));


        TextInstantiator.InstantiateText(FirstNumPlace, "a-" + Y1.text, XPos + 50, Ypos + 50, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 50, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));


        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeDeno.ToString(), Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, SlopeDeno.ToString(), XPos + 50, Ypos - 50, 0, false, 120);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "=", XPos + 130, Ypos, 0, false, 100);
        XPos += 100;

        TextInstantiator.InstantiateText(FirstNumPlace, (-1/TanAngel).ToString(), XPos + 150, Ypos , 0, false, 130);


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextMeshProUGUI ADeno = GameObject.Find("120").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI AIntText = GameObject.Find("130").GetComponent<TextMeshProUGUI>();

        ADeno.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, ADeno.text, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain)));
        AIntText.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, AIntText.text, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));




        float AMinus = ((int.Parse(ADeno.text) * int.Parse(AIntText.text)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, AMinus.ToString(), Explain)));

        XPos = 50;
        Ypos -= 175;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "a", XPos, Ypos, 0, false, 100);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 50, Ypos, 0, false, 100);

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos + 100, Ypos, 0, false);


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "=", XPos + 150, Ypos, 0, false, 100);
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (AMinus.ToString()), Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, AMinus.ToString(), XPos + 200, Ypos, 0, false, 100);

        Ypos -= 175;


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        XPos += 50;
        TextInstantiator.InstantiateText(FirstNumPlace, "a = ", XPos, Ypos, 0, false, 100);

        float Aval = (float)(AMinus + int.Parse(Y1.text));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, AMinus.ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, AMinus.ToString(), XPos + 75, Ypos, 0, false, 96);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "+", XPos + 125, Ypos, 0, false, 100);

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos + 175, Ypos, 0, false, 95);


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 225, Ypos, 0, false, 100);


        TextMeshProUGUI AvalText = GameObject.Find("96").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Y1Text = GameObject.Find("95").GetComponent<TextMeshProUGUI>();

        AvalText.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, AvalText.text, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain)));
        Y1Text.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Aval.ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, Aval.ToString(), XPos + 300, Ypos, 0, false, 100);

    }
    public IEnumerator GetParallelSol()
    {
        YposTemp = Ypos;
        XPos = -430;
        float TanAngel = Mathf.Tan(int.Parse(angel.text) * Mathf.Deg2Rad);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "Since L1 is parallel to L2" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, $"L<sub>2</sub> // L<sub>1</sub>", XPos + 30, Ypos, 0, false);

        XPos += 250;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "1", Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $", m<sub>1</sub>=", XPos - 20, Ypos, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "2", Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>2</sub>", XPos + 100, Ypos, 0, false);

        //--------------------------------------------------------------------
        Ypos -= 175;
        XPos = -470;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));


        TextInstantiator.InstantiateText(FirstNumPlace, "a-" + Y1.text, XPos + 50, Ypos + 50, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 50, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));


        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeDeno.ToString(), Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, SlopeDeno.ToString(), XPos + 50, Ypos - 50, 0, false, 100);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "=", XPos + 130, Ypos, 0, false, 100);
        XPos += 100;

        TextInstantiator.InstantiateText(FirstNumPlace, (TanAngel).ToString(), XPos + 150, Ypos, 0, false, 110);


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextMeshProUGUI ADeno = GameObject.Find("100").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI AIntText = GameObject.Find("110").GetComponent<TextMeshProUGUI>();

        ADeno.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, ADeno.text, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain)));
        AIntText.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, AIntText.text, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));




        float AMinus = ((int.Parse(ADeno.text) * int.Parse(AIntText.text)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, AMinus.ToString(), Explain)));

        XPos = -470;
        Ypos -= 175;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "a", XPos, Ypos, 0, false, 100);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 50, Ypos, 0, false, 100);

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos + 100, Ypos, 0, false);


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "=", XPos + 150, Ypos, 0, false, 100);
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (AMinus.ToString()), Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, AMinus.ToString(), XPos + 200, Ypos, 0, false, 100);

        Ypos -= 175;


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "a = ", XPos, Ypos, 0, false, 100);

        float Aval = (float)(AMinus + int.Parse(Y1.text));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, AMinus.ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, AMinus.ToString(), XPos + 75, Ypos, 0, false, 98);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "+", XPos + 125, Ypos, 0, false, 100);

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos + 175, Ypos, 0, false, 97);


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 225, Ypos, 0, false, 100);


        TextMeshProUGUI AvalText = GameObject.Find("98").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Y1Text = GameObject.Find("97").GetComponent<TextMeshProUGUI>();

        AvalText.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, AvalText.text, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain)));
        Y1Text.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Aval.ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, Aval.ToString(), XPos + 300, Ypos, 0, false, 100);
    }
}
