using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class DivideByScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI SecNumPlace;
    [SerializeField] private TextMeshProUGUI sign;
    [SerializeField] private TextMeshProUGUI ResPlace;
    [SerializeField] private Button LangBtn;

    private AudioClip[] loop;
    public static AudioSource audioSource;

    private bool Explain = false;
    string SpeakerName = "_Sonya_Eng";
    bool IsEng = true;
    public void Start()
    {
        UnityAction langBtnClickAction = () => AdditionScript.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

        AdditionScript.IscalledFromOutSide = true;
        FrstNum.onValueChanged.AddListener((input) => OnInputChanged(FrstNum, input));

        FrstNum.onValidateInput = DecimmalScript.ValidateDecimalInput; SecNum.onValidateInput = ValidateSecInput;


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

    }
    public void explain()
    {
        Explain = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(solve());
    }
    public IEnumerator solve()
    {
        FirstNumPlace.gameObject.SetActive(false);
        SecNumPlace.gameObject.SetActive(false);
        SubtractionScript.ResetAllValues(ResPlace, FirstNumPlace, SecNumPlace, sign);
        GameObject ExplainBtn = GameObject.Find("Explain");
        Button button = ExplainBtn.GetComponent<Button>();
        button.interactable = false;
        FirstNumPlace.text = "";
        SecNumPlace.text = "";
        ResPlace.text = "R = ";
        ResPlace.gameObject.SetActive(false);
        sign.gameObject.SetActive(false);

        string FirstNumCpy = FrstNum.text;// to save real value of the first input
        string SecNumCpy = SecNum.text; // to save real value of the second input

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/first" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the first number" + SpeakerName)));


        string TempFrstNum = "";
        for (int i = 0; i < FrstNum.text.Length; i++)
        {
            TempFrstNum += FrstNum.text[i] + " ";
        }
        FirstNumPlace.text = TempFrstNum;
        FirstNumCpy = FirstNumPlace.text;
        FirstNumPlace.gameObject.SetActive(true);
        if (!FrstNum.text.Contains('.'))
        {
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/we dont have a decimal" + SpeakerName)));
            FirstNumPlace.text += ". 0 ";

        }
        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/then" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the second number" + SpeakerName)));


        TempFrstNum = "";
        for (int i = 0; i < SecNum.text.Length; i++)
        {
            TempFrstNum += SecNum.text[i] + " ";
        }
        SecNumPlace.text = TempFrstNum;

        SecNumPlace.gameObject.SetActive(true);

        sign.gameObject.SetActive(true);

        if (FirstNumPlace != null)
        {
            // Populate textInfo manually
            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);

            // Check if textInfo is available
            if (textInfo != null)
            {
                button.interactable = false;

                TMP_CharacterInfo charInfo = textInfo.characterInfo[FirstNumPlace.text.Length - 2];
                Vector3 characterPosition = FirstNumPlace.transform.TransformPoint(charInfo.bottomLeft);

                button.interactable = false;

                int NumberOfZeros = SecNum.text.Substring(1).Length;

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/count the zeros of the second number" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/its" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(NumberOfZeros.ToString())));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/count" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(NumberOfZeros.ToString())));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/digits" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/starting from the decimal point" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/go left" + SpeakerName)));

                int DecimalPlace = FirstNumPlace.text.IndexOf(".");
                if(DecimalPlace == -1)
                    DecimalPlace = 0;

                bool caughtException = false;

                int i = 2;
                int counter = 0;
                while (counter!=NumberOfZeros)
                {
                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((counter+1).ToString())));

                    try
                    {
                        charInfo = textInfo.characterInfo[i + DecimalPlace];
                        characterPosition = FirstNumPlace.transform.TransformPoint(charInfo.bottomLeft);
                        string temp, temp2;
                        if (i == 2)
                        {
                            temp = FirstNumPlace.text.Substring(0, DecimalPlace - i);
                            temp2 = FirstNumPlace.text.Substring(DecimalPlace - i + 1);
                            temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FirstNumPlace.text[DecimalPlace - i]}</color>";
                            FirstNumPlace.text = temp + temp2; //make the current number in loop red
                        }
                        else
                        {
                            temp = FirstNumPlace.text.Substring(0,FirstNumPlace.text.IndexOf("<color") -2);
                            temp2 = FirstNumPlace.text.Substring(FirstNumPlace.text.IndexOf("<color"));

                            temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FirstNumPlace.text[FirstNumPlace.text.IndexOf("<color") - 2]+" "}</color>";

                            FirstNumPlace.text = temp + temp2;
                        }

                    }
                    catch (Exception)
                    {
                        caughtException = true;
                    }


                    // Handle the coroutine outside of the catch block
                    if (caughtException)
                    {
                        yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/nothing in the next digit so put zero" + SpeakerName));

                        FirstNumPlace.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{"0 "}</color>"+ FirstNumPlace.text;
                    }

                    i -= 2;
                    counter++;
                }
                    yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and put the decimal point after you finished" + SpeakerName));

                if(FirstNumPlace.text.IndexOf("<color")!=0)
                    FirstNumPlace.text = FirstNumPlace.text.Insert(FirstNumPlace.text.IndexOf("<color")-1, " . ");
                else
                {
                    FirstNumPlace.text = FirstNumPlace.text.Insert(0, " . ");

                }
                if (FirstNumPlace.text.Substring(0,2).Equals(" ."))
                {
                    yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/you should put a value" + SpeakerName));

                    FirstNumPlace.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{"0"}</color>" + FirstNumPlace.text;
                }
                yield return new WaitForSeconds(1);
                int FirstDecimal = FirstNumPlace.text.LastIndexOf(".");
                FirstNumPlace.text = FirstNumPlace.text.Remove(FirstDecimal,2);
            }
        }
        if (FirstNumPlace.text[FirstNumPlace.text.Length-2].Equals('0'))
        {
            yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/Remove useless zeros" + SpeakerName));
            FirstNumPlace.text = FirstNumPlace.text.Substring(0,FirstNumPlace.text.Length - 2);
            while (true)
            {
                if (FirstNumPlace.text[FirstNumPlace.text.Length - 2].Equals('0') || FirstNumPlace.text[FirstNumPlace.text.Length - 2].Equals('>') || FirstNumPlace.text[FirstNumPlace.text.Length - 1].Equals('>'))
                {
                    if (FirstNumPlace.text[0].Equals('0'))
                    {
                        FirstNumPlace.text = FirstNumPlace.text.Substring(0,FirstNumPlace.text.Length - 2);
                    }
                    else
                    {
                        int index = FirstNumPlace.text.LastIndexOf("</color");
                        if ((FirstNumPlace.text[index-1].Equals(' ') && FirstNumPlace.text[index - 2].Equals('0')) || FirstNumPlace.text[index - 1].Equals('0'))
                            FirstNumPlace.text = FirstNumPlace.text.Substring(0, FirstNumPlace.text.LastIndexOf("<color"));

                    }
                }
                else if((FirstNumPlace.text[FirstNumPlace.text.Length - 2].Equals('.') || FirstNumPlace.text[FirstNumPlace.text.Length - 1].Equals('.')) || FirstNumPlace.text[FirstNumPlace.text.Length - 3].Equals('.'))
                        FirstNumPlace.text = FirstNumPlace.text.Substring(0, FirstNumPlace.text.Length - 2);

                else
                {
                    break;
                }
            }
        }

        yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName));
        yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/the answer is" + SpeakerName));

        ResPlace.gameObject.SetActive(true);
        ResPlace.text += FirstNumPlace.text;
        button.interactable = true;
    }
    public char ValidateSecInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar) && text.Length < 1 && addedChar.Equals('1'))
        {
            return addedChar;
        }
        else if(text.Length < 5 && addedChar.Equals('0') && text[0].Equals('1'))
        {
            return addedChar;

        }
        else
        {
            return '\0';
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
    }
}