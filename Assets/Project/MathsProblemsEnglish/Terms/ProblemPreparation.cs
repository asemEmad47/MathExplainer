using System;
using System.Collections.Generic;

public static class ProblemPreparation
{
    public static void OrderTerms(ref List<Term> terms)
    {
        terms.Sort((x, y) => string.Compare(x.GetSymbol(), y.GetSymbol(), StringComparison.Ordinal));
    } 
    

    public static void PrepareTerms(ref List<Term> terms) {
        foreach (Term term in terms) {

            if (term.GetBracket() != null)
            {

                foreach (var BracketTerm in term.GetMultipliedTerms())
                {
                    PrepareTermNumbers(BracketTerm);
                }
            }

            if(term.GetMultipliedTerms() != null)
            {
                foreach (var MutlipliedTerm in term.GetMultipliedTerms())
                {
                    PrepareTermNumbers(MutlipliedTerm);
                }
            }

            if(term.GetDevidedTerms() != null)
            {
                foreach (var DividedTerm in term.GetDevidedTerms())
                {
                    PrepareTermNumbers(DividedTerm);
                }
            }
        }
    }

    public static void PrepareTermNumbers(Term term) {

        if (term.GetNumber() == 0)
        {
            term.SetNumber(1);
        }

        if (term.GetDeno() == 0 && term.GetNue() == 0) {
            term.SetNue(1);
            term.SetDeno(1);
        }
    }

}
