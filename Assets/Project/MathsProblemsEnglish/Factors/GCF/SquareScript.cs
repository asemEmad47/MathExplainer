using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SquareScript : MonoBehaviour
{
    public static void InstatiateSquare(float XAxis, float YAxis , GameObject Square , TextMeshProUGUI FirstNumPlace)
    {
        GameObject newSquare = GameObject.Instantiate(Square);
        newSquare.SetActive(true);
        CharacterProbs.CenterInPos(XAxis, YAxis + 120, ref newSquare, FirstNumPlace);
    }
    public static IEnumerator SquarePairs(List<float> FirstNumList, List<float> SecNumList , GameObject Square, TextMeshProUGUI FirstNumPlace)
    {

        float Xval = PrimeFactors.XOffset - 480;
        int counter = GCFListOperaions.CompareListsAndCount(FirstNumList, SecNumList);
        for (int i = 0; i < counter; i++)
        {
            InstatiateSquare(Xval, PrimeFactors.CurrentY , Square , FirstNumPlace);
            yield return new WaitForSeconds(1f);
            Xval += 180;
        }
    }
}
