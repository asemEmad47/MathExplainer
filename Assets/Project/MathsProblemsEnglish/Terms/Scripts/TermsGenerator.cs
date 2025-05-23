using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TermsGenerator : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject Keyboard;
    private TouchScreenKeyboard m_keyboard;
    private void Awake()
    {
        m_keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false);
        inputField = GetComponent<TMP_InputField>();
    }
    void Start()
    {

        // Set up event listeners for when the input field is selected and deselected
        inputField.onSelect.AddListener(OnSelect);
        inputField.onDeselect.AddListener(OnDeselect);
    }
    public void OnSelect(string value)
    {
        // Disable default Unity keyboard when input field is selected
        TouchScreenKeyboard.hideInput = true;
        inputField.GetComponent<TMP_InputField>().ActivateInputField();
        Keyboard.SetActive(true);
    }

    public void OnDeselect(string value)
    {
        // Re-enable default Unity keyboard when input field is deselected
        TouchScreenKeyboard.hideInput = false;

    }
    public void Hidekeyboard()
    {
        Keyboard.SetActive(false);
    }
}
