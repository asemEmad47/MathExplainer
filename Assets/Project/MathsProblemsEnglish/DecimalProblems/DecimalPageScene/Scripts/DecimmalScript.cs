using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DecimmalScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI SecNumPlace;
    [SerializeField] private TextMeshProUGUI Line;
    [SerializeField] private TextMeshProUGUI sign;
    [SerializeField] private GameObject RightArrow;
    [SerializeField] private GameObject LeftArrow;
    [SerializeField] private UnityEngine.UI.Button LangBtn;

    private AudioClip[] loop;
    public static AudioSource audioSource;
    private bool Explain = false;
    private List<int> borrowingList;
    string SpeakerName = "_Sonya_Eng";
    private string firstNumcpy = "";
    private string Language = "Eng";
    bool IsEng = true;
    public static bool IscalledFromOutSide = false;
    public GameObject AdditionObj;
    public GameObject SubtractionnObj;
    private int LongestInt = 0;
    public static bool  IsWrited = false;

    public static string FirstNumber = "";
    public static string SecNumber = "";


    List<TMP_InputField> FieldsList;
    public void Start()
    {
        FieldsList = new List<TMP_InputField> { FrstNum, SecNum };
        FirstNumPlace.gameObject.SetActive(false);
        SecNumPlace.gameObject.SetActive(false);
        ResetValues.ResetAllValues();

        FrstNum.text = FirstNumber;
        SecNum.text = SecNumber;

        GameObject sign2 = GameObject.Find("Sign2");

        if (PlayerPrefs.GetString("type").Equals("add"))
        {
            sign.text = "+";
            TextMeshProUGUI text = sign2.GetComponent<TextMeshProUGUI>();
            text.text = "+";
        }
        else
        {
            sign.text = "-";
            TextMeshProUGUI text = sign2.GetComponent<TextMeshProUGUI>();
            text.text = "-";
        }
        FrstNum.onValueChanged.AddListener((input) => OnInputChanged(FrstNum, input));
        SecNum.onValueChanged.AddListener((input) => OnInputChanged(SecNum, input));
        FrstNum.onValidateInput = InputFieldsActions.ValidateDecimalObsInput;
        SecNum.onValidateInput = InputFieldsActions.ValidateDecimalObsInput;

        InputFieldsActions.InitializePlaceholders(FrstNum);
        InputFieldsActions.InitializePlaceholders(SecNum);


        GameObject.Find("Explain").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ExplainBtnAction);
        GameObject.Find("Solve").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SolveBtnAction);


        try
        {
            UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
            LangBtn.onClick.AddListener(langBtnClickAction);
        }
        catch (Exception e)
        {

            Debug.Log(e);
        }

    }
    void Update()
    {
        ExplainEnableMent.EnableExplain(ref FieldsList);

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
        }
        if (!AdditionVoiceSpeaker.NumPlace.Equals("JennySound/JennyNumbers"))
        {
            if (IsEng)
            {
                SpeakerName = "_Sonya_Eng";
                AdditionVoiceSpeaker.NumPlace = "EngNums";

            }
            else
            {
                SpeakerName = "_Heba_Egy";
                AdditionVoiceSpeaker.NumPlace = "EgyNums";

            }
        }
        AdditionScript.IsEng = IsEng;
        SubtractionScript.IsEng = IsEng;
    }
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
    public IEnumerator solve()
    {
        AdditionScript.IscalledFromOutSide = true;
        SubtractionScript.IscalledFromOutSide = true;
        GameObject ExplainBtn = GameObject.Find("Explain");
        UnityEngine.UI.Button button = ExplainBtn.GetComponent<UnityEngine.UI.Button>();
        button.interactable = false;

        ResetValues.ResetAllValues();
        if (float.Parse(SecNum.text) > float.Parse(FrstNum.text))
        {
            (SecNum.text, FrstNum.text) = (FrstNum.text, SecNum.text);
            (FirstNumPlace.text, SecNumPlace.text) = (SecNumPlace.text, FirstNumPlace.text);

        }

        if (!FrstNum.text.Contains('.'))
        {
            Color FirstNumColor = FrstNum.GetComponent<UnityEngine.UI.Image>().color;
            FrstNum.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"the first number" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"has no decimal point so" + SpeakerName, Explain)));
            FrstNum.text += '.';
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"you should put a value" + SpeakerName, Explain)));
            FrstNum.text += '0';
            FrstNum.GetComponent<UnityEngine.UI.Image>().color = FirstNumColor;


        }


        if (!SecNum.text.Contains('.'))
        {
            Color SecNumColor = SecNum.GetComponent<UnityEngine.UI.Image>().color;

            SecNum.GetComponent<UnityEngine.UI.Image>().color = Color.red;

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"the second number" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"has no decimal point so" + SpeakerName, Explain)));
            SecNum.text += '.';
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"you should put a value" + SpeakerName, Explain)));
            SecNum.text += '0';
            SecNum.GetComponent<UnityEngine.UI.Image>().color = SecNumColor;
        }

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"first" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"write the decimal points" + SpeakerName , Explain)));


        FirstNumPlace.text = ".";
        SecNumPlace.text = ".";


        FirstNumPlace.gameObject.SetActive(true);
        SecNumPlace.gameObject.SetActive(true);

        yield return (StartCoroutine(NumbersWriter.WriteNumber(true,SpeakerName,FrstNum,SecNum,FirstNumPlace,SecNumPlace,Explain,RightArrow,LeftArrow,this)));
        yield return (StartCoroutine(NumbersWriter.WriteNumber(false, SpeakerName, FrstNum, SecNum, FirstNumPlace, SecNumPlace, Explain, RightArrow, LeftArrow, this)));


        if (FrstNum.text.Length != SecNum.text.Length)
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put zeros in empty digits" + SpeakerName , Explain)));
        ZerosOps.DecimalFillWithZeros(FrstNum, SecNum,LongestInt,FirstNumPlace,SecNumPlace);
        TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
        TMP_CharacterInfo charInfo = textInfo.characterInfo[FirstNumPlace.text.IndexOf('.')];

        Vector3 characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, FirstNumPlace.text.IndexOf('.'));
        if(PlayerPrefs.GetString("type").Equals("add"))
            TextInstantiator.InstantiateText(FirstNumPlace, ".", characterPosition.x , FirstNumPlace.transform.position.y, -150, true, 999);
        else
            TextInstantiator.InstantiateText(FirstNumPlace, ".", characterPosition.x, FirstNumPlace.transform.position.y, -200, true,999);

        GameObject res = GameObject.Find("999");
        TextMeshProUGUI ResText = res.GetComponent<TextMeshProUGUI>();
        ResText.color = FirstNumPlace.color;
        Line.gameObject.SetActive(true);

        DoOperation(); // to dynamicly choose sub or addition

        while (button.interactable==false)
        {
            yield return null;
        }
        yield return StartCoroutine(ZerosOps.DecimalRemoveUselessZeros(LongestInt , SpeakerName , Explain , this));
        AdditionScript.IscalledFromOutSide = false;
        SubtractionScript.IscalledFromOutSide = false;
    }

    public void DoOperation()
    {
        switch (PlayerPrefs.GetString("type"))
        {
            case "sub":
                SubtractionScript.IscalledFromOutSide = true;
                SubtractionScript subtraction = SubtractionnObj.GetComponent<SubtractionScript>();
                subtraction.SetComponenets(FrstNum, SecNum, FirstNumPlace, SecNumPlace, Line, sign, LangBtn);
                subtraction.OutSideSolve(Explain);
                break;

            case "add":
                AdditionScript.IscalledFromOutSide = true;
                AdditionScript addition = AdditionObj.GetComponent<AdditionScript>();
                addition.SetComponenets(FrstNum, SecNum, FirstNumPlace, SecNumPlace, Line, sign, LangBtn);
                addition.OutSideSolve(Explain);
                break;

            default:
                break;
        }
    }
    private void OnInputChanged(TMP_InputField inputField, string input)
    {
        if (input == "0" &&IsWrited)
        {
            if (!inputField.text.Contains("."))
            {
                inputField.text = "0.";  
                StartCoroutine(SetCaretPositionAfterFrame(inputField)); 
            }
        }
        else if (input == "0." &&!IsWrited)
        {
            inputField.text = "";
            StartCoroutine(SetCaretPositionAfterFrame(inputField));
        }
        IsWrited = false;
    }

    // Coroutine to set the caret position after a frame delay
    private IEnumerator SetCaretPositionAfterFrame(TMP_InputField inputField)
    {
        // Wait for end of the frame to ensure Unity has updated the input field
        yield return new WaitForEndOfFrame();

        // Set caret to the end of the input (after the '.')
        inputField.caretPosition = inputField.text.Length;
    }
}