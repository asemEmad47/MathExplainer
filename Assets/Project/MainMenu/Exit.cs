using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public GameObject CurrentPanel;
    public List<Button> MenueButtons;
    public Button YesBtn;
    public Button NoBtn;

    public void ExitAction()
    {
        CurrentPanel.SetActive(true);
        foreach (Button button in MenueButtons)
        {
            button.interactable = false;
        }
        YesBtn.onClick.RemoveAllListeners();
        YesBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });


        NoBtn.onClick.RemoveAllListeners(); 
        NoBtn.onClick.AddListener(() =>
        {
            foreach (Button button in MenueButtons)
            {
                button.interactable = true;
            }
            CurrentPanel.SetActive(false);
        });
    }
    public void Update() {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ExitAction();
        }
    }
}
