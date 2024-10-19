using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class MutiplyByScript : MonoBehaviour
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

        FrstNum.onValidateInput = DecimmalScript.ValidateDecimalInput;
        SecNum.onValidateInput = ValidateSecInput;


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
            FirstNumPlace.text += ". ";

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

                if(FrstNum.text.Contains('.'))
                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/starting from the decimal point" + SpeakerName)));

                else
                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/starting from right" + SpeakerName)));

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
                            temp = FirstNumPlace.text.Substring(0, i + DecimalPlace);
                            temp2 = FirstNumPlace.text.Substring(i + DecimalPlace + 1);
                            temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FirstNumPlace.text[i + DecimalPlace]}</color>";
                            FirstNumPlace.text = temp + temp2; //make the current number in loop red
                        }
                        else
                        {
                            temp = FirstNumPlace.text.Substring(0, FirstNumPlace.text.LastIndexOf("</color>") + 9);
                            temp2 = FirstNumPlace.text.Substring(FirstNumPlace.text.LastIndexOf("</color>") + 10);

                            temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FirstNumCpy[i+DecimalPlace]}</color>";
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

                        FirstNumPlace.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{"0 "}</color>";
                    }

                    i += 2;
                    counter++;
                }
                    yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and put the decimal point after you finished" + SpeakerName));

                i = FirstNumPlace.text.LastIndexOf("</color>") +8;

                FirstNumPlace.text = FirstNumPlace.text.Insert(i, " .");
                if (caughtException  || FirstNumPlace.text.Substring(i + 1).Equals(". "))
                {
                    yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/you should put a value" + SpeakerName));

                    FirstNumPlace.text += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{"0 "}</color>";
                }
                yield return new WaitForSeconds(1);
                int FirstDecimal = FirstNumPlace.text.IndexOf(".");
                FirstNumPlace.text = FirstNumPlace.text.Remove(FirstDecimal,2);
            }
            if (FrstNum.text[0].Equals('0'))
            {
                yield return StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/Remove useless zeros" + SpeakerName));
                FirstNumPlace.text = FirstNumPlace.text.Substring(2);
                while (true)
                {
                    if (FirstNumPlace.text[0].Equals('0') || FirstNumPlace.text[0].Equals('<'))
                    {
                        if (FirstNumPlace.text[0].Equals('0'))
                        {
                            FirstNumPlace.text = FirstNumPlace.text.Substring(2);
                        }
                        else
                        {
                            if (FirstNumPlace.text[15].Equals('0'))
                            {
                                FirstNumPlace.text = FirstNumPlace.text.Substring(25);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
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
        else if(text.Length >=1 && text.Length < 5 && addedChar.Equals('0') && text[0].Equals('1'))
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