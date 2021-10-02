using System;
using UnityEngine;

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
                _jumpState = JumpState.Airborne;
                player.DoJump(jumpForce);
            }
        }

        public void Update(IJumper player)
        {
            switch (_jumpState)
            {
                case JumpState.Airborne:
                    if (player.GetVelocity().y <= 0 && player.IsGrounded())
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
        public void DoJump(float withJumpForce);
        public Vector2 GetVelocity();
    }

    public enum JumpState
    {
        Grounded,
        Airborne
    }
}