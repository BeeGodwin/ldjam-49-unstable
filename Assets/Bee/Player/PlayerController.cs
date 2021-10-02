using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bee.Player
{
    public class PlayerController : MonoBehaviour, IJumper
    {
        // dependencies
        private Rigidbody2D _rb;
        private JumpStateMachine _stateMachine;
        
        // parameters
        public float maxSpeed;
        public float accel;
        public float jumpYVelocity;
        public float jumpBounceTime;
        public float moveDamping;
        public float airWalkFactor;
        public float turningFactor;
        public float groundProbeLength;
        public Transform groundProbePoint;
        public LayerMask jumpMask;
        
        // memo
        private Vector2 _velocity;
        private float _jumpBounceTimer;
        
        void Start()
        {
            _rb = this.gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>();
            _stateMachine = new JumpStateMachine();
            _jumpBounceTimer = 0;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector2 direction = transform.TransformDirection(Vector2.down);
            Gizmos.DrawRay(groundProbePoint.position, direction * groundProbeLength);
        }

        void FixedUpdate()
        {
            HandleMovement();
            _stateMachine.Update(this);
        }

        void HandleMovement()
        {
            _velocity = _rb.velocity;
            var x = VelocityXFromMove();
            var y = VelocityYFromJump();
            _rb.velocity = new Vector2(x, y);
        }

        float VelocityXFromMove()
        {
            var xAxis = Input.GetAxis("Horizontal");

            float x = 0;
            
            if (Mathf.Abs(xAxis) > 0)
            {
                var moveForce = _stateMachine.GetState() == JumpState.Grounded ? accel : accel * airWalkFactor;
                var moveDelta = xAxis * moveForce * Time.deltaTime;
                
                if (Math.Abs(Mathf.Sign(xAxis) - Mathf.Sign(_velocity.x)) > Mathf.Epsilon) moveDelta *= turningFactor;

                x = _velocity.x + moveDelta;
            }
            else
            {
                var damping = _velocity.x * moveDamping * Time.deltaTime;
                x = _velocity.x - damping;
            }

            if (Mathf.Abs(x) > maxSpeed)
            {
                x = x > 0 ? maxSpeed : -maxSpeed;
            }
            
            return x;
        }

        private float VelocityYFromJump()
        {
            _jumpBounceTimer -= Time.deltaTime;
            
            if (!Input.GetButton("Jump")) return _velocity.y;

            if (_jumpBounceTimer <= 0 && _stateMachine.GetState() == JumpState.Grounded)
            {
                _stateMachine.SetJumping();
                _jumpBounceTimer = jumpBounceTime;
                return jumpYVelocity;
            }

            return _velocity.y;
        }
        public bool IsGrounded()
        {
            return Physics2D.Raycast(groundProbePoint.position, Vector2.down, groundProbeLength, jumpMask).collider != null;
        }
    }
}

