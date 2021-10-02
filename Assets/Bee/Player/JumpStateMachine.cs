using System;
using UnityEngine;

namespace Bee.Player
{
    public class JumpStateMachine
    {
        private JumpState _jumpState;

        public JumpStateMachine()
        {
            _jumpState = JumpState.Grounded;
        }

        public JumpState GetState()
        {
            return _jumpState;
        }

        public void tryJump(IJumper jumper, float jumpForce)
        {
            if (_jumpState == JumpState.Grounded && jumper.IsGrounded())
            {
                _jumpState = JumpState.Airborne;
                jumper.DoJump(jumpForce);
            }
        }

        public void Update(IJumper jumper)
        {
            switch (_jumpState)
            {
                case JumpState.Airborne:
                    if (jumper.IsGrounded())
                    {
                        Debug.Log("Grounded");
                        _jumpState = JumpState.Grounded;
                    }
                    break;
                case JumpState.Jumping:
                    _jumpState = JumpState.Airborne;
                    break;
                case JumpState.Grounded:
                    //noop
                    break;
            }
        }
    }

    public interface IJumper
    {
        public bool IsGrounded();
        public void DoJump(float yVelocity);
        public Vector2 GetVelocity();
    }

    public enum JumpState
    {
        Grounded,
        Jumping,
        Airborne
    }
}