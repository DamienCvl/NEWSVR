using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Core;

namespace Assets.Scripts.TownSimulation.NewsGO.Reaction
{
    // Handles the number of happy on the reaction box.

    public class HappyNbrView : MonoBehaviour
    {

        private void OnEnable()
        {
            this.GetComponent<Text>().text = Database.NumOfReatcionToNews("Happy", StaticClass.CurrentNewsId);
        }
    }
}
