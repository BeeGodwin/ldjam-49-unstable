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

        public void tryJump(IJumper player, float jumpForce)
        {
            if (_jumpState == JumpState.Grounded && player.IsGrounded())
            {
                player.doJump(jumpForce);
                _jumpState = JumpState.Airborne;
            }
        }

        public void Update(IJumper player)
        {
            switch (_jumpState)
            {
                case JumpState.Airborne:
                    if (player.IsGrounded())
                    {
                        _jumpState = JumpState.Grounded;
                    }
                    break;
                case JumpState.Grounded:
                    //noop
                    break;
                default:
                    break;
            }
        }
    }

    public interface IJumper
    {
        public bool IsGrounded();
        public void doJump(float withJumpForce);
    }

    public enum JumpState
    {
        Grounded,
        Airborne
    }
}