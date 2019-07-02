using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Used because all comments from this news need to acess those variable.
 * ForCommentsPlacement is to put the comments around you and not all at the same place.
 * ForCommentsPlacementDeepness is to put the most recent comments a bit nearer you.
 */
public class FloatPlacementComments : MonoBehaviour {

    public float ForCommentsPlacement;
    public float ForCommentsPlacementDeepness;

    // Use this for initialization
    void Start () {
        ForCommentsPlacement = 0f;
        ForCommentsPlacementDeepness = 0.4f;
    }

    public void UpdateForCommentsPlacementDeepness()
    {
        if(ForCommentsPlacementDeepness < 1.0f)
        {
            ForCommentsPlacementDeepness = ForCommentsPlacementDeepness + 0.1f;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
