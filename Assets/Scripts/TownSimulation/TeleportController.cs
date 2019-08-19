using UnityEngine;

namespace Assets.Scripts.TownSimulation
{
    /// <summary>
    /// Activate or desactivate teleport when you are enter or exit a news.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class TeleportController : MonoBehaviour
    {
        public GameObject Teleport; // Reference to teleport gameobject.

        /// <summary>
        /// Call by NewsSphere script to activate/deactivate teleport when exit/enter a news.
        /// </summary>
        public void ChangeTeleport(bool change)
        {
            Teleport.SetActive(change);
        }
    }
}

