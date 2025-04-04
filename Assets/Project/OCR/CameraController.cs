using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    public RawImage rawImage;
    private WebCamTexture webCamTexture;
    public Vector2 RawImageSIze;
    public Button captureButton;
    public Button ApiCaller;
    public Button CropButton;
    public Canvas canvas;
    public GameObject problemRedirectionObj;
    private string subscriptionKey = "10EjCJg9heuN8o0k4RBXD0Qp9sjfdlmqfAyL6L7aA7q93f49sFJXJQQJ99ALACYeBjFXJ3w3AAAFACOG2ukA"; // Replace with your actual subscription key
    private string endpoint = "https://vmresource2.cognitiveservices.azure.com/"; // Replace with your actual endpoint URL
    private byte[] imageBytes;
    void Start()
    {
        RawImageSIze = new Vector2(Screen.width, Screen.height);
        webCamTexture = new WebCamTexture();
        rawImage.texture = webCamTexture;
        rawImage.material.mainTexture = webCamTexture;
        webCamTexture.Play();
        captureButton.onClick.AddListener(CapturePhoto);
        ApiCaller.onClick.AddListener(SendImage);
    }

    public void CapturePhoto()
    {
        // Save the current camera frame as a Texture2D
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        // Convert photo to byte array (PNG format)
        imageBytes = photo.EncodeToPNG();

        // Start the OCR process
        rawImage.texture = photo;
        webCamTexture.Stop();
        CropButton.gameObject.SetActive(true);
        ApiCaller.gameObject.SetActive(true);
        captureButton.gameObject.SetActive(false);
    }

    public void SendImage()
    {
        StartCoroutine(ExtractTextUsingAzure(imageBytes));
    }
    private IEnumerator ExtractTextUsingAzure(byte[] imageBytes)
    {
        CropButton.gameObject.SetActive(false);
        ApiCaller.gameObject.SetActive(false);
        captureButton.interactable = false;
        string url = endpoint + "/vision/v3.2/ocr"; // OCR endpoint URL

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(imageBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/octet-stream");
        request.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response
            string jsonResponse = request.downloadHandler.text;

            // Extract the text from the response
            string extractedText = ParseOCRResponse(jsonResponse);

            ProblemRedirection.Text = extractedText;
            ProblemRedirection problemRedirection = problemRedirectionObj.GetComponent<ProblemRedirection>();
            if (!problemRedirection.GetProblemPage())
            {
                CropButton.gameObject.SetActive(false);
                ApiCaller.gameObject.SetActive(false);
                captureButton.gameObject.SetActive(true);
                rawImage.GetComponent<RectTransform>().sizeDelta = RawImageSIze;
                webCamTexture = new();
                rawImage.texture = webCamTexture;
                webCamTexture.Play();
            }
            else
            {
                Destroy(rawImage);
                Destroy(webCamTexture);
            }

        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
        captureButton.interactable = true;
    }

    private string ParseOCRResponse(string jsonResponse)
    {
        var response = JsonUtility.FromJson<OCRResponse>(jsonResponse);
        string extractedText = "";

        // Check if the response contains valid regions
        if (response.regions != null)
        {
            foreach (var region in response.regions)
            {
                if (region.lines != null)
                {
                    foreach (var line in region.lines)
                    {
                        if (line.words != null)
                        {
                            foreach (var word in line.words)
                            {
                                extractedText += word.text + " ";
                            }
                        }
                        extractedText += "\n";
                    }
                }
            }
        }

        return extractedText;
    }

}

[System.Serializable]
public class OCRResponse
{
    public OCRRegion[] regions;
}

[System.Serializable]
public class OCRRegion
{
    public OCRLine[] lines;
}

[System.Serializable]
public class OCRLine
{
    public OCRWord[] words;
}

[System.Serializable]
public class OCRWord
{
    public string text;
}
