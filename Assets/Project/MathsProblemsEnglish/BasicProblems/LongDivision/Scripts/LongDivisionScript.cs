using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class LongDivisionScript : MonoBehaviour
{
    private string SpeakerName = "_Sonya_Eng";
    [SerializeField] private ToggleGroup EngtoggleGroup;
    [SerializeField] private ToggleGroup ArtoggleGroup;
    [SerializeField] private TMP_InputField FrstNum;
    [SerializeField] private TMP_InputField SecNum;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;


    [SerializeField] private TextMeshProUGUI SecNumPlace;

    [SerializeField] private GameObject DivideSign;
    [SerializeField] private GameObject TimeSign;
    [SerializeField] private GameObject MinusSign;
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject LongDivSympol;
    [SerializeField] private GameObject Line;
    [SerializeField] private GameObject SecMethodLine;
    [SerializeField] private GameObject SubtractionnObj;
    [SerializeField] private UnityEngine.UI.Button LangBtn;

    public static AudioSource audioSource;
    public static bool InExplain = false;
    public int TimeRes = 0;
    public string FinalResult = "";
    public string FNum = "";
    public float SecProblemY = 0;
    public int LastInsatitedNumber = 0;
    bool IsEng = true;
    private AudioClip[] loop;
    public static bool InLongDev = false;
    public void Start()
    {

        UnityAction langBtnClickAction = () => AdditionScript.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);
        LastInsatitedNumber =1;
        SecProblemY = FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y-100;
        FrstNum.onValidateInput = AdditionScript.ValidateInput;
        SecNum.onValidateInput = DivisionScript.ValidateSecInput;

        AdditionScript.InitializePlaceholders(FrstNum);
        AdditionScript.InitializePlaceholders(SecNum);
    }
    void Update()
    {

        FrstNum.onValidateInput = AdditionScript.ValidateInput;
        SecNum.onValidateInput = DivisionScript.ValidateSecInput;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AdditionScript.EnableExplain(ref FrstNum, ref SecNum);
        if (IsEng) {
            ArtoggleGroup.gameObject.SetActive(false);
            EngtoggleGroup.gameObject.SetActive(true);
            SpeakerName = "_Sonya_Eng";
            AdditionVoiceSpeaker.NumPlace = "EngNums";
        }
        else
        {
            ArtoggleGroup.gameObject.SetActive(true);
            EngtoggleGroup.gameObject.SetActive(false);
            SpeakerName = "_Heba_Egy";
            AdditionVoiceSpeaker.NumPlace = "EgyNums";

        }
        SubtractionScript.IsEng = IsEng;
        if (InExplain)
        {
            GameObject.Find("MethodTwo").GetComponent<Toggle>().interactable = false;
            GameObject.Find("MethodOne").GetComponent<Toggle>().interactable = false;
            GameObject.Find("Explain").GetComponent<Button>().interactable = false;
            GameObject.Find("Explain").GetComponent<Button>().enabled = false;

        }
    }
    public void explain()
    {

        audioSource = GetComponent<AudioSource>();
        InExplain = true;
        StartCoroutine(Solve());

    }

    public IEnumerator Solve()
    {
        InLongDev = true;
        SubtractionScript.ResetAllValues(Line.GetComponent<TextMeshProUGUI>(), FirstNumPlace,SecNumPlace,Line.GetComponent<TextMeshProUGUI>());

        FirstNumPlace.gameObject.SetActive(false);
        FinalResult = "";
        SecProblemY = 0;
        LastInsatitedNumber = 1;
        SecNumPlace.gameObject.SetActive(false);
        LongDivSympol.SetActive(false);
        SecMethodLine.SetActive(false);

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/first" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/Remember the steps of long division" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/divide_verb" + SpeakerName)));
        DivideSign.SetActive(true);

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/multiply" + SpeakerName)));

        TimeSign.SetActive(true);

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/minus" + SpeakerName)));

        MinusSign.SetActive(true);

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/bring the next down" + SpeakerName)));


        Animator animator = Arrow.GetComponent<Animator>();
        Arrow.SetActive(true);
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1);

        TimeSign.SetActive(false);
        MinusSign.SetActive(false);
        DivideSign.SetActive(false);
        Arrow.SetActive(false);


        if (animator != null)
        {
            animator.enabled = false;
        }
        // start solving


        float YDdistance = -30;
        FirstNumPlace.text = "";
        foreach (char latter in FrstNum.text)
        {
            FirstNumPlace.text += latter + " ";
        }
        SecNumPlace.text = SecNum.text;
        string FrstNumCpy = FirstNumPlace.text;
        string Fnum2 = "";
        bool IsIIncreased = false;

        FirstNumPlace.gameObject.SetActive(true);


        SecNumPlace.gameObject.SetActive(true);
        LongDivSympol.SetActive(true);

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/the first step is" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/divide_verb" + SpeakerName)));

        if (FirstNumPlace != null)
        {


            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
            TMP_CharacterInfo charInfo = textInfo.characterInfo[0];
            Vector3 characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, 0);


            FNum = FrstNumCpy[0].ToString();

            if (textInfo != null)
            {
                for (int i = 0; i < FrstNumCpy.Length; i++)
                {

                    string SNum = SecNum.text;

                    if (!FrstNumCpy[i].Equals(' '))
                    {
                        charInfo = textInfo.characterInfo[i];
                        characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, i);
                        //division part
                        if (textInfo.characterInfo != null && i < textInfo.characterInfo.Length)
                        {
                            TextMeshProUGUI myText = new TextMeshProUGUI();

                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));
                            string temp2 = "", temp = "";
 
                            MakeRed(i, FrstNumCpy);


                            myText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{myText.text}</color>";

                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/divide" + SpeakerName)));

                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));


                            temp2 = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{SNum}</color>";

                            SecNumPlace.text = temp2;
                            if (int.Parse(FNum) < int.Parse(SNum) && i + 3 < FrstNumCpy.Length)
                            {
                                i += 2;
                                IsIIncreased = true;


                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/doesnotgo" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/is smaller than" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait("0")));


                                AdditionScript.InstantiateText(FirstNumPlace, "0", characterPosition.x + 15, characterPosition.y, 300, true, 200);

                                FinalResult += "0";

                                charInfo = textInfo.characterInfo[i];
                                characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, i);


                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));


                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/becomes" + SpeakerName)));

                                FNum = FNum[FNum.Length-1].ToString();
                                FNum += FrstNumCpy[i];

                                TextMeshProUGUI TempBackup = null;
                                try
                                {
                                    // Take the next digit
                                    TempBackup = GameObject.Find("Temp" + (i - 4).ToString()).GetComponent<TextMeshProUGUI>();

                                    charInfo = textInfo.characterInfo[i];
                                    characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, i);
                                }
                                catch (Exception e)
                                {
                                    Debug.Log(e);
                                }

                                if (TempBackup != null)
                                {
                                    Vector3 startPosition = new Vector3(characterPosition.x + 230, characterPosition.y +100, 0);
                                    Vector3 targetPosition = new Vector3(characterPosition.x + 190, TempBackup.GetComponent<RectTransform>().anchoredPosition.y + 100, TempBackup.transform.position.z);

                                    yield return StartCoroutine(MoveArrow(startPosition, targetPosition, 1f));

                                    TempBackup.text += " " + FrstNumCpy[i];
                                    Fnum2 += FrstNumCpy[i];
                                    yield return new WaitForSeconds(1f);
                                    Arrow.SetActive(false);
                                }

                                temp = FirstNumPlace.text.Substring(0, FirstNumPlace.text.LastIndexOf("</color>") + 9);
                                temp2 = FirstNumPlace.text.Substring(FirstNumPlace.text.LastIndexOf("</color>") + 10);

                                temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{FNum[1]}</color>";

                                FirstNumPlace.text = temp + temp2;

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));


                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));


                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/divide" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));


                            }
                            else if (int.Parse(FNum) < int.Parse(SNum) && i + 2 >= FrstNumCpy.Length)
                            {
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/doesnotgo" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/is smaller than" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait("0")));



                                AdditionScript.InstantiateText(FirstNumPlace, "0", characterPosition.x + 15, characterPosition.y, 300, true);
                                FinalResult += '0';

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/the remainder is" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                            }



                            Toggle toggle;
                            if (IsEng)
                            {
                                toggle = EngtoggleGroup.ActiveToggles().FirstOrDefault();
                            }
                            else
                            {
                                toggle = ArtoggleGroup.ActiveToggles().FirstOrDefault();
                            }
                            if (toggle.name.Equals("MethodOne"))
                            {
                                yield return StartCoroutine(SolveByMehtodOne(SNum, textInfo, charInfo, characterPosition, YDdistance, IsIIncreased, i ));
                            }
                            else
                            {
                                yield return StartCoroutine(SolveByMehtodTwo(textInfo, charInfo, characterPosition, YDdistance, IsIIncreased, i));

                            }


                            //-------------------------------------
                            //minus part
                            Line.gameObject.SetActive(true);

                            int index = i;
                            if (TimeRes.ToString().Length > 1)
                                index = i - 2;

                            TMP_TextInfo textInfo2 = FirstNumPlace.GetTextInfo(FirstNumPlace.text);
                            TMP_CharacterInfo charInfoSecPlace = textInfo2.characterInfo[index];
                            Vector3 characterPositionSecPlace = AdditionScript.GetCharPoos(FirstNumPlace, charInfoSecPlace, index);

                            SubtractionScript.IsEng = IsEng;
                            SubtractionScript.IscalledFromOutSide = true;
                            SubtractionScript.Explain = true;
                            SubtractionScript subtraction = SubtractionnObj.GetComponent<SubtractionScript>();


                            TextMeshProUGUI FrstNumPlaceCpy;

                            if (i == 0 || (i == 2 && IsIIncreased))
                            {
                                FrstNumPlaceCpy = Instantiate(FirstNumPlace, FirstNumPlace.transform.position, FirstNumPlace.transform.rotation);
                                FrstNumPlaceCpy.text = FrstNumCpy.Substring(0, TimeRes.ToString().Length * 2 - 1);
                            }
                            else
                            {
                                TextMeshProUGUI TempBackup;
                                try
                                {
                                    TempBackup = GameObject.Find("Temp" + (i - 2).ToString()).GetComponent<TextMeshProUGUI>();
                                }
                                catch (Exception )
                                {
                                    TempBackup = GameObject.Find("Temp" + (i - 4).ToString()).GetComponent<TextMeshProUGUI>();
                                }


                                FrstNumPlaceCpy = TempBackup;

                                FrstNumPlaceCpy.text = Fnum2;
                            }

                            FrstNumPlaceCpy.name = "FrstNumPlaceCpy" + i.ToString();

                            FrstNumPlaceCpy.ForceMeshUpdate();
                            yield return null;
                            TextMeshProUGUI SecNumPlaceCpy;
                            SpaceNumbers(ref FNum);
                            try
                            {
                                SecNumPlaceCpy = GameObject.Find((i + 200).ToString()).GetComponent<TextMeshProUGUI>();

                            }
                            catch (Exception)
                            {

                                SecNumPlaceCpy = GameObject.Find((i-2 + 200).ToString()).GetComponent<TextMeshProUGUI>();

                            }
                            SecNumPlaceCpy.text = FNum;

                            if (i != 0 && !(i == 2 && IsIIncreased))
                            {
                                CenterInPos(GameObject.Find("FrstNumPlaceCpy" + (i).ToString()).GetComponent<RectTransform>().anchoredPosition.x, GameObject.Find("FrstNumPlaceCpy" + (i).ToString()).GetComponent<RectTransform>().anchoredPosition.y -70 , ref SecNumPlaceCpy);
                            }
                            else
                            {
                                if((i == 2 && IsIIncreased))
                                {
                                    CenterInPos(SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.x+33, SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.y, ref SecNumPlaceCpy);
                                }
                            }
                            GameObject LineI = Instantiate(Line, Line.transform.position, Line.transform.rotation);
                            Line.gameObject.SetActive(false);
                            LineI.name = "LINEI " + i;
                            TextMeshProUGUI LineItxt = LineI.GetComponent<TextMeshProUGUI>();
     
                            CenterInPos(SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.x -10, SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.y-30 , ref LineItxt);


                            SubtractionScript.ResSpace = -250;
                            SecNumPlaceCpy.enableWordWrapping = false;  
                            SecNumPlaceCpy.overflowMode = TextOverflowModes.Overflow;

                            FrstNumPlaceCpy.enableWordWrapping = false;
                            FrstNumPlaceCpy.overflowMode = TextOverflowModes.Overflow;

                            
                            if (i==0 ||(i == 2 && IsIIncreased))
                            {
                                CenterInPos(SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.x-10, FirstNumPlace.GetComponent<RectTransform>().anchoredPosition.y, ref FrstNumPlaceCpy);
                            }

                            subtraction.SetComponenets(FrstNum, SecNum, FrstNumPlaceCpy, SecNumPlaceCpy, LineItxt, null, null);

                            subtraction.explain();
                            while (SubtractionScript.Explain)
                            {
                                yield return null;
                            }

                            if (i + 2 < FrstNumCpy.Length)
                            {
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/bring the next down" + SpeakerName)));

                                charInfo = textInfo.characterInfo[i];
                                characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, i);

                                Vector3 startPosition = new Vector3(characterPosition.x+305, characterPosition.y+100, 0);

                                Vector3 targetPosition = new Vector3(characterPosition.x + 305, SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.y+100, LineItxt.transform.position.z);

                                // Move the arrow smoothly over 1 second (you can change this duration)
                                yield return StartCoroutine(MoveArrow(startPosition, targetPosition, 1f));


                                TextMeshProUGUI Temp = Instantiate(SecNumPlaceCpy, LineItxt.transform.position, LineItxt.transform.rotation);
                                Temp.text = "";
                                Temp.name = "Temp" + i;
                                FNum = "";
                                GetMinusResult(i + 100, ref Temp, ref FNum);


                                Temp.text += FrstNumCpy[i + 2];
                                Temp.color = UnityEngine.Color.red;
                                FNum += FrstNumCpy[i + 2];
                                Fnum2 = FNum;
                                SpaceNumbers(ref Fnum2);

                                CenterInPos(Temp.GetComponent<RectTransform>().anchoredPosition.x -12*i +40*(4-FrstNum.text.Length), SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.y - 80, ref Temp);

                                YDdistance = Temp.GetComponent<RectTransform>().anchoredPosition.y - 50;
                                SecNumPlace.text = SecNum.text;
                                yield return new WaitForSeconds(1f);
                                Arrow.SetActive(false);
                            }

                            else
                            {

                                TextMeshProUGUI Temp = Instantiate(SecNumPlaceCpy, SecNumPlaceCpy.transform.position, SecNumPlaceCpy.transform.rotation);
                                Temp.text = "";
                                Temp.name = "Temp" + i;
                                FNum = "";

                                GetMinusResult(-1, ref Temp, ref FNum);

                                CenterInPos(SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.x, SecNumPlaceCpy.GetComponent<RectTransform>().anchoredPosition.y - 100, ref Temp);


                                string reminder = int.Parse(FNum).ToString();

                                float XPos = SecNumPlace.GetComponent<RectTransform>().anchoredPosition.x;
                                float YPos = Temp.GetComponent<RectTransform>().anchoredPosition.y;

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/the final answer is" + SpeakerName)));

                                FinalResult = int.Parse(FinalResult).ToString();

                                TextMeshProUGUI FinalAnswer = GameObject.Find("AnswerPlace").GetComponent<TextMeshProUGUI>();
                                FinalAnswer.text = FinalResult;
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FinalResult)));

                                FinalAnswer.enabled = true;



                                if (!reminder[0].Equals('0'))
                                {
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and" + SpeakerName)));


                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/the remainder is" + SpeakerName)));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(reminder)));

                                    FinalAnswer.text += "  R = " + reminder;


                                    // x over y
                                    YPos = FinalAnswer.GetComponent<RectTransform>().anchoredPosition.y-100;
                                    XPos = FinalAnswer.GetComponent<RectTransform>().anchoredPosition.x;

                                    YPos -= 100;

                                    AdditionScript.InstantiateText(FirstNumPlace, FinalResult, XPos + 350, YPos, -400, true);
                                    XPos += 60;

                                    AdditionScript.InstantiateText(FirstNumPlace, reminder.ToString(), XPos+450, YPos, -350, true);

                                    AdditionScript.InstantiateText(FirstNumPlace, "\u2015", XPos+450, YPos, -400, true);

                                    AdditionScript.InstantiateText(FirstNumPlace, SecNum.text.ToString(), XPos + 450, YPos, -450, true);

                                }
                            }

                        }

                    }
                    IsIIncreased = false;
                }
            }
            SubtractionScript.IscalledFromOutSide = false;

            InExplain = false;
            GameObject.Find("MethodTwo").GetComponent<Toggle>().interactable = true;
            GameObject.Find("MethodOne").GetComponent<Toggle>().interactable = true;
            GameObject.Find("Explain").GetComponent<Button>().interactable = true;
            GameObject.Find("Explain").GetComponent<Button>().enabled = true;
            InLongDev = false;

        }
    }

    public void CenterInPos(float XPos, float YPos, ref TextMeshProUGUI obj)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Set the parent without keeping world position
            obj.transform.SetParent(FirstNumPlace.transform.parent, false);

            // Reset the local scale
            rectTransform.localScale = Vector3.one;

            // Convert XPos and YPos to local space of the parent RectTransform
            rectTransform.anchoredPosition = new Vector2(XPos, YPos);

            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);  // Bottom-left corner of the anchors
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);  // Top-right corner of the anchors

            // Optionally, set the pivot to the center as well
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        else
        {
            Debug.LogWarning("No RectTransform found on the TextMeshProUGUI.");
        }
    }    
    public void CenterInPos(float XPos, float YPos, ref ToggleGroup obj)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Set the parent without keeping world position
            obj.transform.SetParent(FirstNumPlace.transform.parent, false);


            // Convert XPos and YPos to local space of the parent RectTransform
            rectTransform.anchoredPosition = new Vector2(XPos, YPos);

            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);  // Bottom-left corner of the anchors
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);  // Top-right corner of the anchors

            // Optionally, set the pivot to the center as well
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        else
        {
            Debug.LogWarning("No RectTransform found on the TextMeshProUGUI.");
        }
    }

    public void GetMinusResult(int i, ref TextMeshProUGUI res, ref string FnumTemp)
    {
        TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>();
        if (textMeshProObjects.Length > 0)
        {
            foreach (TextMeshProUGUI textMeshPro in textMeshProObjects)
            {
                if (int.TryParse(textMeshPro.name, out int number) && ((number < 0)||  (number == i || number == 100)  || (i==-1  && number >=100 && number < 200)))
                {

                    res.text += textMeshPro.text + " ";
                    FnumTemp += textMeshPro.text;
                    if (i!=-1)
                    {
                        CenterInPos(res.GetComponent<RectTransform>().anchoredPosition.x + 60, res.GetComponent<RectTransform>().anchoredPosition.y, ref res);

                    }
                    Destroy(textMeshPro.gameObject);

                }

            }
        }

    }

    public void MakeRed(int i, string FrstNumCpy)
    {
        string FNum = FrstNumCpy[i].ToString();
        string temp = FirstNumPlace.text.Substring(0, i);
        string temp2 = FirstNumPlace.text.Substring(i + 1);
        if (FirstNumPlace.text.LastIndexOf("</color>") != -1)
        {
            temp = FirstNumPlace.text.Substring(0, FirstNumPlace.text.LastIndexOf("</color>") + 9);
            temp2 = FirstNumPlace.text.Substring(FirstNumPlace.text.LastIndexOf("</color>") + 10);
        }

        temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{FNum[FNum.Length - 1]}</color>";
        FirstNumPlace.text = temp + temp2;
    }

    public void SpaceNumbers(ref string number)
    {
        string temp = "";

        foreach (char latter in number)
        {
            temp += latter + " ";
        }
        number = temp;
    }
    // Coroutine to move the arrow with lerping
    IEnumerator MoveArrow(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        Arrow.SetActive(true);

        float timeElapsed = 0;

        RectTransform arrowRect = Arrow.GetComponent<RectTransform>();

        // Continue the animation for the given duration
        while (timeElapsed < duration)
        {
            // Calculate the current position using Lerp for both x and y
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);

            // Ensure the RectTransform moves along both axes
            arrowRect.anchoredPosition = new Vector2(newPosition.x, newPosition.y);

            // Increment time
            timeElapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the arrow reaches the exact target position at the end
        arrowRect.anchoredPosition = new Vector2(targetPosition.x, targetPosition.y);
        yield return null;
    }

    public IEnumerator SolveByMehtodOne( string SNum , TMP_TextInfo textInfo, TMP_CharacterInfo charInfo , Vector3 characterPosition , float YDdistance , bool IsIIncreased , int i)
    {
        float result = ((float)int.Parse(FNum) / int.Parse(SNum));
        bool FirstTimeInLoop = false;
        string FNumCpy = "";


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

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/divide" + SpeakerName)));

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));

            FirstTimeInLoop = true;

        }

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(((int)result).ToString())));

        AdditionScript.InstantiateText(FirstNumPlace, ((int)result).ToString(), characterPosition.x + 15, characterPosition.y, 300, true, i);


        if (IsIIncreased) // to make number under number in a right way
        {
            charInfo = textInfo.characterInfo[i - 2];
            characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, i - 2);
        }
        FinalResult += ((int)result).ToString();


        //------------------------------------------------
        //time part

        SecNumPlace.text = SecNum.text;
        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/the next step is" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/time" + SpeakerName)));

        GameObject DivRes = GameObject.Find(i.ToString());

        TextMeshProUGUI DivResText = DivRes.GetComponent<TextMeshProUGUI>();

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(DivResText.text)));

        TimeRes = int.Parse(DivResText.text) * int.Parse(SecNum.text);

        DivResText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{DivResText.text}</color>";

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/time" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SecNum.text)));

        SecNumPlace.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{SecNum.text}</color>";

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(TimeRes.ToString())));

        if (YDdistance == -30)
            AdditionScript.InstantiateText(FirstNumPlace, TimeRes.ToString(), characterPosition.x + 15, characterPosition.y, YDdistance, true, i + 200);
        else
            AdditionScript.InstantiateText(FirstNumPlace, TimeRes.ToString(), characterPosition.x + 15, characterPosition.y, YDdistance - 100, true, i + 200);
    }

    public IEnumerator SolveByMehtodTwo(TMP_TextInfo textInfo, TMP_CharacterInfo charInfo, Vector3 characterPosition, float YDdistance, bool IsIIncreased, int i)
    {
        float DivResult = MathF.Ceiling((float)int.Parse(FNum) / int.Parse(SecNum.text));
        int result = 0;

        if (LastInsatitedNumber == 1)
        {
            SecMethodLine.SetActive(true);
        }
        bool Inwhile = false;
        while (LastInsatitedNumber < DivResult + 1) {
            Inwhile = true;
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(LastInsatitedNumber.ToString())));

            AdditionScript.InstantiateText(FirstNumPlace, LastInsatitedNumber.ToString(), SecMethodLine.GetComponent<RectTransform>().anchoredPosition.x + 60,SecProblemY, 0, true, 500+LastInsatitedNumber);

            TextMeshProUGUI textMeshProUGUI = GameObject.Find((500 + LastInsatitedNumber).ToString()).GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.color = UnityEngine.Color.red;

            ColorThemAll((500 + LastInsatitedNumber).ToString(), UnityEngine.Color.red);

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/time" + SpeakerName)));


            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SecNum.text)));


            SecNumPlace.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.red)}>{SecNum.text}</color>";

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal" + SpeakerName)));

            TimeRes = LastInsatitedNumber * int.Parse(SecNum.text);


            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(TimeRes.ToString())));


            AdditionScript.InstantiateText(FirstNumPlace, TimeRes.ToString(), SecMethodLine.GetComponent<RectTransform>().anchoredPosition.x - 80, SecProblemY, 0, true, 500 + LastInsatitedNumber);


            if (TimeRes< int.Parse(FNum))
            {
                result = TimeRes;
                yield return new WaitForSeconds(0.5f);

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(TimeRes.ToString())));
                ColorThemAll((500 + LastInsatitedNumber).ToString(), UnityEngine.Color.red);



                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/is smaller than" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum.ToString())));

                yield return new WaitForSeconds(0.5f);



            }
            else
            {
                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(TimeRes.ToString())));
                ColorThemAll((500 + LastInsatitedNumber).ToString(), UnityEngine.Color.red);

                int CurrentNum = 0;
                if(TimeRes > int.Parse(FNum))
                {
                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/is greater than" + SpeakerName)));
                    CurrentNum = LastInsatitedNumber - 1;
                }
                else
                {
                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal" + SpeakerName)));
                    CurrentNum = LastInsatitedNumber;
                }
                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum.ToString())));


                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((CurrentNum).ToString())));

                AdditionScript.InstantiateText(FirstNumPlace, (CurrentNum).ToString(), characterPosition.x + 15, characterPosition.y, 300, true, i);

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/then" + SpeakerName)));


                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/take" + SpeakerName)));

                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(((CurrentNum) * int.Parse(SecNum.text)).ToString())));

                if (IsIIncreased) // to make number under number in a right way
                {
                    charInfo = textInfo.characterInfo[i - 2];
                    characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, i - 2);
                }

                AdditionScript.InstantiateText(FirstNumPlace, "", characterPosition.x + 15, characterPosition.y, YDdistance, true, i + 200);

                FNum = ((CurrentNum) * int.Parse(SecNum.text)).ToString();
            }
            ColorThemAll((500 + LastInsatitedNumber).ToString(), UnityEngine.Color.black);


            SecProblemY -= 75;
            SecNumPlace.text = SecNum.text;

            LastInsatitedNumber++;
        }

        if (!Inwhile)
        {
            DivResult = MathF.Floor((float)int.Parse(FNum) / int.Parse(SecNum.text));

            ColorThemAll((500 + DivResult).ToString(), UnityEngine.Color.red);

            yield return new WaitForSeconds(0.5f);


            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put" + SpeakerName)));

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((DivResult).ToString())));

            AdditionScript.InstantiateText(FirstNumPlace, (DivResult).ToString(), characterPosition.x + 15, characterPosition.y, 300, true, i);

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/then" + SpeakerName)));


            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/take" + SpeakerName)));

            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(((DivResult) * int.Parse(SecNum.text)).ToString())));

            if (IsIIncreased) // to make number under number in a right way
            {
                charInfo = textInfo.characterInfo[i - 2];
                characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, i - 2);
            }

            AdditionScript.InstantiateText(FirstNumPlace, "", characterPosition.x + 15, characterPosition.y, YDdistance, true, i + 200);
            FNum = ((DivResult) * int.Parse(SecNum.text)).ToString();
            TimeRes = int.Parse(FNum);
            FinalResult += (DivResult).ToString();

        }
        else
        {
            FinalResult += (LastInsatitedNumber - 1).ToString();

        }

    }
    public void ColorThemAll(string name, UnityEngine.Color color)
    {
        TextMeshProUGUI[] allTextMeshes = GameObject.FindObjectsOfType<TextMeshProUGUI>();

        // Loop through each TextMeshProUGUI and check if its name matches the target name
        foreach (TextMeshProUGUI textMesh in allTextMeshes)
        {
            if (textMesh.name.Equals(name)) // Check for exact name match
            {
                textMesh.color = color; // Set the color to red
            }
        }
    }
}
