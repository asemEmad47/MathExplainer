using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TermsWriter : MonoBehaviour
{
    public static int TermIndex = 0;
    public static IEnumerator WriteTerms(List<Term> terms, float Xpos, float Ypos, TextMeshProUGUI FirstNumPlace, bool Explain , MonoBehaviour monoBehaviour , GameObject Line , bool InRecersion = false , string Iteration = "", bool isMultiplied = false )
    {
        int counter = 0;
        foreach (Term term in terms)
        {
            ++TermIndex;

            if (isMultiplied)
            {
                TextInstantiator.InstantiateText(FirstNumPlace, "× ", Xpos , Ypos, 0, false, 1, 0);
                Xpos += 60;

            }
            if(counter!=0 && terms.IndexOf(term)!=0 && !isMultiplied)
            {
                if (term.GetSign() != '-')
                {
                    term.SetSign('+');
                    var sign = TextInstantiator.InstantiateText(FirstNumPlace, term.GetSign().ToString(), Xpos - (30*term.GetNumber().ToString().Length), Ypos, 0, false, 1, 0);
                    sign.name = Iteration+" "+TermIndex;


                }


            }
            if (term.GetNumber() != 0)
            {
                var Num =  TextInstantiator.InstantiateText(FirstNumPlace, term.GetNumber().ToString(), Xpos, Ypos, 0, false, 1, 0);

                var NumPow = new TextMeshProUGUI();
                if (!string.IsNullOrEmpty(term.GetNumPow()) & !term.GetNumPow().Equals("0"))
                {
                    NumPow = TextInstantiator.InstantiateText(FirstNumPlace, term.GetNumPow(), Xpos+20, Ypos+75, 0, false, 1, 0);
                }
                if (!string.IsNullOrEmpty(term.GetSymbol()))
                    Xpos += (30 * term.GetNumber().ToString().Length);

                Num.name = Iteration + " " + TermIndex;



                if (NumPow != null && !InRecersion)
                    NumPow.name = Iteration + " " + TermIndex;
                else
                {
                    if ((NumPow != null))
                        NumPow.name = Iteration + " " + TermIndex;

                }
            }

            if ((term.GetNue() != 1 || term.GetDeno() != 1) && (term.GetNue() != 0 || term.GetDeno() != 0))
            {
                TextInstantiator.InstantiateText(FirstNumPlace, "×", Xpos , Ypos, 0, false, 1, 0);
                Xpos += 75;
                SLStaicFunctions.WriteFraction(monoBehaviour, FirstNumPlace, Line, Explain, term.GetNue(), term.GetDeno(), Xpos, Ypos, int.Parse(Iteration));

                Xpos += 75;
            }

            if (!string.IsNullOrEmpty(term.GetSymbol()) )
            {
                var Symbol =  TextInstantiator.InstantiateText(FirstNumPlace, term.GetSymbol(), Xpos, Ypos, 0, false, 1, 0);

                var SymbolPow = new TextMeshProUGUI();
                if (!string.IsNullOrEmpty(term.GetSymbPow()) && !term.GetSymbPow().Equals("0"))
                {
                    SymbolPow =  TextInstantiator.InstantiateText(FirstNumPlace, term.GetSymbPow(), Xpos + 20, Ypos + 75, 0, false, 1, 0);
                }
                Symbol.name = Iteration + " " + TermIndex;

                if (SymbolPow != null && !InRecersion)
                    SymbolPow.name = Iteration + " "+TermIndex;
                else
                {
                    if ((SymbolPow != null))
                        SymbolPow.name = Iteration + " " + TermIndex;
                }
                    Xpos += 20;
            }
            if (term.GetMultipliedTerms() != null && term.GetMultipliedTerms().Count!=0) {
                yield return WriteTerms(term.GetMultipliedTerms(), Xpos+30, Ypos, FirstNumPlace, Explain, monoBehaviour, Line,true , Iteration , true);


                foreach (var item in term.GetMultipliedTerms())
                {
                    Xpos += (60 + 60 * item.GetNumber().ToString().Length);
                }
                Xpos += 40;
            }

            if (term.GetDevidedTerms() != null) {
                if (term.GetDevidedTerms().Count > 0) {

                    TextInstantiator.InstantiateText(FirstNumPlace, "÷(", Xpos + 15, Ypos, 0, false, 1, 0);
                    Xpos += 15;

                    yield return WriteTerms(term.GetDevidedTerms(), Xpos + 110, Ypos, FirstNumPlace, Explain, monoBehaviour, Line,false, Iteration,false);

                    foreach (var item in term.GetDevidedTerms())
                    {
                        Xpos += (100 * item.GetNumber().ToString().Length);
                    }
                    TextInstantiator.InstantiateText(FirstNumPlace, ")", Xpos, Ypos, 0, false, 1, 0);
                    Xpos += 40;
                }

            }

            if (term.GetBracket() != null) {
                TextInstantiator.InstantiateText(FirstNumPlace, "×(", Xpos+20 , Ypos, 0, false, 1, 0);

                yield return WriteTerms(term.GetBracket().GetTerms(), Xpos+35 + 35 * (term.GetBracket().GetTerms()[0].GetNumber().ToString().Length ), Ypos,FirstNumPlace,Explain,monoBehaviour,Line,true, Iteration,false);
                foreach (var item in term.GetBracket().GetTerms())
                {
                    Xpos += (60 +60 * item.GetNumber().ToString().Length);
                }
                TextInstantiator.InstantiateText(FirstNumPlace, ")", Xpos, Ypos, 0, false, 1, 0);
            }

            Xpos += 75;

            counter++;
        }
    }
}
