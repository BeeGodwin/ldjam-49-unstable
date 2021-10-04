using Bee.Game;
using UnityEngine;

namespace Bee.Cargo
{
    public class CargoController : MonoBehaviour, IGameSystem
    {
        // Start is called before the first frame update
        void Start()
        {
            // acquire list of GOs
            // acquire map of GOs to start positions
        }

        public void PlayGame()
        {
            // play all RBs
        }

        public void PauseGame()
        {
            // pause all RBs
        }

        public void ResetGame()
        {
            // set all RBs to original mapped positions
        }
    }
}
