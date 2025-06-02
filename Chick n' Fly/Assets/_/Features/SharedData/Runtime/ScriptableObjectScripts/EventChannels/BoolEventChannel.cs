using UnityEngine;

namespace SharedData.Runtime
{
    [CreateAssetMenu(menuName = "Event Channels/Bool EventChannel")]
    [System.Serializable]
    public class BoolEventChannel : EventChannel<bool> {}
}