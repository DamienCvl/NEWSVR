using System.Collections;
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

    private SteamVR_Action_Boolean teleportAction = SteamVR_Input.GetBooleanAction("Teleport");


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

        float dist = Vector3.Distance(destinationReticle.transform.position, transform.position);

        if (dist <= previewAreaRadius && !isEntered && destinationReticle.activeSelf)
        {
            isEntered = true;
            newsPreview.transform.Find("Panel/Title").GetComponent<Text>().text = titleOfTheNews.text;
            newsPreview.transform.Find("Panel/Infos").gameObject.GetComponent<Text>().text = contentOfTheNews.text;
            newsPreview.transform.position = followHead.transform.TransformPoint(panelPreviewPostion);
            newsPreview.transform.rotation = Quaternion.LookRotation(newsPreview.transform.position - followHead.transform.position, Vector3.up);
            newsPreview.transform.Find("Panel").gameObject.SetActive(true);
        }

        if (isEntered && ( !(destinationReticle.activeSelf) || dist > previewAreaRadius))
        {
            isEntered = false;
            newsPreview.transform.Find("Panel").gameObject.SetActive(false);
        }
    }
}
