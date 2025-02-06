using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThirdProblem : MonoBehaviour
{
    [SerializeField] TMP_InputField X1;
    [SerializeField] TMP_InputField Y1;
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

    TextMeshProUGUI X1Point;
    TextMeshProUGUI Y1Point;
    TextMeshProUGUI X2Point;
    TextMeshProUGUI Y2Point;

    public static string X = "";
    public static string Y = "";

    private void OnDisable()
    {
        XTemp = XPos;
        XPos = -470;
        Ypos = 500;
        Arrow.SetActive(false);
        SLStaicFunctions.RemoveTexts();

    }
    private void Start()
    {
        if (!X.Equals(""))
        {
            X1.text = X;
        }
        if (!Y.Equals(""))
        {
            Y1.text = Y;
        }
        AdditionVoiceSpeaker.NumPlace = "JennySound/JennyNumbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        SLStaicFunctions.SpeakerName = SpeakerName;

        X1.onValidateInput = InputFieldsActions.ValidateEqsInput;
        Y1.onValidateInput = InputFieldsActions.ValidateEqsInput;
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

        if (Explain || X1.text.Equals("")  || Y1.text.Equals("") )
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
        Ypos = 500;
        SLStaicFunctions.RemoveTexts();
        Arrow.SetActive(false);
        yield return StartCoroutine(GetLineOneSol());
        yield return StartCoroutine(GetLineTwoSol());
        FirstProblem.IsCalledFromOutSide = true;
        FirstProblem firstProblem = FirstProblemObj.GetComponent<FirstProblem>();
        GameObject inputFieldObject = new GameObject("DynamicInputField");
        TMP_InputField inputField = inputFieldObject.AddComponent<TMP_InputField>();
        inputField.text = "0";
        firstProblem.SetComponents(X1, inputField, FirstNumPlace, Line, Arrow, -470, Ypos, SlopeNue, SlopeDeno, Explain, BackGroundColor);
        yield return StartCoroutine(firstProblem.SolveStepByStep());
        Explain = false;
    }
    public IEnumerator GetLineOneSol()
    {

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "straight line intersects with x" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X1.text, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "(" , XPos, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace,   X1.text , XPos + 50, Ypos, 0, false , -88);
        TextInstantiator.InstantiateText(FirstNumPlace,   ", " , XPos + 100, Ypos, 0, false);


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "0" , XPos + 170, Ypos, 0, false , -77);
        TextInstantiator.InstantiateText(FirstNumPlace, ")" , XPos + 230, Ypos, 0, false );


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and with y axis at" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "(" , XPos + 300, Ypos, 0, false);
        TextInstantiator.InstantiateText(FirstNumPlace, "0" , XPos + 350, Ypos, 0, false, -66);
        TextInstantiator.InstantiateText(FirstNumPlace, "," , XPos + 420, Ypos, 0, false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text , XPos + 490, Ypos, 0, false , -55);
        TextInstantiator.InstantiateText(FirstNumPlace, ")", XPos + 540, Ypos, 0, false );
        Ypos -= 150;

    }
    public IEnumerator GetLineTwoSol()
    {
        X1Point = GameObject.Find("-88").GetComponent<TextMeshProUGUI>();
        Y1Point = GameObject.Find("-77").GetComponent<TextMeshProUGUI>();       
        
        X2Point = GameObject.Find("-66").GetComponent<TextMeshProUGUI>();
        Y2Point = GameObject.Find("-55").GetComponent<TextMeshProUGUI>();
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so m equals to" + SpeakerName, Explain)));

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
        Y1.GetComponent<Image>().color = Color.red;
        Y2Point.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        if (int.Parse(Y1.text) < 0)
            XPos += 30;
        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos, Ypos, 0, false, 1);
        XPos += 50;
        Y1.GetComponent<Image>().color = BackGroundColor;

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 20, Ypos, 0, false, 0);


        XPos += 60;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));
        Y1Point.color = Color.red;
        TextInstantiator.InstantiateText(FirstNumPlace, "0", XPos, Ypos, 0, false, 11);

        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos - 50, Ypos - 30, 0), "BigLine", FirstNumPlace, Explain)));

        XPos -= 100;
        Ypos -= 50;
        X2Point.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "0", XPos, Ypos, 0, false, 2);

        XPos += 40;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        TextInstantiator.InstantiateText(FirstNumPlace, "-", XPos + 20, Ypos, 0, false);
        X1.GetComponent<Image>().color = Color.red;
        X1Point.color = Color.red;

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

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));
        y2.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));
        y1.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

        Ypos += 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Y1.text, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, Y1.text, XPos + 100, Ypos, 0, false);
        SlopeNue = (int.Parse(Y1.text));

        // voice for over 
        Ypos -= 50;

        yield return (StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 100, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain)));

        Ypos -= 50;
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));
        x2.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, X1.text, Explain)));
        x1.color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (- int.Parse(X1.text)).ToString(), Explain)));


        TextInstantiator.InstantiateText(FirstNumPlace, ( - int.Parse(X1.text)).ToString(), XPos + 100, Ypos, 0, false);
        SlopeDeno =  - int.Parse(X1.text);

        int OldNue = SlopeNue;

        FractionCalculator.SimplifyFraction(ref SlopeNue, ref SlopeDeno);
        if (SlopeNue > 0 && SlopeDeno < 0)
        {
            SlopeNue = -SlopeNue;
            SlopeDeno = -SlopeDeno;
        }
        if (OldNue != SlopeNue && SlopeDeno!=1)
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
        else if(SlopeDeno ==1)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 180, Ypos + 50, 0, false);

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SlopeNue.ToString(), Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, SlopeNue.ToString(), XPos + 250, Ypos + 50, 0, false);

        }
        Ypos -= 100;

    }

}
