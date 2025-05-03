using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GCFScript : MonoBehaviour, IScrollHandler
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

    [SerializeField] private TMP_FontAsset Autmn;
    [SerializeField] private TMP_FontAsset Amari;
    private float previousScrollPosition;
    private int YThrasholdVal = 1902;
    public GameObject PmfObj;

    private AudioClip[] loop;
    public static bool IsCalledFromOutside = false;

    public static bool Explain = false;
    public static string SpeakerName = "_Sonya_Eng";
    public static string FirstNum = "";
    public static string SecNum = "";
    public static bool IsEng = true;
    private string GCFValue = "";
    private bool IsExplain = false;
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


    List<TMP_InputField> FieldsList;
    private void Awake()
    {
        TwoDigitsMultiplicationScript.IsCalledFromOutSide = true;
        AdditionScript.IscalledFromOutSide = true;
        OneDigitMultiplicationScript.IscalledFromOutSide = true;
        PrimeFactors.IsCalledFromOutside = true;

        SpeakerName = "_Jenny_Eng";
        SLStaicFunctions.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.NumPlace = "JennySound/Numbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";
        AdditionVoiceSpeaker.IsEng = true;
        AdditionVoiceSpeaker.LoadAllAudioClips();
    }
    public void Start()
    {

        PrimeFactors.CurrentY = GameObject.Find("Explain").GetComponent<RectTransform>().anchoredPosition.y + 3400;
        scrollRect.enabled = false;
        ResetValues.ResetAllValues();

        FieldsList = new List<TMP_InputField> { Fnum, Snum }; 
        if (!IsCalledFromOutside)
        {

            Fnum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
            Snum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        }


        InputFieldsActions.InitializePlaceholders(Fnum);
        InputFieldsActions.InitializePlaceholders(Snum);


        UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop, "_Shakir_arab", "ShakirSound/Numbers", "_Jenny_Eng", "JennySound/Numbers", "ShakirSound", "JennySound");
        LangBtn.onClick.AddListener(langBtnClickAction);
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
        if (!IsCalledFromOutside)
        {
            GameObject.Find("Explain").GetComponent<Button>().onClick.AddListener(ExplainBtnAction);
            GameObject.Find("Solve").GetComponent<Button>().onClick.AddListener(SolveBtnAction);
        }
    }

    void Update()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ExplainEnableMent.EnableExplain(ref FieldsList);
        ExplainEnableMent.DisableExplain(Explain);


        if (PrimeFactors.CurrentY <= YThrasholdVal && IsExplain)
        {
            StartCoroutine(SmoothScrollCoroutine(scrollRect.content.localPosition.y+1000, 0.3f));
            YThrasholdVal -= 500;
        }

    }
    public void OnScroll(PointerEventData eventData)
    {
        Debug.Log(scrollRect.verticalNormalizedPosition + " " + previousScrollPosition);

        if (scrollRect.verticalNormalizedPosition <= previousScrollPosition)
        {
            scrollRect.verticalNormalizedPosition = previousScrollPosition;
        }
        else
        {
            previousScrollPosition = scrollRect.verticalNormalizedPosition;
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

        YThrasholdVal = 1902;
        PrimeFactors.CurrentY = GameObject.Find("Explain").GetComponent<RectTransform>().anchoredPosition.y + 3400;
        IsExplain = true;

        ResetValues.GCFDelElements();
        yield return null;
        Line2.gameObject.SetActive(false);
        FirstNumList.Clear();
        SecNumList.Clear();

        ResetValues.ResetAllValues();
        PrimeFactors Pmf = PmfObj.GetComponent<PrimeFactors>();
        Pmf.SetComponenets(Fnum, FirstNumPlace, LangBtn, Circle, Line, scrollRect);
        PrimeFactors.InPrimeFactors = true;
        Pmf.solve();

        while (PrimeFactors.InPrimeFactors)
        {
            yield return null;
        }
        FirstNumList = new List<float>(PrimeFactors.FirstNumList);

        PrimeFactors.CurrentY -= 150;
        Pmf.SetComponenets(Snum, FirstNumPlace, LangBtn, Circle, Line, scrollRect);

        PrimeFactors.branches = 0;
        PrimeFactors.InPrimeFactors = true;
        Pmf.solve();
        while (PrimeFactors.InPrimeFactors)
        {
            yield return null;
        }
        PrimeFactors.CurrentY -= 150;

        SecNumList = new List<float>(PrimeFactors.FirstNumList);

        FinalAnswerScript finalAnswerScript = gameObject.AddComponent<FinalAnswerScript>();

        finalAnswerScript.SetComponents(FirstNumPlace
            ,SecNumPlace
            ,Line2,AdditionLine, Sign2,Circle,Square,Line,FirstNumPlaceAddition,SecNumPlaceAddition,Fnum.text,Snum.text , FirstNumList ,SecNumList , Explain , AdditionVoiceSpeaker.SpeakerName , Autmn,Amari);

        finalAnswerScript.SetTDM(TwoDigitsMultiplication);
        yield return StartCoroutine(finalAnswerScript.GCF_LCM_FinalAnswer());




        PrimeFactors.branches = 0;
        scrollRect.enabled = true;
        IsExplain = false;

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
        previousScrollPosition = finalPosition.y;
    }
}