using System.Collections.Generic;

public class Term 
{
    private string Symbol;
    private  char sign;
    private  int number = 0;

    private int nue;
    private int Deno;
    private string NumPow = "";
    private string SymbPow = "";
    private  List<Term> MutliPlyedTerm;
    private  List<Term> DevidedTerm;
    private  BracketTerms bracketTerm;

    private List<string> SympPowers;
    private List<string> NumPowers;
    public Term(string symbol, char sign , int nue = 0 , int deno = 0, int number = 0,string NumPow = "",string SymbPow = "" , List<Term> MultiplyedOnes = null , List<Term> DividedTerms = null , BracketTerms bracketTerms = null)
    {
        this.Symbol = symbol;
        this.number = number;
        this.sign = sign;
        this.nue = nue;
        this.Deno = deno;
        this.NumPow = NumPow;
        this.SymbPow = SymbPow;
        this.MutliPlyedTerm = MultiplyedOnes;
        this.DevidedTerm = DividedTerms;
        this.bracketTerm = bracketTerms;
    }

    public string GetSymbol()
    {
        return Symbol;
    }

    public int GetNumber()
    {
        return number;
    }   
    public int GetDeno()
    {
        return Deno;
    }   
    public int GetNue()
    {
        return nue;
    }

    public char GetSign()
    {
        return sign;
    }

    public string GetNumPow()
    {
        return NumPow;
    }    
    public string GetSymbPow()
    {
        return SymbPow;
    }
    public List<Term> GetMultipliedTerms()
    {
        return MutliPlyedTerm;
    }
    public List<Term> GetDevidedTerms()
    {
        return DevidedTerm;
    }
    public BracketTerms GetBracket()
    {
        return bracketTerm;
    }
    public void SetSign(char sign)
    {
        this.sign = sign;
    }   
    
    public void SetNumber(int number)
    {
        this.number = number;
    } 
    
    public void SetDeno(int deno)
    {
        this.Deno = deno;
    }
    public void SetNue(int nue)
    {
        this.nue = nue;
    }
    public void AddBracket(BracketTerms BracketTerm)
    {
        this.bracketTerm = BracketTerm;
    }   
    public void AddMutlipliedTerm(List<Term> MultiplyedOnes)
    {
        this.MutliPlyedTerm = MultiplyedOnes;
    }

    public void AddSympPow(string SympPow)
    {
        if (SympPowers == null)
        {
            SympPowers = new List<string>();
        }
        SympPowers.Add(SympPow);
    }

    public void AddNumPow(string NumPow)
    {
        if (NumPowers == null)
        {
            NumPowers = new List<string>();
        }
        NumPowers.Add(NumPow);
    }
}
