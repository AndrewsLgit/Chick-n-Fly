using System.Collections.Generic;
using UnityEngine;

namespace SharedData.Runtime
{
    public abstract class EventChannel<T> : ScriptableObject
    {
        #region Private Variables

        private readonly HashSet<EventListener<T>> _listeners = new HashSet<EventListener<T>>();

        #endregion
        
        #region Main Methods
        
        public void SubscribeToEvent(EventListener<T> listener) => _listeners.Add(listener);

        public void UnsubscribeFromEvent(EventListener<T> listener) => _listeners.Remove(listener);

        public void Invoke(T value)
        {
            foreach (var gameEventListener in _listeners)
            {
                gameEventListener.RaiseEvent(value);
            }
        }
        
        #endregion
    }
    
    public readonly struct Empty {}
    
    [CreateAssetMenu(menuName = "Event Channels/EventChannel")]
    public class EventChannel : EventChannel<Empty> { }
}