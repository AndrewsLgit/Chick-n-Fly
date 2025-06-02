using System;
using PrimeTween;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerCharacter : BigBrother
    {
        #region Private Variables

        private Rigidbody _rigidbody;
        [SerializeField] private GameObject _directionArrowPivot;
 
        [Header("Movement Values")]
        [SerializeField] private float _speed = 10;
        
        [Header("Rotation Values")]
        [SerializeField] private float _rotationSpeed = 1.2f;
        [SerializeField] private float _minRotationAngle = 0;
        [SerializeField] private float _maxRotationAngle = 180f;
        [SerializeField] private float _rotationDuration;
        private Vector3 _initialRotation;

        [Header("Jump Values")] 
        [SerializeField] private float _jumpForce = 10f;
        
        

        #endregion
        
        #region Unity API

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _initialRotation = transform.localRotation.eulerAngles;
        }

        private void OnEnable()
        {
            ArrowRotation();
        }

        private void Update()
        {
            // execute player rotation logic
        }

        #endregion
        
        #region Public Methods
        
            // Continuous rotation of player arrow
            private void ArrowRotation()
            {
                // _directionArrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                Tween.Rotation(_directionArrowPivot.transform, endValue: Quaternion.Euler(_maxRotationAngle, 0, 0), duration: _rotationDuration);
            }

            public void Jump()
            {
                //todo: make player jump with _rigidbody.AddForce() with impulse
            }
        
        #endregion
    }
}