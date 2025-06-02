using SharedData.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input.Runtime
{
    public class PlayerController : BigBrother, IPlayerController, GameInputSystem.IPlayerActions
    {
        #region Public Members
        
        // BoolEventChannel OnPlayerJump { get; }
        //public Vector2ScriptableObject PlayerInputVector2 { get; }

        #endregion

        #region Private Variables

        private GameInputSystem _gameInputSystem;
        private bool _isJumping = false;
        [Header("Movement Events")] 
        [SerializeField] private BoolEventChannel _onPlayerJump;

        #endregion
        
        #region Unity API
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            _gameInputSystem = new GameInputSystem();
            _gameInputSystem.Enable();
            // todo: fix this shit
            _gameInputSystem.Player.SetCallbacks(this);
        }

        private void OnDisable()
        {
            _gameInputSystem.Disable();
        }

        // Update is called once per frame
        void Update()
        {
            // _rigidbody.linearVelocity = transform.forward * _speed;
        }
        
        #endregion
        
        #region Main Methods

        public void OnJump(InputAction.CallbackContext context)
        {
            // send event when player jumps
            if (context.performed)
            {
                _isJumping = true;
                _onPlayerJump?.Invoke(_isJumping);
            }
            else if (context.canceled)
            {
                _isJumping = false;
                _onPlayerJump?.Invoke(_isJumping);
            }
        }

        // only rotate target using movement input
        // send player input value to scriptable object for the Player script
        // invoke OnPlayerMove event for game manager to get player input
        // public void OnMove(InputAction.CallbackContext context)
        // {
        //     if (!context.performed) return;
        //     var moveInput = context.ReadValue<Vector2>();
        //     
        //     // OnPlayerMoveInputValue.m_value = moveInput;
        //     // OnPlayerMove.Invoke(OnPlayerMoveInputValue.m_value);
        //     
        //     //PlayerInputVector2.m_value = moveInput;
        //     // Remove next line and use player input inside PlayerCharacter script
        //     this.transform.Rotate(new Vector3(0, moveInput.x, 0));
        // }

        // ToDo: remove following code if useless
        // public void SubToMoveEvent(Vector2 moveInput)
        // {
        // }
        // public void UnsubFromMoveEvent()
        // {
        // }
        
        #endregion
        
    }
}
