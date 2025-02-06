using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DecimalDirector : MonoBehaviour
{
    public void Add()
    {
        AdditionScript.IsBasic = false;
        SceneManager.LoadScene("DecimalScene");
        PlayerPrefs.SetString("type", "add");
    }
    public void Sub()
    {
        AdditionScript.IsBasic = false;
        SceneManager.LoadScene("DecimalScene");
        PlayerPrefs.SetString("type", "sub");
    }

    public void Multiply()
    {
        AdditionScript.IsBasic = false;
        SceneManager.LoadScene("TwoDigitsMultiplicationScene");
    }

    public void Devide()
    {
        AdditionScript.IsBasic = false;
        SceneManager.LoadScene("DecimalScene");
        PlayerPrefs.SetString("type", "division");
    }

    public void LCM()
    {
        PlayerPrefs.SetString("type", "LCM");
        SceneManager.LoadScene("GCF");
    }    
    public void GCF()
    {
        PlayerPrefs.SetString("type", "GCF");
        SceneManager.LoadScene("GCF");
    }
    private void Awake()
    {
        GCFScript.IsCalledFromOutside = false;
        AdditionScript.IscalledFromOutSide = false;
        SubtractionScript.IscalledFromOutSide = false;
        TwoDigitsMultiplicationScript.IsCalledFromOutSide = false;
        OneDigitMultiplicationScript.IscalledFromOutSide = false;
        GCFScript.IsCalledFromOutside = false;
        AdditionScript.IsBasic = true;
    }

}
