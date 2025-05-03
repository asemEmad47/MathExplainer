using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DivNueDeno : MonoBehaviour
{
    public static IEnumerator Div(TextMeshProUGUI Nue, TextMeshProUGUI Deno, MonoBehaviour monoBehaviour, GameObject Remove, bool Explain, TextMeshProUGUI FirstNumPlace, int Distance,Vector3 NuePos)
    {
        string EditedDeno = PrepareDeno(Deno.text);
        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, Nue.text.ToString(), Explain)));
        Nue.color = Color.red;
        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "divide" + SLStaicFunctions.SpeakerName, Explain)));

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour, EditedDeno, Explain)));
        Deno.color = Color.red;

        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "equal" + SLStaicFunctions.SpeakerName, Explain)));
        yield return (monoBehaviour.StartCoroutine(SLStaicFunctions.PlayVoiceNumberAndWait(monoBehaviour,(int.Parse(Nue.text)/int.Parse(EditedDeno)).ToString(), Explain)));
        TMP_CharacterInfo charInfo = new();

        NuePos = new Vector3(NuePos.x, NuePos.y+Distance, NuePos.z);
        yield return monoBehaviour.StartCoroutine(SLStaicFunctions.SpawnAndAnimate(Remove,NuePos,"SmallLine",FirstNumPlace,Explain,30));

        if (Distance > 0)
            Distance += 70;
        TextInstantiator.InstantiateText(FirstNumPlace, (int.Parse(Nue.text) / int.Parse(EditedDeno)).ToString(), NuePos.x, NuePos.y,Distance,false);

    }

    public static string PrepareDeno(string Deno)
    {
        if (Deno[0].Equals('÷'))
        {
            return Deno.Substring(2);
        }
        return Deno;
    }
}
