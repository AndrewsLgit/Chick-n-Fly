using UnityEngine;
using UnityEngine.Events;

namespace SharedData.Runtime
{
    public abstract class EventListener<T> : BigBrother
    {
        #region Private Variables
        
        [SerializeField] private EventChannel<T> _eventChannel;
        [SerializeField] private UnityEvent<T> _unityEvent;
        #endregion
        
        #region Main Methods

        private void Awake()
        {
            _eventChannel.SubscribeToEvent(this);
        }
        private void OnDisable()
        {
            _eventChannel.UnsubscribeFromEvent(this);
        }
        
        public void RaiseEvent(T value)
        {
            _unityEvent?.Invoke(value);
        }
        
        #endregion
    }
    // Empty EventListener for "no type"
    public class EventListener : EventListener<Empty>{}
}