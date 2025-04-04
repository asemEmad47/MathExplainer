using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DivisionScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI sign;
    [SerializeField] private Button LangBtn;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    private bool Explain = false;
    string SpeakerName = "_Sonya_Eng";
    bool IsEng = true;
    public static bool IscalledFromOutSide = false;

    public static bool IsFinshed = false;

    public static string FirstNumber = "";
    public static string SecNumber = "";

    private Button PauseBtn;
    private Button ResumeBtn;

    public void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        PauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        ResumeBtn = GameObject.Find("Resume").GetComponent<Button>();
        PauseBtn.onClick.AddListener(PauseScript.Pause);
        ResumeBtn.onClick.AddListener(PauseScript.Resume);

        AdditionVoiceSpeaker.VoiceClipsPlace = "AdditionTerms/AdditionSound";
        AdditionVoiceSpeaker.NumPlace = "AdditionTerms/AdditionSound/EngLoop";
        FrstNum.text = FirstNumber;
        SecNum.text = SecNumber;
        IsFinshed = false;

        UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

        FrstNum.onValidateInput = InputFieldsActions.ValidateEqsInput;
        SecNum.onValidateInput = ValidateSecInput;

        InputFieldsActions.InitializePlaceholders(FrstNum);
        InputFieldsActions.InitializePlaceholders(SecNum);
    }
    void Update()
    {
        PauseScript.ControlPause();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AdditionScript.IsEng = IsEng;

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
    public void OutSideSolve(bool Explain)
    {
        this.Explain = Explain;
        StartCoroutine(solve());

    }
    public IEnumerator solve()
    {
        IsFinshed = false;
        GameObject ExplainBtn = GameObject.Find("Explain");
        Button button = ExplainBtn.GetComponent<Button>();
        button.interactable = false;
        ResetValues.ResetAllValues();

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

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"first" + SpeakerName , Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"write the first number" + SpeakerName, Explain)));


            FirstNumPlace.gameObject.SetActive(true);

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"then" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"write the second number" + SpeakerName, Explain)));

            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
            TMP_CharacterInfo charInfo = textInfo.characterInfo[0];
            Vector3 characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, 0);



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
            newTextRect.GetComponent<RectTransform>().anchoredPosition = new Vector2(FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.x +400, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y);
            CharacterProbs.CenterInPos(FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.x + 250, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, ref sign, FirstNumPlace);
            SecNumPlace.raycastTarget = false;

            sign.gameObject.SetActive(true);
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"in division operation" + SpeakerName, Explain)));


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
                        characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i);
                        characterPosition = new Vector3(characterPosition.x, characterPosition.y + 300, characterPosition.z);
                        if (textInfo.characterInfo != null && i < textInfo.characterInfo.Length)
                        {
                            TextMeshProUGUI myText = new TextMeshProUGUI();
                            if (reminder != 0)
                            {
                                GameObject textGameObject = GameObject.Find((i - 3).ToString());
                                myText = textGameObject.GetComponent<TextMeshProUGUI>();
                                FNum = myText.text + FNum;

                            }
                            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));


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

                            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"divide" + SpeakerName, Explain)));

                            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum, Explain)));


                            temp2 = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum}</color>";

                            SecNumPlace.text = temp2;
                            if (int.Parse(FNum) < int.Parse(SNum) && i+3 < FrstNumCpy.Length)
                            {
                                i += 3;
                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"doesnotgo" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"is smaller than" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum, Explain)));


                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,"0", Explain)));

                                TextInstantiator.InstantiateText(FirstNumPlace, "0", characterPosition.x + 15, characterPosition.y, -400, true);

                                FinalResult += "0";

                                /*
                                charInfo = textInfo.characterInfo[i];
                                characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i);
                                */
                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"becomes" + SpeakerName, Explain)));
                                FNum += FrstNumCpy[i];

                                temp = FirstNumPlace.text.Substring(0,FirstNumPlace.text.LastIndexOf("</color>") + 10);
                                temp2 = FirstNumPlace.text.Substring(FirstNumPlace.text.LastIndexOf("</color>") + 11);
                                temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum[1]}</color>";
                                FirstNumPlace.text = temp + temp2;

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));


                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));


                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"divide" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum, Explain)));


                            }
                            else if(int.Parse(FNum) < int.Parse(SNum) && i + 3 >= FrstNumCpy.Length)
                            {
                                IsFinshed = true;
                                reminder = int.Parse(FNum);
                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"doesnotgo" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"is smaller than" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,"0", Explain)));



                                TextInstantiator.InstantiateText(FirstNumPlace, "0", characterPosition.x + 15, characterPosition.y, -400, true);
                                FinalResult += '0';

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"the remainder is" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));

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
                                    reminder += 1;

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"divide" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum, Explain)));

                                    FirstTimeInLoop = true;

                                }

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"equal" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,((int)result).ToString(), Explain)));


                                TextInstantiator.InstantiateText(FirstNumPlace, ((int)result).ToString(), characterPosition.x + 15, characterPosition.y, -400, true);

                                FinalResult += ((int)result).ToString();

                                if (FirstTimeInLoop)
                                {
                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"we took" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum.ToString(), Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"from" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNumCpy, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"the remainder is" + SpeakerName , Explain)));

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,reminder.ToString(), Explain)));



                                }
                            }
                            if (i+3 < FrstNumCpy.Length)
                            {
                                if (FirstTimeInLoop)
                                {
                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put it beside the next digit" + SpeakerName, Explain)));

                                    TextInstantiator.InstantiateText(FirstNumPlace, reminder.ToString(), characterPosition.x + 112, characterPosition.y, -235, true , i);
                                    GameObject textGameObject = GameObject.Find((i).ToString());
                                    myText = textGameObject.GetComponent<TextMeshProUGUI>();
                                    Color color = myText.color;
                                    color.a = 0.7f; // Set opacity to 70%
                                    myText.color = color;

                                }

                            }
                            else
                            {
                                if (reminder != 0)
                                {
                                    characterPosition = new Vector3(characterPosition.x+350 , characterPosition.y+150, characterPosition.z);
                                    FinalResult = int.Parse(FinalResult).ToString();

                                    TextInstantiator.InstantiateText(FirstNumPlace, "R ", characterPosition.x + 150, characterPosition.y, -400, true);

                                    TextInstantiator.InstantiateText(FirstNumPlace, reminder.ToString(), characterPosition.x + 300, characterPosition.y, -400, true);

                                    TextInstantiator.InstantiateText(FirstNumPlace, FinalResult, characterPosition.x - 120, characterPosition.y, -550, true);

                                    TextInstantiator.InstantiateText(FirstNumPlace, reminder.ToString(), characterPosition.x, characterPosition.y, -500, true);

                                    TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", characterPosition.x, characterPosition.y, -550, true);

                                    TextInstantiator.InstantiateText(FirstNumPlace, SecNum.text.ToString(), characterPosition.x, characterPosition.y, -600, true);


                                }
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
        Explain = false;
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
