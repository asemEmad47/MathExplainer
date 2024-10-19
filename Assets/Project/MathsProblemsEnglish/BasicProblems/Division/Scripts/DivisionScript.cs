using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DivisionScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI Line;
    [SerializeField] private TextMeshProUGUI sign;
    [SerializeField] private Button LangBtn;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    private bool Explain = false;
    string SpeakerName = "_Sonya_Eng";
    bool IsEng = true;
    public static bool IscalledFromOutSide = false;

    public static bool IsFinshed = false;
    public void Start()
    {
        IsFinshed = false;

        UnityAction langBtnClickAction = () => AdditionScript.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

        FrstNum.onValidateInput = AdditionScript.ValidateInput;
        SecNum.onValidateInput = ValidateSecInput;

        AdditionScript.InitializePlaceholders(FrstNum);
        AdditionScript.InitializePlaceholders(SecNum);
    }
    void Update()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AdditionScript.IsEng = IsEng;
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
        IsFinshed = false;
        GameObject ExplainBtn = GameObject.Find("Explain");
        Button button = ExplainBtn.GetComponent<Button>();
        button.interactable = false;
        SubtractionScript.ResetAllValues( Line, FirstNumPlace, FirstNumPlace, sign);

        FirstNumPlace.text = "";
        foreach (char latter in FrstNum.text)
        {
            FirstNumPlace.text += latter + "  ";
        }
        string FrstNumCpy = FirstNumPlace.text;
        string FinalResult = "";
        if (FirstNumPlace != null)
        {
            // Populate textInfo manually

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/first" + SpeakerName)));

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the first number" + SpeakerName)));


            FirstNumPlace.gameObject.SetActive(true);

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/then" + SpeakerName)));

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the second number" + SpeakerName)));

            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
            TMP_CharacterInfo charInfo = textInfo.characterInfo[0];
            Vector3 characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, 0);



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
            newTextRect.GetComponent<RectTransform>().anchoredPosition = new Vector2(characterPosition.x+25 , characterPosition.y - 70);
            SecNumPlace.raycastTarget = false;

            Line.gameObject.SetActive(true);
            sign.gameObject.SetActive(true);
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/in division operation" + SpeakerName)));


            if (textInfo != null)
            {
                int reminder = 0;
                int OriginalCounter = 0;
                bool IsFinshed = false;
                for (int i = 0; i < FrstNumCpy.Length; i++)
                {


                    string FNum = FrstNumCpy[i].ToString();
                    string SNum = SecNum.text;
                    OriginalCounter = i;

                    if (!FrstNumCpy[i].Equals(' '))
                    {
                        charInfo = textInfo.characterInfo[i];
                        characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, i);
                        if (textInfo.characterInfo != null && i < textInfo.characterInfo.Length)
                        {
                            TextMeshProUGUI myText = new TextMeshProUGUI();
                            if (reminder != 0)
                            {
                                GameObject textGameObject = GameObject.Find((i - 3).ToString());
                                myText = textGameObject.GetComponent<TextMeshProUGUI>();
                                FNum = myText.text + FNum;

                            }
                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));


                            string temp = FirstNumPlace.text.Substring(0, i);
                            string temp2 = FirstNumPlace.text.Substring(i + 1);
                            if (FirstNumPlace.text.LastIndexOf("</color>") !=-1)
                            {
                                temp = FirstNumPlace.text.Substring(0, FirstNumPlace.text.LastIndexOf("</color>") + 10);
                                temp2 = FirstNumPlace.text.Substring(FirstNumPlace.text.LastIndexOf("</color>") + 11);
                            }

                            temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum[FNum.Length-1]}</color>";
                            FirstNumPlace.text = temp + temp2;
                            myText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{myText.text}</color>";

                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/divide" + SpeakerName)));

                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));


                            temp2 = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum}</color>";

                            SecNumPlace.text = temp2;
                            if (int.Parse(FNum) < int.Parse(SNum) && i+3 < FrstNumCpy.Length)
                            {
                                i += 3;
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/doesnotgo" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/is smaller than" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));


                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait("0")));

                                AdditionScript.InstantiateText(FirstNumPlace, "0", characterPosition.x + 15, characterPosition.y, -400, true);

                                FinalResult += "0";

                                charInfo = textInfo.characterInfo[i];
                                characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, i);

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/becomes" + SpeakerName)));
                                FNum += FrstNumCpy[i];

                                temp = FirstNumPlace.text.Substring(0,FirstNumPlace.text.LastIndexOf("</color>") + 10);
                                temp2 = FirstNumPlace.text.Substring(FirstNumPlace.text.LastIndexOf("</color>") + 11);
                                temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum[1]}</color>";
                                FirstNumPlace.text = temp + temp2;

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));


                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));


                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/divide" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));


                            }
                            else if(int.Parse(FNum) < int.Parse(SNum) && i + 3 >= FrstNumCpy.Length)
                            {
                                IsFinshed = true;
                                reminder = int.Parse(FNum);
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/doesnotgo" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/is smaller than" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait("0")));



                                AdditionScript.InstantiateText(FirstNumPlace, "0", characterPosition.x + 15, characterPosition.y, -400, true);
                                FinalResult += '0';

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/the remainder is" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                            }
                            float result = ((float)int.Parse(FNum) / int.Parse(SNum));
                            bool FirstTimeInLoop = false;
                            string FNumCpy = "";

                            if (!IsFinshed)
                            {
                                reminder = 0;

                                result = float.Parse(FNum) / float.Parse(SecNum.text);
                                while (float.Parse(FNum) % float.Parse(SecNum.text) != 0)
                                {
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/doesnotgo" + SpeakerName)));

                                    if (!FirstTimeInLoop)
                                    {
                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));

                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/is greater than" + SpeakerName)));

                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));

                                        FNumCpy = FNum;

                                    }
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName))
                                        );
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/reduce one" + SpeakerName)));

                                    FNum = (int.Parse(FNum) - 1).ToString();
                                    reminder += 1;

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/divide" + SpeakerName)));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));

                                    FirstTimeInLoop = true;

                                }

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(((int)result).ToString())));


                                AdditionScript.InstantiateText(FirstNumPlace, ((int)result).ToString(), characterPosition.x + 15, characterPosition.y, -400, true);

                                FinalResult += ((int)result).ToString();

                                if (FirstTimeInLoop)
                                {
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/we took" + SpeakerName)));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum.ToString())));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/from" + SpeakerName)));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNumCpy)));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/the remainder is" + SpeakerName)));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(reminder.ToString())));



                                }
                            }
                            if (i+3 < FrstNumCpy.Length)
                            {
                                if (FirstTimeInLoop)
                                {
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put it beside the next digit" + SpeakerName)));

                                    AdditionScript.InstantiateText(FirstNumPlace, reminder.ToString(), characterPosition.x + 112, characterPosition.y, 55, true , i);
                                    GameObject textGameObject = GameObject.Find((i).ToString());
                                    myText = textGameObject.GetComponent<TextMeshProUGUI>();
                                    Color color = myText.color;
                                    color.a = 0.7f; // Set opacity to 70%
                                    myText.color = color;

                                }

                            }
                            else
                            {
                                FinalResult = int.Parse(FinalResult).ToString();

                                AdditionScript.InstantiateText(FirstNumPlace, "R = ", characterPosition.x + 120, characterPosition.y, -400, true);

                                AdditionScript.InstantiateText(FirstNumPlace, reminder.ToString(), characterPosition.x + 220, characterPosition.y, -400, true);

                                AdditionScript.InstantiateText(FirstNumPlace, FinalResult, characterPosition.x-120 , characterPosition.y, -550, true);

                                AdditionScript.InstantiateText(FirstNumPlace, reminder.ToString(), characterPosition.x, characterPosition.y, -500, true);

                                AdditionScript.InstantiateText(FirstNumPlace, "\u2015", characterPosition.x , characterPosition.y, -550, true);

                                AdditionScript.InstantiateText(FirstNumPlace, SecNum.text.ToString(), characterPosition.x, characterPosition.y, -600, true);

                            }

                            if (!FirstTimeInLoop)
                            {
                                reminder = 0;
                            }
                        }
                    }
                    SecNumPlace.text = SecNum.text;
                }
            }
        }

        button.interactable = true;
        IsFinshed = true;
    }
    public static char ValidateSecInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar) && text.Length < 1 && addedChar!='0')
        {
            return addedChar;
        }
        else
        {
            return '\0';
        }
    }


}
