using System.Collections.Generic;
using Bee.Game;
using UnityEngine;

namespace Bee.Cargo
{
    public class CargoController : MonoBehaviour, IGameSystem
    {
        private List<Rigidbody2D> _rbs;

        void Start()
        {
            _rbs = new List<Rigidbody2D>(GetComponentsInChildren<Rigidbody2D>());
            // acquire map of GOs to start positions
        }

        public void PlayGame()
        {
            _rbs.ForEach(rb => rb.simulated = true);
        }

        public void PauseGame()
        {
            _rbs.ForEach(rb => rb.simulated = false);
        }

        public void ResetGame()
        {
            // set all RBs to original mapped positions
        }
    }
}
