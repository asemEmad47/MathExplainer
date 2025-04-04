using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstProblem : MonoBehaviour
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
    private bool Explain = false;
    public static AudioSource audioSource;
    public static string SpeakerName = "_Jenny_Eng";
    public static bool IsCalledFromOutSide = false;
    private float XTemp = 0;

    private int SlopeNue = 0;
    private int SlopeDeno = 0;    
    
    private int OriginalSlopeNue = 0;
    private int OriginalSlopeDeno = 0;
    Color BackGroundColor ;

    public static string Xval = "";
    public static string Yval = "";

    public static string FNue = "";
    public static string FDeno = "";


    private Button PauseBtn;
    private Button ResumeBtn;
    private void OnDisable()
    {
        SlopeNue = int.Parse(SlopeNuemerator.text);
        SlopeDeno = int.Parse(SlopeDenominator.text);

        XTemp = 0;

        SlopeNue = int.Parse(SlopeNuemerator.text);
        SlopeDeno = int.Parse(SlopeDenominator.text);

        Arrow.SetActive(false);

        if (!IsCalledFromOutSide)
        {
            XPos = -470;
            Ypos = 420;
            SLStaicFunctions.RemoveTexts();
        }

    }
    public void Start()
    {
        PauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        ResumeBtn = GameObject.Find("Resume").GetComponent<Button>();
        PauseBtn.onClick.AddListener(PauseScript.Pause);
        ResumeBtn.onClick.AddListener(PauseScript.Resume);
        AdditionScript.IscalledFromOutSide = true;
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
        SlopeNue = int.Parse(SlopeNuemerator.text);
        SlopeDeno = int.Parse(SlopeDenominator.text);
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

            if (Explain || X1.text.Equals("") || Y1.text.Equals("") || SlopeDeno ==0)
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
        if (!IsCalledFromOutSide)
        {
            SlopeNue = int.Parse(SlopeNuemerator.text);
            SlopeDeno = int.Parse(SlopeDenominator.text);

        }
        OriginalSlopeNue = SlopeNue;
        OriginalSlopeDeno = SlopeDeno;
        XTemp = XPos;

        if (!IsCalledFromOutSide)
        {
            XPos = -470;
            Ypos = 420;
            SLStaicFunctions.RemoveTexts();
            Arrow.SetActive(false);
            yield return StartCoroutine(GetLineThreeSol());

        }
        yield return StartCoroutine(GetLineFourSol());
        yield return StartCoroutine(GetLineFiveSol());
        yield return StartCoroutine(GetLineSixSol());
        yield return StartCoroutine(GetLineSevenSol());
        yield return StartCoroutine(GetLineEightSol());
        yield return StartCoroutine(GetLineNineSol());
        Explain = false;
    }

    public void SetComponents(TMP_InputField X1 , TMP_InputField Y1 , TextMeshProUGUI FirstNumPlace , GameObject Line , GameObject Arrow , float Xpos , float Ypos , int SlopeNue , int SlopeDeno , bool Explain , Color color) {
        
        this.X1 = X1;
        this.Y1 = Y1;
        this.FirstNumPlace = FirstNumPlace;
        this.Line = Line;
        this.Arrow = Arrow;
        this.XPos = Xpos;
        this.Ypos = Ypos;
        this.SlopeNue = SlopeNue;
        this.SlopeDeno = SlopeDeno;
        this.OriginalSlopeDeno = SlopeDeno;
        this.OriginalSlopeNue = SlopeNue;
        this.Explain = Explain;
        this.BackGroundColor = color;
        AdditionVoiceSpeaker.IsEng = true;
    }
    public IEnumerator GetLineThreeSol()
    {

        XPos = XTemp;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "first put m equals to slope" + SpeakerName, Explain)));
        SlopeNuemerator.GetComponent<Image>().color = Color.red;
        SlopeDenominator.GetComponent<Image>().color = Color.red;
        TextInstantiator.InstantiateText(FirstNumPlace, "m =", XPos-10, Ypos, 0, false);

        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 150, Ypos, -1, Explain));
        if (!IsCalledFromOutSide) {
            OriginalSlopeDeno = SlopeDeno;
            OriginalSlopeNue = SlopeNue;
        }
        SlopeNuemerator.GetComponent<Image>().color = BackGroundColor;
        SlopeDenominator.GetComponent<Image>().color = BackGroundColor;
        if(SlopeDeno==1)
            Ypos -= 100;
        else
            Ypos -= 125;

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
        if (SlopeDeno == 1)
            Ypos -= 125;
        else
            Ypos -= 150;

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

        if (SlopeDeno != 1)
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "x ", XPos + 220, Ypos, 0, false);

            TextInstantiator.InstantiateText(FirstNumPlace, " + c  (" + X1.text + "," + Y1.text + ")", XPos + 400, Ypos, 0, false);
        }
        else
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "x ", XPos + 190, Ypos, 0, false);

            TextInstantiator.InstantiateText(FirstNumPlace, " + c  (" + X1.text + "," + Y1.text + ")", XPos + 380, Ypos, 0, false);
        }

        if (SlopeNuemerator != null)
        {
            SlopeNuemerator.GetComponent<Image>().color = BackGroundColor;
            SlopeDenominator.GetComponent<Image>().color = BackGroundColor;
        }

        if (SlopeDeno == 1)
            Ypos -= 150;
        else
            Ypos -= 200;
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

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "replace y" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text.ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text + " = ", XPos - 10, Ypos, 0, false);
        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 150, Ypos, 6 , false));
        try
        {
            Y1.GetComponent<Image>().color = BackGroundColor;

        }
        catch (System.Exception e)
        {

            Debug.Log(e);
        }
        X1.GetComponent<Image>().color = Color.red;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "replace x" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X1.text.ToString(), Explain)));
        X1.GetComponent<Image>().color = BackGroundColor;

        if (SlopeDeno != 1)
        {
            TextInstantiator.InstantiateText(FirstNumPlace, " × " + X1.text, XPos + 280, Ypos, 0, false, 6666);
            TextInstantiator.InstantiateText(FirstNumPlace, " + c ", XPos + 430, Ypos, 0, false);

        }
        else
        {
            TextInstantiator.InstantiateText(FirstNumPlace, " × " + X1.text, XPos + 260, Ypos, 0, false, 6666);
            TextInstantiator.InstantiateText(FirstNumPlace, " + c ", XPos + 410, Ypos, 0, false);
        }
        if ((SlopeDeno)  == 1 )
            Ypos -= 150;
        else
            Ypos -= 200;
        yield return null;
    }
    public IEnumerator GetLineSevenSol()
    {
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "multiply x with the slope" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));

        TextMeshProUGUI nue = new(), deno = new(), x = new();
        try
        {
            nue = GameObject.Find("6").GetComponent<TextMeshProUGUI>();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }   
        
        try
        {
            deno = GameObject.Find("36").GetComponent<TextMeshProUGUI>();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }       
        
        try
        {
            x = GameObject.Find("6666").GetComponent<TextMeshProUGUI>();
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


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X1.text.ToString(), Explain)));
        x.color = Color.red;


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        SlopeNue *= int.Parse(X1.text);
        FractionCalculator.SimplifyFraction(ref SlopeNue, ref SlopeDeno);

        if (SlopeDeno != 1)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeDeno.ToString(), Explain)));
            deno.color = Color.red;

        }
        else
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (SlopeNue / SlopeDeno).ToString(), Explain)));

        }

        XPos = XTemp;

        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text + " = ", XPos - 10, Ypos, 0, false);
        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 150, Ypos, -1 , false));
        TextInstantiator.InstantiateText(FirstNumPlace, " + c ", XPos + 250, Ypos, 0, false);
        if (SlopeDeno == 1)
            Ypos -= 150;
        else
            Ypos -= 200;
        yield return null;
    }
    public IEnumerator GetLineEightSol()
    {
        XPos = XTemp;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "replace right handside with left handside" + SpeakerName, Explain)));

        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos - 20, Ypos,10 , false));

        TextInstantiator.InstantiateText(FirstNumPlace, "  + c ", XPos + 75, Ypos, 0, false);

        TextInstantiator.InstantiateText(FirstNumPlace, " = " + Y1.text, XPos + 200, Ypos, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so send" + SpeakerName, Explain)));


        if (SlopeDeno == 1)
        {
            TextMeshProUGUI SendedValue = GameObject.Find("10").GetComponent<TextMeshProUGUI>();
            SendedValue.color = Color.red;
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (SlopeNue).ToString(), Explain)));

        }
        else
        {
            TextMeshProUGUI SendedNueValue = GameObject.Find("10").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI SendedDenoValue = GameObject.Find("100").GetComponent<TextMeshProUGUI>();

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));
            SendedNueValue.color = Color.red;

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeDeno.ToString(), Explain)));
            SendedDenoValue.color = Color.red;

        }
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "to the right side with a negative sign" + SpeakerName, Explain)));
        CharacterProbs.CenterInPos(XPos +190, Ypos+30, ref Arrow, FirstNumPlace);
        Arrow.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        if (SlopeDeno == 1)
        {
            TextInstantiator.InstantiateText(FirstNumPlace, " - " + ((SlopeNue / SlopeDeno)).ToString(), XPos + 350, Ypos, 0, false, 9999);
            TextMeshProUGUI SendedRight = GameObject.Find("9999").GetComponent<TextMeshProUGUI>();
            SendedRight.color = Color.red;
        }
        else
        {
            TextInstantiator.InstantiateText(FirstNumPlace, " - " , XPos + 340, Ypos, 0, false);
            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 430, Ypos, 9, Explain));
            TextMeshProUGUI nue = GameObject.Find("9").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI deno = GameObject.Find("81").GetComponent<TextMeshProUGUI>();
            nue.color = Color.red;
            deno.color = Color.red;
        }

        if (SlopeDeno == 1)
            Ypos -= 150;
        else
            Ypos -= 200;
        yield return null;
    }
    public IEnumerator GetLineNineSol()
    {
        XPos = XTemp;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so c equals to" + SpeakerName, Explain)));


        TextInstantiator.InstantiateText(FirstNumPlace, "c =", XPos, Ypos, 0, false);
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text.ToString(), Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));


        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text + " - ", XPos + 150, Ypos, 0, false);
        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 250, Ypos, -1 , Explain));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        XPos += 30;
        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 300, Ypos, 0, false);

        FractionCalculator.SubtractFractionFromNumber(int.Parse(Y1.text), ref SlopeNue, ref SlopeDeno);

        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 400, Ypos , -1, Explain));

        XPos -= 30;
        Ypos -= 300;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "straight line equation sol" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "y" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        Ypos += 120;
        TextInstantiator.InstantiateText(FirstNumPlace, "y =  ", XPos, Ypos, 0, false);
        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, OriginalSlopeNue, OriginalSlopeDeno, XPos + 150, Ypos,-1, Explain));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "x" + SpeakerName, Explain)));


        if (int.Parse(SlopeNuemerator.text)% int.Parse(SlopeDenominator.text) !=0)
        {
            TextInstantiator.InstantiateText(FirstNumPlace, "x", XPos + 250, Ypos, 0, false);

            if (SlopeDeno >= 0 && SlopeNue >= 0)
            {
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));

                TextInstantiator.InstantiateText(FirstNumPlace, "+ " + SlopeNue, XPos + 330, Ypos, 0, false);
            }

            else
            {
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));

                TextInstantiator.InstantiateText(FirstNumPlace, SlopeNue.ToString(), XPos + 330, Ypos, 0, false);

            }


        }
        else
        {
            Debug.Log("here");
            TextInstantiator.InstantiateText(FirstNumPlace, "x", XPos + 200, Ypos, 0, false);

            if (SlopeDeno >= 0 && SlopeNue >= 0)
            {

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "plus" + SpeakerName, Explain)));

                TextInstantiator.InstantiateText(FirstNumPlace, "+ ", XPos + 250, Ypos, 0, false);

            }
            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SlopeNue, SlopeDeno, XPos + 300, Ypos, -1, Explain));


        }


        //slope nue

        if (SlopeDeno == 1)
            Ypos -= 150;
        else
            Ypos -= 200;
        yield return null;
    }

}

