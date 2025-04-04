using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiplyingTwoFractionsScript : MonoBehaviour, IScrollHandler
{
    [SerializeField] private TMP_InputField FDeno;
    [SerializeField] private TMP_InputField SDeno;
    [SerializeField] GameObject Line;
    [SerializeField] GameObject Remove;
    [SerializeField] GameObject Square;

    [SerializeField] private TMP_InputField FNue;
    [SerializeField] private TMP_InputField SNue;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI Sign2;
    [SerializeField] private Button LangBtn;
    [SerializeField] private ToggleGroup EngtoggleGroup;
    [SerializeField] private ToggleGroup ArtoggleGroup;
    [SerializeField] private ScrollRect scrollRect; // Reference to the ScrollRect component

    [SerializeField] private GameObject Arrow1;
    [SerializeField] private GameObject Arrow2;
    bool IsExplain = true;


    private AudioClip[] loop;
    public static AudioSource audioSource;
    private int YThrasholdVal = 400; 

    private bool Explain = false;
    public static string SpeakerName = "_Jenny_Eng";
    bool IsEng = true;


    public static bool IsCalledFromOutSide = false;

    public static string FirstNumber = "";
    public static string SecNumber = "";

    public static string ThirdNumber = "";
    public static string FourthNumber = "";
    float YPos = 1500;
    float XPos = -400;

    List<TMP_InputField> FieldsList;

    private Button PauseBtn;
    private Button ResumeBtn;

    private float previousScrollPosition;

    public void Start()
    {
        scrollRect.enabled = false;
        PauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        ResumeBtn = GameObject.Find("Resume").GetComponent<Button>();
        PauseBtn.onClick.AddListener(PauseScript.Pause);
        ResumeBtn.onClick.AddListener(PauseScript.Resume);
        FieldsList = new List<TMP_InputField> { FNue ,FDeno , SNue , SDeno };
        CharacterProbs.CenterInPos(XPos + 130, YPos + 30, ref Arrow1, FirstNumPlace);
        CharacterProbs.CenterInPos(XPos + 130, YPos + 30, ref Arrow2, FirstNumPlace);

        SpeakerName = "_Jenny_Eng";
        SLStaicFunctions.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.NumPlace = "JennySound/Numbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";
        AdditionVoiceSpeaker.IsEng = true;
        AdditionVoiceSpeaker.LoadAllAudioClips();

        if (IsCalledFromOutSide)
        {
            GameObject.Find("Sign").GetComponent<TextMeshProUGUI>().text = "÷";

        }
        else
        {
            GameObject.Find("Sign").GetComponent<TextMeshProUGUI>().text = "×";
        }
        FNue.text = FirstNumber;
        FDeno.text = SecNumber;

        SNue.text = ThirdNumber;
        SDeno.text = FourthNumber;


        UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop, "_Shakir_arab", "ShakirSound/Numbers", "_Jenny_Eng", "JennySound/Numbers", "ShakirSound", "JennySound");
        LangBtn.onClick.AddListener(langBtnClickAction);

        InputFieldsActions.InitializePlaceholders(FNue);
        InputFieldsActions.InitializePlaceholders(FDeno);
        InputFieldsActions.InitializePlaceholders(SNue);
        InputFieldsActions.InitializePlaceholders(SDeno);

        GameObject.Find("Explain").GetComponent<Button>().onClick.AddListener(ExplainBtnAction);
        GameObject.Find("Solve").GetComponent<Button>().onClick.AddListener(SolveBtnAction);

    }


    void Update()
    {


        PauseScript.ControlPause();

        FNue.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        SNue.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        FDeno.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        SDeno.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ExplainEnableMent.EnableExplain(ref FieldsList);
        ExplainEnableMent.DisableExplain(Explain);

        if (IsEng) {
            ArtoggleGroup.gameObject.SetActive(false);
            EngtoggleGroup.gameObject.SetActive(true);
        }
        else
        {
            ArtoggleGroup.gameObject.SetActive(true);
            EngtoggleGroup.gameObject.SetActive(false);
        }


        if (YPos <= YThrasholdVal && IsExplain)
        {
            StartCoroutine(SmoothScrollCoroutine(YThrasholdVal - 300, 0.3f));
            YThrasholdVal -= 600;
        }
    }
    public void SolveBtnAction()
    {
        Explain = false;
        StartCoroutine(StaretExplaining());
    }  
    public void ExplainBtnAction()
    {
        Explain = true;
        StartCoroutine(StaretExplaining());
    }
    public IEnumerator StaretExplaining()
    {
        scrollRect.enabled = false;
        IsExplain = true;
        Arrow1.SetActive(false);
        Arrow2.SetActive(false);
        YPos = 1500;
        XPos = -400;
        ResetValues.ResetAllValues();
        ResetValues.DelGameObjsWithName(new List<string>() { "Line(Clone)", "Remove(Clone)" });

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "first" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "write the two fractions" + SpeakerName, Explain)));

        int FNueParsed = int.Parse(FNue.text);
        int FDenoParsed = int.Parse(FDeno.text);
        int SNueParsed = int.Parse(SNue.text);
        int SDenoParsed = int.Parse(SDeno.text);

        if (!IsCalledFromOutSide)
        {
            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, FNueParsed, FDenoParsed, XPos, YPos, 2, Explain, false));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain)));
            TextInstantiator.InstantiateText(FirstNumPlace, "×", XPos + 110, YPos, 0, true, -1);
            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SNueParsed, SDenoParsed, XPos + 200, YPos, 3, Explain, false));


        }
        else
        {
            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, FNueParsed, FDenoParsed, XPos, YPos, -1, Explain, false));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "divide" + SpeakerName, Explain)));
            TextInstantiator.InstantiateText(FirstNumPlace, "÷", XPos + 110, YPos, 0, true, -1);
            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SNueParsed, SDenoParsed, XPos + 200, YPos, 100, Explain, false));

        }

        int counter = 2;

        if (IsCalledFromOutSide)
        {
            yield return (StartCoroutine(ReplaceStep(SNueParsed, SDenoParsed)));
            (SNueParsed, SDenoParsed) = (SDenoParsed, SNueParsed);
            YPos -= 300;
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, FNueParsed, FDenoParsed, XPos, YPos, counter, Explain, false));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain)));
            TextInstantiator.InstantiateText(FirstNumPlace, "×", XPos + 110, YPos, 0, true, -1);

            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SNueParsed, SDenoParsed, XPos + 200, YPos, MathTricks.GetNextPrime(counter), Explain, false));
        }

        Toggle toggle;
        StrategySpecifier strategySpecifier = gameObject.AddComponent<StrategySpecifier>();
        strategySpecifier.SetComponents(XPos, YPos, FNueParsed, FDenoParsed, SNueParsed, SDenoParsed, Square, Remove, FirstNumPlace, Explain, (counter).ToString(), (Math.Pow(counter, 2).ToString()), (MathTricks.GetNextPrime(counter)).ToString(), (Math.Pow(MathTricks.GetNextPrime(counter), 2).ToString()));

        if (IsEng)
        {
            toggle = EngtoggleGroup.ActiveToggles().FirstOrDefault();
        }
        else
        {
            toggle = ArtoggleGroup.ActiveToggles().FirstOrDefault();
        }


        yield return (StartCoroutine(TakeStrategyHandler(FNueParsed,FDenoParsed,SNueParsed, SDenoParsed,toggle,strategySpecifier)));


        if (MathTricks.GetGCF(FNueParsed, FDenoParsed) == 1 && MathTricks.GetGCF(SNueParsed, SDenoParsed) == 1
            && MathTricks.GetGCF(FNueParsed, SDenoParsed) == 1 && MathTricks.GetGCF(SNueParsed, FDenoParsed) == 1
            )
        {

            yield return StartCoroutine(MultiplyTwoFractions((counter).ToString(), (Math.Pow(counter, 2).ToString()), (MathTricks.GetNextPrime(counter)).ToString(), Math.Pow(MathTricks.GetNextPrime(counter), 2).ToString(), this, Explain, XPos + 300, YPos, FirstNumPlace, Line));
            yield break;
        }
        strategySpecifier.GetNumbers(ref FNueParsed, ref FDenoParsed, ref SNueParsed, ref SDenoParsed);

        counter = MathTricks.GetNextPrime(MathTricks.GetNextPrime(counter));

        while (!(MathTricks.GetGCF(FNueParsed, FDenoParsed) == 1 && MathTricks.GetGCF(SNueParsed, SDenoParsed) == 1
            && MathTricks.GetGCF(FNueParsed, SDenoParsed) == 1 && MathTricks.GetGCF(SNueParsed, FDenoParsed) == 1
            ))
        {
            YPos -= 300;
            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, FNueParsed, FDenoParsed, XPos, YPos, counter, Explain, false));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, "×", XPos + 110, YPos, 0, true, -1);

            yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SNueParsed, SDenoParsed, XPos + 200, YPos, MathTricks.GetNextPrime(counter), Explain, false));

            strategySpecifier.SetComponents(XPos, YPos, FNueParsed, FDenoParsed, SNueParsed, SDenoParsed, Square, Remove, FirstNumPlace, Explain, (counter).ToString(), (Math.Pow(counter,2).ToString()), (MathTricks.GetNextPrime(counter)).ToString(), (Math.Pow(MathTricks.GetNextPrime(counter), 2).ToString()));
            yield return (StartCoroutine(TakeStrategyHandler(FNueParsed, FDenoParsed, SNueParsed, SDenoParsed, toggle, strategySpecifier)));
            strategySpecifier.GetNumbers(ref FNueParsed, ref FDenoParsed, ref SNueParsed, ref SDenoParsed);

            counter = MathTricks.GetNextPrime(MathTricks.GetNextPrime(counter));

        }
        strategySpecifier.GetNumbers(ref FNueParsed, ref FDenoParsed, ref SNueParsed, ref SDenoParsed);

        YPos -= 300;

        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, FNueParsed, FDenoParsed, XPos, YPos, counter, Explain, false));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, "×", XPos + 110, YPos, 0, true, -1);

        yield return StartCoroutine(SLStaicFunctions.WriteFraction(this, FirstNumPlace, Line, Explain, SNueParsed, SDenoParsed, XPos + 200, YPos, MathTricks.GetNextPrime(counter), Explain, false));

        yield return StartCoroutine(MultiplyTwoFractions( (counter).ToString(), (Math.Pow(counter, 2).ToString()), (MathTricks.GetNextPrime(counter)).ToString(), Math.Pow(MathTricks.GetNextPrime(counter), 2).ToString(), this, Explain, XPos + 300, YPos, FirstNumPlace, Line));

        IsExplain = false;
        scrollRect.enabled = true;
        previousScrollPosition = YPos;
    }

    public static IEnumerator MultiplyTwoFractions(string FNueName , string FDenoName , string SNueName , string SDenoName , MonoBehaviour monoBehaviour , bool Explain , float XPos , float YPos , TextMeshProUGUI FirstNumPlace , GameObject Line)
    {
        TextMeshProUGUI FNue = GameObject.Find(FNueName).GetComponent<TextMeshProUGUI>(); 
        TextMeshProUGUI FDeno = GameObject.Find(FDenoName).GetComponent<TextMeshProUGUI>(); 
        TextMeshProUGUI SNue = GameObject.Find(SNueName).GetComponent<TextMeshProUGUI>(); 
        TextMeshProUGUI SDeno = GameObject.Find(SDenoName).GetComponent<TextMeshProUGUI>();

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, FNue.text.ToString(), Explain)));
        FNue.color = Color.red;

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "time" + SLStaicFunctions.SpeakerName, Explain)));

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, SNue.text.ToString(), Explain)));
        SNue.color = Color.red;

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "equal" + SLStaicFunctions.SpeakerName, Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, " = ", XPos , YPos, 0, true, -1);

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, (int.Parse(SNue.text) * int.Parse(FNue.text)).ToString(), Explain)));


        TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(SNue.text) * int.Parse(FNue.text)).ToString(), XPos+ 100, YPos, 50, true, -1);

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "over" + SLStaicFunctions.SpeakerName, Explain)));

        yield return monoBehaviour.StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Line, new Vector3(XPos + 100 , YPos-20,0), "SmallLine", FirstNumPlace, Explain));



        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, FDeno.text.ToString(), Explain)));
        FDeno.color = Color.red;

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "time" + SLStaicFunctions.SpeakerName, Explain)));

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, SDeno.text.ToString(), Explain)));
        SDeno.color = Color.red;

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "equal" + SLStaicFunctions.SpeakerName, Explain)));

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, (int.Parse(SDeno.text) * int.Parse(FDeno.text)).ToString(), Explain)));

        TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(SDeno.text) * int.Parse(FDeno.text)).ToString(), XPos + 100, YPos, -50, true, -1);

    }

    public IEnumerator TakeStrategyHandler(int FNueParsed,  int FDenoParsed, int SNueParsed, int SDenoParsed , Toggle toggle , StrategySpecifier strategySpecifier)
    {

        if (toggle.name.Equals("MethodOne"))
        {
            yield return StartCoroutine(strategySpecifier.TakeStrategy(SimplifyScript.GetDivisiorByFirstMethod(FNueParsed, SDenoParsed), SimplifyScript.GetDivisiorByFirstMethod(FDenoParsed, SNueParsed),true));
        }
        else
        {
            yield return StartCoroutine(strategySpecifier.TakeStrategy(SimplifyScript.GetDivisiorBySecondMethod(FNueParsed, SDenoParsed), SimplifyScript.GetDivisiorBySecondMethod(FDenoParsed, SNueParsed) , false));

        }

    }
    public IEnumerator ReplaceStep(int SNue ,int SDeno)
    {
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "replace" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SNue.ToString() , Explain)));
        GameObject.Find("100").GetComponent<TextMeshProUGUI>().color = Color.red;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "with" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SDeno.ToString(), Explain)));
        GameObject.Find("10000").GetComponent<TextMeshProUGUI>().color = Color.red;


        CharacterProbs.CenterInPos(XPos + 160, YPos-10 , ref Arrow1, FirstNumPlace,-150);
        Arrow1.SetActive(true);

        CharacterProbs.CenterInPos(XPos + 240, YPos-20 , ref Arrow2, FirstNumPlace,-150);
        Arrow2.SetActive(true);      
        

        yield return new WaitForSeconds(1.5f);

    }

    private IEnumerator SmoothScrollCoroutine(float targetYPosition, float scrollSpeed)
    {
        while (Mathf.Abs(scrollRect.content.localPosition.y - targetYPosition) > 1f && IsExplain)
        {
            Vector3 newPosition = scrollRect.content.localPosition;
            newPosition.y = Mathf.Lerp(scrollRect.content.localPosition.y, targetYPosition, scrollSpeed * Time.deltaTime);
            scrollRect.content.localPosition = newPosition;
            yield return null;
        }
        // Ensure the final position is exactly the target position
        Vector3 finalPosition = scrollRect.content.localPosition;
        finalPosition.y = targetYPosition;
        scrollRect.content.localPosition = finalPosition;
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (scrollRect.verticalNormalizedPosition <= previousScrollPosition)
        {
            scrollRect.verticalNormalizedPosition = previousScrollPosition;
        }
        else
        {
            previousScrollPosition = scrollRect.verticalNormalizedPosition;
        }
    }
}
