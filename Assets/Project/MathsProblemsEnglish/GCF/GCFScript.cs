using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GCFScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField Fnum;
    [SerializeField] private TMP_InputField Snum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI SecNumPlace;
    [SerializeField] private TextMeshProUGUI Line2;
    [SerializeField] private TextMeshProUGUI AdditionLine;
    [SerializeField] private TextMeshProUGUI Sign2;
    [SerializeField] private GameObject Circle;
    [SerializeField] private GameObject Square;
    [SerializeField] private GameObject Line;
    [SerializeField] private ScrollRect scrollRect; // Reference to the ScrollRect component
    [SerializeField] private Button LangBtn;
    [SerializeField] private GameObject TwoDigitsMultiplication;
    [SerializeField] private TextMeshProUGUI FirstNumPlaceAddition;
    [SerializeField] private TextMeshProUGUI SecNumPlaceAddition;
    private float targetScrollPos = 0f; // Target position for scrolling
    private int lastScrollTriggerValue = 0; // Track last trigger poin
    public GameObject PmfObj;

    private AudioClip[] loop;
    public static bool IsCalledFromOutside = false;

    public static bool Explain = false;
    public static string SpeakerName = "_Sonya_Eng";
    public static string FirstNum = "";
    public static string SecNum = "";
    public static bool IsEng = true;
    private string GCFValue = "";
    List<float> FirstNumList = new List<float>();
    List<float> SecNumList = new List<float>();

    public void SetComponenets(TMP_InputField FrstNum, TMP_InputField SecNum, TextMeshProUGUI FirstNumPlace, TextMeshProUGUI SecNumPlace, GameObject Square, GameObject Line, GameObject Circle, UnityEngine.UI.Button LangBtn, ScrollRect scrollRect, GameObject PmfObj, GameObject TDM, TextMeshProUGUI FirstNumPlaceAddition, TextMeshProUGUI SecNumPlaceAddition, TextMeshProUGUI Line2, TextMeshProUGUI AdditionLine, TextMeshProUGUI Sign2)
    {
        this.Fnum = FrstNum;
        this.Snum = SecNum;
        this.FirstNumPlace = FirstNumPlace;
        this.SecNumPlace = SecNumPlace;
        this.LangBtn = LangBtn;
        this.Circle = Circle;
        this.PmfObj = PmfObj;
        this.Line = Line;
        this.scrollRect = scrollRect;
        this.TwoDigitsMultiplication = TDM;
        this.FirstNumPlaceAddition = FirstNumPlaceAddition;
        this.SecNumPlaceAddition = SecNumPlaceAddition;
        this.Line2 = Line2;
        this.AdditionLine = AdditionLine;
        this.Sign2 = Sign2;
    }


    void Start()
    {
        scrollRect.enabled = false;
        if (!IsCalledFromOutside)
        {

            Fnum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
            Snum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        }
        UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

        InputFieldsActions.InitializePlaceholders(Fnum);
        InputFieldsActions.InitializePlaceholders(Snum);

        AdditionVoiceSpeaker.NumPlace = "JennySound/JennyNumbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";

        AdditionVoiceSpeaker.LoadAllAudioClips();
        if (!FirstNum.Equals(""))
        {
            try
            {
                Fnum.text = FirstNum;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                Snum.text = SecNum;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

        }
    }

    void Update()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ExplainEnableMent.EnableExplain(ref Fnum, ref Snum);
        if (IsEng)
        {
            SpeakerName = "_Jenny_Eng";
            AdditionVoiceSpeaker.IsEng = true;
        }
        else
        {
            //SpeakerName = "_Jenny_Egy";
            //AdditionVoiceSpeaker.IsEng = false;

        }
        if (Fnum.text.Length == 0 || Snum.text.Length == 0 || Explain)
        {
            GameObject ExplainBtn = GameObject.Find("Explain");
            GameObject SolveBtn = GameObject.Find("Solve");
            UnityEngine.UI.Button button = ExplainBtn.GetComponent<UnityEngine.UI.Button>();
            button.interactable = false;

            UnityEngine.UI.Button Solvebutton = SolveBtn.GetComponent<UnityEngine.UI.Button>();
            Solvebutton.interactable = false;

        }
        if (Fnum.text.Length != 0 && Snum.text.Length != 0 && !Explain)
        {
            GameObject SolveBtn = GameObject.Find("Solve");
            UnityEngine.UI.Button Solvebutton = SolveBtn.GetComponent<UnityEngine.UI.Button>();
            Solvebutton.interactable = true;
        }


    }
    public void SolveBtnAction()
    {
        PrimeFactors.Explain = false;
        Explain = false;
        Solve();
    }
    public void ExplainBtnAction()
    {
        PrimeFactors.Explain = true;
        Explain = true;
        Solve();
    }
    public void Solve()
    {
        scrollRect.enabled = false;

        GameObject[] GameObjs = FindObjectsOfType<GameObject>();

        FirstNumList.Clear();
        StartCoroutine(StartExplaining());
    }
    public IEnumerator StartExplaining()
    {
        ScrollControler.Ypos = 0;

        ResetValues.GCFDelElements();
        yield return null;
        Line2.gameObject.SetActive(false);
        FirstNumList.Clear();
        SecNumList.Clear();

        ResetValues.ResetAllValues(Line2, FirstNumPlace, SecNumPlace, Sign2);
        PrimeFactors.CurrentY = GameObject.Find("Explain").GetComponent<RectTransform>().anchoredPosition.y - 500;
        PrimeFactors Pmf = PmfObj.GetComponent<PrimeFactors>();
        PrimeFactors.IsCalledFromOutside = true;
        Pmf.SetComponenets(Fnum, FirstNumPlace, LangBtn, Circle, Line, scrollRect);

        Pmf.solve();

        while (PrimeFactors.InPrimeFactors)
        {
            yield return null;
        }
        FirstNumList = new List<float>(PrimeFactors.FirstNumList);

        PrimeFactors.CurrentY -= 150;
        Pmf.SetComponenets(Snum, FirstNumPlace, LangBtn, Circle, Line, scrollRect);

        yield return StartCoroutine(ScrollScript.ScrollToPositionCoroutine(scrollRect));
        PrimeFactors.branches = 0;
        Pmf.solve();
        while (PrimeFactors.InPrimeFactors)
        {
            yield return null;
        }
        PrimeFactors.CurrentY -= 150;

        SecNumList = new List<float>(PrimeFactors.FirstNumList);
        yield return StartCoroutine(ScrollScript.ScrollToPositionCoroutine(scrollRect));


        FinalAnswerScript finalAnswerScript = gameObject.AddComponent<FinalAnswerScript>();

        finalAnswerScript.SetComponents(FirstNumPlace
            ,SecNumPlace
            ,Line2,AdditionLine, Sign2,Circle,Square,Line,FirstNumPlaceAddition,SecNumPlaceAddition,Fnum.text,Snum.text , FirstNumList ,SecNumList , Explain , SpeakerName);

        finalAnswerScript.SetTDM(TwoDigitsMultiplication);
        yield return StartCoroutine(finalAnswerScript.GCF_LCM_FinalAnswer());



        PrimeFactors.IsCalledFromOutside = false;
        PrimeFactors.branches = 0;
        ScrollControler.Ypos = scrollRect.verticalNormalizedPosition;
        scrollRect.enabled = true;
    }
}