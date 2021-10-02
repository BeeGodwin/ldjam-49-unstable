using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bee.Player
{
    public class PlayerController : MonoBehaviour, IJumper
    {
        // dependencies
        private Rigidbody2D _rb;
        private PlayerJumpStateMachine _stateMachine;
        
        // parameters
        public float maxSpeed;
        public float accel;
        public float jumpForce;
        public float airWalkDamping;
        public float groundProbeLength;
        public Transform groundProbePoint;
        public LayerMask jumpMask;
        
        void Start()
        {
            _rb = this.gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>();
            _stateMachine = new PlayerJumpStateMachine();
        }

        void Update()
        {
            _stateMachine.Update(this);
            
            var xAxis = Input.GetAxis("Horizontal");
            var jump = Input.GetButton("Jump");
            
            if (jump) HandleJump();
            
            if (Math.Abs(xAxis) > 0) HandleMove(xAxis);
            
            // we might need a grab key?
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector2 direction = transform.TransformDirection(Vector2.down);
            Gizmos.DrawRay(groundProbePoint.position, direction * groundProbeLength);
        }

        private void HandleJump()
        {
            _stateMachine.tryJump(this, jumpForce);
        }

        private void HandleMove(float xAxis)
        {
            // flip player if needed

            var moveForce = _stateMachine.GetState() == JumpState.Grounded ? accel : accel * airWalkDamping;
            
            _rb.AddForce(new Vector2(xAxis, 0) * moveForce); // should this be relative to the raft?
            if (_rb.velocity.magnitude > maxSpeed)
            {
                var cappedVelocity = _rb.velocity.normalized * maxSpeed;
                _rb.velocity = cappedVelocity;
            }
        }

        public bool IsGrounded()
        {
            return Physics2D.Raycast(groundProbePoint.position, Vector2.down, groundProbeLength, jumpMask).collider != null;
        }

        public void DoJump(float withJumpForce)
        {
            _rb.AddForce(new Vector2(0, withJumpForce));
        }

        public Vector2 GetVelocity()
        {
            return _rb.velocity;
        }
    }
}

