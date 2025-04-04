using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExplainEnableMent : MonoBehaviour
{
    public static void EnableExplain(ref List<TMP_InputField> AllFields)
    {
        GameObject Explain = GameObject.Find("Explain");
        GameObject Solve = GameObject.Find("Solve");
        Button ExplainBtn = Explain.GetComponent<Button>();
        Button SolveBtn = Solve.GetComponent<Button>();
        bool IsFinshed = true;
        try
        {
            foreach (TMP_InputField field in AllFields) {
                bool IsValidNum = float.TryParse(field.text, out float Fnum);
                if (!IsValidNum) {
                    IsFinshed = false ;
                    SolveBtn.interactable = false ;
                    ExplainBtn.interactable = false;
                    break;
                }
            }
            if (IsFinshed) {
                SolveBtn.interactable = true;
                ExplainBtn.interactable = true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    public static void DisableExplain(bool InExplain)
    {
        GameObject Explain = GameObject.Find("Explain");
        GameObject Solve = GameObject.Find("Solve");
        Button ExplainBtn = Explain.GetComponent<Button>();
        Button SolveBtn = Solve.GetComponent<Button>();
        if (InExplain)
        {
            SolveBtn.interactable = false;
            ExplainBtn.interactable = false;
        }
        else
        {
            SolveBtn.interactable = true;
            ExplainBtn.interactable = true;
        }
    }
}
