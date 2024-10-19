using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SubtractionScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI SecNumPlace;
    [SerializeField] private TextMeshProUGUI Line;
    [SerializeField] private TextMeshProUGUI sign;
    [SerializeField] private UnityEngine.UI.Button LangBtn;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    public static bool Explain = false;
    private List<int> borrowingList;
    string SpeakerName = "_Sonya_Eng";
    private string firstNumcpy = "";
    private string Language = "Eng";
    public static bool IsEng = true;
    public static bool IscalledFromOutSide = false;
    public static int ResSpace = -400;
    public void Start()
    {
        UnityAction langBtnClickAction = () => AdditionScript.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

        if (AdditionScript.IsBasic)
        {
            FrstNum.onValidateInput = AdditionScript.ValidateInput;
            SecNum.onValidateInput = AdditionScript.ValidateInput;
        }
        AdditionScript.InitializePlaceholders(FrstNum);
        AdditionScript.InitializePlaceholders(SecNum);
    }
    void Update()
    {
        AdditionScript.EnableExplain(ref FrstNum, ref SecNum);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AdditionScript.IsEng = IsEng;

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

    public void SetComponenets(TMP_InputField FrstNum , TMP_InputField SecNum , TextMeshProUGUI FirstNumPlace , TextMeshProUGUI SecNumPlace , TextMeshProUGUI Line , TextMeshProUGUI sign ,UnityEngine.UI.Button LangBtn)
    {
        this.FrstNum = FrstNum;
        this.SecNum = SecNum;
        this.FirstNumPlace = FirstNumPlace;
        this.SecNumPlace = SecNumPlace;
        this.Line = Line;
        this.sign = sign;

        this.FirstNumPlace = FirstNumPlace;
        this.SecNumPlace = SecNumPlace;
    }
    public IEnumerator solve()
    {
        GameObject ExplainBtn = GameObject.Find("Explain");
        UnityEngine.UI.Button button = ExplainBtn.GetComponent<UnityEngine.UI.Button>();
        button.interactable = false;

        if (Explain)
        {
            if (!IscalledFromOutSide)
            {

                SecNumPlace.gameObject.SetActive(false);
                ResetAllValues(Line, FirstNumPlace, SecNumPlace, sign);
                if (float.Parse(SecNum.text) > float.Parse(FrstNum.text))
                {
                    (SecNum.text, FrstNum.text) = (FrstNum.text, SecNum.text);
                    (FirstNumPlace.text, SecNumPlace.text) = (SecNumPlace.text, FirstNumPlace.text);

                }

                AdditionScript.FillWithZeros(FrstNum, SecNum, FirstNumPlace, SecNumPlace, " ");
                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/first" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the first number" + SpeakerName)));

                FirstNumPlace.gameObject.SetActive(true);

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/then" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the second number" + SpeakerName)));

                SecNumPlace.gameObject.SetActive(true);
                sign.gameObject.SetActive(true);
                if (FrstNum.text.Length != SecNum.text.Length)
                {
                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put zeros in empty digits" + SpeakerName)));
                    AdditionScript.FillWithZeros(FrstNum, SecNum, FirstNumPlace, SecNumPlace, "0");
                }
            }
            else
            {
                while(FirstNumPlace.text.Length > SecNumPlace.text.Length)
                {
                    SecNumPlace.text = "0 " + SecNumPlace.text;
                }
            }
            borrowingList = new List<int>();
            for (int i = 0; i < FirstNumPlace.text.Length; i++) // this list holds the the borrowed value
            {
                borrowingList.Add(0);
            }
            Line.gameObject.SetActive(true);
        }
        if (FirstNumPlace != null)
        {
            // Populate textInfo manually

            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
            firstNumcpy = FirstNumPlace.text;
            // Check if textInfo is available
            if (textInfo != null)
            {
                if (LongDivisionScript.InExplain)
                {

                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/the next step is" + SpeakerName)));

                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/minus" + SpeakerName)));
                }
                for (int i = firstNumcpy.Length - 1; i >= 0; i--)
                {
                    if (char.IsDigit(firstNumcpy[i]))
                    {
                        TMP_CharacterInfo charInfoSecPlace = textInfo.characterInfo[i];
                        Vector3 characterPositionSecPlace = AdditionScript.GetCharPoos(SecNumPlace, charInfoSecPlace, i);

                        string FNum = (Mathf.Abs(int.Parse(firstNumcpy[i].ToString()) - borrowingList[i])).ToString(); // get the number after borrowing
                        string SNum = (SecNumPlace.text[i]).ToString();
                        if (Explain)
                        {
                            yield return (StartCoroutine(Sub(i, FNum, SNum)));


                            if (textInfo.characterInfo != null && i < textInfo.characterInfo.Length)
                            {
                                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                                Vector2 characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, i);


                                int result = int.Parse(FNum.ToString())  - int.Parse(SNum.ToString());
                                if (result >=0)
                                {
                                    if (LongDivisionScript.InLongDev)
                                    {
                                        AdditionScript.InstantiateText(FirstNumPlace, (result).ToString(), characterPosition.x + 10, characterPosition.y, ResSpace+120, true, -100 + i);

                                    }
                                    else
                                        AdditionScript.InstantiateText(FirstNumPlace, (result).ToString(), characterPosition.x + 10, characterPosition.y, ResSpace-100, true, -100 + i);
                                }
                                else // borrowing case
                                {
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+ "/doesnotgo" + SpeakerName)));
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+"/so lend one from" + SpeakerName)));
                                    int counter = 1;
                                    for (int j = i-1; j >=0; j--) // looping for a number to borrow from
                                    {
                                        if(char.IsDigit(FirstNumPlace.text[j]))
                                        {
                                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FirstNumPlace.text[j].ToString())));

                                            string temp = FirstNumPlace.text.Substring(0, j);
                                            string temp2 = FirstNumPlace.text.Substring(j + 1);
                                            temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FirstNumPlace.text[j]}</color>";
                                            FirstNumPlace.text = temp + temp2; //make the current number in loop red
                                            if (firstNumcpy[j].Equals('0')) // if the current nubmber is zero we can't borrow
                                            {
                                                counter++;
                                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound" + "/doesnotgo" + SpeakerName)));
                                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound" + "/so lend one from" + SpeakerName)));
                                            }
                                            else // here we can borrow
                                            {
                                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+"/so" + SpeakerName)));
                                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(firstNumcpy[j].ToString())));
                                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+"/becomes" + SpeakerName)));
                                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((int.Parse(firstNumcpy[j].ToString())-1).ToString())));

                                                borrowingList[j] += 1; // we had borrowed one from this number
                                                charInfo = textInfo.characterInfo[j];
                                                characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, j );
                                                AdditionScript.InstantiateText(FirstNumPlace, (int.Parse(firstNumcpy[j].ToString()) - 1).ToString(), characterPosition.x + (20), characterPosition.y, 150, false, j);

                                                if (counter != 1) { // we borrowed from the next number (no zeros)
                                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+"/zero special case minus" + SpeakerName)));

                                                    for (int k = j+1; k < i; k++)
                                                    {
                                                        if (!firstNumcpy[k].Equals(' ') && !firstNumcpy[k].Equals('.'))
                                                        {
                                                            charInfo = textInfo.characterInfo[k + 1];
                                                            characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, k);
                                                            AdditionScript.InstantiateText(FirstNumPlace, ("9").ToString(), characterPosition.x + 20, characterPosition.y, 150, false, k);
                                                            borrowingList[k] += 9;

                                                        }
                                                    }

                                                }
                                                else // if there are zeros and we make them all 9
                                                {

                                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+"/and" + SpeakerName)));
                                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum.ToString())));
                                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound"+"/becomes" + SpeakerName)));
                                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((int.Parse(FNum) + 10).ToString())));
                                                }

                                                break;
                                            }
                                        }
                                    }

                                    if (borrowingList[i] == 0) // if no prev borrowing for this number
                                    {
                                        AdditionScript.InstantiateText(FirstNumPlace, (int.Parse(FNum.ToString()) + 10).ToString(), characterPositionSecPlace.x+20 , characterPosition.y, 150, false, i);

                                    }
                                    else // if someone is borrowed from him already
                                    {
                                        MakeTextRed(i.ToString(), (int.Parse(FNum.ToString()) + 10).ToString()); // change the prev value and make it red
                                    }

                                    FNum = (int.Parse(FNum)+10).ToString();
                                    result = int.Parse(FNum.ToString()) - int.Parse(SNum.ToString());

                                    yield return (StartCoroutine(Sub(i, FNum, SNum)));
                                    if (LongDivisionScript.InLongDev)
                                    {
                                        AdditionScript.InstantiateText(FirstNumPlace, (result).ToString(), characterPositionSecPlace.x + 15, characterPosition.y, ResSpace +120, true,200 + i);
                                    }
                                    else
                                    {
                                        AdditionScript.InstantiateText(FirstNumPlace, (result).ToString(), characterPositionSecPlace.x + 15, characterPosition.y, ResSpace - 100, true,200 + i);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            
        }
        if (!IscalledFromOutSide)
        {
            StartCoroutine(RemoveUselessZeros());
        }
        Explain = false;
        button.interactable = true;    
    }
    public static void ResetAllValues( TextMeshProUGUI Line , TextMeshProUGUI FirstNumPlace, TextMeshProUGUI SecNumPlace, TextMeshProUGUI sign)
    {
        string[] allowedNames ={
                FirstNumPlace.name,
                SecNumPlace.name,
                Line.name,
                sign.name,

                "Placeholder",
                "EnPlaceholder",
                "ArPlaceholder",
                "Text",
                "Sign2",
                "EffectText",
                "AnswerPlace"
            };
        TextMeshProUGUI[] textMeshPros = FindObjectsOfType<TextMeshProUGUI>();

        foreach (TextMeshProUGUI textMeshPro in textMeshPros)
        {
            // Check if the current textMeshPro's name is not in the allowedNames array
            if (!System.Array.Exists(allowedNames, name => name.Equals(textMeshPro.name)))
            {
                // Destroy the entire GameObject that the TextMeshProUGUI component is part of
                Destroy(textMeshPro.gameObject);
            }
        }
        Line.gameObject.SetActive(false);
        FirstNumPlace.text ="";
        SecNumPlace.text = "";
        FirstNumPlace.gameObject.SetActive(false) ;
        sign.gameObject.SetActive(false) ;
    }
    public IEnumerator Sub(int i , string FNum , string SNum) {
        string temp = "", temp2 = "";
        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));
        if (int.TryParse(FirstNumPlace.text[i].ToString(), out _)) {
            temp = FirstNumPlace.text.Substring(0, i);
            temp2 = FirstNumPlace.text.Substring(i + 1);
            temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum}</color>";
            FirstNumPlace.text = temp + temp2;
        }
        else
        {
            MakeTextRed(i.ToString(), FNum);
        }
        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/minus" + SpeakerName)));
        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));
        if(int.TryParse(SecNumPlace.text[i].ToString(), out _))
        {
            temp = SecNumPlace.text.Substring(0, i);
            temp2 = SecNumPlace.text.Substring(i + 1);
            temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum}</color>";
            SecNumPlace.text = temp + temp2;
        }
        int result = int.Parse(FNum.ToString()) - int.Parse(SNum.ToString());
        if (result >= 2) {
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put" + SpeakerName)));
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum.ToString())));
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/in your mind" + SpeakerName)));
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and count after" + SpeakerName)));
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum.ToString())));
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/on your fingers" + SpeakerName)));
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/to reach" + SpeakerName)));
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum.ToString())));
            yield return AdditionScript.AnimateAJ(false, FNum, SNum, false);
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));
        }

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/the answer is" + SpeakerName)));
        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(result.ToString())));
    }

    public static void MakeTextRed(string name  , string FNum)
    {
        GameObject textGameObject = GameObject.Find((name).ToString());
        TextMeshProUGUI myText = textGameObject.GetComponent<TextMeshProUGUI>();
        myText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum}</color>";
    }

    public IEnumerator RemoveUselessZeros()
    {
        TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>();
        if(textMeshProObjects.Length > 0)
        {
            bool FirstTimeInLoop = true;   
            foreach (TextMeshProUGUI textMeshPro in textMeshProObjects)
            {
                // Check if the text is a number and greater than or equal to 100

                if (int.TryParse(textMeshPro.name, out int number) && number >= 100 && textMeshPro.text == "0")
                {
                    if (FirstTimeInLoop)
                    {
                        FirstTimeInLoop = false;
                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound" + "/Remove useless zeros" + SpeakerName)));

                    }
                    textMeshPro.color = Color.grey;
                    yield return new WaitForSeconds(0.5f);

                    Destroy(textMeshPro.gameObject);
                }
                else
                {
                    break;
                }
            }
        }

    }


}