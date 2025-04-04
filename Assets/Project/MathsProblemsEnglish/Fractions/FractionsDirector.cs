using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FractionsDirector : MonoBehaviour
{
    [SerializeField] string SceneName;
    public void DirectToAdding()
    {
        AddingTwoFractionsScript.IsCalledFromOutSide = false;
        SceneManager.LoadScene(SceneName);
    }    
    public void DirectToSubtracting()
    {
        AddingTwoFractionsScript.IsCalledFromOutSide = true;
        SceneManager.LoadScene(SceneName);
    }    
    public void DirectToMutliply()
    {
        MultiplyingTwoFractionsScript.IsCalledFromOutSide = false;
        SceneManager.LoadScene(SceneName);
    }   
    public void DirectToDivide()
    {
        MultiplyingTwoFractionsScript.IsCalledFromOutSide = true;
        SceneManager.LoadScene(SceneName);
    }
}
