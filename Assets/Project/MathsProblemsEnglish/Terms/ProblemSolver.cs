using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public interface ProblemSolver 
{
    IEnumerator Solve(List<Term> terms);
    void SetComponents(TextMeshProUGUI FirstNumPlace, GameObject Line, float Xpos, float Ypos, bool Explain , MonoBehaviour monoBehaviour);
}
