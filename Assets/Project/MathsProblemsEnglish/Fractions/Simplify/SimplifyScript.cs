using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SimplifyScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField Nuemerator;
    [SerializeField] private TMP_InputField Denominator;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI Line;
    [SerializeField] private ToggleGroup EngtoggleGroup;
    [SerializeField] private ToggleGroup ArtoggleGroup;

    [SerializeField] private Button LangBtn;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    private bool Explain = false;
    public static string SpeakerName = "_Sonya_Eng";
    bool IsEng = true;

    public static string FirstNum = "";
    public static string SecNum = "";

    private Button PauseBtn;
    private Button ResumeBtn;
    List<TMP_InputField> FieldsList;

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
        FieldsList = new List<TMP_InputField> { Nuemerator, Denominator };
        PauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        ResumeBtn = GameObject.Find("Resume").GetComponent<Button>();
        PauseBtn.onClick.AddListener(PauseScript.Pause);
        ResumeBtn.onClick.AddListener(PauseScript.Resume);

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
        UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop, "_Shakir_arab", "ShakirSound/Numbers", "_Jenny_Eng", "JennySound/Numbers", "ShakirSound", "JennySound");
        LangBtn.onClick.AddListener(langBtnClickAction);

        InputFieldsActions.InitializePlaceholders(Nuemerator);
        InputFieldsActions.InitializePlaceholders(Denominator);


        SpeakerName = "_Jenny_Eng";
        SLStaicFunctions.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.NumPlace = "JennySound/Numbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";

        AdditionVoiceSpeaker.LoadAllAudioClips();
    }

    void Update()
    {
        PauseScript.ControlPause();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AdditionScript.IsEng = IsEng;
        ExplainEnableMent.EnableExplain(ref FieldsList);
        ExplainEnableMent.DisableExplain(Explain);
        if (IsEng)
        {
            ArtoggleGroup.gameObject.SetActive(false);
            EngtoggleGroup.gameObject.SetActive(true);
        }
        else
        {
            ArtoggleGroup.gameObject.SetActive(true);
            EngtoggleGroup.gameObject.SetActive(false);
        }
    }


    public IEnumerator solve()
    {
        int ParsedNuemerator = int.Parse(Nuemerator.text);
        int ParsedDenominator = int.Parse(Denominator.text);
        
        int XPos = -250;
        int YPos = 200;
        ResetValues.ResetAllValues();
        Line.gameObject.SetActive(true);
        TextInstantiator.InstantiateText(FirstNumPlace, (ParsedNuemerator).ToString(), XPos, YPos, 0, true , 1);
        YPos -= 50;
        TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos, YPos, 0, true , -99);
        YPos -= 50;
        TextInstantiator.InstantiateText(FirstNumPlace, (ParsedDenominator).ToString(), XPos, YPos, 0, true , 0);

        int Divisor = 0;

        Toggle toggle;
        if (IsEng)
        {
            toggle = EngtoggleGroup.ActiveToggles().FirstOrDefault();
        }
        else
        {
            toggle = ArtoggleGroup.ActiveToggles().FirstOrDefault();
        }
        if (toggle.name.Equals("MethodOne"))
        {
            Divisor =  GetDivisiorByFirstMethod(ParsedNuemerator, ParsedDenominator);
        }
        else
        {
            Divisor =  GetDivisiorBySecondMethod(ParsedNuemerator, ParsedDenominator);

        }

        if (Divisor == 1) {
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
            while (Divisor != 1 && Divisor != 0)
            {

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "each of these numbers can be divided by" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Divisor.ToString(), Explain)));

                TextInstantiator.InstantiateText(FirstNumPlace, "÷" + Divisor, XPos + 15 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 90, YPos, 70, true, -4);
                TextMeshProUGUI NueDevisor = GameObject.Find("-4").GetComponent<TextMeshProUGUI>();
                NueDevisor.fontSize = 45;
                yield return new WaitForSeconds(1f);
                TextInstantiator.InstantiateText(FirstNumPlace, "÷" + Divisor, XPos + 5 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 90, YPos, -20, true, -5);

                TextMeshProUGUI DenoDevisor = GameObject.Find("-5").GetComponent<TextMeshProUGUI>();
                DenoDevisor.fontSize = 45;
                yield return new WaitForSeconds(1f);

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, ParsedNuemerator.ToString(), Explain)));

                TextMeshProUGUI Nue = GameObject.Find("1").GetComponent<TextMeshProUGUI>();
                Nue.color = Color.red;

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "divide" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Divisor.ToString(), Explain)));

                NueDevisor.color = Color.red;

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

                TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos + 5 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 180, YPos, 50, true, -1);

                int DevidedNue = ParsedNuemerator / Divisor;
                int DevidedDeno = ParsedDenominator / Divisor;

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, DevidedNue.ToString(), Explain)));


                TextMeshProUGUI NewNue = TextInstantiator.InstantiateText(FirstNumPlace, DevidedNue.ToString(), XPos + 5 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 270, YPos, 100, true, -4);


                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, ParsedDenominator.ToString(), Explain)));

                TextMeshProUGUI Deno = GameObject.Find("0").GetComponent<TextMeshProUGUI>();
                Deno.color = Color.red;


                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "divide" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Divisor.ToString(), Explain)));

                DenoDevisor.color = Color.red;

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, DevidedDeno.ToString(), Explain)));

                TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos + 10 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 270, YPos, 50, true,99);

                TextMeshProUGUI NewDeno = TextInstantiator.InstantiateText(FirstNumPlace, DevidedDeno.ToString(), XPos + 5 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 270, YPos, 0, true, -5);


                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));


                if (DevidedNue % DevidedDeno != 0)
                {

                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, DevidedNue.ToString(), Explain)));

                    NueDevisor = GameObject.Find("-4").GetComponent<TextMeshProUGUI>();
                    NueDevisor.color = Color.red;

                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "over" + SpeakerName, Explain)));


                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, DevidedDeno.ToString(), Explain)));


                    DenoDevisor = GameObject.Find("-5").GetComponent<TextMeshProUGUI>();
                    DenoDevisor.color = Color.red;


                }
                else
                {
                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, (DevidedNue / DevidedDeno).ToString(), Explain)));

                    TextInstantiator.InstantiateText(FirstNumPlace, " = " + (DevidedNue / DevidedDeno).ToString(), XPos + 5 * (Math.Abs(Nuemerator.text.Length - Denominator.text.Length)) + 380, YPos, 50, true, -4);

                }

                ParsedNuemerator/=Divisor;
                ParsedDenominator/=Divisor;
                NueDevisor.name = "99";
                DenoDevisor.name = "99";
                Nue.name = "99";
                Deno.name = "99";
                NewNue.name = "1";
                NewDeno.name = "0";
                NueDevisor = new();
                DenoDevisor = new();

                if (toggle.name.Equals("MethodOne"))
                {
                    Divisor = GetDivisiorByFirstMethod(ParsedNuemerator, ParsedDenominator);
                }
                else
                {
                    Divisor = GetDivisiorBySecondMethod(ParsedNuemerator, ParsedDenominator);

                }
                XPos += 250;
            }
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "is" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "the" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "simplest form" + SpeakerName, Explain)));
        }
        Explain = false;
    }
    public static int GetDivisiorBySecondMethod(int ParsedNuemerator, int ParsedDenominator)
    {
        int Divisor =1;


        if(ParsedDenominator >= 11 && ParsedDenominator >= 11){
            Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 11);
        }
        if (ParsedDenominator >= 7 && ParsedDenominator >= 7 && Divisor ==1)
        {
            Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 7);

        }            
        if (ParsedDenominator >= 5 && ParsedDenominator >= 5 && Divisor == 1)
        {
            Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 5);
        }            
        if (ParsedDenominator >= 3 && ParsedDenominator >= 3 && Divisor == 1)
        {
            Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 3);
        }           
        if (ParsedDenominator >= 2 && ParsedDenominator >= 2 && Divisor == 1)
        {
            Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 2);
        }
        if (Divisor == 1) {
            Divisor = GetDivisiorByFirstMethod(ParsedNuemerator , ParsedDenominator);
        }
        return Divisor;
    }    
    public static int GetDivisiorByFirstMethod(int ParsedNuemerator, int ParsedDenominator)
    {

        int Divisor = 0;
        int i = 12;

        for (; i >= 1; i--)
        {
            if (ParsedNuemerator % i == 0 && ParsedDenominator % i == 0)
            {
                Divisor = i;
                break;
            }
        }
        if(Divisor == 1)
        {
            if (ParsedDenominator >= 97 && ParsedDenominator >= 97 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 97);
            }

            if (ParsedDenominator >= 93 && ParsedDenominator >= 93 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 93);
            }

            if (ParsedDenominator >= 89 && ParsedDenominator >= 89 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 89);
            }

            if (ParsedDenominator >= 83 && ParsedDenominator >= 83 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 83);
            }

            if (ParsedDenominator >= 79 && ParsedDenominator >= 79 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 79);
            }

            if (ParsedDenominator >= 73 && ParsedDenominator >= 73 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator,73);
            }                 
            
            if(ParsedDenominator >= 71 && ParsedDenominator >= 71 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 71);
            }                   
            
            if(ParsedDenominator >= 67 && ParsedDenominator >= 67 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 67);
            }             
            
            if(ParsedDenominator >= 61 && ParsedDenominator >= 61 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 61);
            }             
            
            if(ParsedDenominator >= 59 && ParsedDenominator >= 59 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 59);
            }

            if (ParsedDenominator >= 53 && ParsedDenominator >= 53 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 53);
            }

            if (ParsedDenominator >= 47 && ParsedDenominator >= 47 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 47);
            }
            if (ParsedDenominator >= 43 && ParsedDenominator >= 43 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 43);

            }            
            if (ParsedDenominator >= 41 && ParsedDenominator >= 41 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 41);
            }            
            if (ParsedDenominator >= 37 && ParsedDenominator >= 37 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 37);
            }           
            if (ParsedDenominator >= 31 && ParsedDenominator >= 31 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 31);
            }            
            if (ParsedDenominator >= 29 && ParsedDenominator >= 29 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 29);
            }            
            if (ParsedDenominator >= 23 && ParsedDenominator >= 23 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 23);
            }            
            if (ParsedDenominator >= 19 && ParsedDenominator >= 19 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 19);
            }           
            if (ParsedDenominator >= 17 && ParsedDenominator >= 17 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 17);
            }           
            if (ParsedDenominator >= 13 && ParsedDenominator >= 13 && Divisor == 1)
            {
                Divisor = GetPrimeDivisor(ParsedNuemerator, ParsedDenominator, 13);
            }
        }

        return Divisor;
    }

    public static int GetPrimeDivisor(int ParsedNuemerator, int ParsedDenominator , int Divisor)
    {
        if (ParsedNuemerator % Divisor == 0 && ParsedDenominator % Divisor == 0)
        {
            return Divisor;
        }
        else
            return 1;
    }
}
