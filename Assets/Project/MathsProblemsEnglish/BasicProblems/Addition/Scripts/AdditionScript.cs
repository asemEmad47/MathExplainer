using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdditionScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI SecNumPlace;
    [SerializeField] private TextMeshProUGUI Line;
    [SerializeField] private TextMeshProUGUI sign;

    [SerializeField] private Button LangBtn;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    private bool Explain = false;
    public static string SpeakerName = "_Sonya_Eng";
    public static bool IsEng = true;
    public static bool IscalledFromOutSide = false;
    public static bool IsBasic = true;


    public static string FirstNumber = "";
    public static string SecNumber = "";


    public void Start()
    {
        AdditionVoiceSpeaker.VoiceClipsPlace = "AdditionTerms/AdditionSound";
        FrstNum.text = FirstNumber;
        SecNum.text = SecNumber;
        LoadAllAudioClips();
        UnityAction langBtnClickAction = () =>LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);
        if (!IscalledFromOutSide && IsBasic)
        {
            FrstNum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
            SecNum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        }

        InputFieldsActions.InitializePlaceholders(FrstNum);
        InputFieldsActions.InitializePlaceholders(SecNum);

    }
    void Update()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ExplainEnableMent.EnableExplain(ref FrstNum, ref SecNum);

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
    }
    public void SetComponenets(TMP_InputField FrstNum, TMP_InputField SecNum, TextMeshProUGUI FirstNumPlace, TextMeshProUGUI SecNumPlace, TextMeshProUGUI Line, TextMeshProUGUI sign,  Button LangBtn)
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
    public void ExplainBtnAction()
    {
        LoadAllAudioClips();
        Explain = true;
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
        audioSource = GetComponent<AudioSource>();
        GameObject ExplainBtn = GameObject.Find("Explain");
        Button button = ExplainBtn.GetComponent<Button>();
        button.interactable = false;

        LoadAllAudioClips();

        if (!IscalledFromOutSide)
        {
            FirstNumPlace.text = "";
            SecNumPlace.text = "";
            FirstNumPlace.gameObject.SetActive(false);
            SecNumPlace.gameObject.SetActive(false);
            ResetValues.ResetAllValues(Line, FirstNumPlace, SecNumPlace, sign);

            ZerosOps.FillWithZeros(FrstNum, SecNum, FirstNumPlace, SecNumPlace, " ");
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"first" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"write the first number" + SpeakerName, Explain)));

            FirstNumPlace.gameObject.SetActive(true);

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"then" + SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"write the second number" + SpeakerName, Explain)));

            SecNumPlace.gameObject.SetActive(true);
            sign.gameObject.SetActive(true);
            if (FrstNum.text.Length != SecNum.text.Length)
            {
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put zeros in empty digits" + SpeakerName, Explain)));
                ZerosOps.FillWithZeros(FrstNum, SecNum, FirstNumPlace, SecNumPlace, "0");
            }
            Line.gameObject.SetActive(true);

        }

        if (FirstNumPlace != null)
        {
            // Populate textInfo manually
            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);

            // Check if textInfo is available
            if (textInfo != null)
            {
                bool IsCarried = false;
                for (int i = FirstNumPlace.text.Length - 1; i >= 0; i--)
                {

                    if (!FirstNumPlace.text[i].Equals(' ') && !FirstNumPlace.text[i].Equals('.'))
                    {
                        string FNum = FirstNumPlace.text[i].ToString();
                        string SNum = SecNumPlace.text[i].ToString();

                        int SmallestNum = Math.Min(int.Parse(FNum.ToString()), int.Parse(SNum.ToString()));

                        for (int j = 0; j < loop.Length - 1; j++)
                        {
                            if (IsCarried && j == 0)
                            {
                                GameObject textGameObject;
                                TextMeshProUGUI myText;
                                if (FirstNumPlace.text[i + 2].Equals('.'))
                                {

                                    textGameObject = GameObject.Find((i + 4).ToString()); 
                                }
                                else{
                                    textGameObject = GameObject.Find((i + 2).ToString());
                                }
                                myText = textGameObject.GetComponent<TextMeshProUGUI>();

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,"1",Explain)));
                                myText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>1</color>";

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"plus" + SpeakerName, Explain)));

                            }
                            if (j == 0)
                            {
                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum,Explain)));

                                if (IsCarried)
                                {

                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,(int.Parse(FNum) + 1).ToString(), Explain)));

                                }
                                string temp = FirstNumPlace.text.Substring(0, i);
                                string temp2 = FirstNumPlace.text.Substring(i + 1);
                                temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum}</color>";
                                FirstNumPlace.text = temp + temp2;
                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"plus" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum, Explain)));

                                temp = SecNumPlace.text.Substring(0, i);
                                temp2 = SecNumPlace.text.Substring(i + 1);
                                temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum}</color>";
                                SecNumPlace.text = temp + temp2;
                                if(SmallestNum <= 2)
                                {
                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"equal" + SpeakerName, Explain)));
                                    break;
                                }

                                if(SmallestNum == int.Parse(FNum))
                                {
                                    (FNum , SNum) = (SNum , FNum);
                                }
                            }
                            if (j != 5 && Explain)
                            {
                                audioSource.clip = loop[j];
                                audioSource.Play();
                                yield return new WaitForSeconds(audioSource.clip.length);
                            }
                            else
                            {
                                if (Explain)
                                    yield return AJAnimationHandler.AnimateAJ(IsCarried , FNum , SNum ,true);

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"the answer is" + SpeakerName, Explain)));
                            }
                            if (j == 0)
                            {
                                if (IsCarried)
                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,(int.Parse(FNum) + 1).ToString(), Explain)));
                                else
                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));
                            }
                            else if (j == 2)
                            {
                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum , Explain)));
                            }
                            else if (j == 3)
                            {
                                if (IsCarried)
                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,(int.Parse(FNum) + 1).ToString(), Explain)));
                                else
                                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum , Explain)));
                            }
                        }
                        TMP_CharacterInfo charInfo = new TMP_CharacterInfo();
                        if (textInfo.characterInfo != null && i < textInfo.characterInfo.Length)
                        {

                            Vector2 uiPosition =CharacterProbs.GetCharPoos(FirstNumPlace,charInfo, i);
                            int result = int.Parse(FNum.ToString()) + int.Parse(SNum.ToString());

                            int carried = 0;
                            if(IsCarried)
                                carried=1;
                            if (int.Parse(FNum.ToString()) +carried+ int.Parse(SNum.ToString()) < 10)
                            {
                                if (IsCarried)
                                    result += 1;
                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,result.ToString() , Explain)));

                                if((int)uiPosition.y -430 > FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y)
                                {
                                    TextInstantiator.InstantiateText(FirstNumPlace, (result).ToString(), uiPosition.x + 10, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, -200, true);

                                }
                                else
                                {
                                    TextInstantiator.InstantiateText(FirstNumPlace, (result).ToString(), uiPosition.x + 10, uiPosition.y, -330, true);
                                }
                                IsCarried = false;
                            }
                            else
                            {
                                if (IsCarried)
                                    result += 1;
                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,result.ToString(), Explain)));

                                result -= 10;
                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,result.ToString(), Explain)));


                                if ((int)uiPosition.y - 430 > FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y)
                                {
                                    TextInstantiator.InstantiateText(FirstNumPlace, (result).ToString(), uiPosition.x + 10, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, -200, true);

                                }
                                else
                                {
                                    TextInstantiator.InstantiateText(FirstNumPlace, (result).ToString(), uiPosition.x + 10, uiPosition.y, -330, true);

                                }


                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and carry up one" + SpeakerName, Explain)));


                                if (i - 2 >= 0 && !FirstNumPlace.text[i-2].Equals('.'))
                                {
                                    uiPosition = CharacterProbs.GetCharPoos(FirstNumPlace,charInfo, i - 2);
                                    if (!TwoDigitsMultiplicationScript.IsCalledFromOutSide)
                                    {
                                        TextInstantiator.InstantiateText(FirstNumPlace, "1", uiPosition.x + 20, uiPosition.y, 150, false, i);

                                    }
                                    else
                                    {
                                        TextInstantiator.InstantiateText(FirstNumPlace, "1", uiPosition.x + 20, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, -100, false, i);
                                    }
                                    IsCarried = true;
                                }
                                else if(i - 2 >= 0 && FirstNumPlace.text[i - 2].Equals('.'))
                                {
                                    uiPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i - 4);
                                    TextInstantiator.InstantiateText(FirstNumPlace, "1", uiPosition.x + 20, uiPosition.y, 150, false, i);
                                    IsCarried = true;
                                }
                                if(IsCarried  && i == 0)
                                {
                                    if (!TwoDigitsMultiplicationScript.IsCalledFromOutSide)
                                    {
                                        TextInstantiator.InstantiateText(FirstNumPlace, "1", uiPosition.x - 70, uiPosition.y, 150, false, i);
                                    }
                                    else
                                    {
                                        TextInstantiator.InstantiateText(FirstNumPlace, "1", uiPosition.x - 70, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, 60, false, i);

                                    }
                                    yield return new WaitForSeconds(0.3f);
                                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and write it down" + SpeakerName, Explain)));
                                    if (!TwoDigitsMultiplicationScript.IsCalledFromOutSide)
                                    {
                                        TextInstantiator.InstantiateText(FirstNumPlace, "1", uiPosition.x - 70, uiPosition.y, -340, false, i);

                                    }
                                    else
                                    {
                                        TextInstantiator.InstantiateText(FirstNumPlace, "1", uiPosition.x - 70, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, -200, false, i);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        IscalledFromOutSide = false;
        Explain = false;
    }
    public void LoadAllAudioClips()
    {
        if (IsEng)
        {
            loop = Resources.LoadAll<AudioClip>("AdditionTerms/AdditionSound/EngLoop");
        }
        else
        {
            loop = Resources.LoadAll<AudioClip>("AdditionTerms/AdditionSound/ArabLoop");
        }
    }
}