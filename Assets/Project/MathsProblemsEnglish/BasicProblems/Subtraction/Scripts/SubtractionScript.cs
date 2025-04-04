using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SubtractionScript : MonoBehaviour
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
    public static bool Explain = false;
    private List<int> borrowingList;
    string SpeakerName = "_Sonya_Eng";
    private string firstNumcpy = "";
    private string Language = "Eng";
    public static bool IsEng = true;
    public static bool IscalledFromOutSide = false;
    public static int ResSpace = -400;

    public static string FirstNumber = "";
    public static string SecNumber = "";
    List<TMP_InputField> FieldsList;
    private Button PauseBtn;
    private Button ResumeBtn;
    public void Start()
    {
        PauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        ResumeBtn = GameObject.Find("Resume").GetComponent<Button>();
        PauseBtn.onClick.AddListener(PauseScript.Pause);
        ResumeBtn.onClick.AddListener(PauseScript.Resume);

        FieldsList = new List<TMP_InputField> { FrstNum, SecNum };
        AdditionVoiceSpeaker.VoiceClipsPlace = "AdditionTerms/AdditionSound";
        FrstNum.text = FirstNumber;
        SecNum.text = SecNumber;


        UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

        if (AdditionScript.IsBasic)
        {
            FrstNum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
            SecNum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
        }
        InputFieldsActions.InitializePlaceholders(FrstNum);
        InputFieldsActions.InitializePlaceholders(SecNum);

        GameObject.Find("Explain").GetComponent<Button>().onClick.AddListener(ExplainBtnAction);
        GameObject.Find("Solve").GetComponent<Button>().onClick.AddListener(SolveBtnAction);
    }
    void Update()
    {
        PauseScript.ControlPause();
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

        if (Input.GetKeyDown(KeyCode.Escape) && !IscalledFromOutSide)
        {
            SceneManager.LoadScene("BasicOpScene");
        }

        if (!AdditionVoiceSpeaker.VoiceClipsPlace.Equals("JennySound"))
        {
            if (AdditionVoiceSpeaker.IsEng)
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

    public void SolveBtnAction()
    {
        Explain = false;
        StartCoroutine(solve());

    }
    public void ExplainBtnAction()
    {
        Explain = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(solve());
    }
    public void OutSideSolve(bool Explain)
    {
        SubtractionScript.Explain = Explain;
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

        if (!IscalledFromOutSide)
        {

            SecNumPlace.gameObject.SetActive(false);
            ResetValues.ResetAllValues();
            if (float.Parse(SecNum.text) > float.Parse(FrstNum.text))
            {
                (SecNum.text, FrstNum.text) = (FrstNum.text, SecNum.text);
                (FirstNumPlace.text, SecNumPlace.text) = (SecNumPlace.text, FirstNumPlace.text);

            }

            ZerosOps.FillWithZeros(FrstNum, SecNum, FirstNumPlace, SecNumPlace, " ");
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"first" + SpeakerName , Explain)));

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
        if (FirstNumPlace != null)
        {
            // Populate textInfo manually

            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
            firstNumcpy = FirstNumPlace.text;
            // Check if textInfo is available
            if (textInfo != null)
            {
                if (LongDivisionScript.InLongDev)
                {

                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"the next step is" + SpeakerName, Explain)));

                    yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"minus" + SpeakerName, Explain)));

                }
                for (int i = firstNumcpy.Length - 1; i >= 0; i--)
                {
                    if (char.IsDigit(firstNumcpy[i]))
                    {
                        TMP_CharacterInfo charInfoSecPlace = textInfo.characterInfo[i];
                        Vector3 characterPositionSecPlace = CharacterProbs.GetCharPoos(SecNumPlace, charInfoSecPlace, i);

                        string FNum = (Mathf.Abs(int.Parse(firstNumcpy[i].ToString()) - borrowingList[i])).ToString(); // get the number after borrowing
                        string SNum = (SecNumPlace.text[i]).ToString();
                        if (LongDivisionScript.InLongDev)
                        {
                            if (int.Parse(ZerosOps.GetNumberWithoutZeros(FirstNumPlace.text, i)) != 0 )
                            {
                                yield return (StartCoroutine(Sub(i, FNum, SNum)));
                            }
                        }
                        else
                        {
                            yield return (StartCoroutine(Sub(i, FNum, SNum)));
                        }


                        if (textInfo.characterInfo != null && i < textInfo.characterInfo.Length)
                        {
                            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                            Vector2 characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i);

                            int result = int.Parse(FNum.ToString())  - int.Parse(SNum.ToString());
                            if (result >=0)
                            {
                                if (LongDivisionScript.InLongDev)
                                {
       
                                    TextInstantiator.InstantiateText(FirstNumPlace, (result).ToString(), characterPosition.x + 10,        characterPosition.y, ResSpace + 120, true, -100 + i);

                                }
                                else
                                {
                                    if (i < firstNumcpy.Length-2&& firstNumcpy[i + 2].Equals('.') )
                                    {
                                        TextInstantiator.InstantiateText(FirstNumPlace, (result).ToString(), characterPosition.x + 10, characterPosition.y, ResSpace , true, 999);

                                    }
                                    else
                                    {
                                        TextInstantiator.InstantiateText(FirstNumPlace, (result).ToString(), characterPosition.x + 10, characterPosition.y, ResSpace, true, -100 + i);

                                    }

                                }
                            }
                            else // borrowing case
                            {
                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this , "doesnotgo" + SpeakerName, Explain)));
                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so lend one from" + SpeakerName, Explain)));
                                int counter = 1;
                                for (int j = i-1; j >=0; j--) // looping for a number to borrow from
                                {
                                    if(char.IsDigit(FirstNumPlace.text[j]))
                                    {
                                        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FirstNumPlace.text[j].ToString(), Explain)));

                                        string temp = FirstNumPlace.text.Substring(0, j);
                                        string temp2 = FirstNumPlace.text.Substring(j + 1);
                                        temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FirstNumPlace.text[j]}</color>";
                                        FirstNumPlace.text = temp + temp2; //make the current number in loop red
                                        if (firstNumcpy[j].Equals('0')) // if the current nubmber is zero we can't borrow
                                        {
                                            counter++;
                                            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"doesnotgo" + SpeakerName, Explain)));
                                            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so lend one from" + SpeakerName, Explain)));
                                        }
                                        else // here we can borrow
                                        {
                                            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));
                                            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,firstNumcpy[j].ToString(), Explain)));
                                            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"becomes" + SpeakerName, Explain)));
                                            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,(int.Parse(firstNumcpy[j].ToString())-1).ToString(), Explain)));

                                            borrowingList[j] += 1; // we had borrowed one from this number
                                            charInfo = textInfo.characterInfo[j];
                                            characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, j );
                                            TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(firstNumcpy[j].ToString()) - 1).ToString(), characterPosition.x + (20), characterPosition.y, 150, false, j);

                                            if (counter != 1) { // we borrowed from the next number (no zeros)
                                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"zero special case minus" + SpeakerName, Explain)));

                                                for (int k = j+1; k < i; k++)
                                                {
                                                    if (!firstNumcpy[k].Equals(' ') && !firstNumcpy[k].Equals('.'))
                                                    {
                                                        charInfo = textInfo.characterInfo[k + 1];
                                                        characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, k);
                                                        TextInstantiator.InstantiateText(FirstNumPlace, ("9").ToString(), characterPosition.x + 20, characterPosition.y, 150, false, k);
                                                        borrowingList[k] += 9;

                                                    }
                                                }

                                            }
                                            else // if there are zeros and we make them all 9
                                            {

                                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and" + SpeakerName, Explain)));
                                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum.ToString(), Explain)));
                                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"becomes" + SpeakerName, Explain)));
                                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,(int.Parse(FNum) + 10).ToString(), Explain)));
                                            }

                                            break;
                                        }
                                    }
                                }

                                if (borrowingList[i] == 0) // if no prev borrowing for this number
                                {
                                    TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(FNum.ToString()) + 10).ToString(), characterPositionSecPlace.x+20 , characterPosition.y, 150, false, i);

                                }
                                else // if someone is borrowed from him already
                                {
                                    TextOperations.MakeTextRed(i.ToString(), (int.Parse(FNum.ToString()) + 10).ToString()); // change the prev value and make it red
                                }

                                FNum = (int.Parse(FNum)+10).ToString();
                                result = int.Parse(FNum.ToString()) - int.Parse(SNum.ToString());

                                yield return (StartCoroutine(Sub(i, FNum, SNum)));
                                if (LongDivisionScript.InLongDev)
                                {
                                    TextInstantiator.InstantiateText(FirstNumPlace, (result).ToString(), characterPositionSecPlace.x + 15, characterPosition.y, ResSpace +120, true,200 + i);
                                }
                                else
                                {
                                    TextInstantiator.InstantiateText(FirstNumPlace, (result).ToString(), characterPositionSecPlace.x + 15, characterPosition.y, ResSpace , true,200 + i);
                                }
                            }
                        }

                    }
                }
            }
            
        }
        IscalledFromOutSide = false;
        if (!LongDivisionScript.InLongDev)
        {
            StartCoroutine(ZerosOps.RemoveUselessZeros(FirstNumPlace , this , SpeakerName,Explain));
        }
        Explain = false;
        button.interactable = true;    
    }
    public IEnumerator Sub(int i , string FNum , string SNum) {
        string temp = "", temp2 = "";
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum, Explain)));
        if (int.TryParse(FirstNumPlace.text[i].ToString(), out _)) {
            temp = FirstNumPlace.text.Substring(0, i);
            temp2 = FirstNumPlace.text.Substring(i + 1);
            temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum}</color>";
            FirstNumPlace.text = temp + temp2;
        }
        else
        {
            TextOperations.MakeTextRed(i.ToString(), FNum);
        }
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"minus" + SpeakerName, Explain)));
        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum, Explain)));
        if(int.TryParse(SecNumPlace.text[i].ToString(), out _))
        {
            temp = SecNumPlace.text.Substring(0, i);
            temp2 = SecNumPlace.text.Substring(i + 1);
            temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum}</color>";
            SecNumPlace.text = temp + temp2;
        }
        int result = int.Parse(FNum.ToString()) - int.Parse(SNum.ToString());
        if (result >= 2 &&(int.Parse(FNum.ToString()) > 2 && int.Parse(SNum.ToString())>2)) {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum.ToString() , Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"in your mind" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and count after" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,SNum.ToString(), Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"on your fingers" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"to reach" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,FNum.ToString(), Explain)));
            if(Explain)
                yield return AJAnimationHandler.AnimateAJ(false, FNum, SNum, false);
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"so" + SpeakerName, Explain)));
        }
        if(result >= 0)
        {

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"the answer is" + SpeakerName, Explain)));
            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this ,result.ToString(), Explain)));

        }
    }
}