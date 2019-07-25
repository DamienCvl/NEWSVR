using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

[RequireComponent(typeof(NewsGameObject))]
public class PreviewAreaTrigger : MonoBehaviour
{
    public float minimalDistantFromPlayer = 13.0f;
    public float previewAreaRadius = 7.0f;
    public Vector3 panelPreviewPostion = new Vector3(-0.3f, 0f, 0.5f);

    private GameObject destinationReticle;
    private GameObject newsPreviewGameObject;
    private GameObject teleporting;
    private Transform followHead;

    [HideInInspector]
    public float distReticleToNews;

    [HideInInspector]
    public float distPlayerToNews;

    // true when the reticle 
    private bool isPreviewEnabled = false;

    // Start is called before the first frame update
    void Start()
    {

        followHead = GameObject.Find("FollowHead").transform;

        newsPreviewGameObject = GetComponentInParent<NewsPlacement>().newsPreview;

        teleporting = GameObject.FindObjectOfType<Teleport>().gameObject;
        destinationReticle = teleporting.transform.Find("DestinationReticle").gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        distReticleToNews = Vector3.Distance(destinationReticle.transform.position, transform.position);
        distPlayerToNews = Vector3.Distance(followHead.transform.position, transform.position);

        if (distReticleToNews <= previewAreaRadius && !isPreviewEnabled && destinationReticle.activeSelf && distPlayerToNews >= minimalDistantFromPlayer)
        {
            isPreviewEnabled = true;
            newsPreviewGameObject.GetComponent<NewsPreview>().SetPreviewInfos(GetComponent<NewsGameObject>());
            newsPreviewGameObject.SetActive(true);
        }

        if (isPreviewEnabled && (!(destinationReticle.activeSelf) || distReticleToNews > previewAreaRadius || distPlayerToNews < minimalDistantFromPlayer))
        {
            isPreviewEnabled = false;
            newsPreviewGameObject.SetActive(false);
        }
    }
}
