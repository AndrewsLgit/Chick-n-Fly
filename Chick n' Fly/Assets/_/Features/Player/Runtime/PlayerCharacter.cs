using System;
using PrimeTween;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerCharacter : BigBrother
    {
        #region Private Variables
        // todo: remove rigidbody and use linearVelocity
        // todo: make player jump towards arrow direction
        // todo: make arrow stop moving when _isJumping -> DONE
        // make arrow disappear after Jumping until Grounded
        // add collider to stick to surfaces
        
        // TODO ????? IMPLEMENT STATE MACHINE FOR ANIMATION TRANSITION AND JUMPING,GROUNDED STATES
        private Rigidbody2D _rigidbody;
        private Tween _tween;
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
        }

        private void Update()
        {
            
            // execute player rotation logic
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

            public void Jump()
            {
                _rigidbody.linearVelocity = _directionArrowPivot.transform.up * _jumpForce;
                //_rigidbody.AddForce(new Vector3(0, _jumpForce, 0), ForceMode2D.Impulse);
            }
        
        #endregion

        #region Utils

        public void SetJumping(bool isJumping) => _isJumping = isJumping;

        #endregion
    }
}