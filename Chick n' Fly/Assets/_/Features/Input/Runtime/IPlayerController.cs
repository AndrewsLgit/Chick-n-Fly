using SharedData.Runtime;
using UnityEngine;

namespace Input.Runtime
{
    public interface IPlayerController //: GameInputSystem.IPlayerActions
    {
        public Vector2 MoveInput { get; }
        public Vector2EventChannel OnPlayerMove { get; }
        
        //public Vector2ScriptableObject PlayerInputVector2 { get; }
       
        // ToDo: remove commented code if useless
        // public void SubToMoveEvent(Vector2 moveInput);
        // public void UnsubFromMoveEvent();
    }
}