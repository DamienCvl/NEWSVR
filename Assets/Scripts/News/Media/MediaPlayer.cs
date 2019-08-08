﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using Valve.VR.InteractionSystem;

public class MediaPlayer : MonoBehaviour
{
    // Fill with MediaContainer
    private IMediaControll mediaController;
    
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    private Sprite audioSpriteInit;
    public Image image;
    public Slider slider;
    public LinearDrive linearDrive;
    public GameObject controlBar;

    private float previousLinearMappingValue;

    void OnEnable()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponentInChildren<VideoPlayer>();

        if (audioSource == null)
            audioSource = GetComponentInChildren<AudioSource>();

        audioSpriteInit = audioSource.GetComponent<Image>().sprite;

        if (slider == null)
            slider = GetComponentInChildren<Slider>();

        if (linearDrive == null)
            linearDrive = GetComponentInChildren<LinearDrive>();

        previousLinearMappingValue = linearDrive.linearMapping.value;

        if (image == null)
        {
            image = GetComponentInChildren<Image>();
        }

        if (controlBar == null)
            controlBar = transform.Find("ControlBar").gameObject;
    }

    private void Update()
    {
        if (linearDrive.linearMapping.value != previousLinearMappingValue)
        {
            previousLinearMappingValue = linearDrive.linearMapping.value;
            slider.value = previousLinearMappingValue;
        }
    }

    // Call by MediaContainer
    /// <summary>
    /// Set control bar behaviours
    /// </summary>
    /// <param name="choice">0 = Video, 1 = Audio</param>
    public void SetMediaController(byte choice)
    {
        switch (choice)
        {
            case 0: // Video controller
                mediaController = new MediaVideoController(videoPlayer, slider);
                break;
            case 1: // Audio controller
                mediaController = new MediaAudioController(audioSource, slider, audioSpriteInit);
                break;
            default:
                break;
        }
        controlBar.SetActive(true);
    }

    public IEnumerator SetImageFromWeb(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Impossible to get the image");
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            image.overrideSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            float ratio = (float)texture.width / texture.height;
            float ratio16_9 = 16f / 9f;
            if (ratio > ratio16_9)
                image.transform.GetChild(0).localScale = new Vector3(10f, 1f, 5.625f / (ratio / ratio16_9));
            else if (ratio < ratio16_9)
                image.transform.GetChild(0).localScale = new Vector3(10f / (ratio16_9 / ratio), 1f, 5.625f);
            else
                image.transform.GetChild(0).localScale = new Vector3(10f, 1f, 5.625f);
        }
    }

    public IEnumerator SetAudioFromWeb(string url)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS); // AudioType need to be automatic depending on the audio source
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Impossible to get the audio");
        }
        else
        {
            audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
        }
    }

    // Fonctions called by the control bar //

    public void PlayPause()
    {
        if (mediaController != null)
            mediaController.PlayPause();
    }

    public void Stop()
    {
        if (mediaController != null)
            mediaController.Stop();
    }

    public void OnChangeVolume()
    {
        if (mediaController != null)
            mediaController.OnChangeVolume();
    }

    //***************************************//
    private interface IMediaControll
    {
        void PlayPause();
        void Stop();
        void OnChangeVolume();
    }

    //*****************VIDEO*****************//
    private class MediaVideoController : IMediaControll
    {
        private VideoPlayer videoPlayer;
        private Slider slider;

        public MediaVideoController(VideoPlayer v, Slider s)
        {
            videoPlayer = v;
            slider = s;
            videoPlayer.Prepare();
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

    //*****************Audio*****************//
    private class MediaAudioController : IMediaControll
    {
        private AudioSource audioSource;
        private Slider slider;
        private Animator anim;
        private Sprite audioSpriteInit;

        public MediaAudioController(AudioSource a, Slider s, Sprite sp)
        {
            audioSource = a;
            slider = s;
            anim = audioSource.transform.GetComponent<Animator>();
            audioSpriteInit = sp;
        }

        public void PlayPause()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                anim.ResetTrigger("ButtonPlay");
                anim.SetTrigger("ButtonStop");
            }
            else
            {
                audioSource.Play();
                anim.ResetTrigger("ButtonStop");
                anim.SetTrigger("ButtonPlay");
            }
        }

        public void Stop()
        {
            audioSource.Stop();
            anim.ResetTrigger("ButtonPlay");
            anim.SetTrigger("ButtonStop");
            audioSource.GetComponent<Image>().sprite = audioSpriteInit;
        }

        public void OnChangeVolume()
        {
            audioSource.volume = slider.value;
        }
    }
}
