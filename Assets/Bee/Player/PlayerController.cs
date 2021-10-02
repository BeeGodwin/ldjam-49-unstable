using System;
using UnityEngine;

namespace Bee.Player
{
    public class PlayerController : MonoBehaviour, IJumper
    {
        // components
        private Rigidbody2D rb;
        private PlayerJumpStateMachine stateMachine;
        
        // parameters
        public float maxSpeed;
        public float accel;
        public float jumpForce;
        public float airWalkDamping;

        void Start()
        {
            rb = this.gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>();
            stateMachine = new PlayerJumpStateMachine();
        }

        void Update()
        {
            stateMachine.Update(this);
            
            var xAxis = Input.GetAxis("Horizontal");
            var jump = Input.GetButton("Jump");
            
            if (jump) HandleJump();
            
            if (Math.Abs(xAxis) > 0) HandleMove(xAxis);
            
            // we might need a grab key?
        }

        private void HandleJump()
        {
            stateMachine.tryJump(this, jumpForce);
        }

        private void HandleMove(float xAxis)
        {
            // flip player if needed

            var moveForce = stateMachine.GetState() == JumpState.Grounded ? jumpForce : jumpForce * airWalkDamping;
            
            rb.AddForce(new Vector2(xAxis, 0) * moveForce); // should this be relative to the raft?
            if (rb.velocity.magnitude > maxSpeed)
            {
                var cappedVelocity = rb.velocity.normalized * maxSpeed;
                rb.velocity = cappedVelocity;
            }
        }

        public bool IsGrounded()
        {
            return true;
        }

        public void doJump(float withJumpForce)
        {
            rb.AddForce(new Vector2(0, withJumpForce));
        }
    }
}

