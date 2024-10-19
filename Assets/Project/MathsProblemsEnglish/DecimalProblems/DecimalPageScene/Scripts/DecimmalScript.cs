using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    public void Start()
    {
        FirstNumPlace.gameObject.SetActive(false);
        SecNumPlace.gameObject.SetActive(false);
        SubtractionScript.ResetAllValues(Line, FirstNumPlace, SecNumPlace, sign);
        UnityAction langBtnClickAction = () => AdditionScript.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

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
        FrstNum.onValidateInput = ValidateDecimalInput;
        SecNum.onValidateInput = ValidateDecimalInput;



        AdditionScript.InitializePlaceholders(FrstNum);
        AdditionScript.InitializePlaceholders(SecNum);
    }
    void Update()
    {
        AdditionScript.EnableExplain(ref FrstNum, ref SecNum);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
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
        AdditionScript.IsEng = IsEng;
        SubtractionScript.IsEng = IsEng;

    }
    public void explain()
    {
        Explain = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(solve());
    }


    public IEnumerator solve()
    {
        AdditionScript.IscalledFromOutSide = true;
        SubtractionScript.IscalledFromOutSide = true;
        GameObject ExplainBtn = GameObject.Find("Explain");
        UnityEngine.UI.Button button = ExplainBtn.GetComponent<UnityEngine.UI.Button>();
        button.interactable = false;

        SubtractionScript.ResetAllValues(Line, FirstNumPlace, SecNumPlace, sign);
        if (float.Parse(SecNum.text) > float.Parse(FrstNum.text))
        {
            (SecNum.text, FrstNum.text) = (FrstNum.text, SecNum.text);
            (FirstNumPlace.text, SecNumPlace.text) = (SecNumPlace.text, FirstNumPlace.text);

        }

        if (!FrstNum.text.Contains('.'))
        {
            FrstNum.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/the first number" + SpeakerName)));
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/has no decimal point so" + SpeakerName)));
            FrstNum.text += '.';
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/you should put a value" + SpeakerName)));
            FrstNum.text += '0';
            FrstNum.GetComponent<UnityEngine.UI.Image>().color = Color.white;


        }


        if (!SecNum.text.Contains('.'))
        {
            SecNum.GetComponent<UnityEngine.UI.Image>().color = Color.red;

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/the second number" + SpeakerName)));
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/has no decimal point so" + SpeakerName)));
            SecNum.text += '.';
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/you should put a value" + SpeakerName)));
            SecNum.text += '0';
            SecNum.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/first" + SpeakerName)));
        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/write the decimal points" + SpeakerName)));


        FirstNumPlace.text = ".";
        SecNumPlace.text = ".";


        FirstNumPlace.gameObject.SetActive(true);
        SecNumPlace.gameObject.SetActive(true);

        yield return (StartCoroutine(WriteNumber(true)));
        yield return (StartCoroutine(WriteNumber(false)));

        if(FrstNum.text.Length != SecNum.text.Length)
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound" + "/put zeros in empty digits" + SpeakerName)));
        FillWithZeros();
        TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
        TMP_CharacterInfo charInfo = textInfo.characterInfo[FirstNumPlace.text.IndexOf('.')];

        Vector3 characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, FirstNumPlace.text.IndexOf('.'));

        AdditionScript.InstantiateText(FirstNumPlace, ".", characterPosition.x , FirstNumPlace.transform.position.y, -280, true, 999);
        GameObject res = GameObject.Find("999");
        TextMeshProUGUI ResText = res.GetComponent<TextMeshProUGUI>();
        ResText.color = FirstNumPlace.color;
        Line.gameObject.SetActive(true);

        DoOperation(); // to dynamicly choose sub or addition

        while (button.interactable==false)
        {
            yield return null;
        }
        StartCoroutine(RemoveUselessZeros());
        AdditionScript.IscalledFromOutSide = false;
        SubtractionScript.IscalledFromOutSide = false;
    }
    public IEnumerator WriteEachNumberInOrder(TMP_InputField number , bool IsRight , TextMeshProUGUI NumPlace)
    {
        string WritedNubmer = "";
        if (IsRight)
        {
            WritedNubmer = number.text.Substring(number.text.IndexOf('.') + 1, number.text.Length - number.text.IndexOf('.') - 1);
            for (int i = 0; i < WritedNubmer.Length; i++)
            {
                if (char.IsDigit(WritedNubmer[i]))
                {
                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(WritedNubmer[i].ToString())));
                    NumPlace.text =  "   "+NumPlace.text;
                    if(i==0)
                        NumPlace.text += " "+WritedNubmer[i] + " ";
                    else
                        NumPlace.text += WritedNubmer[i] + " ";

                }
            }
        }
        else
        {
            WritedNubmer = number.text.Substring(0, number.text.IndexOf('.'));
            StringBuilder temp = new StringBuilder(NumPlace.text);
            int CurrentSpace = NumPlace.text.IndexOf('.') - 2;
            for (int i = WritedNubmer.Length-1; i >= 0; i--)
            {
                yield return StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(WritedNubmer[i].ToString()));

                temp = new StringBuilder(NumPlace.text);
                if(CurrentSpace < 0)
                {
                    temp.Insert(0, WritedNubmer[i]);
                }
                else
                {
                    temp[CurrentSpace] = WritedNubmer[i];
                }
                CurrentSpace -= 2;
                NumPlace.text = temp.ToString();
            }

        }

    }
    public IEnumerator WriteNumber(bool IsRight)
    {
        if(IsRight)
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/write the first number" + SpeakerName)));
        else
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/write the second number" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/starting from the decimal point" + SpeakerName)));
        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/go right" + SpeakerName)));


        RightArrow.SetActive(true);

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/and write each number in order" + SpeakerName)));

        if(IsRight)
            yield return (StartCoroutine(WriteEachNumberInOrder(FrstNum, true, FirstNumPlace)));
        else
            yield return (StartCoroutine(WriteEachNumberInOrder(SecNum, true, SecNumPlace)));
        RightArrow.SetActive(false);

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/go left" + SpeakerName)));

        LeftArrow.SetActive(true);
        if(IsRight)
            yield return (StartCoroutine(WriteEachNumberInOrder(FrstNum, false, FirstNumPlace)));
        else
            yield return (StartCoroutine(WriteEachNumberInOrder(SecNum, false, SecNumPlace)));

        LeftArrow.SetActive(false);

    }

    public void FillWithZeros()
    {
        // Parse inputs from the TMP_InputFields
        if (float.TryParse(FrstNum.text, out float firstNumber) && float.TryParse(SecNum.text, out float secondNumber))
        {
            // Convert the numbers to strings
            string firstStr = firstNumber.ToString();
            string secondStr = secondNumber.ToString();

            // Split into integer and decimal parts
            string[] firstParts = firstStr.Split('.');
            string[] secondParts = secondStr.Split('.');

            string firstInteger = firstParts[0];
            string firstDecimal = firstParts.Length > 1 ? firstParts[1] : "";

            string secondInteger = secondParts[0];
            string secondDecimal = secondParts.Length > 1 ? secondParts[1] : "";

            // Make integer parts equal in length by padding with leading zeros
            int maxIntegerLength = Mathf.Max(firstInteger.Length, secondInteger.Length);
            LongestInt = maxIntegerLength;

            firstInteger = firstInteger.PadLeft(maxIntegerLength, '0');
            secondInteger = secondInteger.PadLeft(maxIntegerLength, '0');

            // Make decimal parts equal in length by padding with trailing zeros
            int maxDecimalLength = Mathf.Max(firstDecimal.Length, secondDecimal.Length);
            firstDecimal = firstDecimal.PadRight(maxDecimalLength, '0');
            secondDecimal = secondDecimal.PadRight(maxDecimalLength, '0');

            // Format both integer and decimal parts with spaces
            if (firstDecimal.Equals(""))
            {
                firstDecimal = "0";
            }
            if (secondDecimal.Equals(""))
            {
                secondDecimal = "0";
            }

            string formattedFirstNum = FormatNumberWithSpaces(firstInteger, firstDecimal, ".");
            string formattedSecondNum = FormatNumberWithSpaces(secondInteger, secondDecimal, ".");

            // Update the TextMeshProUGUI fields with formatted numbers

            FirstNumPlace.text = formattedFirstNum;
            SecNumPlace.text = formattedSecondNum;
        }
        else
        {
            Debug.LogError("Invalid input: Please ensure both input fields contain valid float numbers.");
        }
    }

    // Helper function to format the number with spaces between digits
    private string FormatNumberWithSpaces(string integerPart, string decimalPart, string separator)
    {
        // Add space between digits of the integer part
        string formattedInteger = string.Join(" ", integerPart.ToCharArray());

        // Add space between digits of the decimal part
        string formattedDecimal = string.Join(" ", decimalPart.ToCharArray());

        // Return the formatted number with separator between integer and decimal parts
        return formattedInteger + " " + separator + " " + formattedDecimal;
    }

    public IEnumerator RemoveUselessZeros()
    {
        TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>();

        int counter = 0;
        bool IsFirstTime = true;
        if (textMeshProObjects.Length > 0)
        {
            foreach (TextMeshProUGUI textMeshPro in textMeshProObjects)
            {
                // Check if the text is a number and greater than or equal to 100
                if (int.TryParse(textMeshPro.name, out int number) && number >= 100 && textMeshPro.text == "0" && counter < LongestInt-1)
                {
                    if (IsFirstTime)
                    {
                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound" + "/Remove useless zeros" + SpeakerName)));
                        IsFirstTime = false;
                        yield return new WaitForSeconds(0.5f);
                    }
                    textMeshPro.color = Color.grey;
                    yield return new WaitForSeconds(0.5f);

                    Destroy(textMeshPro.gameObject);
                    counter++;
                }
                else
                {
                    break;
                }
            }
        }

    }
    public void DoOperation()
    {
        switch (PlayerPrefs.GetString("type"))
        {
            case "sub":
                SubtractionScript subtraction = SubtractionnObj.GetComponent<SubtractionScript>();
                subtraction.SetComponenets(FrstNum, SecNum, FirstNumPlace, SecNumPlace, Line, sign, LangBtn);
                subtraction.explain();
                break;

            case "add":
                AdditionScript addition = AdditionObj.GetComponent<AdditionScript>();
                addition.SetComponenets(FrstNum, SecNum, FirstNumPlace, SecNumPlace, Line, sign, LangBtn);
                addition.explain();
                break;

            default:
                break;
        }
    }

    public static char ValidateDecimalInput(string text, int charIndex, char addedChar)
    {
        IsWrited = true;
        if (char.IsDigit(addedChar))
        {
            return addedChar;
        }
        else if (!text.Contains('.') && addedChar.Equals('.'))
        {
            return addedChar;
        }
        else
        {
            return '\0'; // Invalid input
        }
    }      
    public static char ValidateSecDecimalInput(string text, int charIndex, char addedChar)
    {
        IsWrited = true;
        if (char.IsDigit(addedChar) && ((!text.Contains('.') && text.Length <=1) ||(text.Contains('.') && text.Length <= 2)))
        {
            return addedChar;
        }
        else if (!text.Contains('.') && addedChar.Equals('.') && text.Length<2)
        {
            return addedChar;
        }
        else
        {
            return '\0'; // Invalid input
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