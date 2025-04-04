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
    public static IEnumerator PlayFraction(MonoBehaviour monoBehaviour, string Nue ,string Deno, bool Explain)
    {
        if (int.Parse(Nue) > 0 && int.Parse(Deno) < 0)
        {
            Nue = (-int.Parse(Nue)).ToString();
            Deno = (-int.Parse(Deno)).ToString();
        }
        if (int.Parse(Nue) % int.Parse(Deno) == 0)
        {
            yield return (monoBehaviour.StartCoroutine(PlayVoiceNumberAndWait(monoBehaviour, (int.Parse(Nue)/ int.Parse(Deno)).ToString(), Explain)));

        }
        else
        {
            if (Explain)
            {
                yield return (monoBehaviour.StartCoroutine(PlayVoiceNumberAndWait(monoBehaviour, Nue, Explain)));
                yield return (monoBehaviour.StartCoroutine(PlayByAddress(monoBehaviour, "over" + SpeakerName, Explain)));
                yield return (monoBehaviour.StartCoroutine(PlayVoiceNumberAndWait(monoBehaviour, Deno, Explain)));
            }
            else
                yield return null;
        }


    }
    public static IEnumerator SpawnAndAnimate(GameObject prefab, Vector3 spawnPosition, string triggerValue, TextMeshProUGUI FirstNumPlace, bool InExplain = false , int Rotation = 0)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned.");
            yield break;
        }

        // Instantiate the GameObject at the specified position
        GameObject newObject = Instantiate(prefab, spawnPosition, Quaternion.Euler(0,0,Rotation));
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
    public static IEnumerator WriteFraction(MonoBehaviour monoBehaviour, TextMeshProUGUI FirstNumPlace, GameObject Line, bool Explain, int Nue, int Deno, float Xpos, float Ypos, int TextName = -1, bool Speak = true , bool Simplify = true ,bool IsArab = false)
    {
        if (Nue > 0 && Deno < 0)
        {
            Nue *= -1;
            Deno *= -1;
        }
        if (Nue % Deno != 0 || !Simplify)
        {
            if (Speak)
            {
                yield return monoBehaviour.StartCoroutine(PlayVoiceNumberAndWait(monoBehaviour, Nue.ToString(), Speak));
                TextInstantiator.InstantiateText(FirstNumPlace, Nue.ToString(), Xpos, Ypos + 50, 0, false, TextName,0,IsArab);
                yield return monoBehaviour.StartCoroutine(PlayByAddress(monoBehaviour, "over" + SpeakerName, Speak));

                if(!IsArab)
                    yield return monoBehaviour.StartCoroutine(SpawnAndAnimate(Line, new Vector3(Xpos + 10, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));

                else
                    yield return monoBehaviour.StartCoroutine(SpawnAndAnimate(Line, new Vector3(Xpos + 10, Ypos - 30, 0), "SmallLine", FirstNumPlace, Explain));

                yield return monoBehaviour.StartCoroutine(PlayVoiceNumberAndWait(monoBehaviour, Deno.ToString(), Speak));
                TextInstantiator.InstantiateText(FirstNumPlace, Deno.ToString(), Xpos, Ypos - 50, 0, false, TextName * TextName , 0 , IsArab);
            }
            else
            {
                TextInstantiator.InstantiateText(FirstNumPlace, Nue.ToString(), Xpos, Ypos + 50, 0, false, TextName , 0 , IsArab);

                if (!IsArab)
                    yield return monoBehaviour.StartCoroutine(SpawnAndAnimate(Line, new Vector3(Xpos + 10, Ypos - 20, 0), "SmallLine", FirstNumPlace, Explain));

                else
                    yield return monoBehaviour.StartCoroutine(SpawnAndAnimate(Line, new Vector3(Xpos + 10, Ypos - 30, 0), "SmallLine", FirstNumPlace, Explain));

                TextInstantiator.InstantiateText(FirstNumPlace, Deno.ToString(), Xpos, Ypos - 50, 0, false, TextName * TextName, 0, IsArab);
            }
        }
        else
        {
            if (Speak)
                yield return monoBehaviour.StartCoroutine(PlayVoiceNumberAndWait(monoBehaviour, (Nue / Deno).ToString(), Speak));

            TextInstantiator.InstantiateText(FirstNumPlace, (Nue / Deno).ToString(), Xpos, Ypos, 0, false, TextName ,0,IsArab);
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
    public static IEnumerator PronunceTerm(MonoBehaviour monoBehaviour , Term term , bool Explain)
    {
        if (term.GetNumber() != 1)
        {
            yield return SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, term.GetNumber().ToString(), Explain);
        }
        if (term.GetNumPow() != null && term.GetNumPow() != "")
        {
            yield return SLStaicFunctions.PlayByAddress(monoBehaviour, "power", Explain);
            yield return SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, term.GetNumPow(), Explain);


        }
        if ((term.GetNue() != 1 || term.GetDeno() != 1) && (term.GetNue() != 0 || term.GetDeno() != 0))
        {
            yield return SLStaicFunctions.PlayFraction(monoBehaviour, term.GetNue().ToString(), term.GetDeno().ToString(), Explain);

        }
        if (term.GetSymbol() != "" && term.GetSymbol() != null)
        {
            yield return SLStaicFunctions.PlayByAddress(monoBehaviour, term.GetSymbol(), Explain);

        }

        if (term.GetSymbPow() != null && term.GetSymbPow() != "")
        {
            yield return SLStaicFunctions.PlayByAddress(monoBehaviour, "power", Explain);
            yield return SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, term.GetSymbPow(), Explain);


        }
    }
}
