using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrimeFactors : MonoBehaviour
{
    [SerializeField] private TMP_InputField Number;
    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private GameObject Circle;
    [SerializeField] private GameObject Line;
    [SerializeField] private Button LangBtn;
    private AudioClip[] loop;
    public static AudioSource audioSource;
    public static bool Explain = false;
    public static string SpeakerName = "_Sonya_Eng";
    public static bool IsEng = true;
    public static bool InPrimeFactors = false;
    public static List<float> FirstNumList = new List<float>();
    private int NumOfCircles = 0;
    int CurrentLevel = 0;
    public static float XOffset = 200f; // Horizontal offset for left/right placement
    public static float YOffset = 250f; // Vertical offset for each level
    public static float CurrentY = YOffset; 
    public static bool IsCalledFromOutside = false;

    [SerializeField] private ScrollRect scrollRect; // Reference to the ScrollRect component
    public float scrollSpeed = 0.1f; // Scrolling speed
    private float targetScrollPos = 0f; // Target position for scrolling
    private int lastScrollTriggerValue = 0; // Track last trigger poin
    public static string StaticNumber = "";
    public static int branches = 0;

    List<TMP_InputField> FieldsList;


    private Button PauseBtn;
    private Button ResumeBtn;
    public void Start()
    {
        PauseBtn = GameObject.Find("Pause").GetComponent<Button>();
        ResumeBtn = GameObject.Find("Resume").GetComponent<Button>();
        PauseBtn.onClick.AddListener(PauseScript.Pause);
        ResumeBtn.onClick.AddListener(PauseScript.Resume);
        FieldsList = new List<TMP_InputField> { Number };
        try
        {
            Number.text = StaticNumber;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
        if (!IsCalledFromOutside)
        {
            Number.onValidateInput = InputFieldsActions.ValidateEqsInput;

        }

        SpeakerName = "_Jenny_Eng";
        SLStaicFunctions.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.NumPlace = "JennySound/Numbers";
        AdditionVoiceSpeaker.VoiceClipsPlace = "JennySound";

        InputFieldsActions.InitializePlaceholders(Number);


        AdditionVoiceSpeaker.LoadAllAudioClips();

        UnityAction langBtnClickAction = () => LangBtnActions.LangBtnClick(ref IsEng, ref SpeakerName, ref loop, "_Shakir_arab", "ShakirSound/Numbers", "_Jenny_Eng", "JennySound/Numbers", "ShakirSound", "JennySound");
        LangBtn.onClick.AddListener(langBtnClickAction);

        if (!IsCalledFromOutside)
        {
            GameObject.Find("Explain").GetComponent<Button>().onClick.AddListener(ExplainBtnAction);
            GameObject.Find("Solve").GetComponent<Button>().onClick.AddListener(SolveBtnAction);
        }
    }

    void Update()
    {
        PauseScript.ControlPause();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        ExplainEnableMent.EnableExplain(ref FieldsList);
        ExplainEnableMent.DisableExplain(Explain);

    }
    public void SetComponenets(TMP_InputField FrstNum,  TextMeshProUGUI FirstNumPlace, UnityEngine.UI.Button LangBtn,GameObject Circle,  GameObject Line , ScrollRect scrollRect)
    {
        this.Number = FrstNum;
        this.FirstNumPlace = FirstNumPlace;
        this.LangBtn = LangBtn;
        this.Circle = Circle;
        this.Line = Line;   
        this.scrollRect = scrollRect;
    }
    
    public void SolveBtnAction()
    {
        Explain = false;
        solve();
    }
    public void ExplainBtnAction()
    {
        Explain = true;
        solve();
    }
    public void solve()
    {
        Debug.Log(AdditionVoiceSpeaker.NumPlace);
        if (!IsCalledFromOutside)
            ResetValues.ResetAllValues();

        if (!IsCalledFromOutside)
        {
            XOffset = 200f;
            YOffset = 250f;
            GameObject[] GameObjs = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in GameObjs)
            {
                if (obj.name.Contains("Circle") && obj.name.Length > 6)
                {
                    Destroy(obj);
                }
                else if (obj.name.Contains("Line") && obj.name.Length > 4)
                {
                    Destroy(obj);
                }
                else if (obj.name.Contains("text") && obj.name.Length > 4)
                {
                    Destroy(obj);
                }
            }
        }
        FirstNumList.Clear();
        FirstNumList = new List<float>();
        StartCoroutine(ExplainSpaek());

    }  
    public IEnumerator ExplainSpaek()
    {
        int FNum = int.Parse(Number.text);
        if (IsCalledFromOutside)
        {
            yield return CreateTree(FNum, XOffset - 200, CurrentY, 0);
        }
        else {
            yield return CreateTree(FNum, XOffset - 200, YOffset+250, 0);

        }
        if (!IsCalledFromOutside)
        {
            CurrentY -= 150;

            GCFListOperaions gCFListOperaions = gameObject.AddComponent<GCFListOperaions>();
            gCFListOperaions.SetComponents(FirstNumPlace, FirstNumPlace, FirstNumPlace, FirstNumPlace, FirstNumPlace, Circle, Circle, Line, FirstNumPlace, FirstNumPlace, FirstNumList, FirstNumList, Explain, SpeakerName);
            yield return StartCoroutine(gCFListOperaions.WriteList(Number.text,FirstNumList,false));

            XOffset = -320;
            CurrentY -= 120;
            float CurrentNum = FirstNumList[0];
            TextInstantiator.InstantiateText(FirstNumPlace, " = "+(CurrentNum).ToString() ,XOffset , CurrentY-30, 0, true);

            int counter = 0;
            foreach (float Number in FirstNumList)
            {
                if(Number == CurrentNum)
                {
                    counter++;
                }
                else
                {
                    if (counter != 1)
                    {
                        TextInstantiator.InstantiateText(FirstNumPlace, (counter).ToString(), XOffset + 100, CurrentY + 20, 0, true);
                    }
                    XOffset += 200;
                    TextInstantiator.InstantiateText(FirstNumPlace, " × " + (Number).ToString() , XOffset, CurrentY -30, 0, true);
                    counter = 1;
                    CurrentNum = Number;
                }
            }
            if( counter != 1)
            {
                TextInstantiator.InstantiateText(FirstNumPlace, (counter).ToString(), XOffset + 100, CurrentY + 20, 0, true);
            }
        }
        InPrimeFactors = false;

    }
    public IEnumerator CreateTree(float Num, float XAxis, float YAxis, int CurrentLevel , TextMeshProUGUI ParentTxt= null)
    {
        CurrentY = YAxis;
        yield return StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this , Num.ToString() , Explain));

        if (ParentTxt != null)
        {
            ParentTxt.color = Color.black;
        }
        if (IsPrimeScript.IsPrime((int)Num))
        {
            yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "is prime" + SpeakerName, Explain));


            if (CurrentLevel == 0)
            {
               ObjectsInstantiation.InstantiateCircle(XAxis, YAxis+200 , Circle , ref NumOfCircles , FirstNumPlace);
                if(!Explain)
                    yield return new WaitForSeconds(1f);
                ObjectsInstantiation.InstantiateText(Number.text ,ref NumOfCircles , Number , FirstNumPlace);
                CurrentY-=160;
                FirstNumList.Add(Num);
            }
            else
            {
                ParentTxt.color = Color.white;

            }

            yield break;
        }
        else
        {
            branches += 1;
            if (CurrentLevel != 0)
            {
                yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "is composite" + SpeakerName, Explain));
                yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "so" + SpeakerName, Explain));

            }


            int i = ((int)Num / 2) - 1;

            if (i < 2)
                i = 2;
            else if (i > 12)
                i = 12;

            for (; i >= 2; i--)
            {
                if (Num % i == 0)
                {
                    float GreaterNum = (Num / i), SmallerNum = i;
                    if (i > (Num / i))
                        (GreaterNum, SmallerNum) = (SmallerNum, GreaterNum);

                    if (CurrentLevel == 0)
                    {
                        ObjectsInstantiation.InstantiateCircle(XAxis, YAxis + YOffset, Circle, ref NumOfCircles, FirstNumPlace);
                        if (Explain)
                            yield return new WaitForSeconds(1f);
                        ObjectsInstantiation.InstantiateText(Number.text, ref NumOfCircles, Number, FirstNumPlace);
                    }
                    // Play voice for current number and decomposition
                    yield return StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this , Num.ToString() , Explain));

                    yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this ,"can be written as" + SpeakerName, Explain));

                    // Calculate adjusted offset based on current level
                    float adjustedOffset = XOffset / (CurrentLevel + 1f); // Decrease offset as level increases

                    // Play and Instantiate SmallerNum (Left Node)
                    yield return StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SmallerNum.ToString(), Explain));
                    if(CurrentLevel != 0)
                        ObjectsInstantiation.InstantiateLine(XAxis - adjustedOffset+20, YAxis+150, true ,Line, FirstNumPlace);
                    else
                        ObjectsInstantiation.InstantiateLine(XAxis - adjustedOffset + 100, YAxis + 150, true ,Line,  FirstNumPlace);
                    ObjectsInstantiation.InstantiateCircle(XAxis - adjustedOffset, YAxis, Circle, ref NumOfCircles, FirstNumPlace); // Left position
                    if (Explain)
                        yield return new WaitForSeconds(1f);
                    TextMeshProUGUI SmallestTxt = ObjectsInstantiation.InstantiateText(SmallerNum.ToString(),ref NumOfCircles,Number,FirstNumPlace);

                    // Voice for multiplication sign
                    yield return StartCoroutine(SLStaicFunctions.PlayByAddress(this, "time" + SpeakerName, Explain));

                    // Play and Instantiate GreaterNum (Right Node)
                    yield return StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, GreaterNum.ToString(), Explain));
                    if (CurrentLevel != 0)
                    {
                        ObjectsInstantiation.InstantiateLine(XAxis + adjustedOffset - 20, YAxis + 150, false, Line, FirstNumPlace);

                    }
                    else
                    {
                        ObjectsInstantiation.InstantiateLine(XAxis + adjustedOffset - 90, YAxis + 150, false , Line , FirstNumPlace);

                    }
                    ObjectsInstantiation.InstantiateCircle(XAxis + adjustedOffset, YAxis, Circle, ref NumOfCircles, FirstNumPlace); // Right position

                    if (Explain)
                        yield return new WaitForSeconds(1f);
                    TextMeshProUGUI GreaterTxt = ObjectsInstantiation.InstantiateText(GreaterNum.ToString() , ref NumOfCircles , Number , FirstNumPlace);

                    CurrentLevel++;
                    YAxis -= YOffset;

                    // Store numbers for further processing
                    if (IsPrimeScript.IsPrime((int)SmallerNum))
                    {
                        FirstNumList.Add(SmallerNum);
                    }

                    if (IsPrimeScript.IsPrime((int)GreaterNum))
                    {
                        FirstNumList.Add(GreaterNum);

                    }
                    if (ParentTxt != null)
                    {
                        ParentTxt.color = Color.white;
                    }
                    yield return StartCoroutine(CreateTree(SmallerNum, XAxis - adjustedOffset, YAxis, CurrentLevel , SmallestTxt)); // Left subtree
                    yield return StartCoroutine(CreateTree(GreaterNum, XAxis + adjustedOffset, YAxis, CurrentLevel , GreaterTxt)); // Right subtree

                    yield break;
                }
            }
        }
    }
}
