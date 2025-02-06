using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectorScript : MonoBehaviour
{
    [SerializeField] private string SceneName;
    public void Direct()
    {
        SceneManager.LoadScene(SceneName);
        AdditionScript.IscalledFromOutSide = false;
        OneDigitMultiplicationScript.IscalledFromOutSide = false;
    }
}
