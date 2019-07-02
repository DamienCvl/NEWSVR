using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

/*
 * Handle the placement of the panel with the indication at the beginning:
 * You can take it and move it around too:
 */

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    public class BeginInformation : Grabbable
    {

        private GameObject HeadCollider;

        private void Start()
        {
            HeadCollider = GameObject.Find("HeadCollider");

            // Put the news in front of the player the first time the player pick up the newsSphere
            transform.position = new Vector3(HeadCollider.transform.position.x, 1.5f, HeadCollider.transform.position.z);
            transform.rotation = new Quaternion(0, HeadCollider.transform.rotation.y, 0, HeadCollider.transform.rotation.w);
            transform.Translate(new Vector3(HeadCollider.transform.forward.x, 0.0f, HeadCollider.transform.forward.z), Space.World);
        }


        private void OnEnable()
        {

            HeadCollider = GameObject.Find("HeadCollider");

            // Put the news in front of the player every time the player pick up the newsSphere
            transform.position = new Vector3(HeadCollider.transform.position.x, 1.5f, HeadCollider.transform.position.z);
            transform.rotation = new Quaternion(0, HeadCollider.transform.rotation.y, 0, HeadCollider.transform.rotation.w);
            transform.Translate(new Vector3(HeadCollider.transform.forward.x, 0.0f, HeadCollider.transform.forward.z), Space.World);

        }
    }
}
