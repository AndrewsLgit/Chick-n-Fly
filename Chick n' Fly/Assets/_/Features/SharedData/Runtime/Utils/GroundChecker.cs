using UnityEngine;

namespace SharedData.Runtime 
{
    public class GroundChecker : BigBrother
    {
        [SerializeField] float _groundDistance = 0.1f;
        [SerializeField] LayerMask _groundLayers;
        
        public bool IsGrounded { get; private set; }

        private void Update() {
            // changed direction: to new Vector 3 right and up
            IsGrounded = Physics.SphereCast(transform.position, _groundDistance, new Vector3(1,1,0), out var _ , _groundDistance, _groundLayers);
            if (IsGrounded) Info($"Currently grounded: {IsGrounded}");
        }
    } 
}