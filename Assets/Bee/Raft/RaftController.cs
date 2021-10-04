using System.Collections.Generic;
using Bee.Game;
using Bee.Sky;
using UnityEngine;

namespace Bee.Raft
{
    public class RaftController : MonoBehaviour, IBlowable, IGameSystem
    {
        private List<Rigidbody2D> _rbs;

        void Start()
        {
            _rbs = new List<Rigidbody2D>
            {
                GameObject.Find("Mast").GetComponent<Rigidbody2D>(),
                GameObject.Find("RaftBody").GetComponent<Rigidbody2D>()
            };
            _rbs.ForEach(rb => rb.simulated = false);
        }
        
        public void ApplyWindForce(float force)
        {
            _rbs[0].AddForce(Vector2.right * force);
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
            // noop
        }
    }
}
