﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class PreviewAreaTrigger : MonoBehaviour
{

    public Text titleOfTheNews;
    public Text contentOfTheNews;

    public float previewAreaRadius = 5.0f;
    public Vector3 panelPreviewPostion = new Vector3(-0.3f, 0f, 0.5f);

    private GameObject destinationReticle;
    private GameObject newsPreview;
    private GameObject teleporting;
    private Transform followHead;

    [HideInInspector]
    public float distReticleToNews;

    [HideInInspector]
    public float distPlayerToNews;

    // true when the reticle 
    private bool isEntered = false;

    // Start is called before the first frame update
    void Start()
    {

        followHead = GameObject.Find("FollowHead").transform;

        newsPreview = GameObject.Find("NewsPreview");

        teleporting = GameObject.FindObjectOfType<Teleport>().gameObject;
        destinationReticle = teleporting.transform.Find("DestinationReticle").gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        distReticleToNews = Vector3.Distance(destinationReticle.transform.position, transform.position);
        distPlayerToNews = Vector3.Distance(followHead.transform.position, transform.position);

        if (distReticleToNews <= previewAreaRadius && !isEntered && destinationReticle.activeSelf && distPlayerToNews > 12.0f)
        {
            isEntered = true;
            newsPreview.transform.Find("Panel/Title").GetComponent<Text>().text = titleOfTheNews.text;
            newsPreview.transform.Find("Panel/Infos").gameObject.GetComponent<Text>().text = contentOfTheNews.text;
            newsPreview.transform.position = followHead.transform.TransformPoint(panelPreviewPostion);
            newsPreview.transform.rotation = Quaternion.LookRotation(newsPreview.transform.position - followHead.transform.position, Vector3.up);
            newsPreview.transform.Find("Panel").gameObject.SetActive(true);
        }

        if (isEntered && (!(destinationReticle.activeSelf) || distReticleToNews > previewAreaRadius))
        {
            isEntered = false;
            newsPreview.transform.Find("Panel").gameObject.SetActive(false);
        }
    }
}
