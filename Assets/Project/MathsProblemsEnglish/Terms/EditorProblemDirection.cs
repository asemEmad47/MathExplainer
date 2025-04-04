
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorProblemDirection : MonoBehaviour
{
    public static string Problem;
    public static ProblemSolver ChooseProblem()
    {
        switch (Problem)
        {
            case "simplify":
                return new MathEditorSimplfy();
            default:
                break;
        }
        return null;
    }
    public void MakeItSimplify() {
        Problem = "simplify";
        SceneManager.LoadScene("TermsScene");
    }  
}
