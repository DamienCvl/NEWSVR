using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Valve.VR;

[RequireComponent(typeof(Collider))]
public class PreviewAreaTrigger : MonoBehaviour
{

    public GameObject player;
    private Hand hand = null;
    public GameObject head;

    private SteamVR_Action_Boolean teleportAction = SteamVR_Input.GetBooleanAction("Teleport");

    public GameObject newsPreviewPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<Player>().gameObject;
        }

        if (newsPreviewPanel == null)
        {
            newsPreviewPanel = GameObject.FindObjectOfType<Teleport>().transform.Find("NewsPreview").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (teleportAction[SteamVR_Input_Sources.LeftHand].state && hand == null)
        {
            hand = player.GetComponent<Player>().leftHand;
        }
        else if (teleportAction[SteamVR_Input_Sources.RightHand].state && hand == null)
        {
            hand = player.GetComponent<Player>().rightHand;
        }
        else if (!(teleportAction.state) && hand != null)
        {
            hand = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PreviewArea")
        {
            newsPreviewPanel.transform.Find("Title").gameObject.GetComponent<Text>().text = other.transform.parent.parent.Find("Title").GetComponent<Text>().text;
            newsPreviewPanel.transform.Find("Infos").gameObject.GetComponent<Text>().text = other.transform.parent.parent.Find("Content").GetComponent<Text>().text;
            newsPreviewPanel.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "PreviewArea")
        {
            newsPreviewPanel.transform.position = hand.transform.TransformPoint(new Vector3(0, 0.2f, 0));
            newsPreviewPanel.transform.rotation = Quaternion.LookRotation(hand.transform.position - head.transform.position, Vector3.up);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "PreviewArea")
        {
            newsPreviewPanel.SetActive(false);
        }
    }
}
