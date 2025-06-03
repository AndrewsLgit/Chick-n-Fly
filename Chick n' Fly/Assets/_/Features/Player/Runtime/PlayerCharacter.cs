using System;
using System.Collections.Generic;
using PrimeTween;
using SharedData.Runtime;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerCharacter : BigBrother
    {
        #region Private Variables
        // todo: remove rigidbody and use linearVelocity -> DONE
        // todo: make player jump towards arrow direction -> DONE
        // todo: make arrow stop moving when _isJumping -> DONE
        // make arrow disappear after Jumping until Grounded
        // add collider to stick to surfaces
        
        // TODO ????? IMPLEMENT STATE MACHINE FOR ANIMATION TRANSITION AND JUMPING,GROUNDED STATES
        [Header("References")]
        [SerializeField] private GameObject _directionArrowPivot;
 
        [Header("Movement Values")]
        [SerializeField] private float _speed = 10;
        
        [Header("Rotation Values")]
        // [SerializeField] private float _rotationSpeed = 1.2f;
        // [SerializeField] private float _rotationAngleRight = 0;
        [SerializeField] private float _rotationAngle = 90f;
        [SerializeField] private float _rotationDuration;
        // private Vector3 _initialRotation;

        [Header("Jump Values")] 
        [SerializeField] private float _jumpForce = 10f;
        private bool _isJumping = false;
        private bool _isGrounded = true;
        
        private Rigidbody2D _rigidbody;
        private Tween _tween;
        private StopwatchTimer _jumpTimer;
        private List<Timer>  _timers = new List<Timer>();

        #endregion
        
        #region Unity API

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            // _initialRotation = transform.localRotation.eulerAngles;
            // _rotationAngleRight = -_rotationAngle;
        }

        private void OnEnable()
        {
            ArrowRotationLeft();
            SetupTimers();
        }

        private void Update()
        {
            HandleTimers();
        }

        private void FixedUpdate()
        {
            // execute player rotation logic
            if (_isJumping)
            {
                _tween.Stop();
                Jump();
            }
        }

        #endregion
        
        #region Public Methods
        
            // Continuous rotation of player arrow
            private void ArrowRotationLeft()
            {
                if (_isGrounded && !_isJumping)
                {
                    _tween = Tween.Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(0, 0, _rotationAngle), duration: _rotationDuration);
                    // if (_isJumping || !_isGrounded)
                    // {
                    //     _tween.Stop();
                    // }
                    // rotate to other side on finish
                    _tween.OnComplete(ArrowRotationRight); 
                    
                }
                // _directionArrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                    
            }
            // player arrow rotation to the other side
            private void ArrowRotationRight()
            {
                // if (_isGrounded && !_isJumping)
                //     Tween.Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(0, 0, _rotationAngleRight),
                //         duration: _rotationDuration).OnComplete(ArrowRotationLeft);

                if (_isGrounded && !_isJumping)
                {
                    // invert _rotationAngle
                    _tween =  Tween.Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(0, 0, -_rotationAngle), duration: _rotationDuration);
                    // if (_isJumping || !_isGrounded)
                    // {
                    //     _tween.Stop();
                    // }
                    // Rotate to other side on finish
                    _tween.OnComplete(ArrowRotationLeft);
                }
            }

            //DEPRECATED
            public void Jump()
            {
                _rigidbody.linearVelocity = _directionArrowPivot.transform.up * _jumpForce;
                //_rigidbody.AddForce(new Vector3(0, _jumpForce, 0), ForceMode2D.Impulse);
            }

            public void OnJump(bool isJumping)
            {
                // if player released the jump key and the timer isn't running yet (isn't in a jump state)
                // todo: add ground checker
                if(!isJumping && !_jumpTimer.IsRunning) _jumpTimer.Start();
            }
            private void HandleJump()
            {
                // var jumpForce = _jumpForce;
                //todo: add ground checker
                if (_jumpTimer.IsRunning)
                {
                    _rigidbody.linearVelocity = _directionArrowPivot.transform.up * (_jumpForce * Time.deltaTime);
                }
            }
        
        #endregion

        #region Utils

        public void SetJumping(bool isJumping) => _isJumping = isJumping;

        private void SetupTimers()
        {
            _jumpTimer = new StopwatchTimer();
            // Disable arrow (direction indicator) when the player jumps
            _jumpTimer.OnTimerStart += () => _directionArrowPivot.SetActive(false);
            _jumpTimer.OnTimerStop += () => _directionArrowPivot.SetActive(true);
            
            _timers.Add(_jumpTimer);
        }

        private void HandleTimers()
        {
            foreach (var timer in _timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        #endregion
    }
}