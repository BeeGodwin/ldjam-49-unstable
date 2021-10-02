using System;
using UnityEngine;

namespace Bee.Player
{
    public class PlayerController : MonoBehaviour
    {
    
        // make player state machine
        
        // components
        private Rigidbody2D rb;
        private PlayerJumpStateMachine state;
        
        // parameters
        public float maxSpeed;
        public float accel;
        public float jumpForce;

        void Start()
        {
            rb = this.gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>();
            state = new PlayerJumpStateMachine();
        }

        void Update()
        {
            var xAxis = Input.GetAxis("Horizontal");
            var jump = Input.GetButton("Jump");
            
            if (jump) HandleJump();
            
            if (Math.Abs(xAxis) > 0) HandleMove(xAxis);
            
            // we might need a grab key?
        }

        private void HandleJump()
        {
            // consult state machine
            // changes velocity
            rb.AddForce(new Vector2(0, jumpForce));
        }

        private void HandleMove(float xAxis)
        {
            // consult state machine
            // flip player if needed
            
            rb.AddForce(new Vector2(xAxis, 0) * accel); // should this be relative to the raft?
            if (rb.velocity.magnitude > maxSpeed)
            {
                var cappedVelocity = rb.velocity.normalized * maxSpeed;
                rb.velocity = cappedVelocity;
            }
        }
    }
}

