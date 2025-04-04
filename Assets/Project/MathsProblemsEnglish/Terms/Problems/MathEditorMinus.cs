using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MathEditorSimplfy : MonoBehaviour, ProblemSolver
{
    private string SpeakerName = "_Jenny_Eng";
    private string VoicesPlace = "JennySound";
    private string NumPlace = "JennySound/Numbers";
    private bool InExplain = false;

    private TextMeshProUGUI FirstNumPlace;
    private GameObject Line;
    private float Xpos;
    private float Ypos;
    private bool Explain;
    private MonoBehaviour monoBehaviour;
    public void SetComponents(TextMeshProUGUI FirstNumPlace , GameObject Line , float Xpos , float Ypos , bool Explain , MonoBehaviour monoBehaviour)
    {
        this.FirstNumPlace = FirstNumPlace;
        this.Line = Line;
        this.Xpos = Xpos;
        this.Ypos = Ypos;
        this.Explain = Explain;
        this.monoBehaviour = monoBehaviour;

        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.NumPlace = NumPlace;
        AdditionVoiceSpeaker.IsEng = true;
        AdditionVoiceSpeaker.VoiceClipsPlace = VoicesPlace;
        SLStaicFunctions.SpeakerName = SpeakerName;
    }
    public IEnumerator Solve(List<Term> terms)
    {
        ProblemPreparation.PrepareTerms(ref terms);
        ResetValues.ResetAllValues();
        Xpos -= 100;
        Ypos+=400;
        TermsWriter.TermIndex = 0;
        yield return monoBehaviour.StartCoroutine(TermsWriter.WriteTerms(terms, Xpos, Ypos, FirstNumPlace, Explain, this, Line ,false,"1"));

        yield return monoBehaviour.StartCoroutine(SolveMultipliedList(terms, "1", Explain, monoBehaviour));


    }

    void Update()
    {
        PauseScript.ControlPause();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AdditionVoiceSpeaker.SpeakerName = SpeakerName;
        AdditionVoiceSpeaker.NumPlace = NumPlace;
        AdditionVoiceSpeaker.VoiceClipsPlace = VoicesPlace;
    }
    public IEnumerator DoFirstStep(List<Term> terms)
    {
        Ypos -= 200;
        List<Term> OldTerms = new(terms);
        ProblemPreparation.OrderTerms(ref terms);
        if (OldTerms != terms)
        {
            yield return monoBehaviour.StartCoroutine(SLStaicFunctions.PlayByAddress(monoBehaviour, "First , we want to order terms by symbol" + SpeakerName, Explain));

            TermsWriter.TermIndex = 0;
            yield return monoBehaviour.StartCoroutine(TermsWriter.WriteTerms(terms, Xpos, Ypos, FirstNumPlace, Explain, this, Line , false,"2"));
        }
    }
    public static IEnumerator SolveMultipliedList(List<Term> terms , string iteration , bool Explain , MonoBehaviour monoBehavior)
    {
        int counter = 1;
        foreach (var term in terms)
        {
            ColoringScript.ColorThemAll(iteration + " " + counter, Color.red);
            yield return SLStaicFunctions.PronunceTerm(monoBehavior, term, Explain);
            if (term.GetMultipliedTerms().Count!=0)
            {

                foreach (var MulitpliedTerm in term.GetMultipliedTerms())
                {
                    yield return SLStaicFunctions.PlayByAddress(monoBehavior, "time", Explain);

                    ColoringScript.ColorThemAll(iteration + " " + counter, Color.red);
                    yield return SLStaicFunctions.PronunceTerm(monoBehavior, MulitpliedTerm, Explain);
                    counter++;

                    yield return SLStaicFunctions.PlayByAddress(monoBehavior, "time", Explain);

                }
            }
            counter++;
        }
        yield return null;
    }
    public static IEnumerator SolveBrackets(List<Term> terms) {
        int counter = 1;
        foreach (var term in terms)
        {
            if(term.GetBracket()!= null)
            {

                foreach (var bracketTerm in term.GetBracket().GetTerms())
                {

                }
            }
            counter++;
        }
        yield return null;
    }

    //public static Term MultiplyTwoTerms(Term term1, Term term2) {
        
    //    if(term1.GetNumber()!=1)
        
    //}
}
