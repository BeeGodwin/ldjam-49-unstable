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
        public float moveDamping;
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

            HandleJump();
            HandleMove();
            
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
            var jump = Input.GetButton("Jump");
            if (jump)
                _stateMachine.tryJump(this, jumpForce);
        }

        private void HandleMove()
        {
            // TODO: flip player X if needed

            var xAxis = Input.GetAxis("Horizontal");
            if (Mathf.Abs(xAxis) > 0)
            {
                var moveForce = _stateMachine.GetState() == JumpState.Grounded ? accel : accel * airWalkDamping;
                _rb.AddForce(new Vector2(xAxis, 0) * moveForce * Time.deltaTime); // should this be relative to the raft?
                
            }
            else
            {
                var velocity = _rb.velocity;
                var difference = -velocity.x * moveDamping * Time.deltaTime;
                _rb.velocity = new Vector2(velocity.x + difference, velocity.y);
            }

            if (Mathf.Abs(_rb.velocity.x) > maxSpeed)
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

