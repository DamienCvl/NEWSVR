using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour {

    public GameObject Teleport;

    public void changeTeleport(bool change)
    {
        Teleport.SetActive(change);
    }
}
