using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SixthSlopeProblem : MonoBehaviour
{
    [SerializeField] TMP_InputField a1;
    [SerializeField] TMP_InputField b1;
    [SerializeField] TMP_InputField c1;    
    
    [SerializeField] TMP_InputField a2;
    [SerializeField] TMP_InputField b2;
    [SerializeField] TMP_InputField c2;

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
            a1.text = A1;
        }
        if (!B1.Equals(""))
        {
            b1.text = B1;

        }
        if (!C1.Equals(""))
        {
            c1.text = C1;

        }
        if (!A2.Equals(""))
        {
            a2.text = A2;

        }
        if (!B2.Equals(""))
        {
            b2.text = B2;

        }
        if (!C2.Equals(""))
        {
            c2.text = C2;

        }



        XPosTemp = XPos;
        AdditionVoiceSpeaker.NumPlace = "JennySound/Numbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        SLStaicFunctions.SpeakerName = SpeakerName;
        BackGroundColor = a1.GetComponent<Image>().color;
        a1.onValidateInput = InputFieldsActions.ValidateEqsInput;
        b1.onValidateInput = InputFieldsActions.ValidateEqsInput;
        c1.onValidateInput = InputFieldsActions.ValidateEqsInput;    
        
        a2.onValidateInput = InputFieldsActions.ValidateEqsInput;
        b2.onValidateInput = InputFieldsActions.ValidateEqsInput;
        c2.onValidateInput = InputFieldsActions.ValidateEqsInput;

    }
    private void Update()
    {
        PauseScript.ControlPause();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Button SolveBtn = GameObject.Find("Solve").GetComponent<Button>();
        Button ExplainBtn = GameObject.Find("Explain").GetComponent<Button>();

        if (a1 != null)
        {

            if (Explain || a1.text.Equals("") || b1.text.Equals("") || c1.text.Equals("") || a2.text.Equals("") || b2.text.Equals("") || c2.text.Equals(""))
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
        Ypos = 340;
        XPos = -470;
        SLStaicFunctions.RemoveTexts();
        AdditionVoiceSpeaker.IsEng = true;
        yield return StartCoroutine(GetLineOneSol());
        yield return StartCoroutine(GetLineTwoSol());

        Explain = false;
    }
    public IEnumerator GetLineOneSol()
    {
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "first" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "put" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "1", Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "b" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>1</sub> = ", XPos, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, "-a", XPos + 200, Ypos + 50, 0, false);
        yield return StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 200, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));
        TextInstantiator.InstantiateText(FirstNumPlace, "b", XPos + 200, Ypos - 50, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 300, Ypos, 0, false);


        int AInt = -int.Parse(a1.text);
        int BInt = int.Parse(b1.text);

        XPos += 150;

        TextInstantiator.InstantiateText(FirstNumPlace, "-" + a1.text, XPos + 270, Ypos + 50, 0, false);

        a1.GetComponent<Image>().color = Color.red;

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (AInt).ToString(), Explain)));

        yield return StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 270, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        a1.GetComponent<Image>().color = BackGroundColor;

        b1.GetComponent<Image>().color = Color.red;

        TextInstantiator.InstantiateText(FirstNumPlace, b1.text, XPos + 270, Ypos - 50, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, BInt.ToString(), Explain)));

        b1.GetComponent<Image>().color = BackGroundColor;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 350, Ypos, 0, false);


        FractionCalculator.SimplifyFraction(ref AInt, ref BInt);

        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, AInt, BInt, XPos + 450, Ypos, -1, Explain));
        Ypos -= 175;
        XPos -= 150;


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "put" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "2", Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "a" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "b" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>2</sub> = ", XPos, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, "-a", XPos + 200, Ypos + 50, 0, false);
        yield return StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 200, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));
        TextInstantiator.InstantiateText(FirstNumPlace, "b", XPos + 200, Ypos - 50, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 300, Ypos, 0, false);


        int AInt1 = -int.Parse(a2.text);

        XPos += 150;

        TextInstantiator.InstantiateText(FirstNumPlace, "-" + a2.text, XPos + 270, Ypos + 50, 0, false);

        a2.GetComponent<Image>().color = Color.red;

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (AInt1).ToString(), Explain)));

        yield return StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 270, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        a2.GetComponent<Image>().color = BackGroundColor;

        b2.GetComponent<Image>().color = Color.red;

        TextInstantiator.InstantiateText(FirstNumPlace, b2.text, XPos + 270, Ypos - 50, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, b2.text + SpeakerName, Explain)));


        b2.GetComponent<Image>().color = BackGroundColor;


        Ypos -= 175;

    }
    public IEnumerator GetLineTwoSol()
    {
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "Since L1 is parallel to L2" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, " L1 // L2 ", XPos-50 , Ypos, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "1", Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "2", Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, $", m<sub>1</sub> = ", XPos+250, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, $"m<sub>2</sub> ", XPos+400, Ypos, 0, false);

        Ypos -= 175;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));

        int AInt = -int.Parse(a1.text);
        int BInt = int.Parse(b1.text);

        FractionCalculator.SimplifyFraction(ref AInt, ref BInt);

        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, AInt, BInt, XPos , Ypos, 10, Explain));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 120, Ypos, 0, false);



        int AInt1 = -int.Parse(a2.text);
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (AInt1).ToString(), Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, AInt1.ToString(), XPos + 300, Ypos +50, 0, false ,110);

        yield return StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 300, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, b2.text, XPos + 300, Ypos - 50, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, b2.text + SpeakerName, Explain)));

        Ypos -= 175;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "b" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        TextMeshProUGUI BNue = GameObject.Find("110").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI ANue = GameObject.Find("10").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI ADeno = GameObject.Find("100").GetComponent<TextMeshProUGUI>();

        BNue.color = Color.red;
        TextInstantiator.InstantiateText(FirstNumPlace, "b = ", XPos, Ypos , 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, BNue.text, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, BNue.text, XPos+75, Ypos + 50, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "×", XPos + 150, Ypos + 50, 0, false);

        ADeno.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, ADeno.text, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, ADeno.text, XPos + 225, Ypos+50, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (int.Parse(BNue.text) * int.Parse(ADeno.text)).ToString(), Explain)));


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 150, Ypos - 30, 0), "BigLine", FirstNumPlace, Explain)));

        ANue.color = Color.red;

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, ANue.text, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, ANue.text, XPos +150, Ypos - 50, 0, false);


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));



        Ypos -= 175;
        TextInstantiator.InstantiateText(FirstNumPlace,"b = ", XPos , Ypos , 0, false);
        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, (int.Parse(BNue.text) * int.Parse(ADeno.text)),int.Parse(ANue.text), XPos+150, Ypos, 10, Explain));


    }

}
