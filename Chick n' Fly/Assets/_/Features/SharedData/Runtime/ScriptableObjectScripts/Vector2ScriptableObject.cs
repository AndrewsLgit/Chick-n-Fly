using UnityEngine;

namespace SharedData.Runtime
{
    [CreateAssetMenu(fileName = "New Vector2 ScriptableObject", menuName = "SharedData/ScriptableObjects/Vector2Values")]
    public class Vector2ScriptableObject : ScriptableObject
    {
        public Vector2 m_value;
    }
}