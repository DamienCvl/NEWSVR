﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;
using Valve.VR;


[RequireComponent(typeof(Interactable))]
public class OldCommentArrow : MonoBehaviour
{
    public SteamVR_Action_Boolean UI_Interaction_Action = SteamVR_Input.GetBooleanAction("InteractUI");
    public UnityEvent OnClickEvent;

    // Start is called before the first frame update
    void Start()
    {

        if (OnClickEvent == null)
        {
            OnClickEvent = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void HandHoverUpdate(Hand hand)
    {
        if (UI_Interaction_Action.GetStateDown(hand.handType))
        {
            OnClickEvent.Invoke();
        }
    }
}
