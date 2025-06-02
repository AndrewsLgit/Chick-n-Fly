using UnityEngine;

namespace SharedData.Runtime
{
    [CreateAssetMenu(menuName = "Event Channels/Vector2EventChannel")]
    [System.Serializable]
    public class Vector2EventChannel : EventChannel<Vector2> {}
}