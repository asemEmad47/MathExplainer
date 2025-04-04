using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResetValues : MonoBehaviour
{
    public static void ResetAllValues()
    {
        DelGameObjsWithName(new List<string> { "Clone" });


        TextMeshProUGUI[] textMeshPros = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textMeshPro in textMeshPros)
        {
            if (int.TryParse(textMeshPro.name , out _))
            {
                Destroy(textMeshPro.gameObject);
            }
            else if(textMeshPro.name.Contains("Clone"))
            {
                Destroy(textMeshPro.gameObject);
            }
        }
    }   
    public static void ResetAllValuesLongDiv()
    {
        ResetAllValues();
        DelGameObjsWithName(new List<string> { "HandeldZero", "LINEI" , "FrstNumPlaceCpy" });
    }
    public static void GCFDelElements()
    {
        GameObject[] CirclesAndLines = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in CirclesAndLines)
        {
            if (obj.name.Contains("Circle") && !obj.name.Equals("Circle"))
            {
                Destroy(obj);
            }
            else if (obj.name.Contains("Line") && !obj.name.Equals("Line2") && !obj.name.Equals("Line3") && !obj.name.Equals("Line"))
            {
                Destroy(obj);

            }
            else if (obj.name.Contains("Square") && !obj.name.Equals("Square"))
            {
                Destroy(obj);
            }
            else if (obj.name.Contains("InputField"))
            {
                Destroy(obj);
            }
        }
    }
    public static void DelGameObjsWithName(List<string> ObjsList)
    {
        GameObject[] CirclesAndLines = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in CirclesAndLines)
        {
            foreach (string name in ObjsList)
            {
                if (obj.name.Contains(name))
                {
                    Destroy(obj);
                }
            }

        }
    }

}
