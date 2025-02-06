using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AJAnimationHandler : MonoBehaviour
{
    public static Animator AJAnimator;
    public static bool IsRunning;

    public static void SetAnimator(Animator AJAnimator2)
    {
        AJAnimator = AJAnimator2;
    }

    // Start this coroutine from an instance
    public static void StartCounting(int SecNumber, int FrstNumber, bool isAddition)
    {
        AJAnimationHandler instance = FindObjectOfType<AJAnimationHandler>();
        if (instance != null)
        {
            instance.StartCoroutine(instance.Count(SecNumber, FrstNumber , isAddition));
        }
        else
        {
            Debug.LogError("No instance of HandAnimationHandler found in the scene.");
        }
    }

    private IEnumerator Count(int SecNumber, int FrstNumber, bool isAddition)
    {

        yield return StartCoroutine(CountOnTwoHands(SecNumber, FrstNumber , isAddition ));
        IsRunning = false;
    }

    private IEnumerator CountOnTwoHands(int SecNumber, int FrstNumber , bool IsAddition)
    {
        GameObject AJ = GameObject.Find("Aj");
        if (!IsAddition)
        {
            (FrstNumber , SecNumber) = (SecNumber , FrstNumber);
            SecNumber-=FrstNumber;

        }
        for (int i = 1; i <= SecNumber; i++)
        {
            yield return (StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait((FrstNumber + i).ToString())));
            AJAnimator.SetTrigger($"Number{i}");

            GameObject ParentEffect = GameObject.Find("ParentEffect");
            Transform effect = ParentEffect.transform.Find("CFXR Hit D 3D (Yellow)");
            effect.gameObject.SetActive(true);
            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
            particleSystem.Play();

            yield return StartCoroutine(AnimateTextOpacity((FrstNumber + i).ToString()));
        }
    }

    private IEnumerator AnimateTextOpacity(string text)
    {
        // Find the EffectText GameObject
        GameObject effectTextObj = GameObject.Find("EffectText");
        if (effectTextObj != null)
        {
            // Get the TextMeshProUGUI component
            TMPro.TextMeshProUGUI effectText = effectTextObj.GetComponent<TMPro.TextMeshProUGUI>();
            effectText.text = text;
            if (effectText != null)
            {
                // Ensure that the TextMeshPro component is enabled
                effectText.enabled = true;

                // Animate the opacity from 0 to 1 over 1.5 seconds
                float duration = 1.5f;
                float elapsedTime = 0f;
                Color originalColor = effectText.color;

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    // Use Mathf.Lerp to calculate the new opacity
                    float newOpacity = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                    // Update the color of the text with the new opacity
                    effectText.color = new Color(originalColor.r, originalColor.g, originalColor.b, newOpacity);

                    yield return null; // Wait for the next frame
                }

                // Ensure the opacity ends fully at 100%
                effectText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            }
            effectText.enabled = false;

        }
        else
        {
            Debug.LogError("EffectText object not found!");
        }
    }
    public static IEnumerator AnimateAJ(bool IsCarried, string FNum, string SNum, bool IsAddition)
    {
        GameObject AJ = GameObject.Find("Aj");


        // Find the child object named 'Hand_rigged' in LeftHand and RightHand
        Transform Body = AJ.transform.Find("Boy01_Body_Geo");
        Transform Brows = AJ.transform.Find("Boy01_Brows_Geo");
        Transform Eyes = AJ.transform.Find("Boy01_Eyes_Geo");
        Transform h_Geo = AJ.transform.Find("h_Geo");


        // Get the SkinnedMeshRenderer component from the Hand_rigged child object
        SkinnedMeshRenderer BodyRenderer = Body.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer BrowsRenderer = Brows.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer EyesRenderer = Eyes.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer h_GeoRenderer = h_Geo.GetComponent<SkinnedMeshRenderer>();

        if (BodyRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer component not found on 'Hand_rigged' in LeftHand.");
            yield break;
        }


        // Enable the renderers
        BodyRenderer.enabled = true;
        BrowsRenderer.enabled = true;
        EyesRenderer.enabled = true;
        h_GeoRenderer.enabled = true;

        // Get the Animator components
        Animator AjAnimator = AJ.GetComponent<Animator>();

        // Set the animators in HandAnimationHandler
        AJAnimationHandler.SetAnimator(AjAnimator);
        AJAnimationHandler.IsRunning = true;

        if (!IsCarried)
            AJAnimationHandler.StartCounting(int.Parse(SNum), int.Parse(FNum), IsAddition);
        else
        {
            AJAnimationHandler.StartCounting(int.Parse(SNum), int.Parse(FNum) + 1, IsAddition);
        }


        while (AJAnimationHandler.IsRunning)
        {
            yield return null;
        }

        // Disable the renderers and deactivate objects after animation is complete
        BodyRenderer.enabled = false;
        BrowsRenderer.enabled = false;
        EyesRenderer.enabled = false;
        h_GeoRenderer.enabled = false;


    }
}
