using Bee.Sky;
using UnityEngine;

namespace Bee.Raft
{
    public class RaftController : MonoBehaviour, IBlowable
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
    }
}
