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
        TwoDigitsMultiplicationScript.IsCalledFromOutSide = true;
        SceneManager.LoadScene("TwoDigitsMultiplicationScene");
    }

    public void Devide()
    {
        AdditionScript.IsBasic = false;
        SceneManager.LoadScene("DecimalScene");
        PlayerPrefs.SetString("type", "division");
    }
}
