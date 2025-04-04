using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Parser : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField inputField;

    [SerializeField] private TextMeshProUGUI FirstNumPlace;
    [SerializeField] private GameObject Line;
    [SerializeField] private float Xpos;
    [SerializeField] private float Ypos;
    [SerializeField] private bool Explain;

    List<Term> terms = new List<Term>();
    private string InputFieldCpy;

    public void ParseInputField()
    {
        GameObject.Find("KeyBoard").SetActive(false);
        InputFieldCpy = inputField.text;
        int i = 0;
        string number = "", nue = "", Deno = "",Symbol = "" , NumPow="",SymbPow="";
        bool IsAbs = false;
        bool FirstSymb = true;
        List<Term> MultiplyedTerms = new();
        List<Term> DividedTerms = new();
        bool IsBracketTerm = false;
        BracketTerms BracketTerm =  new();
        if (inputField.text.IndexOf('\n') != -1)
        {
            i = inputField.text.IndexOf('\n');
        }

        if (inputField.text[i].Equals('\n')) {
            i++;
        }
        while (i<inputField.text.Length )
        {
            if (inputField.text[i].Equals('+') || inputField.text[i].Equals('-'))
            {
                if(!string.IsNullOrEmpty(number) || !string.IsNullOrEmpty(nue)|| !string.IsNullOrEmpty(Deno) || !string.IsNullOrEmpty(Symbol))
                {
                    
                    Term CurrentTerm = CreateTerm(number, nue, Deno, Symbol, SymbPow, NumPow, MultiplyedTerms, DividedTerms);
                    if (!IsBracketTerm)
                    {
                        terms.Add(CurrentTerm);
                    }
                    else
                    {
                        BracketTerm.AddNewTerm(CurrentTerm);
                    }
                    ResetValues(ref number, ref nue, ref Deno, ref Symbol, ref SymbPow, ref NumPow);
                }
                if (inputField.text[i].Equals('-'))
                    number += '-';

                if (!FirstSymb) {
                    terms[terms.Count - 1].AddMutlipliedTerm(MultiplyedTerms);
                }
                FirstSymb = true;
                MultiplyedTerms = new();
                DividedTerms = new();
            }
            else if (inputField.text[i].Equals('×') || inputField.text[i].Equals('÷'))
            {
                if (inputField.text[i].Equals('÷'))
                {
                    if (number.Equals("-"))
                        number = "-1";
                    Debug.Log("here adding divided");
                    Term CurrentTerm = CreateTerm(number, nue, Deno, Symbol, SymbPow, NumPow, MultiplyedTerms, DividedTerms);
                    ResetValues(ref number, ref nue, ref Deno, ref Symbol, ref SymbPow, ref NumPow);
                    DividedTerms.Add(CurrentTerm);
                }
                else
                {
                    if (number.Equals("-"))
                        number = "-1";
                    Term CurrentTerm = CreateTerm(number, nue, Deno, Symbol, SymbPow, NumPow, MultiplyedTerms, DividedTerms);
                    ResetValues(ref number, ref nue, ref Deno, ref Symbol, ref SymbPow, ref NumPow);
                    MultiplyedTerms.Add(CurrentTerm);
                }
            }
            else if (isSymbol(inputField.text[i]))
            {
                if (i +1 < inputField.text.Length  && inputField.text[i + 1] == ' ')
                {
                    ExtractFractions(i+1, ref nue, ref Deno, ref SymbPow, ref NumPow);
                }
                Symbol += inputField.text[i];
                if (i + 1 < inputField.text.Length && inputField.text[i + 1] != '(' && inputField.text[i + 1] != ')' )
                {
                    Term CurrentTerm = CreateTerm(number, nue, Deno, Symbol, SymbPow, NumPow, null);

                    if (FirstSymb)
                    {
                        FirstSymb = false;
                        if(!IsBracketTerm)
                            terms.Add(CurrentTerm);
                        else
                            BracketTerm.AddNewTerm(CurrentTerm);
                    }
                    else
                    {
                        MultiplyedTerms.Add(CurrentTerm);
                    }
                    ResetValues(ref number, ref nue, ref Deno, ref Symbol, ref SymbPow, ref NumPow);
                }

            }
            else if (inputField.text[i].Equals('('))
            {
                BracketTerm = new();
                if (number.Equals("-"))
                    number = "-1";

                Term CurrentTerm = CreateTerm(number, nue, Deno, Symbol, SymbPow, NumPow, MultiplyedTerms);
                terms.Add(CurrentTerm);
                ResetValues(ref number, ref nue, ref Deno, ref Symbol, ref SymbPow, ref NumPow);
                IsBracketTerm = true;
            }
            else if (inputField.text[i].Equals(')'))
            {
                Term CurrentTerm = CreateTerm(number, nue, Deno, Symbol, SymbPow, NumPow, MultiplyedTerms);
                BracketTerm.AddNewTerm(CurrentTerm);

                terms[terms.Count-1].AddBracket(BracketTerm);
                BracketTerm = new();
                ResetValues(ref number, ref nue, ref Deno, ref Symbol, ref NumPow, ref SymbPow);
                IsBracketTerm = false;


            }
            else if (!inputField.text[i].Equals('(') && !inputField.text[i].Equals('―') && !inputField.text[i].Equals(' '))
            {
                if (i + 1 < inputField.text.Length && inputField.text[i + 1] == ' ')
                {
                    ExtractFractions(i+1, ref nue, ref Deno, ref SymbPow, ref NumPow);
                }
                number += inputField.text[i];
            }
            else if (inputField.text[i].Equals('|') && !IsAbs)
            {
                IsAbs = true;
            }
            else if (inputField.text[i].Equals('|') && IsAbs)
            {
                if (IsAbs && number[0] == '-')
                {
                    number = number.Substring(1);
                }
            }
            else if(
                (i ==0 || (i > 0 && inputField.text[i-1]!=' ')
                ) && inputField.text[i]==' ' )
            {
                ExtractFractions(i , ref nue, ref Deno, ref SymbPow, ref NumPow);
            }
            i++;
        }
        Term term1;
        int DenoTemp1 = 0;
        int NeuTemp1 = 0;
        if (!nue.Equals(""))
        {
            NeuTemp1 = int.Parse(nue);
            DenoTemp1 = int.Parse(Deno);
        }

        if (SymbPow.Equals(""))
            SymbPow = "0";

        if (NumPow.Equals(""))
            NumPow = "0";


        if (string.IsNullOrEmpty(number.ToString().Trim().TrimEnd()))
            term1 = new Term(Symbol, '+', NeuTemp1, DenoTemp1, int.Parse(SymbPow), (NumPow));

        else if(number.Equals("-"))
            term1 = new Term(Symbol, '-', NeuTemp1, DenoTemp1, int.Parse(SymbPow), (NumPow));

        else
        {
            if (number[0].Equals("-"))
            {
                term1 = new Term(Symbol, '-', NeuTemp1, DenoTemp1, int.Parse(number), (SymbPow), (NumPow));
            }
            else
            {
                term1 = new Term(Symbol, '+', NeuTemp1, DenoTemp1, int.Parse(number), (SymbPow), (NumPow));

            }
        }

        if (MultiplyedTerms != null)
        {
            term1.AddMutlipliedTerm(MultiplyedTerms);
        }
        ResetValues(ref number, ref nue, ref Deno, ref Symbol,ref NumPow,ref SymbPow);
        inputField.text = InputFieldCpy;

        EditorProblemDirection.Problem = "simplify";
        ProblemSolver solver = EditorProblemDirection.ChooseProblem();
        solver.SetComponents(FirstNumPlace, Line, Xpos, Ypos, Explain ,this);
        StartCoroutine(solver.Solve(terms));
    }
    public void ResetValues(ref string number , ref string nue, ref string deno,ref string symbol, ref string NumPow, ref string SymbPow)
    {
        number = "";
        nue = "";
        deno = "";
        symbol = "";
        NumPow = "";
        SymbPow = "";
    }

    public bool isSymbol(char latter)
    {
        if(latter.Equals('a') || latter.Equals('b') || latter.Equals('c') || latter.Equals('x') || latter.Equals('y') || latter.Equals('z'))
            return true;
        else
            return false;
    }
    public Term CreateTerm(string number = "", string nue = "", string Deno = "", string Symbol = "", string SymbPow = "", string NumPow = "", List<Term> MultiplyedTerms = null,List<Term> DividedTerms = null)
    {
        Term term;
        int DenoTemp = 0;
        int NeuTemp = 0;

        if (!nue.Equals(""))
        {
            NeuTemp = int.Parse(nue);
            DenoTemp = int.Parse(Deno);
        }
        if (SymbPow.Equals(""))
            SymbPow = "0";
        if (NumPow.Equals(""))
            NumPow = "0";
        if (number.Equals("-"))
            number = "-1";



        if (!string.IsNullOrEmpty(nue) && !int.TryParse(nue, out NeuTemp))
        {
            Debug.LogError($"Invalid integer format for nue: {nue}");
        }

        if (!string.IsNullOrEmpty(Deno) && !int.TryParse(Deno, out DenoTemp))
        {
            Debug.LogError($"Invalid integer format for Deno: {Deno}");
        }

        if (!string.IsNullOrEmpty(number) && !int.TryParse(number, out int parsedNumber))
        {
            number = "1";
        }

        if (string.IsNullOrEmpty(number.TrimEnd().Trim()))
            term = new Term(Symbol, '+', NeuTemp, DenoTemp,0, NumPow, (SymbPow), MultiplyedTerms, DividedTerms);
        else
        {
            term = new Term(Symbol, number[0], NeuTemp, DenoTemp, int.Parse(number), (NumPow), (SymbPow), MultiplyedTerms, DividedTerms);

        }


        return term;
    }
    public void ExtractFractions(int i , ref string nue , ref string deno , ref string SymbPow , ref string NumPow)
    {
        foreach (var item in ButtonAction.TmpRefrerenceChar)
        {
            string FieldName;
            int FNum, SNum;
            string[] parts = item.Key.ToString().Split(' '); // Split by spaces
            if (parts.Length >= 3) // Ensure there are at least 4 parts ("Pow X Y Z")
            {
                FieldName = parts[0];
                int.TryParse(parts[1], out FNum);
                int.TryParse(parts[2], out SNum);
                if (SNum == i)
                {
                    if (FieldName.Equals("Pow"))
                    {
                        if (isSymbol(inputField.text[i - 1]))
                        {
                            SymbPow = item.Key.text;
                        }
                        else
                        {
                            NumPow = item.Key.text;
                        }
                    }

                }
                if(SNum == i + 1)
                {
                    if (FieldName.Equals("Nue"))
                    {
                        nue = item.Key.text;
                    }
                    else if (FieldName.Equals("Deno"))
                    {
                        deno = item.Key.text;
                    }
                }
            }
        }
    }
}
