using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollScript : MonoBehaviour
{
    public static IEnumerator ScrollToPositionCoroutine(ScrollRect scrollRect)
    {
        float scrollSpeed = 0.5f; // Scrolling speed

        float targetYPosition = scrollRect.content.localPosition.y + 230 * (PrimeFactors.branches + 1);
        // Smoothly interpolate the scroll position to the target Y position
        while (Mathf.Abs(scrollRect.content.localPosition.y - targetYPosition) > 50f) // Stop when close to the target
        {
            // Smoothly interpolate the content's local Y position
            Vector3 newPosition = scrollRect.content.localPosition;
            newPosition.y = Mathf.Lerp(
                scrollRect.content.localPosition.y,
                targetYPosition,
                scrollSpeed * Time.deltaTime
            );

            // Update the content's position
            scrollRect.content.localPosition = newPosition;


            yield return null; // Wait until the next frame
        }

        // Ensure the final position is set exactly to the target
        Vector3 finalPosition = scrollRect.content.localPosition;
        finalPosition.y = targetYPosition;
        scrollRect.content.localPosition = finalPosition;

    }
}
