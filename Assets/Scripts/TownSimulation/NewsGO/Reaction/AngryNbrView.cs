using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Core;

namespace Assets.Scripts.TownSimulation.NewsGO.Reaction
{
    // Handles the number of angry on the reaction box.

    public class AngryNbrView : MonoBehaviour
    {

        private void OnEnable()
        {
            this.GetComponent<Text>().text = Database.NumOfReatcionToNews("Angry", StaticClass.CurrentNewsId);
        }
    }
}
