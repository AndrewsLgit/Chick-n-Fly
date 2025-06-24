using System;
using System.Collections;
using System.Collections.Generic;
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
        // public GameObject DebugMarker;

        #endregion
        
        #region Private Variables
        
        // todo: remove rigidbody and use linearVelocity -> DONE
        // todo: make player jump towards arrow direction -> DONE
        // todo: make arrow stop moving when _isJumping -> DONE
        // make arrow disappear after Jumping until Grounded
        // add collider to stick to surfaces -> DONE
        
        // TODO ????? IMPLEMENT STATE MACHINE FOR ANIMATION TRANSITION AND JUMPING,GROUNDED STATES -> DONE ?
        [Header("References")]
        [SerializeField] private GameObject _directionArrowPivot;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float _colliderReactivationTime = 0.2f;
        
        [Header("Platform Tags")]
        [SerializeField] private string _stickyPlatformTag = "Sticky";
        [SerializeField] private string _bubblePlatformTag = "Bubble";
        [SerializeField] private string _unsafePlatformTag = "Unsafe";
        [SerializeField] private string _nestPlatformTag = "Nest";

 
        //[Header("Movement Values")]
        //[SerializeField] private float _speed = 10;
        
        [Header("Rotation Values")]
        // [SerializeField] private float _rotationSpeed = 1.2f;
        // [SerializeField] private float _rotationAngleRight = 0;
        [SerializeField] private float _rotationAngle = 90f;
        [SerializeField] private float _rotationDuration;
        [SerializeField] private float _currentAimDegrees;
        [SerializeField] private float _rotationSpeed;
        // private Vector3 _initialRotation;

        [Header("Jump Values")] 
        [SerializeField] private float _jumpForce = 10f;

        [Header("Event Channels")] 
        [SerializeField] private EventChannel _onPlayerDeath;
        [SerializeField] private EventChannel _onPlayerVictory;
        
        private bool _isGrounded = true;
        
        private Tween _tween;
        private StopwatchTimer _jumpTimer;
        private List<Timer>  _timers = new List<Timer>();
        private bool _jumpStateOnStart = false;
        private bool _movedRight = false;
        private bool _canJumpAgain = false;

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

            if (m_currentPlatform != null)
            {
                HandleCurrentPlatform(m_currentPlatform);
            }
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
                    if (!_jumpStateOnStart)
                    {
                        if (_jumpTimer.IsRunning)
                        {
                            m_currentState = STATE.JUMP;
                            // m_currentPlatform = null;
                            return;
                        }
                        HandleAiming();
                    }
                    //if (_jumpTimer.IsRunning && !_jumpStateStarted) m_currentState =  STATE.POWER;
                    break;
                case STATE.POWER:
                    var degreesToRad = _currentAimDegrees * Mathf.Deg2Rad;
                    Vector2 direction = new Vector2(Mathf.Cos(degreesToRad), Mathf.Sin(degreesToRad));
                    //degreeToRad = aimDegree * mathf.Deg2Rad
                    //vector2 direction = vector2(mathf.cos(degreeToRad), mathf.sin(degreeToRad)
                    // the above only happens once!
                    //todo: maybe have a switch on update to manage states
                    // and a switch statement inside of an input method to change states between power and jump
                    //_tween.Stop();
                    //Tween.StopAll(_directionArrowPivot);
                    //Tween.SetPausedAll(true, _directionArrowPivot);
                    if (_jumpTimer.IsRunning)
                    {
                        // m_currentPlatform = null;
                        m_currentState = STATE.JUMP;
                    }
                    break;
                case STATE.JUMP:
                    // if (_jumpTimer.IsRunning) m_currentPlatform = null;
                    HandleJump();
                    if (!_jumpTimer.IsRunning || m_currentPlatform != null) m_currentState = STATE.AIMING;
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
            private IEnumerator ArrowRotationCoroutine()
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

            private void ArrowRotation()
            {
                // aimDegree is the current degree
                // aimDegree -= rotationSpeed * time.delta
                // if aimDegree < minDegrees -> right = false
                
                // vice versa aimDegree += rotationSpeed * time.delta
                // aimDegree > maxDegrees -> right = true

                switch (_movedRight)
                {
                    case false:
                        _currentAimDegrees = _currentAimDegrees - _rotationSpeed * Time.deltaTime;
                        if (_currentAimDegrees < 0) _movedRight =  false;
                        break;
                    case true:
                        _currentAimDegrees = _currentAimDegrees + _rotationSpeed * Time.deltaTime;
                        if (_currentAimDegrees > _rotationAngle) _movedRight =  true;
                        break;
                }
                _directionArrowPivot.transform.rotation = Quaternion.Euler(0, 0, _currentAimDegrees);
                //
                // switch (_movedRight)
                // {
                //     case false:
                //         _tween = Tween
                //             .Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(0, 0, _rotationAngle),
                //                 duration: _rotationDuration).OnComplete(() => _movedRight = true);
                //         break;
                //     case true:
                //         _tween = Tween
                //             .Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(0, 0, -_rotationAngle),
                //                 duration: _rotationDuration).OnComplete(() => _movedRight = false);
                //         break;
                // }
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
            private IEnumerator ReactivateCollider()
            {
                yield return new WaitForSeconds(_colliderReactivationTime);
                _collider.enabled = true;
            }
            public void OnJump(bool isJumping)
            {
                // if player released the jump key and the timer isn't running yet (isn't in a jump state)
                _jumpStateOnStart = isJumping;
                if (_jumpStateOnStart && !_timers[0].IsRunning)
                {
                    Info($"Started Jump");
                    _jumpTimer.Start();
                    if (m_currentPlatform != null)
                    {
                        _collider.enabled = false;
                        StartCoroutine(ReactivateCollider());
                        m_currentPlatform =  null;
                    }
                    //m_currentPlatform = null;
                    _rigidbody.linearVelocity = _directionArrowPivot.transform.up * _jumpForce;
                }
                _canJumpAgain = false;
                // if player jump timer is running and the player touched the ground -> stop timer
                //if(_jumpTimer.IsRunning /* &&  _groundChecker.IsGrounded*/) _jumpTimer.Stop();
            }
            private void HandleJump()
            {
                // var jumpForce = _jumpForce;
                
                // if (_jumpTimer.IsRunning && !_groundChecker.IsGrounded && !_jumpStateStarted)
                // {
                if (!_jumpTimer.IsRunning) return;
                // DebugMarker?.SetActive(_jumpTimer.IsRunning);
                if (!_canJumpAgain) //&& _jumpTimer.IsRunning)
                {
                    //_rigidbody.linearVelocity = _directionArrowPivot.transform.up * (_jumpForce); 
                    // _rigidbody.linearVelocityX = 0f;
                }
                if (m_currentPlatform != null) // && m_currentPlatform != null)
                {
                    Info("Stopped Jump");
                    _timers[0].Stop();
                    _directionArrowPivot.SetActive(true);
                    _canJumpAgain = true;
                }
                // }
                
            }

            private void HandleAiming()
            {
                // if the player is on the ground and not jumping
                // if (_groundChecker.IsGrounded && !_jumpTimer.IsRunning)
                // {
                    ArrowRotation();
                // }
            }
        
        #endregion

        #region Utils

        //public void SetJumping(bool isJumping) => _isJumping = isJumping;

        private void SetupTimers()
        {
            _jumpTimer = new StopwatchTimer();
            // Disable arrow (direction indicator) when the player jumps
            _jumpTimer.OnTimerStart += () =>
            {
                // m_currentPlatform = null;
                _directionArrowPivot.SetActive(false);
            };
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
        
        private void LookAtTarget(Transform target, Vector2 direction)
        {
            target.rotation = Quaternion.LookRotation(direction);
        }

        private void HandleCurrentPlatform(Platform platform)
        {
            switch (platform.tag)
            {
                case "Sticky": case "Bubble":
                    if (m_currentState == STATE.JUMP) return;
                    transform.position = m_currentPlatform.transform.position;
                    transform.rotation = m_currentPlatform.transform.rotation;
                    break;
                case "Unsafe":
                    OnDeath();
                    break;
                case "Nest":
                    OnVictory();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnDeath()
        {
            _onPlayerDeath?.Invoke(Empty);
            //gameObject.SetActive(false);
        }

        public void OnVictory()
        {
            _onPlayerVictory?.Invoke(Empty); 
            //gameObject.SetActive(false);
        }

        public Empty Empty { get; }

        #endregion
    }
}