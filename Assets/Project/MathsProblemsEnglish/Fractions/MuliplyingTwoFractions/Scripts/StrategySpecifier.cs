using System.Collections;
using TMPro;
using UnityEngine;

public class StrategySpecifier : MonoBehaviour
{
    private float XPos;
    private float YPos;

    private int FNue;
    private int FDeno;
    private int SNue;
    private int SDeno;   
    
    private string FNueName;
    private string FDenoName;
    private string SNueName;
    private string SDenoName;

    private bool Explain;
    private TextMeshProUGUI FirstNumPlace;

    private GameObject Square;
    private GameObject Remove;
    public void SetComponents(float XPos , float YPos , int Fnue ,  int FDeno , int SNue , int SDeno,GameObject Square ,GameObject Remove  , TextMeshProUGUI FirstNumPlace , bool Explain, string FNueName, string FDenoName, string SNueName, string SDenoName)
    {
        this.XPos = XPos;
        this.YPos = YPos;
        this.FNue = Fnue;
        this.FDeno = FDeno;
        this.SNue = SNue;
        this.SDeno = SDeno;
        this.Square = Square;
        this.FirstNumPlace = FirstNumPlace;
        this.Explain = Explain;
        this.Remove = Remove;

        this.SDenoName = SDenoName;
        this.FNueName = FNueName;
        this.FDenoName = FDenoName;
        this.SNueName = SNueName;
    }
    public IEnumerator TakeStrategy(int Div1 , int Div2 , bool IsFirstMethod)
    {
        if (Div1!= 1){

            yield return StartCoroutine(AnswerByFirstStrategy(true , Div1));

        }

        if (Div2!= 1)
        {
            yield return StartCoroutine(AnswerByFirstStrategy(false , Div2));

        }
        if (Div2 == 1 && Div1 ==1)
        {
            if (!IsFirstMethod)
            {
                yield return StartCoroutine(AnswerBySecondStrategy(true, SimplifyScript.GetDivisiorBySecondMethod(FNue, FDeno)));
                yield return StartCoroutine(AnswerBySecondStrategy(false, SimplifyScript.GetDivisiorBySecondMethod(SNue, SDeno)));
            }
            else
            {
                yield return StartCoroutine(AnswerBySecondStrategy(true, SimplifyScript.GetDivisiorByFirstMethod(FNue, FDeno)));
                yield return StartCoroutine(AnswerBySecondStrategy(false, SimplifyScript.GetDivisiorByFirstMethod(SNue, SDeno)));
            }
        }
    }
    public IEnumerator AnswerByFirstStrategy(bool IsFirst , int Divisor )
    {
        GameObject FirstNueSquare;
        GameObject SecondDenoSquare;
        TMP_CharacterInfo charInfo = new();
        if (IsFirst)
        {


            FirstNueSquare = ObjectsInstantiation.InstantiateOBJ(XPos + 80, YPos - 10, Square, FirstNumPlace);
            FirstNueSquare.transform.rotation = Quaternion.Euler(0, 0, 65);

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "take" + SLStaicFunctions.SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FNue.ToString(), Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "with" + SLStaicFunctions.SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SDeno.ToString(), Explain)));


            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and divide them by" + SLStaicFunctions.SpeakerName, Explain)));


            yield return new WaitForSeconds(2f);


            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Divisor.ToString(), Explain)));


            TextMeshProUGUI FNueTxt = TextInstantiator.InstantiateText(FirstNumPlace, "÷ "+Divisor, XPos + 40 + 20*FNue.ToString().Length, YPos, 30, true, -1);
            TextMeshProUGUI SDenoTxt = TextInstantiator.InstantiateText(FirstNumPlace, "÷ "+Divisor, XPos + 250 + 20 * SDeno.ToString().Length, YPos, -50, true, -1);

            FNueTxt.fontSize = GameObject.Find(FNueName).GetComponent<TextMeshProUGUI>().fontSize-15;
            SDenoTxt.fontSize = GameObject.Find(SDenoName).GetComponent<TextMeshProUGUI>().fontSize-15;

            Destroy(FirstNueSquare);

            yield return StartCoroutine(DivNueDeno.Div(GameObject.Find(FNueName).GetComponent<TextMeshProUGUI>(), FNueTxt, this,Remove, Explain,FirstNumPlace , 20 , new Vector3(XPos + 20,YPos,0)));

            yield return StartCoroutine(DivNueDeno.Div(GameObject.Find(SDenoName).GetComponent<TextMeshProUGUI>(), SDenoTxt, this, Remove, Explain, FirstNumPlace,-60, new Vector3(XPos+230, YPos, 0)));

            FNue/=Divisor;
            SDeno/=Divisor;
        }
        else
        {

            SecondDenoSquare = ObjectsInstantiation.InstantiateOBJ(XPos + 80, YPos - 20, Square, FirstNumPlace);
            SecondDenoSquare.transform.rotation = Quaternion.Euler(0, 0, -65);

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "take" + SLStaicFunctions.SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FDeno.ToString(), Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "with" + SLStaicFunctions.SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SNue.ToString(), Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and divide them by" + SLStaicFunctions.SpeakerName, Explain)));


            yield return new WaitForSeconds(2f);

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Divisor.ToString(), Explain)));




            TextMeshProUGUI FDenoTxt = TextInstantiator.InstantiateText(FirstNumPlace, "÷ " + Divisor, XPos + 40 + 20 * FDeno.ToString().Length, YPos, -50, true, -1);
            TextMeshProUGUI SNueTxt = TextInstantiator.InstantiateText(FirstNumPlace, "÷ " + Divisor, XPos + 250 + 20 * SNue.ToString().Length, YPos, 30, true, -1);

            FDenoTxt.fontSize = GameObject.Find(FDenoName).GetComponent<TextMeshProUGUI>().fontSize-15;
            SNueTxt.fontSize = GameObject.Find(SNueName).GetComponent<TextMeshProUGUI>().fontSize-15;

            Destroy(SecondDenoSquare);

            yield return StartCoroutine(DivNueDeno.Div(GameObject.Find(FDenoName).GetComponent<TextMeshProUGUI>(), FDenoTxt, this, Remove, Explain, FirstNumPlace, -60, new Vector3(XPos + 20, YPos, 0)));


            yield return StartCoroutine(DivNueDeno.Div(GameObject.Find(SNueName).GetComponent<TextMeshProUGUI>(), SNueTxt, this, Remove, Explain, FirstNumPlace,20, new Vector3(XPos + 230, YPos, 0)));

            FDeno /= Divisor;
            SNue /= Divisor;
        }
    }    
    public IEnumerator AnswerBySecondStrategy(bool IsFirst, int Divisor)
    {
        GameObject FirstNueSquare;
        GameObject SecondDenoSquare;

        TMP_CharacterInfo charInfo = new();
        if (IsFirst && Divisor !=1)
        {


            FirstNueSquare = ObjectsInstantiation.InstantiateOBJ(XPos, YPos - 10, Square, FirstNumPlace);

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "take" + SLStaicFunctions.SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FNue.ToString(), Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "with" + SLStaicFunctions.SpeakerName, Explain)));

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, FDeno.ToString(), Explain)));


            yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and divide them by" + SLStaicFunctions.SpeakerName, Explain)));

            yield return new WaitForSeconds(2f);

            yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Divisor.ToString(), Explain)));

            TextMeshProUGUI FNueTxt= TextInstantiator.InstantiateText(FirstNumPlace, "÷ " + Divisor, XPos + 40 + 20 * FNue.ToString().Length, YPos, 30, true, -1);
            TextMeshProUGUI FDenoTxt = TextInstantiator.InstantiateText(FirstNumPlace, "÷ " + Divisor, XPos + 40 + 20 * FDeno.ToString().Length, YPos, -50, true, -1);

            FNueTxt.fontSize = GameObject.Find(FNueName).GetComponent<TextMeshProUGUI>().fontSize - 15;
            FDenoTxt.fontSize = GameObject.Find(FDenoName).GetComponent<TextMeshProUGUI>().fontSize - 15;

            Destroy(FirstNueSquare);

            yield return StartCoroutine(DivNueDeno.Div(GameObject.Find(FNueName).GetComponent<TextMeshProUGUI>(), FNueTxt, this, Remove, Explain, FirstNumPlace, 20, new Vector3(XPos + 20, YPos, 0)));

            yield return StartCoroutine(DivNueDeno.Div(GameObject.Find(FDenoName).GetComponent<TextMeshProUGUI>(), FDenoTxt, this, Remove, Explain, FirstNumPlace, -60, new Vector3(XPos + 20, YPos, 0)));

            FNue /= Divisor;
            FDeno/=Divisor;
        }
        else
        {
            if (Divisor != 1)
            {

                SecondDenoSquare = ObjectsInstantiation.InstantiateOBJ(XPos + 200, YPos - 20, Square, FirstNumPlace);

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "take" + SLStaicFunctions.SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SNue.ToString(), Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "with" + SLStaicFunctions.SpeakerName, Explain)));

                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, SDeno.ToString(), Explain)));


                yield return (StartCoroutine(SLStaicFunctions.PlayByAddress(this, "and divide them by" + SLStaicFunctions.SpeakerName, Explain)));

                yield return new WaitForSeconds(2f);


                yield return (StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(this, Divisor.ToString(), Explain)));


                TextMeshProUGUI SNueTxt = TextInstantiator.InstantiateText(FirstNumPlace, "÷ " + Divisor, XPos + 250 + 20 * SNue.ToString().Length, YPos, 30, true, -1);
                TextMeshProUGUI SDenoTxt = TextInstantiator.InstantiateText(FirstNumPlace, "÷ " + Divisor, XPos + 250 + 20 * SDeno.ToString().Length, YPos, -50, true, -1);

                SNueTxt.fontSize = GameObject.Find(SNueName).GetComponent<TextMeshProUGUI>().fontSize - 15;
                SDenoTxt.fontSize = GameObject.Find(SDenoName).GetComponent<TextMeshProUGUI>().fontSize - 15;

                Destroy(SecondDenoSquare);
                yield return StartCoroutine(DivNueDeno.Div(GameObject.Find(SNueName).GetComponent<TextMeshProUGUI>(), SNueTxt, this, Remove, Explain, FirstNumPlace, 20, new Vector3(XPos + 230 , YPos, 0)));

                yield return StartCoroutine(DivNueDeno.Div(GameObject.Find(SDenoName).GetComponent<TextMeshProUGUI>(), SDenoTxt, this, Remove, Explain, FirstNumPlace, -60, new Vector3(XPos + 230 , YPos, 0)));

                SNue /= Divisor;
                SDeno /= Divisor;

            }
        }
    }
    public void GetNumbers(ref int Fnue, ref int FDeno, ref int SNue,ref int SDeno)
    {
        Fnue = this.FNue;
        FDeno = this.FDeno;
        SNue = this.SNue;
        SDeno = this.SDeno;
    }
}
