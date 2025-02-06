using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SLStaicFunctions : MonoBehaviour
{
    public static string SpeakerName;
    public static IEnumerator PlayByAddress(MonoBehaviour monoBehaviour, string text, bool explain)
    {
        if (explain)
            yield return monoBehaviour.StartCoroutine(AdditionVoiceSpeaker.PlayByAddress(AdditionVoiceSpeaker.VoiceClipsPlace + "/" + text));
        else
            yield return null;
    }
    public static IEnumerator PlayVoiceNumberAndWait(MonoBehaviour monoBehaviour, string number, bool Explain)
    {
        if (Explain)
        {
            yield return (monoBehaviour.StartCoroutine(AdditionVoiceSpeaker.PlayVoiceNumberAndWait(number)));

        }

        else
            yield return null;

    }
    public static IEnumerator SpawnAndAnimate(GameObject prefab, Vector3 spawnPosition, string triggerValue, TextMeshProUGUI FirstNumPlace, bool InExplain = false)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned.");
            yield break;
        }

        // Instantiate the GameObject at the specified position
        GameObject newObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        newObject.transform.parent = prefab.transform.parent;
        CharacterProbs.CenterInPos(spawnPosition.x, spawnPosition.y, ref newObject, FirstNumPlace);
        // Get the Animator component from the instantiated object
        Animator animator = newObject.GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetTrigger(triggerValue);

            if (InExplain)
            {
                // Wait until the animation has finished
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                while (stateInfo.normalizedTime < 1.0f || animator.IsInTransition(0))
                {
                    yield return null;
                    stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                }
            }


        }
        else
        {
            Debug.LogError("No Animator component found on the instantiated object.");
        }
    }
    public static IEnumerator WriteFraction(MonoBehaviour monoBehaviour, TextMeshProUGUI FirstNumPlace, GameObject Line, bool Explain, int Nue, int Deno, float Xpos, float Ypos, int TextName = -1, bool Speak = true)
    {
        if (Nue % Deno != 0)
        {
            if (Speak)
            {
                yield return monoBehaviour.StartCoroutine(PlayVoiceNumberAndWait(monoBehaviour, Nue.ToString(), Speak));
                TextInstantiator.InstantiateText(FirstNumPlace, Nue.ToString(), Xpos, Ypos + 50, 0, false, TextName);

                yield return monoBehaviour.StartCoroutine(PlayByAddress(monoBehaviour, "over" + SpeakerName, Speak));
                yield return monoBehaviour.StartCoroutine(SpawnAndAnimate(Line, new Vector3(Xpos + 10, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));

                yield return monoBehaviour.StartCoroutine(PlayVoiceNumberAndWait(monoBehaviour, Deno.ToString(), Speak));
                TextInstantiator.InstantiateText(FirstNumPlace, Deno.ToString(), Xpos, Ypos - 50, 0, false, TextName * TextName);
            }
            else
            {
                TextInstantiator.InstantiateText(FirstNumPlace, Nue.ToString(), Xpos, Ypos + 50, 0, false, TextName);
                yield return monoBehaviour.StartCoroutine(SpawnAndAnimate(Line, new Vector3(Xpos + 10, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));
                TextInstantiator.InstantiateText(FirstNumPlace, Deno.ToString(), Xpos, Ypos - 50, 0, false, TextName * TextName);
            }
        }
        else
        {
            if (Speak)
                yield return monoBehaviour.StartCoroutine(PlayVoiceNumberAndWait(monoBehaviour, (Nue / Deno).ToString(), Speak));

            TextInstantiator.InstantiateText(FirstNumPlace, (Nue / Deno).ToString(), Xpos, Ypos, 0, false, TextName );
        }
    }

    public static void RemoveTexts()
    {

        TextMeshProUGUI[] textMeshPros = FindObjectsOfType<TextMeshProUGUI>();

        foreach (TextMeshProUGUI textMeshPro in textMeshPros)
        {
            if (int.TryParse(textMeshPro.name, out _))
            {
                Destroy(textMeshPro.gameObject);
            }
        }

        GameObject[] Squares = FindObjectsOfType<GameObject>();
        foreach (GameObject GmObj in Squares)
        {
            if (GmObj.name.Equals("Square(Clone)"))
            {
                Destroy(GmObj);
            }
        }

    }
}
