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

        public void SetJumping()
        {
            _jumpState = JumpState.Jumping;
        }
        
        public void Update(IJumper jumper)
        {
            switch (_jumpState)
            {
                case JumpState.Airborne:
                    if (jumper.IsGrounded())
                    {
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
    }

    public enum JumpState
    {
        Grounded,
        Jumping,
        Airborne
    }
}