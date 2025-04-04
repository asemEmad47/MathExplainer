using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicBack : MonoBehaviour
{
    [SerializeField] private string SceneName;
    public void Back()
    {
        SceneManager.LoadScene(SceneName);
    }
    public void LoadTwoDigits()
    {
        if (!AdditionScript.IsBasic)
        {
            SceneManager.LoadScene("DecimaMainScene");

        }
        else
            SceneManager.LoadScene("BasicOpScene");

    }
}
