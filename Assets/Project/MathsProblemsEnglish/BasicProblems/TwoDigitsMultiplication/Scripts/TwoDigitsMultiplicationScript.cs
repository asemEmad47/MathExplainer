using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class TwoDigitsMultiplicationScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI Line;
    [SerializeField] private TextMeshProUGUI Line2;
    [SerializeField] private TextMeshProUGUI sign;
    [SerializeField] private TextMeshProUGUI AdditionSign;


    [SerializeField] private TextMeshProUGUI FirstNumPlaceAdditon;
    [SerializeField] private TextMeshProUGUI SecNumPlaceAdditon;
    [SerializeField] private Button LangBtn;

    public GameObject OneDigitMultiplicationObj;
    public GameObject AdditionObj;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    public static bool IsCalledFromOutSide = false;

    private bool Explain = false;
    string SpeakerName = "_Sonya_Eng";
    bool IsEng = true;
    private Vector3 ResVector;

    private string FirstRes;
    private string SecRes;

    public void Start()
    {
        ResVector = new Vector3();
        UnityAction langBtnClickAction = () => AdditionScript.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);


        if (AdditionScript.IsBasic)
        {
            FrstNum.onValidateInput = AdditionScript.ValidateInput;
            SecNum.onValidateInput = ValidateSecInput;

        }
        else
        {
            SecNum.onValueChanged.AddListener((input) => OnInputChanged(SecNum, input));
            FrstNum.onValueChanged.AddListener((input) => OnInputChanged(FrstNum, input));

            FrstNum.onValidateInput = DecimmalScript.ValidateDecimalInput;
            SecNum.onValidateInput = DecimmalScript.ValidateSecDecimalInput;
        }

        AdditionScript.InitializePlaceholders(FrstNum);
        AdditionScript.InitializePlaceholders(SecNum);
    }

    void Update()
    {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AdditionScript.EnableExplain(ref FrstNum, ref SecNum);

        if (IsEng)
        {
            SpeakerName = "_Sonya_Eng";
        }
        else
        {
            SpeakerName = "_Heba_Egy";
        }
        AdditionScript.SpeakerName = SpeakerName;
        AdditionScript.IsEng = IsEng;
        OneDigitMultiplicationScript.SpeakerName = SpeakerName;
        OneDigitMultiplicationScript.IsEng = IsEng;
    }

    public void explain()
    {
        Explain = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(solve());
    }
    public IEnumerator solve()
    {
        OneDigitMultiplicationScript.IscalledFromOutSide = true;
        AdditionScript.IscalledFromOutSide = true;

        GameObject ExplainBtn = GameObject.Find("Explain");

        Button button = ExplainBtn.GetComponent<Button>();
        button.interactable = false;

        OneDigitMultiplicationScript.IscalledFromOutSide = true;

        int DecimalDigitsCount = (FrstNum.text.Length - 1 - FrstNum.text.IndexOf('.')) + (SecNum.text.Length - 1 - SecNum.text.IndexOf('.'));

        if(FrstNum.text.IndexOf('.') == -1)
        {
            DecimalDigitsCount = SecNum.text.Length - 1 - SecNum.text.IndexOf('.');
        }
        if(SecNum.text.IndexOf('.') == -1)
        {
            DecimalDigitsCount = (FrstNum.text.Length - 1 - FrstNum.text.IndexOf('.'));
        }
        if(SecNum.text.IndexOf('.') == -1 && FrstNum.text.IndexOf('.') == -1)
        {
            DecimalDigitsCount = 0;
        }

        string FirstNumCpy = FrstNum.text;// to save real value of the first input
        string SecNumCpy = SecNum.text; // to save real value of the second input
        SubtractionScript.ResetAllValues(Line, FirstNumPlace, FirstNumPlace, sign);
        FirstNumPlaceAdditon.text = "";
        SecNumPlaceAdditon.text = "";
        FirstRes = "";
        SecRes = "";
        AdditionSign.gameObject.SetActive(false);
        Line2.gameObject.SetActive(false);



        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/first" + SpeakerName)));
        if (IsCalledFromOutSide && FrstNum.text.Contains('.'))
        {
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the first number without decimal point" + SpeakerName)));

        }
        else
        {
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the first number" + SpeakerName)));
        }
        try
        {
            FrstNum.text = FrstNum.text.Remove(FrstNum.text.IndexOf('.'),1);
        }

        catch (Exception e)
        {
            Debug.Log(e);
        }
        string TempFrstNum = "";
        for (int i = 0; i < FrstNum.text.Length; i++)
        {
            TempFrstNum += FrstNum.text[i] + " ";
        }
        FirstNumPlace.text = TempFrstNum;

        FirstNumPlace.gameObject.SetActive(true);

        if (FirstNumPlace.text[0].Equals('0'))
        {
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/Remove useless zeros" + SpeakerName)));
            for (int i = 0; i < FirstNumPlace.text.Length; i += 2)
            {
                if(FirstNumPlace.text[i].Equals('0')){
                    FirstNumPlace.text = FirstNumPlace.text.Substring(i + 2);
                    FrstNum.text=FrstNum.text.Substring(1);
                }
                else
                {
                    break;
                }
            }
        }
        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/then" + SpeakerName)));

        if (IsCalledFromOutSide && SecNum.text.Contains('.'))
        {
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the second number without decimal point" + SpeakerName)));

        }
        else
        {
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the second number" + SpeakerName)));

        }
        try
        {
            SecNum.text = SecNum.text.Remove(SecNum.text.IndexOf('.'),1);

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);

        TMP_CharacterInfo charInfo = textInfo.characterInfo[FirstNumPlace.text.Length - 2];
        Vector3 characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, FirstNumPlace.text.Length - 2);


        GameObject newTextObject = new GameObject("SecNumPlace");
        TextMeshProUGUI SecNumPlace = newTextObject.AddComponent<TextMeshProUGUI>();

        // Copy text properties
        SecNumPlace.text = SecNum.text;
        SecNumPlace.font = FirstNumPlace.font;
        SecNumPlace.fontSize = 90;
        SecNumPlace.color = Color.black;
        SecNumPlace.alignment = FirstNumPlace.alignment;
        SecNumPlace.fontStyle = FontStyles.Bold;
        newTextObject.transform.SetParent(FirstNumPlace.transform.parent);

        RectTransform newTextRect = newTextObject.GetComponent<RectTransform>();
        newTextRect.localScale = Vector3.one;
        newTextRect.GetComponent<RectTransform>().anchoredPosition = new Vector2(characterPosition.x - 35, characterPosition.y - 150);
        SecNumPlace.raycastTarget = false;

        sign.gameObject.SetActive(true);
        Line.gameObject.SetActive(true);

        if (SecNumPlace.text[0].Equals('0'))
        {
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/Remove useless zeros" + SpeakerName)));
            for (int i = 0; i < SecNumPlace.text.Length; i ++)
            {
                if (SecNumPlace.text[i].Equals('0'))
                {
                    SecNumPlace.text = SecNumPlace.text.Substring(i+1);
                    SecNum.text = SecNum.text.Substring(i + 1);
                }
                else
                {
                    break;
                }
            }
        }



        if (FirstNumPlace != null)
        {


            // Check if textInfo is available
            if (textInfo != null)
            {
                int index = 1;
                if (SecNum.text.Length < 2)
                    index = 0;
                yield return StartCoroutine(MultiplyByEachDigit(index, SecNumPlace));

                OneDigitMultiplicationScript OneMultiplication = OneDigitMultiplicationObj.GetComponent<OneDigitMultiplicationScript>(); // use the one multplication script in the first number
                OneMultiplication.SetComponenets(FrstNum, SecNum, FirstNumPlace, Line, sign, LangBtn, index);
                OneMultiplication.explain();


                button.interactable = false;

                while (!OneDigitMultiplicationScript.IsFinshed)
                {
                    yield return null;
                }
                OneDigitMultiplicationScript.ResDistance = -500;
                OneDigitMultiplicationScript.SupDistance = 250;

                charInfo = textInfo.characterInfo[FirstNumPlace.text.Length - 2];
                characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, FirstNumPlace.text.Length - 2);


                button.interactable = false;

                if(SecNum.text.Length >= 2)
                {

                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put zero in the first digit" + SpeakerName))); // adding zero in the first digit

                    AdditionScript.InstantiateText(FirstNumPlace, "0", characterPosition.x + 15, characterPosition.y, OneDigitMultiplicationScript.ResDistance, true, 99999); // putting zero in first digit

                    GameObject temp = GameObject.Find("99999");
                    temp.name = "Res0";

                    yield return StartCoroutine(MultiplyByEachDigit(0, SecNumPlace));

                    MultiplyByEachDigit(0, SecNumPlace);
                    OneMultiplication.SetComponenets(FrstNum, SecNum, FirstNumPlace, Line, sign, LangBtn, 0);
                    OneMultiplication.explain(); // use the one multplication script in the second number


                    while (!OneDigitMultiplicationScript.IsFinshed)
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(1f);

                    GetResult();
                    ResVector = new Vector3(characterPosition.x + 15, characterPosition.y, 0);

                    DelResult();

                    FirstNumPlaceAdditon.text = FirstRes;
                    SecNumPlaceAdditon.text = SecRes;

                    FirstNumPlaceAdditon.gameObject.SetActive(true);
                    SecNumPlaceAdditon.gameObject.SetActive(true);

                    Line2.gameObject.SetActive(true);
                    AdditionSign.gameObject.SetActive(true);


                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put zeros in empty digits" + SpeakerName)));


                    string FrstNumTemp = FrstNum.text;
                    string SecNumTemp = SecNum.text;
                    FrstNum.text = FirstRes;
                    SecNum.text = SecRes;
                    AdditionScript.FillWithZeros(FrstNum, SecNum, FirstNumPlaceAdditon, SecNumPlaceAdditon, "0");

                    FrstNum.text = FrstNumTemp;
                    SecNum.text = SecNumTemp;

                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/start adding" + SpeakerName)));
                    OneDigitMultiplicationScript.ResDistance = -400;
                    OneDigitMultiplicationScript.SupDistance = 150;


                    AdditionScript addition = AdditionObj.GetComponent<AdditionScript>();
                    addition.SetComponenets(FrstNum, SecNum, FirstNumPlaceAdditon, SecNumPlaceAdditon, Line2, AdditionSign, LangBtn);
                    addition.explain();
                    while (AdditionScript.IscalledFromOutSide)
                    {
                        yield return null;
                    }
                }


                if (IsCalledFromOutSide && DecimalDigitsCount !=0)
                {
                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/count the decimal digits" + SpeakerName)));

                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/its" + SpeakerName)));

                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(DecimalDigitsCount.ToString())));

                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/count" + SpeakerName)));

                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(DecimalDigitsCount.ToString())));

                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/digits" + SpeakerName)));

                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/starting from right" + SpeakerName)));

                    TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>().Where(obj => obj.name == "-1").ToArray();

                    bool caughtException = false;

                    for (int i = 1; i <= DecimalDigitsCount; i++)
                    {
                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(i.ToString())));

                        try
                        {
                            textMeshProObjects[textMeshProObjects.Length - i].text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{textMeshProObjects[textMeshProObjects.Length - i].text}</color>";
                            characterPosition.x = textMeshProObjects[textMeshProObjects.Length - i].transform.position.x;
                            characterPosition.y = textMeshProObjects[textMeshProObjects.Length - i].transform.position.y;
                        }
                        catch (Exception)
                        {
                            caughtException = true;
                        }


                        // Handle the coroutine outside of the catch block
                        if (caughtException)
                        {
                            yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/nothing in the next digit so put zero" + SpeakerName));

                            AdditionScript.InstantiateText(FirstNumPlace, "0", characterPosition.x - 30, characterPosition.y, OneDigitMultiplicationScript.ResDistance, true);
                            characterPosition = new Vector3(characterPosition.x - 30, characterPosition.y, 0);
                        }
                    }
                    yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and put the decimal point after you finished" + SpeakerName));

                    AdditionScript.InstantiateText(FirstNumPlace, ".", characterPosition.x - 30, characterPosition.y, 0, true);
                    characterPosition = new Vector3(characterPosition.x - 30, characterPosition.y, 0);

                    if (DecimalDigitsCount >= textMeshProObjects.Length)
                    {
                        yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/you should write a value in the unit digit so put" + SpeakerName));
                        AdditionScript.InstantiateText(FirstNumPlace, "0", characterPosition.x - 30, characterPosition.y, 0,true);

                    }
                }
            }
        }
        button.interactable = true;
        OneDigitMultiplicationScript.ResDistance = -400;
        OneDigitMultiplicationScript.SupDistance = 150;
        OneDigitMultiplicationScript.IscalledFromOutSide = false;
        AdditionScript.IscalledFromOutSide = false;
    }
    public char ValidateSecInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar) && text.Length < 2 && !IsCalledFromOutSide)
        {
            return addedChar;
        }
        else if (IsCalledFromOutSide)
        {
            if(char.IsDigit(addedChar) && text.Length < 2)
            {
                return addedChar;

            }
            else if (addedChar.Equals('.') && text.Length==1)
            {
                return addedChar;

            }
            else if(text.Length == 2 && text[1].Equals('.'))
            {
                return addedChar;
            }
            else
            {
                return '\0';
            }
        }
        else
        {
            return '\0';
        }
    }

    public static void RenameResult(int index) // removing first number results
    {
        TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>();
        if (textMeshProObjects.Length > 0)
        {
            foreach (TextMeshProUGUI textMeshPro in textMeshProObjects)
            {
                // Check if the text is a number and greater than or equal to 100
                if (int.TryParse(textMeshPro.name, out int number) && number !=-1)
                {
                    textMeshPro.gameObject.name = "-99";
                }
                else if(number == -1)
                {
                    textMeshPro.gameObject.name = "Res" + index;
                }
            }
        }

    }

    public IEnumerator MultiplyByEachDigit(int index , TextMeshProUGUI SecNumPlace)
    {
        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/multiply" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SecNum.text[index].ToString())));
        string temp = SecNumPlace.text[index].ToString();
        string SecNumPlaceCpy = SecNum.text;
        temp = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{temp}</color>";
        if(index == 1)
            SecNumPlace.text = SecNumPlace.text[0] + temp;
        else {
            try
            {
                SecNumPlace.text = temp + SecNumPlace.text[1];

            }
            catch (Exception)
            {
                SecNumPlace.text = temp;
            }

        }

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/by each digit in the first number" + SpeakerName)));

        string FirstNumPlaceCpy = FirstNumPlace.text;
        for (int i = FirstNumPlace.text.Length - 1; i >= 0; i--)
        {
            if (!FirstNumPlace.text[i].Equals(' ') && !FirstNumPlace.text[i].Equals('.'))
            {
                temp = FirstNumPlace.text.Substring(0, i);
                string temp2 = FirstNumPlace.text.Substring(i + 1);
                temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FirstNumPlace.text[i]}</color>";
                FirstNumPlace.text = temp + temp2;

                yield return new WaitForSeconds(0.2f);
            }
            FirstNumPlace.text = FirstNumPlaceCpy;
        }
        SecNumPlace.text = SecNumPlaceCpy;
    }

    public void GetResult() // saving result into varaiable
    {
        TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>();
        if (textMeshProObjects.Length > 0)
        {
            StringBuilder temp = new StringBuilder();

            foreach (TextMeshProUGUI textMeshPro in textMeshProObjects)
            {
                if (textMeshPro.name.Equals("Res1"))
                {
                    temp = new StringBuilder(FirstRes + textMeshPro.text);
                    FirstRes = temp.ToString();
                    Destroy(textMeshPro.gameObject);
                }
                else if (textMeshPro.name.Equals("Res0"))
                {
                    temp = new StringBuilder(SecRes+ textMeshPro.text );
                    SecRes = temp.ToString();
                    Destroy(textMeshPro.gameObject);

                }
            }
        }
    }

    public void DelResult() // delete results text into varaiable
    {
        TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>();
        if (textMeshProObjects.Length > 0)
        {
            StringBuilder temp = new StringBuilder();

            foreach (TextMeshProUGUI textMeshPro in textMeshProObjects)
            {
                if (textMeshPro.name.Equals("Res1") || textMeshPro.name.Equals("Res0"))
                {
                    Destroy(textMeshPro);
                }
            }
        }
    }
    private void OnInputChanged(TMP_InputField inputField, string input)
    {
        if (input == "0" && DecimmalScript.IsWrited)
        {
            if (!inputField.text.Contains("."))
            {
                inputField.text = "0.";
                StartCoroutine(SetCaretPositionAfterFrame(inputField));
            }
        }
        else if (input == "0." && !DecimmalScript.IsWrited)
        {
            inputField.text = "";
            StartCoroutine(SetCaretPositionAfterFrame(inputField));
        }
        DecimmalScript.IsWrited = false;
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