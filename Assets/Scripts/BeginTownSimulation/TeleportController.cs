using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Call by the news sphere to activate/deactivate teleport when close/open news.

public class TeleportController : MonoBehaviour {

    public GameObject Teleport;

    public void changeTeleport(bool change)
    {
        Teleport.SetActive(change);
    }
}
