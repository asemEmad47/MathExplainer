using System.Collections;
using TMPro;
using Unity.VisualScripting;
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

    [SerializeField] private Button LangBtn;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    private bool Explain = false;
    public static string SpeakerName = "_Sonya_Eng";
    public static bool IsEng = true;
    public static bool IscalledFromOutSide = false;
    private int SecNumIndex = -1;
    public static int ResDistance = -400;
    public static int SupDistance = 150;
    public static bool IsFinshed = false;
    public void Start()
    {
        IsFinshed = false;
        ResDistance = -400;
        SupDistance = 150;
        UnityAction langBtnClickAction = () => AdditionScript.LangBtnClick(ref IsEng, ref SpeakerName, ref loop);
        LangBtn.onClick.AddListener(langBtnClickAction);

        if (AdditionScript.IsBasic)
        {
            FrstNum.onValidateInput = AdditionScript.ValidateInput;
            if (!IscalledFromOutSide)
                SecNum.onValidateInput = ValidateSecInput;
        }


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
    public void SetComponenets(TMP_InputField FrstNum, TMP_InputField SecNum, TextMeshProUGUI FirstNumPlace, TextMeshProUGUI Line, TextMeshProUGUI sign, Button LangBtn, int SecNumIndex)
    {
        this.FrstNum = FrstNum;
        this.SecNum = SecNum;
        this.FirstNumPlace = FirstNumPlace;
        this.Line = Line;
        this.sign = sign;

        this.SecNumIndex = SecNumIndex;
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

        if (Explain)
        {
            if (!IscalledFromOutSide) {
                yield return StartCoroutine(WriteTwoNumbers());
            }

        }

        string FirstNumPlaceCpy = FirstNumPlace.text;

        if (FirstNumPlace != null)
        {
            // Populate textInfo manually
            TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FirstNumPlace.text);

            // Check if textInfo is available

            GameObject newTextObject = GameObject.Find("SecNumPlace");
            TextMeshProUGUI SecNumPlace = newTextObject.GetComponent<TextMeshProUGUI>();
            if (textInfo != null)
            {
                int carriedNumber = 0;
                for (int i = FirstNumPlace.text.Length - 1; i >= 0; i--)
                {
                    string FNum = FirstNumPlace.text[i].ToString();
                    string SNum = SecNum.text;
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                    Vector3 characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, i);
                    if (SecNumIndex != -1)
                    {
                        SNum = SecNum.text[SecNumIndex].ToString();
                    }
                    if(i == FirstNumPlace.text.Length - 1)
                    {
                        RectTransform newTextRect = newTextObject.GetComponent<RectTransform>();
                        newTextRect.localScale = Vector3.one; // Ensure it maintains the correct scale

                        // Set the anchored position relative to the parent's anchor
                        newTextRect.GetComponent<RectTransform>().anchoredPosition = new Vector2(characterPosition.x-25, characterPosition.y-100);

                        // Disable raycasting for this TextMeshPro object
                        SecNumPlace.raycastTarget = false;

                    }
                    if (!FirstNumPlace.text[i].Equals(' '))
                    {
                        if (Explain)
                        {
                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(FNum)));


                            string temp = FirstNumPlace.text.Substring(0, i);
                            string temp2 = FirstNumPlace.text.Substring(i + 1);
                            temp += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{FNum}</color>";
                            FirstNumPlace.text = temp + temp2;
                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/time" + SpeakerName)));

                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(SNum)));


                            temp2 = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{SNum}</color>";

                            if (SecNumIndex == -1)
                            {
                                temp = "";
                            }
                            else
                            {
                                if (SecNumIndex == 0)
                                {
                                    try
                                    {
                                        temp = SecNum.text[1].ToString();
                                        (temp, temp2) = (temp2, temp);
                                    }
                                    catch (System.Exception)
                                    {
                                        temp = "";
                                    }

                                }
                                else
                                {
                                    temp = SecNum.text[0].ToString();
                                }
                            }
                            SecNumPlace.text = temp+temp2 ;

                        }

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
                                characterPosition = AdditionScript.GetCharPoos(FirstNumPlace, charInfo, index);

                            }
                            if (SecNumIndex == 0 && i == 0 && SecNum.text.Length >= 2)
                            {
                                characterPosition = new Vector3(characterPosition.x - 56, characterPosition.y, 0);
                            }
                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal" + SpeakerName)));

                            int result = int.Parse(FNum.ToString()) * int.Parse(SNum);

                            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(result.ToString())));

                            result += carriedNumber;

                            if (carriedNumber != 0)
                            {

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/plus" + SpeakerName)));
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(carriedNumber.ToString())));

                                GameObject textGameObject = GameObject.Find((i).ToString());
                                TextMeshProUGUI myText = textGameObject.GetComponent<TextMeshProUGUI>();
                                myText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{myText.text}</color>";

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/equal" + SpeakerName)));
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(result.ToString())));
                            }

                            if (result >= 10)
                            {
                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/put" + SpeakerName)));

                                AdditionScript.InstantiateText(FirstNumPlace, (result.ToString()[result.ToString().Length-1]).ToString(), characterPosition.x + 15, characterPosition.y, ResDistance, true);

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(result.ToString()[result.ToString().Length-1].ToString())));
                                carriedNumber = int.Parse(result.ToString().Substring(0,result.ToString().Length-1));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and carry up" + SpeakerName)));

                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(carriedNumber.ToString())));

                                if (SecNumIndex != 0) {
                                    AdditionScript.InstantiateText(FirstNumPlace, carriedNumber.ToString(), characterPosition.x - 62, characterPosition.y, SupDistance, false, i - 2);
                                }
                                else
                                {
                                    AdditionScript.InstantiateText(FirstNumPlace, carriedNumber.ToString(), characterPosition.x +23, characterPosition.y, SupDistance+ SupDistance*0.1f, false, i - 2);
                                }


     
                            }
                            else
                            {
                                carriedNumber = 0;
                                AdditionScript.InstantiateText(FirstNumPlace, (result.ToString()[result.ToString().Length - 1]).ToString(), characterPosition.x + 15, characterPosition.y, ResDistance, true);

                            }

                            if (carriedNumber != 0 && i ==0) // if there is a carry and the number is finshed
                            {
                                GameObject textGameObject = GameObject.Find((i-2).ToString());
                                TextMeshProUGUI myText = textGameObject.GetComponent<TextMeshProUGUI>();
                                myText.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{myText.text}</color>";


                                yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/and write it down" + SpeakerName)));
                                AdditionScript.InstantiateText(FirstNumPlace, carriedNumber.ToString(), characterPosition.x - 56, characterPosition.y, ResDistance, true);

                            }
                        }
                    }
                    SecNumPlace.text = SecNum.text;
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
        SubtractionScript.ResetAllValues(Line, FirstNumPlace, FirstNumPlace, sign);

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/first" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the first number" + SpeakerName)));

        string TempFrstNum = "";
        for (int i = 0; i < FrstNum.text.Length; i++)
        {
            TempFrstNum += FrstNum.text[i] + " ";
        }
        FirstNumPlace.text = TempFrstNum;

        FirstNumPlace.gameObject.SetActive(true);

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/then" + SpeakerName)));

        yield return (StartCoroutine(AdditionVoiceSpeaker.PlayByAddress("AdditionTerms/AdditionSound/write the second number" + SpeakerName)));

        GameObject newTextObject = new GameObject("SecNumPlace");
        TextMeshProUGUI newTextMesh = newTextObject.AddComponent<TextMeshProUGUI>();

        // Copy text properties
        newTextMesh.text = SecNum.text;
        newTextMesh.font = FirstNumPlace.font;
        newTextMesh.fontSize = 85;
        newTextMesh.color = Color.black;
        newTextMesh.alignment = FirstNumPlace.alignment;
        newTextMesh.fontStyle = FontStyles.Bold;

        // Set the new object as a child of the same parent
        newTextObject.transform.SetParent(FirstNumPlace.transform.parent);


        sign.gameObject.SetActive(true);
        Line.gameObject.SetActive(true);
    }
}