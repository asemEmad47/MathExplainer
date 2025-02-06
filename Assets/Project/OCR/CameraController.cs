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
    public Button captureButton;
    public Canvas canvas;
    public GameObject problemRedirectionObj;
    private string subscriptionKey = "10EjCJg9heuN8o0k4RBXD0Qp9sjfdlmqfAyL6L7aA7q93f49sFJXJQQJ99ALACYeBjFXJ3w3AAAFACOG2ukA"; // Replace with your actual subscription key
    private string endpoint = "https://vmresource2.cognitiveservices.azure.com/"; // Replace with your actual endpoint URL

    void Start()
    {
        // Initialize camera (same as your previous code)
        webCamTexture = new WebCamTexture();
        rawImage.texture = webCamTexture;
        rawImage.material.mainTexture = webCamTexture;
        webCamTexture.Play();
    }

    public void CapturePhoto()
    {
        // Save the current camera frame as a Texture2D
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        // Convert photo to byte array (PNG format)
        byte[] imageBytes = photo.EncodeToPNG();

        // Start the OCR process
        webCamTexture.Stop();

        StartCoroutine(ExtractTextUsingAzure(imageBytes));

    }


    private IEnumerator ExtractTextUsingAzure(byte[] imageBytes)
    {
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


    void OnDestroy()
    {
        webCamTexture.Stop();
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
