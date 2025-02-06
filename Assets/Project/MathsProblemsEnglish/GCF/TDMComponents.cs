using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TDMComponents : MonoBehaviour
{

    private TextMeshProUGUI FirstNumPlace;
    private TextMeshProUGUI SecNumPlace;
    private TextMeshProUGUI Line2;
    private TextMeshProUGUI AdditionLine;
    private TextMeshProUGUI Sign2;

    private TextMeshProUGUI FirstNumPlaceAddition;
    private TextMeshProUGUI SecNumPlaceAddition;

    public void SetComponents(TextMeshProUGUI FirstNumPlace , TextMeshProUGUI SecNumPlace  , TextMeshProUGUI Line2 , TextMeshProUGUI AdditionSign , TextMeshProUGUI Sign2  , TextMeshProUGUI FirstNumPlaceAddition , TextMeshProUGUI SecNumPlaceAddition)
    {
        this.FirstNumPlace = FirstNumPlace;
        this.SecNumPlace = SecNumPlace;
        this.Line2 = Line2;
        this.AdditionLine = AdditionSign;
        this.Sign2 = Sign2;

        this.FirstNumPlaceAddition = FirstNumPlaceAddition;
        this.SecNumPlaceAddition = SecNumPlaceAddition;
    }

    public void CreateTDMComponents(int finalAnswer, int i , List<string> FinalAnswer)
    {
        // Scale factor
        Vector3 scaleFactor = new Vector3(1.45f, 1.45f, 1f);  // Only scale X and Y, keep Z as 1

        // Create SecNumInputField, apply scale and make it active
        GameObject secNumObj = new GameObject("SecNumInputField" + i);
        TMP_InputField SecNumInputField = secNumObj.AddComponent<TMP_InputField>();
        SecNumInputField.transform.SetParent(FirstNumPlace.transform.parent);
        secNumObj.transform.localScale = scaleFactor;  // Apply scale
        secNumObj.SetActive(true);  // Make it active

        // Create FrstNumInputField, apply scale and make it active
        GameObject frstNumObj = new GameObject("FrstNumInputField" + i);
        TMP_InputField FrstNumInputField = frstNumObj.AddComponent<TMP_InputField>();
        FrstNumInputField.transform.SetParent(FirstNumPlace.transform.parent);
        frstNumObj.transform.localScale = scaleFactor;  // Apply scale
        frstNumObj.SetActive(true);  // Make it active

        // Instantiate FirstNumPlaceAddition, apply scale and make it active
        TextMeshProUGUI frstNumAddition = Instantiate(FirstNumPlaceAddition);
        frstNumAddition.name = "FirstNumPlaceAddition" + i;
        frstNumAddition.transform.SetParent(FirstNumPlace.transform.parent);
        frstNumAddition.transform.localScale = scaleFactor;  // Apply scale
        frstNumAddition.gameObject.SetActive(true);  // Make it active

        // Instantiate SecNumPlaceAddition, apply scale and make it active
        TextMeshProUGUI secNumAddition = Instantiate(SecNumPlaceAddition);
        secNumAddition.name = "SecNumPlaceAddition" + i;
        secNumAddition.transform.SetParent(FirstNumPlace.transform.parent);
        secNumAddition.transform.localScale = scaleFactor;  // Apply scale
        secNumAddition.gameObject.SetActive(true);  // Make it active

        // Instantiate FirstNumPlace, apply scale and make it active
        TextMeshProUGUI FNumPlace = Instantiate(FirstNumPlace);
        FNumPlace.name = "FirstNumPlace" + i;
        FNumPlace.transform.SetParent(FirstNumPlace.transform.parent);
        FNumPlace.transform.localScale = scaleFactor;  // Apply scale
        FNumPlace.gameObject.SetActive(true);  // Make it active

        // Instantiate SecNumPlace, apply scale and make it active
        TextMeshProUGUI secNumPlace = Instantiate(SecNumPlace);
        secNumPlace.name = "SecNumPlace" + i;
        secNumPlace.transform.SetParent(FirstNumPlace.transform.parent);
        secNumPlace.transform.localScale = scaleFactor;  // Apply scale
        secNumPlace.gameObject.SetActive(true);  // Make it active

        // Instantiate Line2, apply scale and make it active
        TextMeshProUGUI line2 = Instantiate(Line2);
        line2.name = "Line2" + i;
        line2.transform.SetParent(FirstNumPlace.transform.parent);
        line2.transform.localScale = new Vector3(5f, 3f, 1f);  // Apply scale
        line2.gameObject.SetActive(true);  // Make it active

        // Instantiate Sign2, apply scale and make it active
        TextMeshProUGUI sign2 = Instantiate(Sign2);
        sign2.name = "Sign2" + i;
        sign2.transform.SetParent(FirstNumPlace.transform.parent);
        sign2.transform.localScale = scaleFactor;  // Apply scale
        sign2.gameObject.SetActive(true);  // Make it active

        // Instantiate AdditionLine, apply scale and make it active
        TextMeshProUGUI additionLine = Instantiate(AdditionLine);
        additionLine.name = "AdditionLine" + i;
        additionLine.transform.SetParent(FirstNumPlace.transform.parent);
        additionLine.transform.localScale = scaleFactor;  // Apply scale
        additionLine.gameObject.SetActive(true);  // Make it active



        InitilizeComponents(SecNumInputField, FrstNumInputField, FNumPlace, frstNumAddition, secNumAddition, secNumPlace, line2, sign2, additionLine, i, finalAnswer , FinalAnswer);
    }

    private void InitilizeComponents(TMP_InputField SecNumInputField, TMP_InputField FrstNumInputField, TextMeshProUGUI FNumPlace,
        TextMeshProUGUI frstNumAddition, TextMeshProUGUI secNumAddition, TextMeshProUGUI secNumPlace, TextMeshProUGUI line2
        , TextMeshProUGUI sign2, TextMeshProUGUI additionLine, int i, int finalAnswer , List<string> FinalAnswer
        )
    {
        SecNumInputField.text = FinalAnswer[i].ToString();
        FrstNumInputField.text = finalAnswer.ToString();

        if (int.Parse(FinalAnswer[i].ToString()) > int.Parse(finalAnswer.ToString()))
        {
            (FrstNumInputField.text, SecNumInputField.text) = (SecNumInputField.text, FrstNumInputField.text);
        }

        FNumPlace.GetComponent<RectTransform>().anchoredPosition = new Vector2(PrimeFactors.XOffset - 100, PrimeFactors.CurrentY - 400);
        FNumPlace.text = "new text";
        TMP_TextInfo textInfo = FirstNumPlace.GetTextInfo(FNumPlace.text);
        TMP_CharacterInfo charInfo = textInfo.characterInfo[FNumPlace.text.Length - 2];
        Vector3 characterPosition = CharacterProbs.GetCharPoos(FNumPlace, charInfo, FNumPlace.text.Length - 2);


        secNumPlace.GetComponent<RectTransform>().anchoredPosition = new Vector3(characterPosition.x + 22, characterPosition.y - 120, 0);

        OneDigitMultiplicationScript.ResDistance = -330;
        OneDigitMultiplicationScript.SupDistance = 150;


        FNumPlace.ForceMeshUpdate();
        TwoDigitsMultiplicationScript.IsCalledFromOutSide = true;
        line2.GetComponent<RectTransform>().anchoredPosition = new Vector3(Line2.GetComponent<RectTransform>().anchoredPosition.x, FNumPlace.GetComponent<RectTransform>().anchoredPosition.y - 40, 0);
        line2.text = Line2.text;

        frstNumAddition.GetComponent<RectTransform>().anchoredPosition = new Vector3(line2.GetComponent<RectTransform>().anchoredPosition.x + 110, secNumAddition.GetComponent<RectTransform>().anchoredPosition.y - 600, 0);
        frstNumAddition.text = FirstNumPlaceAddition.text;

        secNumAddition.GetComponent<RectTransform>().anchoredPosition = new Vector3(line2.GetComponent<RectTransform>().anchoredPosition.x + 110, secNumPlace.GetComponent<RectTransform>().anchoredPosition.y - 700, 0);
        secNumAddition.text = SecNumPlaceAddition.text;

        sign2.GetComponent<RectTransform>().anchoredPosition = new Vector2(FirstNumPlaceAddition.GetComponent<RectTransform>().anchoredPosition.x - 130, line2.GetComponent<RectTransform>().anchoredPosition.y - 50);
        sign2.text = Sign2.text;

        additionLine.text = AdditionLine.text;
    }
}
