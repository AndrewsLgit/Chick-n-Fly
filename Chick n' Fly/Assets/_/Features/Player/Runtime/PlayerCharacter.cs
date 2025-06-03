using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.FastExport;
using PrimeTween;
using SharedData.Runtime;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerCharacter : BigBrother
    {
        #region Public Variables

        public enum STATE
        {
            AIMING,
            POWER,
            JUMP,
            IDLE,
        }

        public STATE m_currentState =  STATE.AIMING;
        public Platform m_currentPlatform; //todo: add platform check to JUMP STATE

        #endregion
        #region Private Variables
        // todo: remove rigidbody and use linearVelocity -> DONE
        // todo: make player jump towards arrow direction -> DONE
        // todo: make arrow stop moving when _isJumping -> DONE
        // make arrow disappear after Jumping until Grounded
        // add collider to stick to surfaces
        
        // TODO ????? IMPLEMENT STATE MACHINE FOR ANIMATION TRANSITION AND JUMPING,GROUNDED STATES
        [Header("References")]
        [SerializeField] private GameObject _directionArrowPivot;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private GroundChecker _groundChecker; // todo: change player 2D collider to Capsule Collider 2D

 
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
        private bool _isGrounded = true;
        
        private Tween _tween;
        private StopwatchTimer _jumpTimer;
        private List<Timer>  _timers = new List<Timer>();
        private bool _jumpStateStarted = false;
        private bool _movedRight = false;


        #endregion
        
        #region Unity API

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _groundChecker = GetComponent<GroundChecker>();
            // _initialRotation = transform.localRotation.eulerAngles;
            // _rotationAngleRight = -_rotationAngle;
        }

        private void OnEnable()
        {
            //ArrowRotationLeft();
            SetupTimers();
        }

        private void Update()
        {
            HandleTimers();
            _isGrounded =  _groundChecker.IsGrounded;
            // manage state transitions here
            // switch (m_currentState)
            // {
            //     case STATE.AIMING:
            //         // we are aiming when the player hasn't jumped yet and is grounded
            //         if (_jumpTimer.IsRunning && _jumpStateStarted) m_currentState =  STATE.POWER; // power state means the player arrow stops moving and player jumps on key release
            //         break;
            //     case STATE.POWER:
            //         // we are in power mode when the player presses the jump key but is still grounded
            //         if (_jumpTimer.IsRunning && !_jumpStateStarted) m_currentState =  STATE.JUMP;
            //         break;
            //     case STATE.JUMP:
            //         // we are in the jump state when the jump timer is running and the player is not grounded
            //         if (!_jumpTimer.IsRunning) m_currentState = STATE.AIMING;
            //         break;
            //     case STATE.IDLE:
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }

            switch (m_currentState)
            {
                case STATE.AIMING:
                    if (!_jumpStateStarted)
                    {
                        HandleAiming();
                        if (_jumpTimer.IsRunning) m_currentState = STATE.POWER;
                    }
                    //if (_jumpTimer.IsRunning && !_jumpStateStarted) m_currentState =  STATE.POWER;
                    break;
                case STATE.POWER:
                    _tween.Stop();
                    if (_jumpTimer.IsRunning) m_currentState = STATE.JUMP;
                    break;
                case STATE.JUMP:
                    HandleJump();
                    if (!_jumpTimer.IsRunning) m_currentState = STATE.AIMING;
                    break;
                case STATE.IDLE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FixedUpdate()
        {
            // execute player rotation logic
            // if (_isJumping)
            // {
            //     _tween.Stop();
            //     Jump();
            // }
            
            // manage HandleSTATE() logic here
            // switch (m_currentState)
            // {
            //     case STATE.AIMING:
            //         //todo: add OnEnter and OnExit methods for the states
            //         if (!_jumpStateStarted)
            //         {
            //             HandleAiming();
            //         }
            //         break;
            //     case STATE.POWER:
            //         _tween.Stop();
            //         break;
            //     case STATE.JUMP:
            //         HandleJump();
            //         break;
            //     case STATE.IDLE:
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
        }

        #endregion
        
        #region Public Methods
        
            // Continuous rotation of player arrow
            private IEnumerator ArrowRotation()
            {
                switch (_movedRight)
                {
                    case false:
                        yield return _tween = Tween.Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(0, 0, _rotationAngle), duration: _rotationDuration).OnComplete(() => _movedRight = true);
                        break;
                    case true:
                        yield return _tween =  Tween.Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(0, 0, -_rotationAngle), duration: _rotationDuration).OnComplete(() => _movedRight = false);
                        break;
                    
 
                }
                // if (_isGrounded && !_isJumping)
                // {
                    // if (_isJumping || !_isGrounded)
                    // {
                    //     _tween.Stop();
                    // }
                    // rotate to other side on finish
                    // _tween.OnComplete(ArrowRotationRight); 
                    
                // }
                // _directionArrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                    
            }
            // player arrow rotation to the other side
            private void ArrowRotationRight()
            {
                // if (_isGrounded && !_isJumping)
                //     Tween.Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(0, 0, _rotationAngleRight),
                //         duration: _rotationDuration).OnComplete(ArrowRotationLeft);

                // if (_isGrounded && !_isJumping)
                // {
                    // invert _rotationAngle
                    _tween =  Tween.Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(0, 0, -_rotationAngle), duration: _rotationDuration);
                    // if (_isJumping || !_isGrounded)
                    // {
                    //     _tween.Stop();
                    // }
                    // Rotate to other side on finish
                    //_tween.OnComplete(ArrowRotationLeft);
                // }
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
                _jumpStateStarted = isJumping;
                if(_jumpStateStarted && !_jumpTimer.IsRunning) _jumpTimer.Start();
                // if player jump timer is running and the player touched the ground -> stop timer
                //if(_jumpTimer.IsRunning /* &&  _groundChecker.IsGrounded*/) _jumpTimer.Stop();
            }
            private void HandleJump()
            {
                // var jumpForce = _jumpForce;
                //todo: add ground checker
                
                // if (_jumpTimer.IsRunning && !_groundChecker.IsGrounded && !_jumpStateStarted)
                // {
                _rigidbody.linearVelocity = _directionArrowPivot.transform.up * (_jumpForce); //todo add back Time.deltaTime
                if (_jumpTimer.IsRunning && m_currentPlatform != null)
                {
                    _jumpTimer.Stop();
                    return;
                }
                // }

                
            }

            private void HandleAiming()
            {
                // if the player is on the ground and not jumping
                // if (_groundChecker.IsGrounded && !_jumpTimer.IsRunning)
                // {
                    StartCoroutine(ArrowRotation());
                // }
            }
        
        #endregion

        #region Utils

        //public void SetJumping(bool isJumping) => _isJumping = isJumping;

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