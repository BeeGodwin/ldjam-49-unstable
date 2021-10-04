using Bee.Game;
using Bee.Sky;
using UnityEngine;

namespace Bee.Raft
{
    public class RaftController : MonoBehaviour, IBlowable, IGameSystem
    {
        private Rigidbody2D _mastRb;

        void Start()
        {
            _mastRb = GetComponentInChildren<Rigidbody2D>();
        }
        
        public void ApplyWindForce(float force)
        {
            _mastRb.AddForce(Vector2.right * force);
        }

        public void PlayGame()
        {
            _mastRb.constraints = RigidbodyConstraints2D.None;
        }

        public void PauseGame()
        {
            _mastRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void ResetGame()
        {
            // noop
        }
    }
}
