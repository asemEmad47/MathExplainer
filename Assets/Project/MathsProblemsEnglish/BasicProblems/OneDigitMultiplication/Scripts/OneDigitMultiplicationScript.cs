using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class OneDigitMultiplicationScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI Line;
    [SerializeField] private TextMeshProUGUI sign;

    [SerializeField] private Button? LangBtn;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    private bool Explain = false;
    public static string SpeakerName = "_Sonya_Eng";
    public static bool IsEng = true;
    public static bool IscalledFromOutSide = false;
    private int SecNumIndex = -1;
    public static int ResDistance = -400;
    public static int SupDistance = 70;   
    
    public static bool IsFinshed = false;
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
        IsFinshed = false;
        ResDistance = -400;
        SupDistance = 150;

        try
        {
            UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
            LangBtn.onClick.AddListener(langBtnClickAction);
        }
        catch (Exception e)
        {

            Debug.Log(e);
        }

        if (AdditionScript.IsBasic)
        {
            FrstNum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
            if (!IscalledFromOutSide)
                SecNum.onValidateInput = ValidateSecInput;
        }


        InputFieldsActions.InitializePlaceholders(FrstNum);
        InputFieldsActions.InitializePlaceholders(SecNum);

        if (!IscalledFromOutSide)
        {
            GameObject.Find("Explain").GetComponent<Button>().onClick.AddListener(ExplainBtnAction);
            GameObject.Find("Solve").GetComponent<Button>().onClick.AddListener(SolveBtnAction);
        }

    }
    void Update()
    {
        PauseScript.ControlPause();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AdditionScript.IsEng = IsEng;
        ExplainEnableMent.EnableExplain(ref FieldsList);
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
    }


    public void SetComponenets(TMP_InputField FrstNum, TMP_InputField SecNum, TextMeshProUGUI FirstNumPlace, TextMeshProUGUI Line, TextMeshProUGUI sign, Button LangBtn, int SecNumIndex)
    {
        this.FrstNum = FrstNum;
        this.SecNum = SecNum;
        this.FirstNumPlace = FirstNumPlace;
        this.Line = Line;
        this.sign = sign;

        this.SecNumIndex = SecNumIndex;
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
    public void ExplainBtnAction()
    {
        Explain = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(solve());
    }
    public IEnumerator solve()
    {
        if(!IscalledFromOutSide)
            ResetValues.ResetAllValues();

        if (AdditionVoiceSpeaker.IsEng)
        {
            AdditionVoiceSpeaker.NumPlace = "EngNums";
            AdditionVoiceSpeaker.SpeakerName = "_Sonya_Eng";
        }
        else
        {
            AdditionVoiceSpeaker.NumPlace = "EgyNums";
            AdditionVoiceSpeaker.SpeakerName = "_Heba_Egy";


        }
        SpeakerName = AdditionVoiceSpeaker.SpeakerName;
        IsFinshed = false;
        GameObject ExplainBtn = GameObject.Find("Explain");
        Button button = ExplainBtn.GetComponent<Button>();
        button.interactable = false;


        if (!IscalledFromOutSide)
        {
            yield return StartCoroutine(WriteTwoNumbers());
        }

        string FirstNumPlaceCpy = FirstNumPlace.text;

        if (FirstNumPlace != null)
        {
            // Populate textInfo manually
            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
            // Check if textInfo is available

            GameObject newTextObject;
            newTextObject = GameObject.Find("SecNumPlace");
            if (newTextObject == null) {
                GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
                int maxNumber = -1;
                GameObject result = null;

                foreach (var obj in allObjects)
                {
                    if (obj.name.Contains("SecNumPlace") && int.TryParse(obj.name.Substring("SecNumPlace".Length), out int currentNumber) && currentNumber > maxNumber)
                    {
                        maxNumber = currentNumber;
                        result = obj;
                    }
                }
                newTextObject = result;
            }
  
            if (newTextObject == null) {
                Debug.Log("null");
            }
            TextMeshProUGUI SecNumPlace = new();
            try
            {
                SecNumPlace = newTextObject.GetComponent<TextMeshProUGUI>();

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            if (textInfo != null)
            {
                int carriedNumber = 0;

                for (int i = FirstNumPlace.text.Length - 1; i >= 0; i--)
                {
                    string FNum = FirstNumPlace.text[i].ToString();
                    string SNum = SecNum.text;
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                    Vector3 characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, i);

                    if (!FirstNumPlace.text[i].Equals(' '))
                    {

                        string temp = "", temp2 = "";
                        if (SecNum.text.Length==1)
                        {
                            temp2 = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum}</color>";
                            SecNumPlace.text = temp2;

                        }
                        else
                        {
                            SecNumPlace.text = SecNum.text[0]+" "+ SecNum.text[1];
                            if (SecNumIndex == 0)
                            {
                                try
                                {
                                    temp = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum[0]}</color>";
                                    temp2 = SecNum.text[1].ToString();

                                }
                                catch (System.Exception)
                                {
                                    temp = "";
                                }
                                SNum = SNum[0].ToString();


                            }
                            else
                            {
                                temp = SecNum.text[0].ToString();
                                temp2 = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum[1]}</color>";
                                SNum = SNum[1].ToString();

                            }
                            if(Explain)
                                yield return new WaitForSeconds(1f);
                            SecNumPlace.text = temp + " " + temp2;
                        }
                        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,SNum,Explain)));

                        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"time" + SpeakerName , Explain)));


                        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,FNum, Explain)));


                        temp = FirstNumPlace.text.Substring(0, i);
                        temp2 = FirstNumPlace.text.Substring(i + 1);
                        temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum}</color>";
                        FirstNumPlace.text = temp + temp2;


                        if (textInfo.characterInfo != null && i < textInfo.characterInfo.Length)
                        {

                            if (SecNumIndex == 0 && SecNum.text.Length >=2) {
                                int index = 0;
                                if (i >= 2)
                                {
                                    index = i- 2;
                                }
                                else
                                {
                                    index = 0;
                                }
                                charInfo = textInfo.characterInfo[index];
                                characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, index);

                            }
                            if (SecNumIndex == 0 && i == 0 && SecNum.text.Length >= 2)
                            {
                                characterPosition = new Vector3(characterPosition.x - 56, characterPosition.y, 0);
                            }
                            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"equal" + SpeakerName, Explain)));

                            int result = int.Parse(FNum.ToString()) * int.Parse(SNum);

                            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,result.ToString(), Explain)));

                            result += carriedNumber;

                            if (carriedNumber != 0)
                            {

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"plus" + SpeakerName, Explain)));
                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,carriedNumber.ToString(), Explain)));

                                GameObject textGameObject = GameObject.Find((i).ToString());
                                TextMeshProUGUI myText = textGameObject.GetComponent<TextMeshProUGUI>();
                                myText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{myText.text}</color>";

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"equal" + SpeakerName, Explain)));
                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,result.ToString(), Explain)));
                            }

                            if (result >= 10)
                            {
                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put" + SpeakerName, Explain)));

                                TextInstantiator.InstantiateText(FirstNumPlace, (result.ToString()[result.ToString().Length-1]).ToString(), characterPosition.x + 15, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y+100, ResDistance, true);

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,result.ToString()[result.ToString().Length-1].ToString(), Explain)));
                                carriedNumber = int.Parse(result.ToString().Substring(0,result.ToString().Length-1));

                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and carry up" + SpeakerName, Explain)));

                                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,carriedNumber.ToString(), Explain)));

                                if(SecNumIndex == 1)
                                {
                                    if(!IscalledFromOutSide)
                                        TextInstantiator.InstantiateText(FirstNumPlace, carriedNumber.ToString(), characterPosition.x - 62, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, SupDistance - 80, false, i - 2);
                                    else
                                        TextInstantiator.InstantiateText(FirstNumPlace, carriedNumber.ToString(), characterPosition.x - 124, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, SupDistance - 80, false, i - 2);


                                }
                                else
                                {
                                    if (!IscalledFromOutSide)
                                        TextInstantiator.InstantiateText(FirstNumPlace, carriedNumber.ToString(), characterPosition.x , FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, SupDistance - 80, false, i - 2);
                                    else
                                        TextInstantiator.InstantiateText(FirstNumPlace, carriedNumber.ToString(), characterPosition.x-62, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, SupDistance - 80, false, i - 2);
                                }
                                TextMeshProUGUI num = GameObject.Find((i-2).ToString()).GetComponent<TextMeshProUGUI>();
     
                            }
                            else
                            {
                                carriedNumber = 0;
                                TextInstantiator.InstantiateText(FirstNumPlace, (result.ToString()[result.ToString().Length - 1]).ToString(), characterPosition.x + 15, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y+100, ResDistance, true);

                            }

                            if (carriedNumber != 0 && i ==0) // if there is a carry and the number is finshed
                            {
                                GameObject textGameObject = GameObject.Find((i-2).ToString());
                                TextMeshProUGUI myText = textGameObject.GetComponent<TextMeshProUGUI>();
                                myText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{myText.text}</color>";


                                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and write it down" + SpeakerName, Explain)));
                                TextInstantiator.InstantiateText(FirstNumPlace, carriedNumber.ToString(), characterPosition.x - 56, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y + 100, ResDistance, true);

                            }
                        }
                    }
                }
            }
        }
        if (IscalledFromOutSide) {
            if(SecNum.text.Length > 1) 
                TwoDigitsMultiplicationScript.RenameResult(SecNumIndex);
            FirstNumPlace.text = FirstNumPlaceCpy;
        }
        button.interactable = true;
        IsFinshed = true;
    }
    public char ValidateSecInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar) && text.Length<1 && addedChar!='0')
        {
            return addedChar;
        }
        else
        {
            return '\0';
        }
    }

    public IEnumerator WriteTwoNumbers()
    {
        if(!IscalledFromOutSide)
            ResetValues.ResetAllValues();

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"first" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"write the first number" + SpeakerName, Explain)));

        string TempFrstNum = "";
        for (int i = 0; i < FrstNum.text.Length; i++)
        {
            TempFrstNum += FrstNum.text[i] + " ";
        }
        FirstNumPlace.text = TempFrstNum;

        FirstNumPlace.gameObject.SetActive(true);

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"then" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"write the second number" + SpeakerName, Explain)));

        GameObject newTextObject = new GameObject("SecNumPlace");
        TextMeshProUGUI newTextMesh = newTextObject.AddComponent<TextMeshProUGUI>();

        // Copy text properties
        newTextMesh.text = SecNum.text;
        newTextMesh.font = FirstNumPlace.font;
        newTextMesh.fontSize = FirstNumPlace.fontSize;
        newTextMesh.color = FirstNumPlace.color;
        newTextMesh.alignment = FirstNumPlace.alignment;
        newTextMesh.fontStyle = FontStyles.Bold;
        // Set the new object as a child of the same parent
        newTextObject.transform.SetParent(FirstNumPlace.transform.parent);
        newTextObject.GetComponent<RectTransform>().localScale = new Vector3 (1.45f, 1.1875f, 1);
        TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
        TMP_CharacterInfo charInfo = textInfo.characterInfo[FirstNumPlace.text.Length-2];
        Vector3 characterPosition = CharacterProbs.GetCharPoos(FirstNumPlace, charInfo, FirstNumPlace.text.Length - 2 );
        newTextObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(characterPosition.x + 22, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y - 120, 0);


        sign.GetComponent<RectTransform>().anchoredPosition = new Vector3(newTextMesh.GetComponent<RectTransform>().anchoredPosition.x-50, newTextObject.GetComponent<RectTransform>().anchoredPosition.y, 0);
        sign.gameObject.SetActive(true);
        Line.gameObject.SetActive(true);
    }
}