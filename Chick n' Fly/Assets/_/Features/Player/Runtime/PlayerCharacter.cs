using System;
using PrimeTween;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerCharacter : BigBrother
    {
        #region Private Variables

        private Rigidbody _rigidbody;
        private Tween _tween;
        [SerializeField] private GameObject _directionArrowPivot;
 
        [Header("Movement Values")]
        [SerializeField] private float _speed = 10;
        
        [Header("Rotation Values")]
        // [SerializeField] private float _rotationSpeed = 1.2f;
        [SerializeField] private float _rotationAngleRight = 0;
        [SerializeField] private float _rotationAngleLeft = 90f;
        [SerializeField] private float _rotationDuration;
        private Vector3 _initialRotation;

        [Header("Jump Values")] 
        [SerializeField] private float _jumpForce = 10f;
        private bool _isJumping = false;
        private bool _isGrounded = true;

        #endregion
        
        #region Unity API

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _initialRotation = transform.localRotation.eulerAngles;
            _rotationAngleRight = -_rotationAngleLeft;
        }

        private void OnEnable()
        {
            ArrowRotationLeft();
        }

        private void Update()
        {
            
            // execute player rotation logic
        }

        private void FixedUpdate()
        {
            if (_isJumping)
            {
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
                    _tween = Tween.Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(0, 0, _rotationAngleLeft), duration: _rotationDuration);
                    if (_isJumping || !_isGrounded)
                    {
                        _tween.Stop();
                    }
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
                    _tween =  Tween.Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(0, 0, _rotationAngleRight), duration: _rotationDuration);
                    if (_isJumping || !_isGrounded)
                    {
                        _tween.Stop();
                    }
                    _tween.OnComplete(ArrowRotationLeft);
                }
            }

            public void Jump()
            {
                _rigidbody.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
                //todo: make player jump with _rigidbody.AddForce() with impulse
            }
        
        #endregion

        #region Utils

        public void SetJumping(bool isJumping) => _isJumping = isJumping;

        #endregion
    }
}