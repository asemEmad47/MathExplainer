using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    private int Score = 0;
    private TextMeshProUGUI ScoreTxt;
    private RawImage Emoji;
    public void IntilizeComponents(int score , TextMeshProUGUI scoreTxt , RawImage EmojiImage)
    {
        this.Score = score;
        this.ScoreTxt = scoreTxt;
        this.Emoji = EmojiImage;
    }

    public void EndGame()
    {
        ScoreTxt.text += Score;
        Texture emojiTexture;
        if (Score <= 40)
        {
            emojiTexture = Resources.Load<Texture>("Emoji/SadPng");
        }
        else if (Score > 40 && Score < 100)
        {
            emojiTexture = Resources.Load<Texture>("Emoji/YellowPng");
        }
        else
        {
            emojiTexture = Resources.Load<Texture>("Emoji/HappyPng");
        }
        Emoji.texture = emojiTexture;

        Button[] buttons = FindObjectsOfType<Button>();

        // Disable all buttons in the scene
        foreach (Button button in buttons)
        {
            if(!button.name.Equals("PlayAgainBtn") && !button.name.Equals("BackToHome"))
            {
                button.interactable = false;
            }
        }
    }
}
