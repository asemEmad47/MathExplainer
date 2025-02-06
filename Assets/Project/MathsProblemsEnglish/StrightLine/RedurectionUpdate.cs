using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RedurectionUpdate : MonoBehaviour
{

    public List<GameObject> ProblemList;

    public static List<bool> ProblemBools;

    public static bool IsCalledFromOutSide = false;

    void Update()
    {
        if (IsCalledFromOutSide)
        {

            for (int i = 0; i < ProblemList.Count; i++)
            {
                if (ProblemBools[i])
                {
                    ProblemList[i].SetActive(true);
                }
                else
                {
                    ProblemList[i].SetActive(false);
                }
            }
            IsCalledFromOutSide = false;
            ProblemBools = null;
        }
    }
}
