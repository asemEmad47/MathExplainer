using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Back : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject StrightLinePanel;

    public void BackBtnChk()
    {
        MenuPanel.SetActive(true);
        StrightLinePanel.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            MenuPanel.SetActive(true);
            StrightLinePanel.gameObject.SetActive(false);
        }
    }
}
