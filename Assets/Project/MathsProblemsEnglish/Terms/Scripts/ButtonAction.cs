using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.VisualScripting.Antlr3.Runtime;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Text;

public class ButtonAction : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button[] buttons;
    [SerializeField] TextMeshProUGUI FirstNumPlace;

    private TMP_InputField InputFieldCpy;
    private int TermsCounter = 0;
    public static Dictionary <TMP_InputField , char> TmpRefrerenceChar = new Dictionary<TMP_InputField, char> ();
    private float lastScrollValue = 0f; 

    private void Start()
    {
        buttons[0].onClick.AddListener(() => WriteOnInput('0'));
        buttons[1].onClick.AddListener(() => WriteOnInput('1'));
        buttons[2].onClick.AddListener(() => WriteOnInput('2'));
        buttons[3].onClick.AddListener(() => WriteOnInput('3'));
        buttons[4].onClick.AddListener(() => WriteOnInput('4'));
        buttons[5].onClick.AddListener(() => WriteOnInput('5'));
        buttons[6].onClick.AddListener(() => WriteOnInput('6'));
        buttons[7].onClick.AddListener(() => WriteOnInput('7'));
        buttons[8].onClick.AddListener(() => WriteOnInput('8'));
        buttons[9].onClick.AddListener(() => WriteOnInput('9'));
        buttons[10].onClick.AddListener(() => WriteOnInput('.'));
        buttons[11].onClick.AddListener(() => WriteOnInput('='));
        buttons[12].onClick.AddListener(() => OperationSign('+'));
        buttons[13].onClick.AddListener(() => OperationSign('-'));
        buttons[14].onClick.AddListener(() => OperationSign('×'));
        buttons[15].onClick.AddListener(() => OperationSign('÷'));
        buttons[16].onClick.AddListener(() => WriteOnInput(','));
        buttons[17].onClick.AddListener(() => WriteOnInput('('));
        buttons[18].onClick.AddListener(() => WriteOnInput(')'));
        buttons[19].onClick.AddListener(() => WriteOnInput('x'));
        buttons[20].onClick.AddListener(() => WriteOnInput('y'));
        buttons[21].onClick.AddListener(() => WriteOnInput('z'));
        buttons[22].onClick.AddListener(() => WriteOnInput('e'));
        buttons[23].onClick.AddListener(() => WriteOnInput('√'));
        buttons[24].onClick.AddListener(() => inputField.text += "||");
        buttons[25].onClick.AddListener(() => WriteOnInput('a'));
        buttons[26].onClick.AddListener(() => WriteOnInput('b'));
        buttons[27].onClick.AddListener(() => WriteOnInput('c'));

        buttons[28].onClick.AddListener(() => RightClick());
        buttons[29].onClick.AddListener(() => LeftClick());     
        
        buttons[30].onClick.AddListener(() => UpClick());
        buttons[31].onClick.AddListener(() => DownClick());

        buttons[32].onClick.AddListener(() => PowerClick());
        buttons[33].onClick.AddListener(() => CreateFraction());
        buttons[34].onClick.AddListener(() => PowerTwoClick());

        buttons[35].onClick.AddListener(() => Del());
    }
    public void OperationSign(char latter)
    {
        WriteOnInput(latter);
        TermsCounter++;
    }
    private void WriteOnInput(char letter)
    {
        if (InputFieldCpy != null)
        {
            int CaretPosCpy = InputFieldCpy.caretPosition;
            string beforeCaretCpy = InputFieldCpy.text.Substring(0, CaretPosCpy);
            string afterCaretCpy = InputFieldCpy.text.Substring(CaretPosCpy);

            InputFieldCpy.text = beforeCaretCpy + " " + afterCaretCpy;
            InputFieldCpy.caretPosition = CaretPosCpy + 1;
        }
        int caretPos = inputField.caretPosition;

        string beforeCaret = inputField.text.Substring(0, caretPos);
        string afterCaret = inputField.text.Substring(caretPos);

        inputField.text = beforeCaret + letter + afterCaret;

        inputField.caretPosition = caretPos + 1;
        if(InputFieldCpy==null)
            TermsFieldActions.ShiftAllFields(letter.ToString(), inputField, FirstNumPlace);
        else
            TermsFieldActions.ShiftAllFields(letter.ToString(), InputFieldCpy, FirstNumPlace);

        inputField.ActivateInputField();
    }
    void Update()
    {
        if (inputField.textComponent != null ) // Ensure the text component exists
        {
            float currentScrollValue = inputField.textComponent.rectTransform.anchoredPosition.x;

            if (Mathf.Abs(currentScrollValue - lastScrollValue) > 0.001f && InputFieldCpy == null) // Avoid small floating-point errors
            {
                if (currentScrollValue < lastScrollValue)
                {
                    TermsFieldActions.ShiftAllFields("", inputField, FirstNumPlace, false);
                }
                else
                {
                    TermsFieldActions.ShiftAllFields("", inputField, FirstNumPlace, true);
                }
            }
            else if (Mathf.Abs(currentScrollValue - lastScrollValue) > 0.001f && InputFieldCpy != null && inputField.name.Contains("Pow"))
            {
                TermsFieldActions.WidenInputField(inputField.gameObject);
            }
            else if(Mathf.Abs(currentScrollValue - lastScrollValue) > 0.001f && InputFieldCpy != null && (inputField.name.Contains("Deno") || inputField.name.Contains("Nue")))
            {
                int FNum = 0, SNum = 0;
                string[] parts = inputField.name.Split(' '); // Split by spaces

                int.TryParse(parts[1], out FNum);
                int.TryParse(parts[2], out SNum);
                if (inputField.name.Contains("Deno"))
                {
                    TMP_InputField NueTmp = GameObject.Find("Nue " + FNum + " " + SNum).GetComponent<TMP_InputField>();
                    TermsFieldActions.WidenInputField(NueTmp.gameObject);
                }
                else
                {
                    TMP_InputField DenoTmp = GameObject.Find("Deno " + FNum + " " + SNum).GetComponent<TMP_InputField>();
                    TermsFieldActions.WidenInputField(DenoTmp.gameObject);
                }
                TextMeshProUGUI Line = GameObject.Find("Line " + FNum + " " + (SNum-1)).GetComponent<TextMeshProUGUI>();

                TermsFieldActions.WidenInputField(inputField.gameObject);
                TermsFieldActions.WidenInputField(Line.gameObject);
            }
            lastScrollValue = currentScrollValue; // Update the tracking variable

        }
    }
    public void RightClick()
    {
        CursorController.GoRight(ref inputField ,ref  InputFieldCpy);
    }   
    public void LeftClick()
    {
        CursorController.GoLeft(ref inputField , ref InputFieldCpy);
    }
    public void UpClick()
    {
        CursorController.UpDown(ref inputField, ref InputFieldCpy,"Pow","Nue");
    }
    public void DownClick()
    {
        CursorController.UpDown(ref inputField, ref InputFieldCpy, "Deno");
    }
    public void PowerTwoClick()
    {
        TMP_InputField Power = TermsFieldActions.InputFieldCreator("Pow", 675, TermsCounter, ref inputField, ref InputFieldCpy, FirstNumPlace, ref TmpRefrerenceChar);

        Power.text = "2";
    }
    public void PowerClick()
    {
        if ((inputField.caretPosition!=0) &&(int.TryParse(inputField.text[inputField.caretPosition-1].ToString() , out _) || inputField.text[inputField.caretPosition-1] =='x' || inputField.text[inputField.caretPosition - 1] == 'y' || inputField.text[inputField.caretPosition - 1] == 'z' || inputField.text[inputField.caretPosition - 1] == 'a' || inputField.text[inputField.caretPosition - 1] == 'b' || inputField.text[inputField.caretPosition - 1] == 'c'))
        {
            TermsFieldActions.InputFieldCreator("Pow", 675, TermsCounter, ref inputField, ref InputFieldCpy, FirstNumPlace, ref TmpRefrerenceChar);
        }
    }
    public void CreateFraction()
    {
        float Xpos = TermsFieldActions.GetXPos(inputField);
        TextMeshProUGUI Line = TextInstantiator.InstantiateText(FirstNumPlace, "\u2015", Xpos, 630, 0, false);
        Line.name = "Line" + " " +TermsCounter +" "+inputField.caretPosition;

        int caretPos = inputField.caretPosition;
        string beforeCaret = inputField.text[..caretPos] + " ";
        string afterCaret = inputField.text[caretPos..];
        inputField.text = beforeCaret + afterCaret;
        inputField.caretPosition = caretPos + 1;
        inputField.ForceLabelUpdate();

        TermsFieldActions.InputFieldCreator("Deno" , 570, TermsCounter, ref inputField, ref InputFieldCpy, FirstNumPlace, ref TmpRefrerenceChar);

        inputField = InputFieldCpy;
        InputFieldCpy = null;

        TermsFieldActions.InputFieldCreator("Nue", 650, TermsCounter , ref inputField, ref InputFieldCpy, FirstNumPlace, ref TmpRefrerenceChar);

        int CaretPosCpy = InputFieldCpy.caretPosition;
        string beforeCaretCpy = InputFieldCpy.text[..CaretPosCpy];
        string afterCaretCpy = InputFieldCpy.text[CaretPosCpy..];
        InputFieldCpy.text = beforeCaretCpy + "    " + afterCaretCpy;
        InputFieldCpy.caretPosition = CaretPosCpy + 4;
        InputFieldCpy.ForceLabelUpdate();
    }
    public void Del()
    {
        try
        {
            if (InputFieldCpy==null && inputField.text[inputField.caretPosition-1]==' ')
            {
                CursorController.UpDown(ref inputField, ref InputFieldCpy, "Pow", "Nue");
            }
            else if(inputField.text.Length == 0 && inputField.name.Contains("Nue"))
            {
                string[] parts = inputField.name.Split(' '); // Split by spaces
                if (parts.Length >= 2) // Ensure there are at least 4 parts ("Pow X Y Z")
                {
                    int.TryParse(parts[1], out int FNum);
                    int.TryParse(parts[2], out int SNum);

                    GameObject DeletedLine = GameObject.Find("Line " + FNum + " " + (SNum-1));
                    TMP_InputField DeletedDeno = GameObject.Find("Deno " + FNum + " " + SNum).GetComponent<TMP_InputField>();

                    Destroy(DeletedLine.gameObject);
                    Destroy(DeletedDeno.gameObject);
                    Destroy(inputField.gameObject);
                    inputField = InputFieldCpy;
                    InputFieldCpy = null;
                    StringBuilder InputFieldTxt = new StringBuilder(inputField.text);
                    inputField.caretPosition -= 1;
                    while (InputFieldTxt[inputField.caretPosition]== ' ')
                    {
                        InputFieldTxt = InputFieldTxt.Remove(inputField.caretPosition, 1);
                    }
                    inputField.text = InputFieldTxt.ToString();
                }
            }
            else
            {
                string InputFieldBeforeDeleting = inputField.text;
                inputField.text = inputField.text[..(inputField.caretPosition - 1)] + inputField.text[(inputField.caretPosition)..];

                if (inputField.text.Length == 0 && inputField.name.Contains("Pow"))
                {
                    int CurrentIndex = InputFieldCpy.caretPosition;
                    StringBuilder InputFieldTxt = new StringBuilder(InputFieldCpy.text);
                    InputFieldCpy.text = InputFieldTxt.Remove(InputFieldCpy.caretPosition, 1).ToString();
                    InputFieldCpy.caretPosition = CurrentIndex;

                    Destroy(inputField.gameObject);

                    inputField = InputFieldCpy;
                    inputField.caretPosition--;
                    InputFieldCpy = null;
                }

                if (inputField.text.Length != 0 && inputField.name.Contains("Pow") && !InputFieldBeforeDeleting.Equals(inputField.text))
                {
                    int CurrentIndex = InputFieldCpy.caretPosition;
                    StringBuilder InputFieldTxt = new StringBuilder(InputFieldCpy.text);
                    InputFieldCpy.text = InputFieldTxt.Remove(InputFieldCpy.caretPosition, 1).ToString();
                    InputFieldCpy.caretPosition = CurrentIndex;

                }
                inputField.caretPosition -= 1;
            }
            inputField.ActivateInputField();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
}