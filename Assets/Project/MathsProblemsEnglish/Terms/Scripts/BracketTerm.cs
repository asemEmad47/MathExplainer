using System.Collections.Generic;

public class BracketTerms 
{
    private List<Term> terms = new List<Term>();
    private int BracketPower = 1;

    public BracketTerms() { }

    public BracketTerms(List<Term> terms, int BracketPower)
    {
        this.BracketPower = BracketPower;
        this.terms = terms;
    }

    public List<Term> GetTerms() { return terms; }
    public int GetPower() { return BracketPower; }
    public void AddNewTerm(Term term) { terms.Add(term); }
}
