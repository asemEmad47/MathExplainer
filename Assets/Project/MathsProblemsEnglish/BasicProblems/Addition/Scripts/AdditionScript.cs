using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    public void Start()
    {

        LoadAllAudioClips();
        UnityAction langBtnClickAction = () => LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);
        if (!IscalledFromOutSide && IsBasic)
        {
            FrstNum.onValidateInput = AdditionScript.ValidateInput;
            SecNum.onValidateInput = AdditionScript.ValidateInput;
        }

        InitializePlaceholders(FrstNum);
        InitializePlaceholders(SecNum);

    }
    void Update()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        EnableExplain(ref FrstNum, ref SecNum);
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
    public void explain()
    {
        Explain = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(solve());
    }

    public static void FillWithZeros(TMP_InputField FrstNum, TMP_InputField SecNum , TextMeshProUGUI FirstNumPlace , TextMeshProUGUI SecNumPlace, string symb)
    {
        int LenDifference = Mathf.Abs(FrstNum.text.Length - SecNum.text.Length);
        string tempstr = "";
        for (int i = 0; i < LenDifference; i++)
        {
            tempstr += symb;
        }
        if (FrstNum.text.Length > SecNum.text.Length)
        {
            tempstr += SecNum.text;
            SecNumPlace.text = tempstr;
            FirstNumPlace.text = FrstNum.text;
        }
        else
        {
            tempstr += FrstNum.text;
            FirstNumPlace.text = tempstr;
            SecNumPlace.text = SecNum.text;
        }
        string TempFrstNum = "";
        string TempSecNum = "";
        for (int i = 0; i < FirstNumPlace.text.Length; i++)
        {
            TempFrstNum += FirstNumPlace.text[i] + " ";
        }
        for (int i = 0; i < SecNumPlace.text.Length; i++)
        {
            TempSecNum += SecNumPlace.text[i] + " ";
        }
        FirstNumPlace.text = TempFrstNum;
        SecNumPlace.text = TempSecNum;
    }
    public IEnumerator solve()
    {

        GameObject ExplainBtn = GameObject.Find("Explain");
        Button button = ExplainBtn.GetComponent<Button>();
        button.interactable = false;

        LoadAllAudioClips();
        if (Explain)
        {

            if (!IscalledFromOutSide)
            {
                FirstNumPlace.text = "";
                SecNumPlace.text = "";
                FirstNumPlace.gameObject.SetActive(false);
                SecNumPlace.gameObject.SetActive(false);
                SubtractionScript.ResetAllValues(Line, FirstNumPlace, SecNumPlace, sign);

                FillWithZeros(FrstNum, SecNum, FirstNumPlace, SecNumPlace, " ");
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
                Line.gameObject.SetActive(true);
            }

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
                        if (Explain)
                        {
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

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait("1")));
                                    myText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>1</color>";

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/plus" + SpeakerName)));

                                }
                                if (j == 0)
                                {
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));

                                    if (IsCarried)
                                    {

                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((int.Parse(FNum) + 1).ToString())));

                                    }
                                    string temp = FirstNumPlace.text.Substring(0, i);
                                    string temp2 = FirstNumPlace.text.Substring(i + 1);
                                    temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum}</color>";
                                    FirstNumPlace.text = temp + temp2;
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/plus" + SpeakerName)));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));

                                    temp = SecNumPlace.text.Substring(0, i);
                                    temp2 = SecNumPlace.text.Substring(i + 1);
                                    temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum}</color>";
                                    SecNumPlace.text = temp + temp2;
                                    if(SmallestNum <= 2)
                                    {
                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal" + SpeakerName)));
                                        break;
                                    }

                                    if(SmallestNum == int.Parse(FNum))
                                    {
                                        (FNum , SNum) = (SNum , FNum);
                                    }
                                }
                                if (j != 5)
                                {
                                    audioSource.clip = loop[j];
                                    audioSource.Play();
                                    yield return new WaitForSeconds(audioSource.clip.length);
                                }
                                else
                                {
                                    yield return AnimateAJ(IsCarried , FNum , SNum ,true);

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/so" + SpeakerName)));

                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/the answer is" + SpeakerName)));
                                }
                                if (j == 0)
                                {
                                    if (IsCarried)
                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((int.Parse(FNum) + 1).ToString())));
                                    else
                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));
                                }
                                else if (j == 2)
                                {
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));
                                }
                                else if (j == 3)
                                {
                                    if (IsCarried)
                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((int.Parse(FNum) + 1).ToString())));
                                    else
                                        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));
                                }
                            }
                        }
                        TMP_CharacterInfo charInfo = new TMP_CharacterInfo();
                        if (textInfo.characterInfo != null && i < textInfo.characterInfo.Length)
                        {

                            Vector2 uiPosition = GetCharPoos(FirstNumPlace,charInfo, i);

                            int result = int.Parse(FNum.ToString()) + int.Parse(SNum.ToString());

                            int carried = 0;
                            if(IsCarried)
                                carried=1;
                            if (int.Parse(FNum.ToString()) +carried+ int.Parse(SNum.ToString()) < 10)
                            {
                                if (IsCarried)
                                    result += 1;
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(result.ToString())));


                                InstantiateText(FirstNumPlace, (result).ToString(), uiPosition.x + 10, uiPosition.y, -430, true);
                                IsCarried = false;
                            }
                            else
                            {
                                if (IsCarried)
                                    result += 1;
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(result.ToString())));

                                result -= 10;
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(result.ToString())));

                                    
                                    InstantiateText(FirstNumPlace,(result).ToString(), uiPosition.x+10, uiPosition.y, -430, true);
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and carry up one" + SpeakerName)));


                                if (i - 2 >= 0 && !FirstNumPlace.text[i-2].Equals('.'))
                                {
                                    uiPosition = GetCharPoos(FirstNumPlace,charInfo, i - 2);
                                    InstantiateText(FirstNumPlace, "1", uiPosition.x+20 , uiPosition.y, 150, false, i);
                                    IsCarried = true;
                                }
                                else if(i - 2 >= 0 && FirstNumPlace.text[i - 2].Equals('.'))
                                {
                                    uiPosition = GetCharPoos(FirstNumPlace, charInfo, i - 4);
                                    InstantiateText(FirstNumPlace, "1", uiPosition.x + 20, uiPosition.y, 150, false, i);
                                    IsCarried = true;
                                }
                                if(IsCarried  && i == 0)
                                {
                                    InstantiateText(FirstNumPlace, "1", uiPosition.x -70, uiPosition.y,150, false, i);
                                    yield return new WaitForSeconds(0.3f);
                                    yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and write it down" + SpeakerName)));
                                    InstantiateText(FirstNumPlace, "1", uiPosition.x - 70, uiPosition.y , -430, true);
                                }
                            }
                        }
                    }

                }
            }
        }
        button.interactable = true;
        IscalledFromOutSide = false;
    }
    public static void InstantiateText(TextMeshProUGUI FirstNumPlace, string txt, float XPos, float YPos, float Distance, bool IsResult, int counter = -1)
    {
        if (FirstNumPlace != null)
        {
            // Create a new GameObject for the text
            GameObject newTextObject = new GameObject(counter.ToString());
            TextMeshProUGUI newTextMesh = newTextObject.AddComponent<TextMeshProUGUI>();

            // Copy text properties
            newTextMesh.text = txt;
            newTextMesh.font = FirstNumPlace.font;
            newTextMesh.fontSize = FirstNumPlace.fontSize;
            newTextMesh.color = Color.black;
            newTextMesh.alignment = FirstNumPlace.alignment;
            newTextMesh.fontStyle = FontStyles.Bold;

            // Set the new object as a child of the same parent
            newTextObject.transform.SetParent(FirstNumPlace.transform.parent);

            // Ensure the new Text object is in the correct position within the Canvas
            RectTransform newTextRect = newTextObject.GetComponent<RectTransform>();
            newTextRect.localScale = Vector3.one; // Ensure it maintains the correct scale

            // Set the anchored position relative to the parent's anchor
            newTextRect.GetComponent<RectTransform>().anchoredPosition = new Vector2(XPos, YPos + Distance);

            // Disable raycasting for this TextMeshPro object
            newTextMesh.raycastTarget = false;

            // Adjust font size if this is the result text
            if (IsResult)
            {
                newTextMesh.fontSize = 85f;
            }
        }
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
    public static void LangBtnClick(ref bool IsEng , ref string SpeakerName , ref AudioClip[] loop)
    {
        if (IsEng)
        {
            SpeakerName = "_Heba_Egy";
            loop = Resources.LoadAll<AudioClip>("AdditionTerms/AdditionSound/ArabLoop");
            ChangeLang(false);
            IsEng = false;
            AdditionVoiceSpeaker.NumPlace = "EgyNums";
        }
        else
        {

            SpeakerName = "_Sonya_Eng";
            loop = Resources.LoadAll<AudioClip>("AdditionTerms/AdditionSound/EngLoop");
            ChangeLang(true);
            AdditionVoiceSpeaker.NumPlace = "EngNums";
            IsEng = true;
        }
    }
    public static char ValidateInput(string text, int charIndex, char addedChar)
    {
        if (!IscalledFromOutSide) {

            if (char.IsDigit(addedChar) && !(text.Length==0&& addedChar =='0'))
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
            if (char.IsDigit(addedChar))
            {
                return addedChar;
            }
            else
            {
                if (addedChar == '.' && !text.Contains(".") && text.Length!=0)
                {
                    return addedChar;
                }
                return '\0';
            }
        }

    }
    public static Vector2 GetCharPoos(TextMeshProUGUI FirstNumPlace, TMP_CharacterInfo charInfo , int i)
    {
        TMP_TextInfo textInfo2 = FirstNumPlace.GetTextInfo(FirstNumPlace.text);

        // Get character information
        charInfo = textInfo2.characterInfo[i];

        Vector3 charLocalPosition = charInfo.bottomLeft;

        // Convert local position to world space
        Vector3 worldPosition = FirstNumPlace.transform.TransformPoint(charLocalPosition);

        // Adjust position if necessary based on canvas scale and offsets
        RectTransform canvasRect = FirstNumPlace.canvas.GetComponent<RectTransform>();
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);

        // Set the correct position using the canvas scaling
        Vector2 uiPosition = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
        );
        return uiPosition;
    }
    public static IEnumerator AnimateAJ(bool IsCarried , string FNum, string SNum , bool IsAddition)
    {
        GameObject AJ = GameObject.Find("Aj");


        // Find the child object named 'Hand_rigged' in LeftHand and RightHand
        Transform Body = AJ.transform.Find("Boy01_Body_Geo");
        Transform Brows = AJ.transform.Find("Boy01_Brows_Geo");
        Transform Eyes = AJ.transform.Find("Boy01_Eyes_Geo");
        Transform h_Geo = AJ.transform.Find("h_Geo");


        // Get the SkinnedMeshRenderer component from the Hand_rigged child object
        SkinnedMeshRenderer BodyRenderer = Body.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer BrowsRenderer = Brows.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer EyesRenderer = Eyes.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer h_GeoRenderer = h_Geo.GetComponent<SkinnedMeshRenderer>();

        if (BodyRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer component not found on 'Hand_rigged' in LeftHand.");
            yield break;
        }


        // Enable the renderers
        BodyRenderer.enabled = true;
        BrowsRenderer.enabled = true;
        EyesRenderer.enabled = true;
        h_GeoRenderer.enabled = true;

        // Get the Animator components
        Animator AjAnimator = AJ.GetComponent<Animator>();

        // Set the animators in HandAnimationHandler
        AJAnimationHandler.SetAnimator(AjAnimator);
        AJAnimationHandler.IsRunning = true;

        if (!IsCarried)
            AJAnimationHandler.StartCounting(int.Parse(SNum), int.Parse(FNum) , IsAddition);
        else
        {
            AJAnimationHandler.StartCounting(int.Parse(SNum), int.Parse(FNum) + 1 , IsAddition);
        }


        while (AJAnimationHandler.IsRunning)
        {
            yield return null;
        }

        // Disable the renderers and deactivate objects after animation is complete
        BodyRenderer.enabled = false;
        BrowsRenderer.enabled = false;
        EyesRenderer.enabled = false;
        h_GeoRenderer.enabled = false;


    }

    public static void ChangeLang(bool IsEng)
    {
        TextMeshProUGUI[] textMeshProObjects = FindObjectsOfType<TextMeshProUGUI>();
        if (textMeshProObjects.Length > 0)
        {
            foreach (TextMeshProUGUI textMeshPro in textMeshProObjects)
            {
                if (textMeshPro.name.Equals("EnPlaceholder"))
                {
                    textMeshPro.GetComponent<TextMeshProUGUI>().enabled = IsEng;
                }
                else if (textMeshPro.name.Equals("ArPlaceholder"))
                {
                    textMeshPro.GetComponent<TextMeshProUGUI>().enabled = !IsEng;
                }
                textMeshPro.alpha = 1;

            }
        }

    }

    public static void InitializePlaceholders(TMP_InputField inputField)
    {
        TextMeshProUGUI enPlaceholder;
        TextMeshProUGUI arPlaceholder;
        // Find the "Text Area" child from the input field's hierarchy
        Transform textArea = inputField.transform.Find("Text Area");

        if (textArea != null)
        {
            // Find the "EnPlaceholder" and "ArPlaceholder" under the "Text Area"
            enPlaceholder = textArea.Find("EnPlaceholder").GetComponent<TextMeshProUGUI>();
            arPlaceholder = textArea.Find("ArPlaceholder").GetComponent<TextMeshProUGUI>();

            // Add listener to the onSelect event of the input field
            inputField.onSelect.AddListener((string input) => OnInputFieldSelected(enPlaceholder, arPlaceholder));
            inputField.onDeselect.AddListener((string input) => OnInputFieldDeselected(inputField, enPlaceholder, arPlaceholder,AdditionScript.IsEng));

            // Set input type to numeric to display numeric keyboard on Android
            inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
        }
    }

    private static void OnInputFieldSelected(TextMeshProUGUI enPlaceholder, TextMeshProUGUI arPlaceholder)
    {

        // Disable both placeholders
        if (enPlaceholder != null)
        {
            enPlaceholder.enabled = false; // Try disabling it
            enPlaceholder.alpha = 0f;      // Set the alpha to 0 to ensure it is invisible
        }

        if (arPlaceholder != null)
        {
            arPlaceholder.enabled = false;
            arPlaceholder.alpha = 0f;      // Set the alpha to 0
        }

        // Force the canvas to update
        Canvas.ForceUpdateCanvases();

    }

    // When the input field is deselected
    private static void OnInputFieldDeselected(TMP_InputField inputField, TextMeshProUGUI enPlaceholder, TextMeshProUGUI arPlaceholder, bool isEng)
    {
        // Re-enable placeholders if input field is empty
        if (string.IsNullOrEmpty(inputField.text))
        {
            if (inputField.text.Length == 0 && isEng) {
                enPlaceholder.enabled = true;
                arPlaceholder.enabled = false;
                enPlaceholder.alpha = 0.5f;
            }
            else if (inputField.text.Length == 0 && !isEng) {
                arPlaceholder.enabled = true;
                enPlaceholder.enabled = false;
                arPlaceholder.alpha = 0.5f;
            } 
        }
    }

    public static void EnableExplain(ref TMP_InputField FrstNum, ref TMP_InputField SecNum)
    {
        GameObject Explain = GameObject.Find("Explain");
        Button ExplainBtn = Explain.GetComponent<Button>();

        try
        {
            // Attempt to parse the text from input fields to float
            bool isFirstNumValid = float.TryParse(FrstNum.text, out float Fnum);
            bool isSecNumValid = float.TryParse(SecNum.text, out float SNum);

            // Check if both input fields contain valid float values and are not empty
            if (isFirstNumValid && isSecNumValid && !string.IsNullOrEmpty(FrstNum.text) && !string.IsNullOrEmpty(SecNum.text))
            {
                ExplainBtn.interactable = true; // Enable the button
            }
            else
            {
                ExplainBtn.interactable = false; // Disable the button
            }
        }
        catch (Exception)
        {
            ExplainBtn.interactable = false; // Disable the button on any exception (e.g., parsing error)
        }
    }

}