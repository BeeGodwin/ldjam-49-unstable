using System;

namespace Bee.Player
{
    public class PlayerJumpStateMachine
    {
        private JumpState _jumpState;

        public PlayerJumpStateMachine()
        {
            _jumpState = JumpState.Grounded;
        }

        public JumpState GetState()
        {
            return _jumpState;
        }
    }

    public interface IJumper
    {
        public bool IsGrounded();
    }

    public enum JumpState
    {
        Grounded,
        Airborne
    }
}