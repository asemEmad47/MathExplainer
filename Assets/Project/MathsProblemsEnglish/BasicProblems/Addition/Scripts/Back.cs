using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicBack : MonoBehaviour
{
    public void Back(string SceneName)
    {

        SceneManager.LoadScene(SceneName);

    }
    public void LoadTwoDigits()
    {
        if (TwoDigitsMultiplicationScript.IsCalledFromOutSide == true)
        {
            SceneManager.LoadScene("DecimaMainScene");

        }
        else
            SceneManager.LoadScene("BasicOpScene");

    }
}
