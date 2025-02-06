using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SimplifyScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField Nuemerator;
    [SerializeField] private TMP_InputField Denominator;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI Line;

    [SerializeField] private Button LangBtn;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    private bool Explain = false;
    public static string SpeakerName = "_Sonya_Eng";
    public static bool IsEng = true;

    public static string FirstNum = "";
    public static string SecNum = "";

    public void ExplainBtnAction()
    {
        Explain = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(solve());
    }
    public void SolveBtnAction()
    {
        Explain = false;
        StartCoroutine(solve());
    }
    public void OutSideSolve(bool Explain)
    {
        this.Explain = Explain;
        StartCoroutine(solve());

    }
    void Start()
    {

        if (!FirstNum.Equals(""))
        {
            try
            {
                Nuemerator.text = FirstNum;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                Denominator.text = SecNum;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

        }
        Nuemerator.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        Denominator.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

        InputFieldsActions.InitializePlaceholders(Nuemerator);
        InputFieldsActions.InitializePlaceholders(Denominator);

        AdditionVoiceSpeaker.NumPlace = "JennySound/JennyNumbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";

        AdditionVoiceSpeaker.LoadAllAudioClips();
    }

    void Update()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if (IsEng)
        {
            SpeakerName = "_Jenny_Eng";
            AdditionVoiceSpeaker.IsEng = true;
        }
        else
        {
            SpeakerName = "_Jenny_Egy";
            AdditionVoiceSpeaker.IsEng = true;
        }

        if (Nuemerator.text.Length == 0 || Denominator.text.Length == 0 || Explain)
        {
            GameObject ExplainBtn = GameObject.Find("Explain");
            GameObject SolveBtn = GameObject.Find("Solve");
            UnityEngine.UI.Button button = ExplainBtn.GetComponent<UnityEngine.UI.Button>();
            button.interactable = false;

            UnityEngine.UI.Button Solvebutton = SolveBtn.GetComponent<UnityEngine.UI.Button>();
            Solvebutton.interactable = false;

        }
        if (Nuemerator.text.Length != 0 && Denominator.text.Length != 0 && !Explain)
        {
            GameObject SolveBtn = GameObject.Find("Solve");
            UnityEngine.UI.Button Solvebutton = SolveBtn.GetComponent<UnityEngine.UI.Button>();
            Solvebutton.interactable = true;

            GameObject ExplainBtn = GameObject.Find("Explain");
            UnityEngine.UI.Button button = ExplainBtn.GetComponent<UnityEngine.UI.Button>();
            button.interactable = true;
        }
    }


    public IEnumerator solve()
    {
        int ParsedNuemerator = int.Parse(Nuemerator.text);
        int ParsedDenominator = int.Parse(Denominator.text);
        int Devisor = 0;
        int XPos = -250;
        int YPos = 200;
        ResetValues.ResetAllValues(Line,FirstNumPlace,FirstNumPlace,FirstNumPlace);
        Line.gameObject.SetActive(true);
        TextInstantiator.InstantiateText(FirstNumPlace, (ParsedNuemerator).ToString(), XPos, YPos, 0, true , 1);
        YPos -= 50;
        TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos, YPos, 0, true , -99);
        YPos -= 50;
        TextInstantiator.InstantiateText(FirstNumPlace, (ParsedDenominator).ToString(), XPos, YPos, 0, true , 0);

        int i = 12;
        if(ParsedNuemerator < 12 || ParsedDenominator< 12)
        {
            i = Math.Min(ParsedNuemerator, ParsedDenominator);
        }
        for (; i >=1; i--) {
            if (ParsedNuemerator % i == 0 && ParsedDenominator % i == 0)
            {
                Devisor = i;
                break;
            }
        }

        if (Devisor == 1) {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"there is no common divisor between" + SpeakerName , Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,ParsedNuemerator.ToString() , Explain)));

            TextMeshProUGUI Nue = GameObject.Find("1").GetComponent<TextMeshProUGUI>();
            Nue.color = Color.red;
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,ParsedDenominator.ToString(), Explain)));

            TextMeshProUGUI Deno = GameObject.Find("0").GetComponent<TextMeshProUGUI>();
            Deno.color = Color.red;

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"this is" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"the" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"simplest form" + SpeakerName, Explain)));

        }
        else
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"each of these numbers can be divided by" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,Devisor.ToString(), Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, "÷" + Devisor, XPos+15*(Math.Abs(Nuemerator.text.Length - Denominator.text.Length))+90, YPos, 70, true, -1);
            TextMeshProUGUI NueDevisor = GameObject.Find("-1").GetComponent<TextMeshProUGUI>();
            NueDevisor.fontSize = 45;
            yield return new WaitForSeconds(1f);
            TextInstantiator.InstantiateText(FirstNumPlace, "÷" + Devisor, XPos + 5 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length))+90, YPos, -20, true, -2);

            TextMeshProUGUI DenoDevisor = GameObject.Find("-2").GetComponent<TextMeshProUGUI>();
            DenoDevisor.fontSize = 45;
            yield return new WaitForSeconds(1f);

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,ParsedNuemerator.ToString(), Explain)));

            TextMeshProUGUI Nue = GameObject.Find("1").GetComponent<TextMeshProUGUI>();
            Nue.color = Color.red;

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"divide" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,Devisor.ToString(), Explain)));

            NueDevisor.color = Color.red;

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"equal" + SpeakerName, Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 5 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 180, YPos, 50, true, -1);

            int DevidedNue = ParsedNuemerator / Devisor;
            int DevidedDeno = ParsedDenominator / Devisor;

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,DevidedNue.ToString(), Explain)));


            TextInstantiator.InstantiateText(FirstNumPlace, DevidedNue.ToString(), XPos + 5 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 270, YPos, 100, true, -4);


            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,ParsedDenominator.ToString(), Explain)));

            TextMeshProUGUI Deno = GameObject.Find("0").GetComponent<TextMeshProUGUI>();
            Deno.color = Color.red;


            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"divide" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,Devisor.ToString(), Explain)));

            DenoDevisor.color = Color.red;

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"equal" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,DevidedDeno.ToString(), Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos + 10 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 270, YPos, 50, true );

            TextInstantiator.InstantiateText(FirstNumPlace, DevidedDeno.ToString(), XPos + 5 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 270, YPos, 0, true , -5);


            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"the" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"simplest form" + SpeakerName, Explain)));

            if (DevidedNue % DevidedDeno != 0) {

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,DevidedNue.ToString(), Explain)));

                NueDevisor = GameObject.Find("-4").GetComponent<TextMeshProUGUI>();
                NueDevisor.color = Color.red;

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"over" + SpeakerName, Explain)));


                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,DevidedDeno.ToString(), Explain)));


                DenoDevisor = GameObject.Find("-5").GetComponent<TextMeshProUGUI>();
                DenoDevisor.color = Color.red;
            }
            else
            {
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"equal" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,(DevidedNue / DevidedDeno).ToString(), Explain   )));

                TextInstantiator.InstantiateText(FirstNumPlace, " = "+ (DevidedNue/DevidedDeno).ToString(), XPos + 5 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 380, YPos, 50, true, -4);

            }
        }
        Explain = false;
    }
}
