using System;
using Bee.Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bee.Player
{
    public class PlayerController : MonoBehaviour, IJumper, IGameSystem
    {
        // dependencies
        private Rigidbody2D _rb;
        private JumpStateMachine _stateMachine;
        private SpriteRenderer _rend;
        private Animator _anim;

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

        // public AnimationClip idleAnim;
        // public AnimationClip jumpAnim;
        // public AnimationClip runAnim;
        
        // memo
        private Vector2 _velocity;
        private float _jumpBounceTimer;
        private Vector2 _startPos;
        // private bool _facingRight = true;
        private static readonly int Velocity = Animator.StringToHash("velocity");
        private static readonly int Jump = Animator.StringToHash("jump");
        private static readonly int Landed = Animator.StringToHash("landed");

        void Start()
        {
            var body = gameObject.transform.GetChild(0);
            _rb = body.GetComponent<Rigidbody2D>();
            _rend = body.GetComponent<SpriteRenderer>();
            _anim = body.GetComponent<Animator>();
            _startPos = _rb.transform.position;
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
            HandleFacing(x);
            _rb.velocity = new Vector2(x, y);
            _anim.SetFloat(Velocity, _rb.velocity.magnitude);
        }

        void HandleFacing(float xVelocity)
        {
            if (!_rend.flipX && xVelocity < 0 || _rend.flipX && xVelocity > 0)
            {
                _rend.flipX = !_rend.flipX;
            }
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
                _anim.ResetTrigger(Landed);
                _anim.SetTrigger(Jump);
                _jumpBounceTimer = jumpBounceTime;
                return jumpYVelocity;
            }

            return _velocity.y;
        }
        public bool IsGrounded()
        {
            return Physics2D.Raycast(groundProbePoint.position, Vector2.down, groundProbeLength, jumpMask).collider != null;
        }

        public void HasLanded()
        {
            _anim.ResetTrigger(Jump);
            _anim.SetTrigger(Landed);
        }

        public void PlayGame()
        {
            _rb.simulated = true;
        }

        public void PauseGame()
        {
            _rb.simulated = false;
        }

        public void ResetGame()
        {
            Debug.Log("reset player");
            _rb.transform.position = _startPos;
            _anim.ResetTrigger(Jump);
            _anim.ResetTrigger(Landed);
        }
    }
}

