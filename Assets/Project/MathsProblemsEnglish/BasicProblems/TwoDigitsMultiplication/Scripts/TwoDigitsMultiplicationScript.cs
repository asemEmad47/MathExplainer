using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TwoDigitsMultiplicationScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private TextMeshProUGUI SecNumPlace;  
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

    public static bool Explain = false;
    string SpeakerName = "_Sonya_Eng";
    bool IsEng = true;
    private Vector3 ResVector;

    private string FirstRes;
    private string SecRes;
    public static string FirstNumber = "";
    public static string SecNumber = "";


    public void Start()
    {
        try
        {
            FrstNum.text = FirstNumber;
            SecNum.text = SecNumber;
        }
        catch (Exception e)
        {

            Debug.Log(e);
        }

        ResVector = new Vector3();
        UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

        if (AdditionScript.IsBasic)
        {

            FrstNum.onValidateInput = InputFieldsActions.ValidateBasicObsInput;
            SecNum.onValidateInput = ValidateSecInput;

        }
        else
        {
            SecNum.onValueChanged.AddListener((input) => OnInputChanged(SecNum, input));
            FrstNum.onValueChanged.AddListener((input) => OnInputChanged(FrstNum, input));

            FrstNum.onValidateInput = InputFieldsActions.ValidateDecimalObsInput;
            SecNum.onValidateInput = InputFieldsActions.ValidateDecimalObsInput;
        }
        InputFieldsActions.InitializePlaceholders(FrstNum);
        InputFieldsActions.InitializePlaceholders(SecNum);
    }
    public void SetComponenets(TMP_InputField FrstNum, TMP_InputField SecNum, TextMeshProUGUI FirstNumPlace, TextMeshProUGUI SecNumPlace, TextMeshProUGUI Line, TextMeshProUGUI sign, UnityEngine.UI.Button LangBtn, TextMeshProUGUI FirstNumPlaceAddition, TextMeshProUGUI SecNumPlaceADdition, TextMeshProUGUI Line2)
    {
        this.FrstNum = FrstNum;
        this.SecNum = SecNum;
        this.FirstNumPlace = FirstNumPlace;
        this.Line = Line;
        this.sign = sign;
        this.SecNumPlace = SecNumPlace;
        this.FirstNumPlaceAdditon = FirstNumPlaceAddition;
        this.SecNumPlaceAdditon = SecNumPlaceADdition;
        this.Line2 = Line2;
        FrstNum.text = FirstNumber;
        SecNum.text = SecNumber;
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

            }
            else
            {
                SpeakerName = "_Heba_Egy";

            }
        }

        AdditionScript.SpeakerName = SpeakerName;
        AdditionScript.IsEng = IsEng;
        OneDigitMultiplicationScript.SpeakerName = SpeakerName;
        OneDigitMultiplicationScript.IsEng = IsEng;
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
    public IEnumerator solve()
    {

        AdditionVoiceSpeaker.VoiceClipsPlace = "AdditionTerms/AdditionSound";

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
        if (!IsCalledFromOutSide)
        {
            ResetValues.ResetAllValues(Line, FirstNumPlace, SecNumPlace, sign);
            FirstNumPlaceAdditon.text = "";
            SecNumPlaceAdditon.text = "";
        }

        FirstRes = "";
        SecRes = "";
        AdditionSign.gameObject.SetActive(false);
        Line2.gameObject.SetActive(false);


        string FrstNumCpy = "";
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"first" + SpeakerName,Explain)));
        if (IsCalledFromOutSide && FrstNum.text.Contains('.'))
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"write the first number without decimal point" + SpeakerName, Explain)));

        }
        else
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"write the first number" + SpeakerName, Explain)));
        }
        try
        {
            FrstNumCpy = FrstNum.text.Remove(FrstNum.text.IndexOf('.'), 1);
        }

        catch (Exception e)
        {
            FrstNumCpy = FrstNum.text;
            Debug.Log(e);
        }
        string TempFrstNum = "";
        for (int i = 0; i < FrstNumCpy.Length; i++)
        {
            TempFrstNum += FrstNumCpy[i] + " ";
        }

        FirstNumPlace.text = TempFrstNum;
        FirstNumPlace.gameObject.SetActive(true);

        if (FirstNumPlace.text[0].Equals('0'))
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"Remove useless zeros" + SpeakerName, Explain)));
            for (int i = 0; i < FirstNumPlace.text.Length; i += 2)
            {
                if(FirstNumPlace.text[i].Equals('0')){
                    FirstNumPlace.text = FirstNumPlace.text.Substring(i + 2);
                    FrstNumCpy =FrstNumCpy.Substring(1);
                }
                else
                {
                    break;
                }
            }
        }


        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"then" + SpeakerName, Explain)));

        if (IsCalledFromOutSide && SecNum.text.Contains('.'))
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"write the second number without decimal point" + SpeakerName , Explain)));

        }
        else
        {
            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"write the second number" + SpeakerName, Explain)));

        }
        try
        {
            SecNum.text = SecNum.text.Remove(SecNum.text.IndexOf('.'),1);

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }


        Vector3 characterPosition =new Vector3 (0, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y - 50,0);



        SecNumPlace.GetComponent<RectTransform>().anchoredPosition = new Vector3(FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.x+40*(FrstNumCpy.Length-SecNum.text.Length), FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y - 100, 0);

        SecNumPlace.text = "";
        for (int i = 0; i < SecNum.text.Length; i++)
        {
            if (i < SecNum.text.Length - 1)
            {
                SecNumPlace.text += SecNum.text[i] + " ";
            }
            else
            {
                SecNumPlace.text += SecNum.text[i];
            }
        }
        SecNumPlace.gameObject.SetActive(true);
        sign.gameObject.SetActive(true);


        Line.gameObject.SetActive(true);




        if (FirstNumPlace != null)
        {
            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
            TMP_CharacterInfo charInfoSecPlace = textInfo.characterInfo[FirstNumPlace.text.Length-2];
            Vector3 characterPositionSecPlace = CharacterProbs.GetCharPoos(FirstNumPlace, charInfoSecPlace, FirstNumPlace.text.Length - 2);
            int index = 1;
            if (SecNum.text.Length < 2)
                index = 0;
            else if (SecNum.text[1] =='0')
            {
                index = 0;
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put zero in the first digit" + SpeakerName, Explain))); // adding zero in the first digit
                TextInstantiator.InstantiateText(FirstNumPlace, "0", characterPositionSecPlace.x+20, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, OneDigitMultiplicationScript.ResDistance, true, 99999); // putting zero in first digit

            }
            else
            {
                yield return StartCoroutine(MultiplyByEachDigit(index, SecNumPlace));

            }

            OneDigitMultiplicationScript OneMultiplication = OneDigitMultiplicationObj.GetComponent<OneDigitMultiplicationScript>(); // use the one multplication script in the first number


            OneMultiplication.SetComponenets(FrstNum, SecNum, FirstNumPlace, Line, sign, LangBtn, index);
            OneMultiplication.OutSideSolve(Explain);


            button.interactable = false;
            AdditionSign.GetComponent<RectTransform>().anchoredPosition = new Vector3(sign.GetComponent<RectTransform>().anchoredPosition.x, AdditionSign.GetComponent<RectTransform>().anchoredPosition.y, 0);

            while (!OneDigitMultiplicationScript.IsFinshed)
            {
                yield return null;
            }
            if (!IsCalledFromOutSide)
            {
                OneDigitMultiplicationScript.ResDistance = -500;
                OneDigitMultiplicationScript.SupDistance = 250;
            }
            else{
                OneDigitMultiplicationScript.ResDistance -=100;
                OneDigitMultiplicationScript.SupDistance +=50;

            }

            if (SecNum.text.Length >= 2 && !SecNum.text[1].Equals("0"))
            {
                charInfoSecPlace = textInfo.characterInfo[FirstNumPlace.text.Length - 2];
                characterPositionSecPlace = CharacterProbs.GetCharPoos(FirstNumPlace, charInfoSecPlace, FirstNumPlace.text.Length - 2);

                button.interactable = false;
                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put zero in the first digit" + SpeakerName, Explain))); // adding zero in the first digit


                TextInstantiator.InstantiateText(FirstNumPlace, "0", characterPositionSecPlace.x+10, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y+100, OneDigitMultiplicationScript.ResDistance, true, 99999); // putting zero in first digit

                GameObject temp = GameObject.Find("99999");
                temp.name = "Res0";

                yield return StartCoroutine(MultiplyByEachDigit(0, SecNumPlace));

                MultiplyByEachDigit(0, SecNumPlace);
                OneMultiplication.SetComponenets(FrstNum, SecNum, FirstNumPlace, Line, sign, LangBtn, 0);
                OneMultiplication.OutSideSolve(Explain);


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


                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"put zeros in empty digits" + SpeakerName, Explain)));


                string FrstNumTemp = FrstNum.text;
                string SecNumTemp = SecNum.text;
                FrstNum.text = FirstRes;
                SecNum.text = SecRes;
                ZerosOps.FillWithZeros(FrstNum, SecNum, FirstNumPlaceAdditon, SecNumPlaceAdditon, "0");

                FrstNum.text = FrstNumTemp;
                SecNum.text = SecNumTemp;

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"start adding" + SpeakerName, Explain)));
                if (!IsCalledFromOutSide)
                {
                    OneDigitMultiplicationScript.ResDistance = -400;
                    OneDigitMultiplicationScript.SupDistance = 150;

                }

                AdditionScript addition = AdditionObj.GetComponent<AdditionScript>();
                addition.SetComponenets(FrstNum, SecNum, FirstNumPlaceAdditon, SecNumPlaceAdditon, Line2, AdditionSign, LangBtn);
                addition.OutSideSolve(Explain);
                while (AdditionScript.IscalledFromOutSide)
                {
                    yield return null;
                }
            }
            if (!AdditionScript.IsBasic &&DecimalDigitsCount !=0)
            {
                try
                {
                    SecNumPlace.text = SecNum.text[0] + " " + SecNum.text[1];

                }
                catch (Exception e)
                {

                    Debug.Log(e);
                }

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"count the decimal digits" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"its" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,DecimalDigitsCount.ToString() , Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"count" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,DecimalDigitsCount.ToString(), Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"digits" + SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"starting from right" + SpeakerName, Explain)));

                TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>().Where(obj => obj.name == "-1").ToArray();

                bool caughtException = false;
                for (int i = 1; i <= DecimalDigitsCount; i++)
                {
                    yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,i.ToString(), Explain)));

                    try
                    {
                        textMeshProObjects[textMeshProObjects.Length - i].text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{textMeshProObjects[textMeshProObjects.Length - i].text}</color>";
                        characterPosition.x = textMeshProObjects[textMeshProObjects.Length - i].GetComponent<RectTransform>().anchoredPosition.x-20;
                        characterPosition.y = textMeshProObjects[textMeshProObjects.Length - i].GetComponent<RectTransform>().anchoredPosition.y-20;
                    }
                    catch (Exception)
                    {
                        caughtException = true;
                    }


                    // Handle the coroutine outside of the catch block
                    if (caughtException)
                    {
                        yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"nothing in the next digit so put zero" + SpeakerName, Explain));

                        TextInstantiator.InstantiateText(FirstNumPlace, "0", characterPosition.x - 30, characterPosition.y, OneDigitMultiplicationScript.ResDistance, true);
                        characterPosition = new Vector3(characterPosition.x - 30, characterPosition.y, 0);
                    }
                }
                yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"and put the decimal point after you finished" + SpeakerName, Explain));

                TextInstantiator.InstantiateText(FirstNumPlace, ".", characterPosition.x - 30, characterPosition.y, 0, true,999999);
                characterPosition = new Vector3(characterPosition.x - 30, characterPosition.y, 0);

                if (DecimalDigitsCount >= textMeshProObjects.Length)
                {
                    yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"you should write a value in the unit digit so put" + SpeakerName, Explain));
                    TextInstantiator.InstantiateText(FirstNumPlace, "0", characterPosition.x - 30, characterPosition.y, 0,true);

                }
            }
        }
        button.interactable = true;
        OneDigitMultiplicationScript.ResDistance = -400;
        OneDigitMultiplicationScript.SupDistance = 150;
        OneDigitMultiplicationScript.IscalledFromOutSide = false;
        AdditionScript.IscalledFromOutSide = false;
        Explain = false;
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
        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"multiply" + SpeakerName, Explain)));

        yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this,SecNum.text[index].ToString(), Explain)));
        string temp = SecNum.text[index].ToString();
        temp = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{temp}</color>";
        if(index == 1)
            SecNumPlace.text = SecNum.text[0] +" "+ temp;
        else {
            try
            {
                SecNumPlace.text = temp +" "+ SecNum.text[1];

            }
            catch (Exception)
            {
                SecNumPlace.text = temp;
            }

        }

        yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"by each digit in the first number" + SpeakerName, Explain)));

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