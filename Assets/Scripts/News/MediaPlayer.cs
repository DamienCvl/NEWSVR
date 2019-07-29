using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MediaPlayer : MonoBehaviour
{

    private VideoPlayer videoPlayer;
    public Slider slider;

    // Start is called before the first frame update
    void OnEnable()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();

        if (slider == null)
        {
            transform.parent.GetComponentInChildren<Slider>();
        }
    }


    public void PlayPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }
    }
    
    public void Stop()
    {
        videoPlayer.Stop();
    }

    public void OnChangeVolume()
    {
        for (ushort i = 0; i < videoPlayer.audioTrackCount; i++)
        {
            videoPlayer.SetDirectAudioVolume(i, slider.value);
        }
    }
}
