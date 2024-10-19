using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public UnityEngine.UI.Button playPauseButton;


    private void Start()
    {
        playPauseButton.onClick.AddListener(TogglePlayPause);

    }

    private void TogglePlayPause()
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
        else
            videoPlayer.Play();
    }


}
