using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NinethProblem : MonoBehaviour
{

    [SerializeField] TMP_InputField Y1;

    [SerializeField] TMP_InputField SlopeNuemerator;
    [SerializeField] TMP_InputField SlopeDenominator;

    [SerializeField] TextMeshProUGUI FirstNumPlace;
    [SerializeField] GameObject Line;
    [SerializeField] GameObject Arrow;
    [SerializeField] float XPos;
    [SerializeField] float Ypos;
    private bool Explain = false;
    public static AudioSource audioSource;
    public static string SpeakerName = "_Jenny_Eng";
    private float XTemp = 0;

    private int SlopeNue = 0;
    private int SlopeDeno = 0;

    private int OriginalSlopeNue = 0;
    private int OriginalSlopeDeno = 0;
    Color BackGroundColor;

    public static string Yval = "";

    public static string FNue = "";
    public static string FDeno = "";


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
        AdditionVoiceSpeaker.NumPlace = "JennySound/Numbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        SLStaicFunctions.SpeakerName = SpeakerName;
        BackGroundColor = Y1.GetComponent<Image>().color;
        Y1.onValidateInput = InputFieldsActions.ValidateEqsInput;

        SlopeNue = int.Parse(SlopeNuemerator.text);
        SlopeDeno = int.Parse(SlopeDenominator.text);

    }
    private void Update()
    {
        PauseScript.ControlPause();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Button SolveBtn = GameObject.Find("Solve").GetComponent<Button>();
        Button ExplainBtn = GameObject.Find("Explain").GetComponent<Button>();

        if (Y1 != null)
        {

            if (Explain ||Y1.text.Equals("") || Y1.text.Equals(""))
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
        XPos = -470;
        Ypos = 420;
        Explain = false;
        StartCoroutine(SolveStepByStep());
    }
    public IEnumerator SolveStepByStep()
    {
        XTemp = XPos;

        yield return StartCoroutine(GetLineThreeSol());

        yield return StartCoroutine(GetLineFourSol());
        yield return StartCoroutine(GetLineFiveSol());
        yield return StartCoroutine(GetLineSixSol());

        yield return StartCoroutine(GetLineNineSol());
        yield return StartCoroutine(GetLineTenSol());
        Explain = false;
    }

    public IEnumerator GetLineThreeSol()
    {
        SLStaicFunctions.RemoveTexts();
        SlopeNue = int.Parse(SlopeNuemerator.text);
        SlopeDeno = int.Parse(SlopeDenominator.text);
        TextInstantiator.InstantiateText(FirstNumPlace, "a)", XPos - 10, Ypos, 0, false,-333);
        TextMeshProUGUI G1 = GameObject.Find("-333").GetComponent<TextMeshProUGUI>();
        G1.color = Color.blue;
        Ypos -= 100;

        XPos = XTemp;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "first put m equals to slope" + SpeakerName, Explain)));
        SlopeNuemerator.GetComponent<Image>().color = Color.red;
        SlopeDenominator.GetComponent<Image>().color = Color.red;
        TextInstantiator.InstantiateText(FirstNumPlace, "m =", XPos - 10, Ypos, 0, false);

        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 150, Ypos, -1, Explain));

        OriginalSlopeDeno = SlopeDeno;
        OriginalSlopeNue = SlopeNue;

        SlopeNuemerator.GetComponent<Image>().color = BackGroundColor;
        SlopeDenominator.GetComponent<Image>().color = BackGroundColor;
        Ypos -= 100;
    }
    public IEnumerator GetLineFourSol()
    {
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "write the equation of the straight line" + SpeakerName, Explain)));

        XPos = XTemp;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "y = ", XPos - 10, Ypos, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "m" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "mx", XPos + 120, Ypos, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "c" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, " + c", XPos + 200, Ypos, 0, false);
        Ypos -= 125;

        yield return null;
    }
    public IEnumerator GetLineFiveSol()
    {
        XPos = XTemp;

        TextInstantiator.InstantiateText(FirstNumPlace, "y = ", XPos - 10, Ypos, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "replace m with the slope value" + SpeakerName, Explain)));
        if (SlopeNuemerator != null)
        {
            SlopeNuemerator.GetComponent<Image>().color = Color.red;
            SlopeDenominator.GetComponent<Image>().color = Color.red;
        }
        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 120, Ypos, -1, Explain));

        if (SlopeNue % SlopeDeno != 0)
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "x + c ", XPos + 300, Ypos, 0, false);
        }
        else
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "x + c ", XPos + 230, Ypos, 0, false);
        }

        if (SlopeNuemerator != null)
        {
            SlopeNuemerator.GetComponent<Image>().color = BackGroundColor;
            SlopeDenominator.GetComponent<Image>().color = BackGroundColor;
        }

        Ypos -= 100;
        yield return null;
    }
    public IEnumerator GetLineSixSol()
    {

        XPos = XTemp;
        try
        {
            Y1.GetComponent<Image>().color = Color.red;
        }
        catch (System.Exception e)
        {

            Debug.Log(e);
        }

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "SL intesects with y" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text.ToString(), Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so c equals to" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text.ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "c  = " + Y1.text, XPos + 150, Ypos, 0, false, 6666);
        Y1.GetComponent<Image>().color = BackGroundColor;


        Ypos -= 125;
        yield return null;
    }

    public IEnumerator GetLineNineSol()
    {

        XPos = XTemp;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "straight line equation sol" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));


        TextInstantiator.InstantiateText(FirstNumPlace, "y =  ", XPos , Ypos, 0, false);


        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, OriginalSlopeNue, OriginalSlopeDeno, XPos + 150, Ypos, -1, Explain));


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain)));


        if (SlopeNue % SlopeDeno == 0)
        {
            XPos -= 30;
            TextInstantiator.InstantiateText(FirstNumPlace, "x", XPos + 250, Ypos, 0, false);

        }
        else
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "x", XPos + 250, Ypos, 0, false);

        }
        if(int.Parse(Y1.text) >= 0)
        {

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
            TextInstantiator.InstantiateText(FirstNumPlace, "+ " + Y1.text, XPos + 350, Ypos, 0, false);

        }
        else {
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
            TextInstantiator.InstantiateText(FirstNumPlace,   Y1.text, XPos + 340, Ypos, 0, false);
        }
        //slope nue
        if (SlopeNue % SlopeDeno == 0)
            XPos += 30;
        Ypos -= 175;

        yield return null;
}
    public IEnumerator GetLineTenSol(){
        TextInstantiator.InstantiateText(FirstNumPlace, "b)", XPos - 10, Ypos, 0, false , -444);
        TextMeshProUGUI G2 = GameObject.Find("-444").GetComponent<TextMeshProUGUI>();
        G2.color = Color.blue;
        Ypos -= 100;
        XPos = XTemp;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "SL intesects with y" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text.ToString(), Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "then" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "x = 0", XPos + 150, Ypos, 0, false);
        Ypos -= 125;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "replace x" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "y = ", XPos , Ypos, 0, false);
        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 150, Ypos, 9, false));



        if (SlopeNue % SlopeDeno != 0)
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "× 0", XPos + 300, Ypos, 0, false, 11);
        }
        else
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "× 0", XPos + 270, Ypos, 0, false, 11);

        }
        if (int.Parse(Y1.text) >= 0)
        {
            TextInstantiator.InstantiateText(FirstNumPlace, " + " + Y1.text, XPos + 400, Ypos, 0, false, 12);

        }
        else
        {
            TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos + 380, Ypos, 0, false, 12);

        }
        Ypos -= 125;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so the point equal" + SpeakerName, Explain)));


        TextMeshProUGUI nue = new(), deno = new(), x = GameObject.Find("11").GetComponent<TextMeshProUGUI>() , c = GameObject.Find("12").GetComponent<TextMeshProUGUI>();
        try
        {
            nue = GameObject.Find("9").GetComponent<TextMeshProUGUI>();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

        try
        {
            deno = GameObject.Find("81").GetComponent<TextMeshProUGUI>();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "the point (0 , ", XPos+300 , Ypos, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and" + SpeakerName, Explain)));

        nue.color = Color.red;
        try
        {
            deno.color = Color.red;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));
        nue.color = Color.red;
        if (SlopeDeno != 1)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeDeno.ToString(), Explain)));
            deno.color = Color.red;

        }

        x.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain)));
        c.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text +")", XPos+600 , Ypos, 0, false);

        //slope nue

        Ypos -= 175;

        yield return null;
    }

}
