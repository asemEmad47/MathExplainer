using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class IntroScript : MonoBehaviour
{
    [SerializeField] private GameObject overlayImage;
    [SerializeField] private TextMeshProUGUI textMesh;
    public float transitionDuration = 3f;
    public float textMovementDuration = 2f; // Duration for text movement
    public float startDelay = 2f;
    private float elapsedTime = 0f;
    private float textElapsedTime = 0f;
    private Vector2 startPosAnchor;
    private Vector2 endPosAnchor;

    private void Awake()
    {
        // Ensure this object is not destroyed when loading new scenes
        DontDestroyOnLoad(this.gameObject);

        // Set the frame rate globally
        UnityEngine.Application.targetFrameRate = 144;
    }
    void Start()
    {
        // Set initial anchor positions for the text
        RectTransform textRectTransform = textMesh.GetComponent<RectTransform>();

        startPosAnchor = new Vector2(1.5f, textRectTransform.anchorMin.y); // Start outside the right of the screen
        endPosAnchor = new Vector2(0.5f, textRectTransform.anchorMin.y);   // Center of the screen

        // Set the starting anchor position
        textRectTransform.anchorMin = startPosAnchor;
        textRectTransform.anchorMax = startPosAnchor;

        // Hide the text at the beginning
        textMesh.gameObject.SetActive(false);

        StartCoroutine(Transition());
    }

    public IEnumerator Transition()
    {
        yield return new WaitForSeconds(startDelay);

        // Show the text
        textMesh.gameObject.SetActive(true);

        // Gradually move the text from the right to the center of the screen using anchor positions
        RectTransform textRectTransform = textMesh.GetComponent<RectTransform>();
        while (textElapsedTime < textMovementDuration)
        {
            float t = textElapsedTime / textMovementDuration;
            Vector2 currentAnchorPos = Vector2.Lerp(startPosAnchor, endPosAnchor, t);
            textRectTransform.anchorMin = currentAnchorPos;
            textRectTransform.anchorMax = currentAnchorPos;

            textElapsedTime += Time.deltaTime;
            yield return null;
        }

        // Gradually increase overlay opacity over transitionDuration seconds
        while (elapsedTime < transitionDuration)
        {
            float alpha = Mathf.Lerp(0.0001f, 1f, elapsedTime / transitionDuration);
            overlayImage.GetComponent<UnityEngine.UI.Image>().color = new Color(overlayImage.GetComponent<UnityEngine.UI.Image>().color.r, overlayImage.GetComponent<UnityEngine.UI.Image>().color.g, overlayImage.GetComponent<UnityEngine.UI.Image>().color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Project/MainMenu/PageDitector/DirectorScene");
    }
}
