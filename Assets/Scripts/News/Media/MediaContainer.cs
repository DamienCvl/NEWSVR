using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediaContainer : MonoBehaviour
{
    public NewsGameObject news;
    public GameObject buttonList;
    public MediaPlayer mediaPlayer;
    private GameObject buttonPrefab;

    private bool buttonsIsRefresh = false;

    private void Awake()
    {
        buttonPrefab = (GameObject)Resources.Load("Prefabs/News/Media/Button", typeof(GameObject));
    
        if (news == null)
        {
            try
            {
                news = transform.parent.parent.GetComponent<NewsGameObject>();
            }
            catch
            {
                Debug.Log("NewsGameObject not found");
            }
        }

        // Create all buttons associate with medium
        if (news != null && news.newsInfos != null)
        {
            foreach (Media m in news.newsInfos.GetMedium())
            {
                // Get components
                GameObject button = Instantiate(buttonPrefab, buttonList.transform);
                RectTransform buttonRect = button.GetComponent<RectTransform>();
                ClickableUIVR buttonClickable = button.GetComponentInChildren<ClickableUIVR>();
                
                // Set button text
                button.GetComponentInChildren<Text>().text = m.GetMediaTypeToString();

                // Use to set the size of the VR clickable area
                StartCoroutine(UpdateVRAreaButton(buttonClickable, buttonRect));

                // Set called function when click on button
                buttonClickable.OnClickEvent.AddListener(() => { ChangeMedia(m); });
                button.GetComponentInChildren<Button>().onClick.AddListener(() => { ChangeMedia(m); });
            }
            if (news.newsInfos.GetMedium().Count > 0)
            {
                ChangeMedia(news.newsInfos.GetMedium()[0]);
            }
        }
    }

    public IEnumerator UpdateVRAreaButton(ClickableUIVR buttonClickable, RectTransform buttonRect)
    {
        yield return null;
        buttonClickable.gameObject.transform.localScale = new Vector3(buttonRect.rect.width / 10, buttonClickable.gameObject.transform.localScale.y, buttonClickable.gameObject.transform.localScale.z);
    }

    public void ChangeMedia(Media m)
    {
        // Set to default media player display
        foreach (Transform child in mediaPlayer.transform)
        {
            child.gameObject.SetActive(false);
        }
        switch (m.GetMediaType())
        {
            case 0: // Image
                StartCoroutine(mediaPlayer.SetImageFromWeb(m.GetUrl()));
                mediaPlayer.image.gameObject.SetActive(true);
                break;
            case 1: // Video
                mediaPlayer.videoPlayer.url = m.GetUrl();
                mediaPlayer.videoPlayer.gameObject.SetActive(true);
                mediaPlayer.SetMediaController(0);
                break;
            case 2: // Audio
                StartCoroutine(mediaPlayer.SetAudioFromWeb(m.GetUrl()));
                mediaPlayer.audioSource.gameObject.SetActive(true);
                mediaPlayer.SetMediaController(1);
                break;
            default:
                Debug.Log("Unknown media type");
                break;
        }
    }

    private void Update()
    {
        if (!buttonsIsRefresh)
        {
            buttonList.SetActive(false);
            buttonList.SetActive(true);
            buttonsIsRefresh = true;
        }
    }
}
