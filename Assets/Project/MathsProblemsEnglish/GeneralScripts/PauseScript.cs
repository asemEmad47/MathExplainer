using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    private static bool isPaused = false;

    public static void Pause()
    {
        isPaused = true;
    }
    public static void Resume()
    {
        isPaused = false;
    }
    public static void ControlPause()
    {
        Button ExplainBtn = GameObject.Find("Explain").GetComponent<Button>();
        Button SolveBtn = GameObject.Find("Solve").GetComponent<Button>();

        if(!ExplainBtn.interactable &&!SolveBtn.interactable)
        {
            if (isPaused)
            {
                Time.timeScale = 0;

                GameObject.Find("Pause").GetComponent<Image>().enabled = false;
                GameObject.Find("Resume").GetComponent<Image>().enabled = true;
            }
            else
            {
                Time.timeScale = 1;
                GameObject.Find("Pause").GetComponent<Image>().enabled = true;
                GameObject.Find("Resume").GetComponent<Image>().enabled = false;
            }
        }
        else
        {
            GameObject.Find("Pause").GetComponent<Image>().enabled = false;
            GameObject.Find("Resume").GetComponent<Image>().enabled = false;
        }
    }
}
