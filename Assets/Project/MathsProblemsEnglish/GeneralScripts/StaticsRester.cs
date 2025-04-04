using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticsRester : MonoBehaviour
{
    public void Start()
    {
        AdditionScript.IscalledFromOutSide = false;
        SubtractionScript.IscalledFromOutSide = false;
        OneDigitMultiplicationScript.IscalledFromOutSide = false;
        TwoDigitsMultiplicationScript.IsCalledFromOutSide = false;
        PrimeFactors.IsCalledFromOutside = false;
        GCFScript.IsCalledFromOutside = false;
        AddingTwoFractionsScript.IsCalledFromOutSide = false ;
        DivisionScript.IscalledFromOutSide= false;
        MultiplyingTwoFractionsScript.IsCalledFromOutSide= false;
    }
}
