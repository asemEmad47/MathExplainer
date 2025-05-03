using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LongDivisionScript : MonoBehaviour
{
    private string SpeakerName = "_Sonya_Eng";
    [SerializeField] private ToggleGroup EngtoggleGroup;
    [SerializeField] private ToggleGroup ArtoggleGroup;
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;


    [SerializeField] private TextMeshProUGUI SecNumPlace;

    [SerializeField] private GameObject DivideSign;
    [SerializeField] private GameObject TimeSign;
    [SerializeField] private GameObject MinusSign;
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject LongDivSympol;
    [SerializeField] private GameObject Line;
    [SerializeField] private GameObject SecMethodLine;
    [SerializeField] private GameObject SubtractionnObj;
    [SerializeField] private GameObject StaticArrow;
    [SerializeField] private UnityEngine.UI.Button LangBtn;

    public static AudioSource audioSource;
    public bool Explain = false;
    public int TimeRes = 0;
    public string FinalResult = "";
    public string FNum = "";
    public float SecProblemY = 0;
    public int LastInsatitedNumber = 0;
    bool IsEng = true;
    private AudioClip[] loop;
    public static bool InLongDev = false;
    Color BlueColor;
    public static string FirstNumber = "";
    public static string SecNumber = "";
    public Vector3 ArrowPos ;

    private Button PauseBtn;
    private Button ResumeBtn;
    public void Start()
    {
        PauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        ResumeBtn = GameObject.Find("Resume").GetComponent<Button>();
        PauseBtn.onClick.AddListener(PauseScript.Pause);
        ResumeBtn.onClick.AddListener(PauseScript.Resume);

        ArrowPos = Arrow.GetComponent<RectTransform>().anchoredPosition;
        FrstNum.text = FirstNumber;
        SecNum.text = SecNumber;

        DivideSign.gameObject.SetActive(false);
        MinusSign.gameObject.SetActive(false);
        TimeSign.gameObject.SetActive(false);

        SpeakerName = "_Sonya_Eng";
        SLStaicFunctions.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.NumPlace = "EngNums";
        AdditionVoiceSpeaker.VoiceClipsPlace = "AdditionTerms/AdditionSound";
        AdditionVoiceSpeaker.IsEng = true;
        AdditionVoiceSpeaker.LoadAllAudioClips();

        UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

        LastInsatitedNumber = 1;
        SecProblemY = FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y - 100;
        FrstNum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        SecNum.onValidateInput = DivisionScript.ValidateSecInput;
        InputFieldsActions.InitializePlaceholders(FrstNum);
        InputFieldsActions.InitializePlaceholders(SecNum);
        BlueColor = DivideSign.GetComponent<TextMeshProUGUI>().color;

        GameObject.Find("Explain").GetComponent<Button>().onClick.AddListener(ExplainBtnAction);
        GameObject.Find("Solve").GetComponent<Button>().onClick.AddListener(SolveBtnAction);
    }
    void Update()
    {
        PauseScript.ControlPause();
        FrstNum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        SecNum.onValidateInput = DivisionScript.ValidateSecInput;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (FrstNum.text.Length == 0 || SecNum.text.Length == 0 || Explain)
        {
            GameObject ExplainBtn = GameObject.Find("Explain");
            GameObject SolveBtn = GameObject.Find("Solve");
            UnityEngine.UI.Button button = ExplainBtn.GetComponent<UnityEngine.UI.Button>();
            button.interactable = false;

            UnityEngine.UI.Button Solvebutton = SolveBtn.GetComponent<UnityEngine.UI.Button>();
            Solvebutton.interactable = false;

        }
        if (FrstNum.text.Length != 0 && SecNum.text.Length != 0 && !Explain)
        {
            GameObject SolveBtn = GameObject.Find("Solve");
            UnityEngine.UI.Button Solvebutton = SolveBtn.GetComponent<UnityEngine.UI.Button>();
            Solvebutton.interactable = true;

            GameObject ExplainBtn = GameObject.Find("Explain");
            UnityEngine.UI.Button button = ExplainBtn.GetComponent<UnityEngine.UI.Button>();
            button.interactable = true;
        }
        if (AdditionVoiceSpeaker.IsEng)
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
    public void ExplainBtnAction()
    {
        Explain = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Solve());
    }
    public void SolveBtnAction()
    {
        Explain = false;
        StartCoroutine(Solve());
    }
    public void OutSideSolve(bool InExplain)
    {
        Explain = InExplain;
        StartCoroutine(Solve());

    }

    public IEnumerator Solve()
    {
        ResetValues.ResetAllValuesLongDiv();
        Arrow.GetComponent<RectTransform>().anchoredPosition = ArrowPos;
        DivideSign.SetActive(false);
        TimeSign.SetActive(false);
        MinusSign.SetActive(false);
        MinusSign.GetComponent<TextMeshProUGUI>().color = BlueColor;

        ResetValues.ResetAllValues();
        InLongDev = true;
        TextMeshProUGUI Reminder2 = GameObject.Find("Reminder").GetComponent<TextMeshProUGUI>();
        Reminder2.text = "R ";
        Reminder2.enabled = false;

        TextMeshProUGUI FinalAnswer2 = GameObject.Find("AnswerPlace").GetComponent<TextMeshProUGUI>();
        FinalAnswer2.text = "";
        FinalAnswer2.enabled = false;

        FirstNumPlace.gameObject.SetActive(false);
        FinalResult = "";
        SecProblemY = 0;
        LastInsatitedNumber = 1;
        SecNumPlace.gameObject.SetActive(false);
        LongDivSympol.SetActive(false);
        SecMethodLine.SetActive(false);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"first" + SpeakerName , Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"Remember the steps of long division" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"divide_verb" + SpeakerName, Explain)));
        DivideSign.SetActive(true);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"multiply" + SpeakerName, Explain)));

        TimeSign.SetActive(true);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"minus" + SpeakerName, Explain)));

        MinusSign.SetActive(true);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"bring the next down" + SpeakerName, Explain)));


        Animator animator = Arrow.GetComponent<Animator>();
        animator.SetTrigger("Start");
        StaticArrow.SetActive(true);
        yield return new WaitForSeconds(1);

        Arrow.SetActive(false);


        if (animator != null)
        {
            animator.enabled = false;
        }
        // start solving


        float YDdistance = -30;
        FirstNumPlace.text = "";
        foreach (char latter in FrstNum.text)
        {
            FirstNumPlace.text += latter + " ";
        }
        SecNumPlace.text = SecNum.text;
        string FrstNumCpy = FirstNumPlace.text;
        string Fnum2 = "";
        bool IsIIncreased = false;

        FirstNumPlace.gameObject.SetActive(true);


        SecNumPlace.gameObject.SetActive(true);
        LongDivSympol.SetActive(true);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"the first step is" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"divide_verb" + SpeakerName, Explain)));

        DivideSign.GetComponent<TextMeshProUGUI>().color = Color.red;
        if (FirstNumPlace != null)
        {


            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
            TMP_CharacterInfo charInfo = textInfo.characterInfo[0];
            Vector3 characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, 0);


            FNum = FrstNumCpy[0].ToString();

            if (textInfo != null)
            {
                for (int i = 0; i < FrstNumCpy.Length; i++)
                {
                    string SNum = SecNum.text;
                    if (!FrstNumCpy[i].Equals(' '))
                    {
                        charInfo = textInfo.characterInfo[i];
                        characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i);

                        GameObject.Find("StaticArrow").transform.Find("StaticBody").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
                        GameObject.Find("StaticArrow").transform.Find("StaticTriangle").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
                        //division part
                        if (textInfo.characterInfo != null && i < textInfo.characterInfo.Length)
                        {
                            TextMeshProUGUI SecNumPlaceCpy = new();


                            if (!(i >= FrstNumCpy.Length - 2 && FNum.Equals("0")))
                            {
                                TextMeshProUGUI myText = new TextMeshProUGUI();

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FNum, Explain)));
                                string temp2 = "", temp = "";

                                MakeRed(i, FrstNumCpy);


                                myText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{myText.text}</color>";

                                DivideSign.GetComponent<TextMeshProUGUI>().color = Color.red;
                                TimeSign.GetComponent<TextMeshProUGUI>().color = BlueColor;
                                MinusSign.GetComponent<TextMeshProUGUI>().color = BlueColor;
                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "divide" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SNum, Explain)));


                                temp2 = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{SNum}</color>";

                                SecNumPlace.text = temp2;
                                if (int.Parse(FNum) < int.Parse(SNum) && i + 3 < FrstNumCpy.Length)
                                {
                                    i += 2;
                                    IsIIncreased = true;


                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "doesnotgo" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FNum, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "is smaller than" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SNum, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "put" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));


                                    TextInstantiator.InstantiateText(FirstNumPlace, "0", characterPosition.x + 15, characterPosition.y, 240, true, 200);
                                    FinalResult += "0";

                                    charInfo = textInfo.characterInfo[i];
                                    characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i);


                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and" + SpeakerName, Explain)));


                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FNum, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "becomes" + SpeakerName, Explain)));

                                    FNum = FNum[FNum.Length - 1].ToString();
                                    FNum += FrstNumCpy[i];

                                    TextMeshProUGUI TempBackup = null;
                                    try
                                    {
                                        // Take the next digit
                                        TempBackup = GameObject.Find("Temp" + (i - 4).ToString()).GetComponent<TextMeshProUGUI>();

                                    }
                                    catch (Exception e)
                                    {
                                        Debug.Log(e);
                                    }

                                    if (TempBackup != null)
                                    {
                                        Vector3 startPosition = new Vector3(characterPosition.x + 230, characterPosition.y + 100, 0);
                                        Vector3 targetPosition = new Vector3(characterPosition.x + 230, TempBackup.GetComponent<RectTransform>().anchoredPosition.y + 200, TempBackup.transform.position.z);
                                        yield return StartCoroutine(MoveArrow(startPosition, targetPosition, 1f));

                                        CharacterProbs.CenterInPos(TempBackup.GetComponent<RectTransform>().anchoredPosition.x + 45, TempBackup.GetComponent<RectTransform>().anchoredPosition.y, ref TempBackup, FirstNumPlace);
                                        TempBackup.text += " " + FrstNumCpy[i];
                                        Fnum2 += FrstNumCpy[i];
                                        yield return new WaitForSeconds(1f);
                                        Arrow.SetActive(false);
                                    }

                                    temp = FirstNumPlace.text.Substring(0, FirstNumPlace.text.LastIndexOf("</color>") + 9);
                                    temp2 = FirstNumPlace.text.Substring(FirstNumPlace.text.LastIndexOf("</color>") + 10);

                                    temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{FNum[1]}</color>";

                                    FirstNumPlace.text = temp + temp2;

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FNum, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));


                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FNum, Explain)));


                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "divide" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SNum, Explain)));


                                }
                                else if (int.Parse(FNum) < int.Parse(SNum) && i + 2 >= FrstNumCpy.Length)
                                {
                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "doesnotgo" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FNum, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "is smaller than" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SNum, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "put" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));



                                    TextInstantiator.InstantiateText(FirstNumPlace, "0", characterPosition.x + 15, characterPosition.y, 240, true);


                                }
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
                                    yield return StartCoroutine(SolveByMehtodOne(SNum, textInfo, charInfo, characterPosition, YDdistance, IsIIncreased, i));
                                }
                                else
                                {
                                    yield return StartCoroutine(SolveByMehtodTwo(textInfo, charInfo, characterPosition, YDdistance, IsIIncreased, i));

                                }

                                //-------------------------------------
                                //minus part
                                DivideSign.GetComponent<TextMeshProUGUI>().color = BlueColor;
                                TimeSign.GetComponent<TextMeshProUGUI>().color = BlueColor;
                                MinusSign.GetComponent<TextMeshProUGUI>().color = Color.red;

                                Line.gameObject.SetActive(true);

                                int index = i;
                                if (TimeRes.ToString().Length > 1)
                                    index = i - 2;

                                TMP_TextInfo textInfo2 = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
                                TMP_CharacterInfo charInfoSecPlace = textInfo2.characterInfo[index];
                                Vector3 characterPositionSecPlace = CharacterProbs.GetCharPoos(FirstNumPlace, charInfoSecPlace, index);

                                SubtractionScript.IsEng = IsEng;
                                SubtractionScript.IscalledFromOutSide = true;
                                SubtractionScript.Explain = true;
                                SubtractionScript subtraction = SubtractionnObj.GetComponent<SubtractionScript>();


                                TextMeshProUGUI FrstNumPlaceCpy;

                                if (i == 0 || (i == 2 && IsIIncreased))
                                {
                                    FrstNumPlaceCpy = Instantiate(FirstNumPlace, FirstNumPlace.transform.position, FirstNumPlace.transform.rotation);
                                    FrstNumPlaceCpy.text = FrstNumCpy.Substring(0, TimeRes.ToString().Length * 2 - 1);
                                }
                                else
                                {
                                    TextMeshProUGUI TempBackup;
                                    try
                                    {
                                        TempBackup = GameObject.Find("Temp" + (i - 2).ToString()).GetComponent<TextMeshProUGUI>();
                                    }
                                    catch (Exception)
                                    {
                                        TempBackup = GameObject.Find("Temp" + (i - 4).ToString()).GetComponent<TextMeshProUGUI>();
                                    }


                                    FrstNumPlaceCpy = TempBackup;

                                    FrstNumPlaceCpy.text = Fnum2;
                                }

                                FrstNumPlaceCpy.name = "FrstNumPlaceCpy" + i.ToString();

                                FrstNumPlaceCpy.ForceMeshUpdate();
                                yield return null;
                                SpaceNumbers(ref FNum);
                                try
                                {
                                    SecNumPlaceCpy = GameObject.Find((i + 200).ToString()).GetComponent<TextMeshProUGUI>();

                                }
                                catch (Exception)
                                {

                                    SecNumPlaceCpy = GameObject.Find((i - 2 + 200).ToString()).GetComponent<TextMeshProUGUI>();

                                }
                                SecNumPlaceCpy.text = FNum;
                                if (i != 0 && !(i == 2 && IsIIncreased))
                                {
                                    CharacterProbs.CenterInPos(GameObject.Find("FrstNumPlaceCpy" + (i).ToString()).GetComponent<RectTransform>().anchoredPosition.x, GameObject.Find("FrstNumPlaceCpy" + (i).ToString()).GetComponent<RectTransform>().anchoredPosition.y - 70, ref SecNumPlaceCpy, FirstNumPlace);
                                }
                                else
                                {
                                    if ((i == 2 && IsIIncreased))
                                    {
                                        CharacterProbs.CenterInPos(SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.x + 33, SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.y, ref SecNumPlaceCpy, FirstNumPlace);
                                    }
                                }

                                GameObject LineI = Instantiate(Line, Line.transform.position, Line.transform.rotation);
                                Line.gameObject.SetActive(false);
                                LineI.name = "LINEI " + i;
                                TextMeshProUGUI LineItxt = LineI.GetComponent<TextMeshProUGUI>();

                                CharacterProbs.CenterInPos(SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.x - 10, SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.y - 30, ref LineItxt, FirstNumPlace);


                                SubtractionScript.ResSpace = -250;
                                SecNumPlaceCpy.enableWordWrapping = false;
                                SecNumPlaceCpy.overflowMode = TextOverflowModes.Overflow;

                                FrstNumPlaceCpy.enableWordWrapping = false;
                                FrstNumPlaceCpy.overflowMode = TextOverflowModes.Overflow;

                                if (i == 0 || (i == 2 && IsIIncreased))
                                {
                                    CharacterProbs.CenterInPos(SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.x - 10, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, ref FrstNumPlaceCpy, FirstNumPlace);
                                }
                                SubtractionScript.IscalledFromOutSide = true;
                                subtraction.SetComponenets(FrstNum, SecNum, FrstNumPlaceCpy, SecNumPlaceCpy, LineItxt, null, null);

                                subtraction.OutSideSolve(Explain);
                                while (SubtractionScript.IscalledFromOutSide)
                                {
                                    yield return null;
                                }
                                if (i + 2 < FrstNumCpy.Length)
                                {
                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "bring the next down" + SpeakerName, Explain)));
                                    GameObject.Find("StaticArrow").transform.Find("StaticBody").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0,1f);
                                    GameObject.Find("StaticArrow").transform.Find("StaticTriangle").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1f);

                                    charInfo = textInfo.characterInfo[i];
                                    characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i);

                                    Vector3 startPosition = new Vector3(characterPosition.x + 305, characterPosition.y + 100, 0);

                                    Vector3 targetPosition = new Vector3(characterPosition.x + 305, SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.y + 100, LineItxt.transform.position.z);

                                    characterPosition = new Vector3(characterPosition.x, characterPosition.y + 200, 0);
                                    if (IsIIncreased)
                                    {
                                        charInfo = textInfo.characterInfo[i];
                                        characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i);
                                    }
                                    // Move the arrow smoothly over 1 second (you can change this duration)
                                    yield return StartCoroutine(MoveArrow(startPosition, targetPosition, 1f));

                                    TextMeshProUGUI Temp = Instantiate(SecNumPlaceCpy, characterPosition, LineItxt.transform.rotation);
                                    Temp.text = "";
                                    Temp.name = "Temp" + i;
                                    FNum = "";
                                    GetMinusResult(i + 100, ref Temp, ref FNum);
                                    Temp.text += FrstNumCpy[i + 2];
                                    Temp.color = UnityEngine.Color.red;
                                    if (FNum.Equals("0"))
                                    {
                                        FNum = "  ";
                                    }
                                    FNum += FrstNumCpy[i + 2];
                                    Fnum2 = FNum;

                                    SpaceNumbers(ref Fnum2);

                                    GameObject HandelZeroPos = FindGameObjectWithLargestXPosition("HandeldZero" + (100 + i).ToString());
                                    if (HandelZeroPos != null)
                                    {
                                        CharacterProbs.CenterInPos(HandelZeroPos.GetComponent<RectTransform>().anchoredPosition.x + 60 + +25 * (Temp.text.Length / 2 + 1), HandelZeroPos.GetComponent<RectTransform>().anchoredPosition.y, ref Temp, FirstNumPlace);
                                    }
                                    else
                                    {
                                        CharacterProbs.CenterInPos(Temp.GetComponent<RectTransform>().anchoredPosition.x + 25 * (Temp.text.Length / 2 + 1), SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.y - 80, ref Temp, FirstNumPlace);
                                    }

                                    YDdistance = Temp.GetComponent<RectTransform>().anchoredPosition.y - 50;
                                    SecNumPlace.text = SecNum.text;
                                    yield return new WaitForSeconds(1f);
                                    Arrow.SetActive(false);
                                }
                            }
                            else
                            {
                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0" , Explain)));
                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain)));
                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SNum , Explain)));
                                SecNumPlace.text =  $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{SecNumPlace.text}</color>";
                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));
                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, "0", Explain)));
                                TextInstantiator.InstantiateText(FirstNumPlace, ("0").ToString(), characterPosition.x + 15, characterPosition.y, 240, true, i);
                                FinalResult += "0";
                            }

                            if (i >= FrstNumCpy.Length - 2)
                            {
                                TextMeshProUGUI Temp;
                                try
                                {
                                    Temp = Instantiate(SecNumPlaceCpy, SecNumPlaceCpy.transform.position, SecNumPlaceCpy.transform.rotation);
                                }
                                catch (Exception e)
                                {
                                    Temp = Instantiate(FirstNumPlace, FirstNumPlace.transform.position, FirstNumPlace.transform.rotation);
                                    Debug.Log(e);
                                }

                                Temp.text = "";
                                Temp.name = "Temp" + i;
                                FNum = "";

                                GetMinusResult(-1, ref Temp, ref FNum);

                                string reminder = "";
                                try
                                {
                                    reminder = int.Parse(FNum).ToString();

                                }
                                catch (Exception)
                                {

                                    reminder = "0";
                                }
                                float XPos = SecNumPlace.GetComponent<RectTransform>().anchoredPosition.x;
                                float YPos = Temp.GetComponent<RectTransform>().anchoredPosition.y;

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "the final answer is" + SpeakerName, Explain)));


                                FinalResult = int.Parse(FinalResult).ToString();
                                TextMeshProUGUI FinalAnswer = GameObject.Find("AnswerPlace").GetComponent<TextMeshProUGUI>();
                                FinalAnswer.text = FrstNum.text + " ÷ " + SecNum.text + " = " + FinalResult;

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FinalResult, Explain)));
                                CharacterProbs.CenterInPos(FinalAnswer.GetComponent<RectTransform>().anchoredPosition.x, PauseBtn.GetComponent<RectTransform>().anchoredPosition.y+50, ref FinalAnswer, FirstNumPlace);
                                FinalAnswer.enabled = true;


                                if (!reminder[0].Equals('0'))
                                {
                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and" + SpeakerName, Explain)));


                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "the remainder is" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, reminder, Explain)));

                                    TextMeshProUGUI Reminder = GameObject.Find("Reminder").GetComponent<TextMeshProUGUI>();
                                    Reminder.text += reminder;
                                    CharacterProbs.CenterInPos(Reminder.GetComponent<RectTransform>().anchoredPosition.x, GameObject.Find("Explain").GetComponent<RectTransform>().anchoredPosition.y - 100, ref Reminder, FirstNumPlace);

                                    Reminder.enabled = true;

                                    // x over y
                                    YPos = FinalAnswer.GetComponent<RectTransform>().anchoredPosition.y - 75;
                                    XPos = FinalAnswer.GetComponent<RectTransform>().anchoredPosition.x - 50;



                                    TextInstantiator.InstantiateText(FirstNumPlace, FinalResult, XPos, YPos, -50, true);
                                    XPos += 60;

                                    TextInstantiator.InstantiateText(FirstNumPlace, reminder.ToString(), XPos + 100, YPos, 0, true);

                                    TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", XPos + 100, YPos, -50, true);

                                    TextInstantiator.InstantiateText(FirstNumPlace, SecNum.text.ToString(), XPos + 100, YPos, -100, true);

                                }
                            }

                        }
                    }
                    IsIIncreased = false;
                }
            }
            SubtractionScript.IscalledFromOutSide = false;
            SubtractionScript.ResSpace = -400;

            Explain = false;
            GameObject.Find("MethodTwo").GetComponent<Toggle>().interactable = true;
            GameObject.Find("MethodOne").GetComponent<Toggle>().interactable = true;
            GameObject.Find("Explain").GetComponent<Button>().interactable = true;
            GameObject.Find("Explain").GetComponent<Button>().enabled = true;
            animator.enabled = true;


        }
    }
    public void GetMinusResult(int i, ref TextMeshProUGUI res, ref string FnumTemp)
    {
        TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>();
        if (textMeshProObjects.Length > 0)
        {
            foreach (TextMeshProUGUI textMeshPro in textMeshProObjects)
            {

                if (int.TryParse(textMeshPro.name, out int number) && 
                    (
                    (number < 0) || (number == i || number == 100) || (i == -1 && number >= 100 && number < 200) )
                    )
                {
                    if (!textMeshPro.text.Equals("0"))
                    {
                        res.text += textMeshPro.text + " ";
                        FnumTemp += textMeshPro.text;
                    }

                    if (!textMeshPro.text.Equals("0") && i!=-1)
                    {
                        Destroy(textMeshPro.gameObject);
                    }
                    else
                    {
                        textMeshPro.name = "HandeldZero" + i;
                    }

                }

            }
        }

    }

    public void MakeRed(int i, string FrstNumCpy)
    {
        string FNum = FrstNumCpy[i].ToString();
        string temp = FirstNumPlace.text.Substring(0, i);
        string temp2 = FirstNumPlace.text.Substring(i + 1);
        if (FirstNumPlace.text.LastIndexOf("</color>") != -1)
        {
            temp = FirstNumPlace.text.Substring(0, FirstNumPlace.text.LastIndexOf("</color>") + 9);
            temp2 = FirstNumPlace.text.Substring(FirstNumPlace.text.LastIndexOf("</color>") + 10);
        }

        temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{FNum[FNum.Length - 1]}</color>";
        FirstNumPlace.text = temp + temp2;
    }

    public void SpaceNumbers(ref string number)
    {
        string temp = "";

        foreach (char latter in number)
        {
            temp += latter + " ";
        }
        number = temp;
    }
    // Coroutine to move the arrow with lerping
    IEnumerator MoveArrow(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        Arrow.SetActive(true);

        float timeElapsed = 0;

        RectTransform arrowRect = Arrow.GetComponent<RectTransform>();

        // Continue the animation for the given duration
        while (timeElapsed < duration)
        {
            // Calculate the current position using Lerp for both x and y
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);

            // Ensure the RectTransform moves along both axes
            arrowRect.anchoredPosition = new Vector2(newPosition.x, newPosition.y);

            // Increment time
            timeElapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the arrow reaches the exact target position at the end
        arrowRect.anchoredPosition = new Vector2(targetPosition.x, targetPosition.y);
        yield return null;
    }

    public IEnumerator SolveByMehtodOne(string SNum, TMP_TextInfo textInfo, TMP_CharacterInfo charInfo, Vector3 characterPosition, float YDdistance, bool IsIIncreased, int i)
    {
        float result = ((float)int.Parse(FNum) / int.Parse(SNum));
        bool FirstTimeInLoop = false;
        string FNumCpy = "";


        result = float.Parse(FNum) / float.Parse(SecNum.text);
        while (float.Parse(FNum) % float.Parse(SecNum.text) != 0 && result >= 1)
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"doesnotgo" + SpeakerName, Explain)));

            if (!FirstTimeInLoop)
            {
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"is greater than" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum, Explain)));

                FNumCpy = FNum;

            }
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain))
                );
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"reduce one" + SpeakerName, Explain)));



            FNum = (int.Parse(FNum) - 1).ToString();

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"divide" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum, Explain)));

            FirstTimeInLoop = true;

        }

        if(result >= 1)
        {

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "equal" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, ((int)result).ToString(), Explain)));

        }
        else
        {
            FNum = "0";
        }
        TextInstantiator.InstantiateText(FirstNumPlace, ((int)result).ToString(), characterPosition.x + 15, characterPosition.y, 240, true, i);


        if (IsIIncreased) // to make number under number in a right way
        {
            charInfo = textInfo.characterInfo[i - 2];
            characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i - 2);
        }
        FinalResult += ((int)result).ToString();


        //------------------------------------------------
        //time part

        SecNumPlace.text = SecNum.text;

        DivideSign.GetComponent<TextMeshProUGUI>().color = BlueColor;
        TimeSign.GetComponent<TextMeshProUGUI>().color = Color.red;
        MinusSign.GetComponent<TextMeshProUGUI>().color = BlueColor;
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"the next step is" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"time" + SpeakerName, Explain)));

        GameObject DivRes = GameObject.Find(i.ToString());

        TextMeshProUGUI DivResText = DivRes.GetComponent<TextMeshProUGUI>();

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,DivResText.text, Explain)));

        TimeRes = int.Parse(DivResText.text) * int.Parse(SecNum.text);

        DivResText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{DivResText.text}</color>";

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"time" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SecNum.text, Explain)));

        SecNumPlace.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{SecNum.text}</color>";

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"equal" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,TimeRes.ToString(), Explain)));

        if (YDdistance == -30)
            TextInstantiator.InstantiateText(FirstNumPlace, TimeRes.ToString(), characterPosition.x + 15, characterPosition.y, YDdistance, true, i + 200);
        else
            TextInstantiator.InstantiateText(FirstNumPlace, TimeRes.ToString(), characterPosition.x + 15, characterPosition.y, YDdistance - 100, true, i + 200);
    }

    public IEnumerator SolveByMehtodTwo(TMP_TextInfo textInfo, TMP_CharacterInfo charInfo, Vector3 characterPosition, float YDdistance, bool IsIIncreased, int i)
    {
        float DivResult = MathF.Ceiling((float)int.Parse(FNum) / int.Parse(SecNum.text));
        int result = 0;

        if (LastInsatitedNumber == 1)
        {
            SecMethodLine.SetActive(true);
        }
        bool Inwhile = false;
        while (LastInsatitedNumber < DivResult + 1)
        {
            Inwhile = true;
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,LastInsatitedNumber.ToString(), Explain)));

            TextInstantiator.InstantiateText(FirstNumPlace, LastInsatitedNumber.ToString(), SecMethodLine.GetComponent<RectTransform>().anchoredPosition.x + 60, SecProblemY, 0, true, 500 + LastInsatitedNumber);

            TextMeshProUGUI textMeshProUGUI = GameObject.Find((500 + LastInsatitedNumber).ToString()).GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.color = UnityEngine.Color.red;

            ColoringScript.ColorThemAll((500 + LastInsatitedNumber).ToString(), UnityEngine.Color.red);

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"time" + SpeakerName, Explain)));


            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SecNum.text, Explain )));


            SecNumPlace.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{SecNum.text}</color>";

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"equal" + SpeakerName, Explain)));

            TimeRes = LastInsatitedNumber * int.Parse(SecNum.text);


            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,TimeRes.ToString(), Explain)));


            TextInstantiator.InstantiateText(FirstNumPlace, TimeRes.ToString(), SecMethodLine.GetComponent<RectTransform>().anchoredPosition.x - 80, SecProblemY, 0, true, 500 + LastInsatitedNumber);


            if (TimeRes < int.Parse(FNum))
            {
                result = TimeRes;
                yield return new WaitForSeconds(0.5f);

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,TimeRes.ToString(), Explain)));
                ColoringScript.ColorThemAll((500 + LastInsatitedNumber).ToString(), UnityEngine.Color.red);



                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"is smaller than" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum.ToString(), Explain)));

                yield return new WaitForSeconds(0.5f);



            }
            else
            {
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,TimeRes.ToString(), Explain)));
                ColoringScript.ColorThemAll((500 + LastInsatitedNumber).ToString(), UnityEngine.Color.red);

                int CurrentNum = 0;
                if (TimeRes > int.Parse(FNum))
                {
                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"is greater than" + SpeakerName, Explain)));
                    CurrentNum = LastInsatitedNumber - 1;
                }
                else
                {
                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"equal" + SpeakerName, Explain)));
                    CurrentNum = LastInsatitedNumber;
                }
                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum.ToString(), Explain)));


                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,(CurrentNum).ToString(), Explain)));

                TextInstantiator.InstantiateText(FirstNumPlace, (CurrentNum).ToString(), characterPosition.x + 15, characterPosition.y, 240, true, i);

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"then" + SpeakerName, Explain)));


                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"take" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,((CurrentNum) * int.Parse(SecNum.text)).ToString(), Explain)));

                if (IsIIncreased) // to make number under number in a right way
                {
                    charInfo = textInfo.characterInfo[i - 2];
                    characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i - 2);
                }

                TextInstantiator.InstantiateText(FirstNumPlace, "", characterPosition.x + 15, characterPosition.y, YDdistance, true, i + 200);

                FNum = ((CurrentNum) * int.Parse(SecNum.text)).ToString();
                FinalResult += (CurrentNum).ToString();
            }
            SecProblemY -= 75;
            SecNumPlace.text = SecNum.text;
            ColoringScript.ColorThemAll((500 + LastInsatitedNumber).ToString(), UnityEngine.Color.black);

            LastInsatitedNumber++;
        }

        if (!Inwhile)
        {
            DivResult = MathF.Floor((float)int.Parse(FNum) / int.Parse(SecNum.text));

            ColoringScript.ColorThemAll((500 + DivResult).ToString(), UnityEngine.Color.red);

            yield return new WaitForSeconds(0.5f);


            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,(DivResult).ToString() , Explain)));


            TextInstantiator.InstantiateText(FirstNumPlace, (DivResult).ToString(), characterPosition.x + 15, characterPosition.y, 240, true, i);

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"then" + SpeakerName, Explain)));


            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"take" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,((DivResult) * int.Parse(SecNum.text)).ToString() , Explain)));

            if (IsIIncreased) // to make number under number in a right way
            {
                charInfo = textInfo.characterInfo[i - 2];
                characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i - 2);
            }

            TextInstantiator.InstantiateText(FirstNumPlace, "", characterPosition.x + 15, characterPosition.y, YDdistance, true, i + 200);
            FNum = ((DivResult) * int.Parse(SecNum.text)).ToString();
            TimeRes = int.Parse(FNum);
            FinalResult += (DivResult).ToString();
            ColoringScript.ColorThemAll((500 + DivResult).ToString(), UnityEngine.Color.black);

        }

    }

    GameObject FindGameObjectWithLargestXPosition(string baseName)
    {
        GameObject largestXObject = null;
        float largestXPosition = float.MinValue;

        // Find all GameObjects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Check if the object's name starts with the base name
            if (obj.name.StartsWith(baseName))
            {
                // Get the x position of the object
                float xPosition = obj.transform.position.x;

                // Update if this object has the largest x position
                if (xPosition > largestXPosition)
                {
                    largestXPosition = xPosition;
                    largestXObject = obj;
                }
            }
        }

        return largestXObject;
    }

}
