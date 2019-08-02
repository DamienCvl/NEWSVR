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

    private void Awake()
    {
        buttonPrefab = (GameObject)Resources.Load("Prefabs/News/Media/Button", typeof(GameObject));
    }

    private void OnEnable()
    {
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
        if (news != null)
        {
            foreach (Media m in news.newsInfos.GetMedium())
            {
                GameObject button = Instantiate(buttonPrefab, buttonList.transform);
                RectTransform buttonRect = button.GetComponent<RectTransform>();
                ClickableUIVR buttonClickable = button.GetComponentInChildren<ClickableUIVR>();

                button.GetComponentInChildren<Text>().text = m.GetMediaTypeToString();
                buttonClickable.gameObject.transform.localScale = new Vector3(buttonRect.rect.width / 10, 1.0f, buttonRect.rect.height / 10); // Use to set the size of the VR clickable area
                buttonClickable.OnClickEvent.AddListener(delegate { ChangeMedia(m); });
                button.GetComponentInChildren<Button>().onClick.AddListener(delegate { ChangeMedia(m); });
            }
            if (news.newsInfos.GetMedium().Count > 0)
                ChangeMedia(news.newsInfos.GetMedium()[0]);
        }
        // Test
        mediaPlayer.image.SetActive(true);
        StartCoroutine(mediaPlayer.SetImageFromWeb("https://interactive-examples.mdn.mozilla.net/media/examples/grapefruit-slice-332-332.jpg"));
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
                mediaPlayer.image.SetActive(true);
                break;
            case 1: // Video
                mediaPlayer.videoPlayer.url = m.GetUrl();
                mediaPlayer.SetMediaController();
                mediaPlayer.videoPlayer.gameObject.SetActive(true);
                break;
            case 2: // Audio
                StartCoroutine(mediaPlayer.SetAudioFromWeb(m.GetUrl()));
                mediaPlayer.SetMediaController();
                mediaPlayer.audioSource.gameObject.SetActive(true);
                break;
            default:
                Debug.Log("Unknown media type");
                break;
        }
    }
}
