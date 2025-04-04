using System;
using System.Collections;
using System.Net;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class AdditionVoiceSpeaker : MonoBehaviour
{
    public static AudioClip[] voiceClips;
    public static AudioClip[] Numbers;
    public static AudioSource audioSource;
    public static string VoiceClipsPlace = "";
    public static string NumPlace = "";
    public static string SpeakerName = "";
    public static bool IsEng = false;
    public static IEnumerator PlayByAddress(string address)
    {
        LoadAllAudioClips();
        GameObject audioObject = new GameObject("VoiceAudioSource");
        audioSource = audioObject.AddComponent<AudioSource>();

        AudioClip AnyClip = Resources.Load<AudioClip>(address.ToLower());
        audioSource.clip = AnyClip;
        audioSource.Play();

        float clipDurationWithoutSilence = GetSoundDurationWithoutSilence(AnyClip);

        yield return new WaitForSeconds(clipDurationWithoutSilence + 1f);

        audioSource.Stop();
        Destroy(audioObject);
        audioSource.clip = null;
    }
    public static IEnumerator PlayVoiceNumberAndWait(String text)
    {
        if (text.Length >= 1)
        {
            LoadAllAudioClips();
            GameObject audioObject = new GameObject("VoiceAudioSource");
            audioSource = audioObject.AddComponent<AudioSource>();
            if (text[0].Equals('-'))
            {
                AudioClip MinusAudioClip;
                MinusAudioClip = Resources.Load<AudioClip>(VoiceClipsPlace + "/negative" + SpeakerName);

                audioSource.clip = MinusAudioClip;
                audioSource.Play();
                Debug.Log(text+" from number");
                yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip) + 1f);
                StringBuilder MinusRemoval = new StringBuilder(text);
                MinusRemoval.Remove(0, 1);
                text = MinusRemoval.ToString();
            }
            bool IsComplex = false;
            if (text.Length == 1)
            {
                audioSource.clip = GetUnder10Numbers(text);
            }
            else if (text.Length == 2)
            {
                audioSource.clip = GetUnder20Numbers(text);
                if (audioSource.clip == null)
                {
                    string temp = "";
                    temp += text[0];
                    temp += "0";
                    if (NumPlace.Equals("EngNums") || IsEng || VoiceClipsPlace.Equals("JennySound"))
                    {
                        audioSource.clip = GetUnder20Numbers(temp);
                        audioSource.Play();
                        yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip));
                        audioSource.clip = GetUnder10Numbers(text[1].ToString());
                        audioSource.Play();
                        yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip));
                    }
                    else
                    {
                        audioSource.clip = GetUnder10Numbers(text[1].ToString());
                        audioSource.Play();
                        yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip));

                        AudioClip AnyClip = Resources.Load<AudioClip>($"{VoiceClipsPlace}/and{SpeakerName}");
                        audioSource.clip = AnyClip;
                        audioSource.Play();
                        yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip));

                        audioSource.clip = GetUnder20Numbers(temp);
                        audioSource.Play();
                        yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip));
                    }
                    IsComplex = true;
                }
            }
            else
            {
                if (text.Equals("100"))
                {
                    audioSource.clip = Numbers[28];
                }
                else
                {
                    int charIndex = text.IndexOf('.');
                    if (charIndex == -1)
                    {
                        foreach (char c in text)
                        {
                            audioSource.clip = GetUnder10Numbers(c.ToString());
                            audioSource.Play();
                            yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip));
                        }
                        audioSource.Stop();
                        Destroy(audioObject);
                        audioSource = null;
                        yield break;
                    }
                    if (charIndex == 2)
                    {
                        audioSource.clip = GetUnder20Numbers(text.Substring(0, 2));
                        if (audioSource.clip == null)
                        {
                            string temp = "";
                            temp += text[0];
                            temp += "0";
                            audioSource.clip = GetUnder20Numbers(temp);
                            audioSource.Play();
                            yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip));
                            audioSource.clip = GetUnder10Numbers(text[1].ToString());
                        }
                    }
                    else
                    {
                        audioSource.clip = GetUnder10Numbers(text.Substring(0, 1));
                    }
                    if (charIndex != -1)
                    {
                        audioSource.Play();
                        yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip));

                        AudioClip PointClip = Resources.Load<AudioClip>(VoiceClipsPlace+"/point"+SpeakerName);
                        audioSource.clip = PointClip;
                        audioSource.Play();
                    }
                    yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip));

                    for (int i = charIndex + 1; i < text.Length && i < charIndex + 4; i++)
                    {
                        audioSource.clip = GetUnder10Numbers(text[i].ToString());
                        audioSource.Play();
                        yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip));
                    }
                    IsComplex = true;
                }
            }
            if (!IsComplex)
            {
                audioSource.Play();
                yield return new WaitForSeconds(GetSoundDurationWithoutSilence(audioSource.clip));
            }
            audioSource.Stop();
            Destroy(audioObject);
            audioSource = null;
        }
    }

    public static AudioClip GetUnder10Numbers(String text)
    {
        switch (text)
        {
            case "0":
                return Numbers[0];
            case "1":
                return Numbers[1];
            case "2":
                return Numbers[2];
            case "3":
                return Numbers[3];
            case "4":
                return Numbers[4];
            case "5":
                return Numbers[5];
            case "6":
                return Numbers[6];
            case "7":
                return Numbers[7];
            case "8":
                return Numbers[8];
            case "9":
                return Numbers[9];
            default:
                return Numbers[0];
        }
    }
    public static AudioClip GetUnder20Numbers(String text)
    {
        switch (text)
        {
            case "10":
                return Numbers[10];
            case "11":
                return Numbers[11];
            case "12":
                return Numbers[12];
            case "13":
                return Numbers[13];
            case "14":
                return Numbers[14];
            case "15":
                return Numbers[15];
            case "16":
                return Numbers[16];
            case "17":
                return Numbers[17];
            case "18":
                return Numbers[18];
            case "19":
                return Numbers[19];
            case "20":
                return Numbers[20];
            case "30":
                return Numbers[21];
            case "40":
                return Numbers[22];
            case "50":
                return Numbers[23];
            case "60":
                return Numbers[24];
            case "70":
                return Numbers[25];
            case "80":
                return Numbers[26];
            case "90":
                return Numbers[27];
            default:
                return null;
        }
    }
    public static void LoadAllAudioClips()
    {
        voiceClips = Resources.LoadAll<AudioClip>(VoiceClipsPlace);
        Numbers = Resources.LoadAll<AudioClip>(NumPlace);
    }
    public static float GetSoundDurationWithoutSilence(AudioClip clip)
    {
        float silenceThreshold = 0.01f;
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        // Scan through the samples and find the last point above the silence threshold
        int lastSoundSample = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            if (Mathf.Abs(samples[i]) > silenceThreshold)
            {
                lastSoundSample = i;
            }
        }

        // Convert the last sound sample position to time (in seconds)
        float lastSoundTime = (float)lastSoundSample / clip.frequency;
        return lastSoundTime;
    }

}
