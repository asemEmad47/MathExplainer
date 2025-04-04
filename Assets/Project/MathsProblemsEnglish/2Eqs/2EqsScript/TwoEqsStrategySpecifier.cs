public class TwoEqsStrategySpecifier
{
    private string Y1;
    private string Y2;
    public TwoEqsStrategySpecifier(string Y1, string Y2)
    {
        this.Y1 = Y1;
        this.Y2 = Y2;
    }

    public bool IsAdding() { 
        int ConvertedY1 = int.Parse(ArabicEngConverter.ConvertToEngNumbers(this.Y1));
        int ConvertedY2 = int.Parse(ArabicEngConverter.ConvertToEngNumbers(this.Y2));
        if ((ConvertedY1 + ConvertedY2 == 0) ||    
            (
            (ConvertedY1 % ConvertedY2 ==0 || ConvertedY2 % ConvertedY1 == 0) && ( 
            (ConvertedY1 > 0 && ConvertedY2 < 0) ||(ConvertedY1 < 0 && ConvertedY2 > 0)))
            )
        {
            return true;
        }
        return false;
    }
}
