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
        public float airWalkDamping;
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

        void Update()
        {
            HandleMovement();
            _stateMachine.Update(this);
        }

        void HandleMovement()
        {
            _jumpBounceTimer += Time.deltaTime;
            _velocity = _rb.velocity;
            HandleJump();
            HandleMove();
            _rb.velocity = _velocity;
        }
        
        private void HandleJump()
        {
            var jump = Input.GetButton("Jump");
            if (jump && _jumpBounceTimer >= jumpBounceTime)
                _stateMachine.tryJump(this, jumpYVelocity);
        }

        private void HandleMove()
        {
            var xAxis = Input.GetAxis("Horizontal");
            
            // if (Math.Abs(Mathf.Sign(xAxis) - Mathf.Sign(_rb.velocity.x)) > Mathf.Epsilon) FlipPlayerX();
            
            if (Mathf.Abs(xAxis) > 0)
            {
                var moveForce = _stateMachine.GetState() == JumpState.Grounded ? accel : accel * airWalkDamping;
                var moveDelta = xAxis * moveForce * Time.deltaTime;
                
                _velocity = new Vector2(_velocity.x + moveDelta, _velocity.y);
            }
            else
            {
                var difference = -_velocity.x * moveDamping * Time.deltaTime;
                _velocity = new Vector2(_velocity.x + difference, _velocity.y);
            }

            if (Mathf.Abs(_rb.velocity.x) > maxSpeed)
            {
                var cappedVelocity = _velocity.normalized * maxSpeed;
                _velocity = cappedVelocity;
            }
        }

        private void FlipPlayerX()
        {
            var velocity = _rb.velocity;
            velocity = new Vector2(velocity.x / moveDamping, velocity.y);
            _rb.velocity = velocity;
        }

        public bool IsGrounded()
        {
            return Physics2D.Raycast(groundProbePoint.position, Vector2.down, groundProbeLength, jumpMask).collider != null;
        }

        public void DoJump(float yVelocity)
        {
            _jumpBounceTimer = 0;
            _rb.AddForce(Vector2.up * yVelocity);
            // _velocity = new Vector2(_velocity.x, yVelocity);
        }

        public Vector2 GetVelocity()
        {
            return _rb.velocity;
        }
    }
}

