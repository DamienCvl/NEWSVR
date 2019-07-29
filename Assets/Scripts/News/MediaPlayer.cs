using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Valve.VR.InteractionSystem;

public class MediaPlayer : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public Slider slider;
    public LinearDrive linearDrive;

    private float previousLinearMappingValue;

    void OnEnable()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponentInChildren<VideoPlayer>();
        }
        videoPlayer.Prepare();

        if (slider == null)
        {
             slider = GetComponentInChildren<Slider>();
        }

        if (linearDrive == null)
        {
            linearDrive = GetComponentInChildren<LinearDrive>();
        }

        previousLinearMappingValue = linearDrive.linearMapping.value;
    }

    private void Update()
    {
        if (linearDrive.linearMapping.value != previousLinearMappingValue)
        {
            previousLinearMappingValue = linearDrive.linearMapping.value;
            slider.value = previousLinearMappingValue;
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
