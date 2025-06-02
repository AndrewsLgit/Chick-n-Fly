using UnityEngine;

public class BigBrother : MonoBehaviour
{
   #region Debug

   [SerializeField, Header("Debug")]
   protected bool m_isVerbose;
   //public GameObject m_prefab; //on validate

   protected void Info(string message)
   {
      if (!m_isVerbose) return;
      Debug.Log($"FROM: {this} | INFO: {message}");
   }

   protected void Warning(string message)
   {
      if (!m_isVerbose) return;
      Debug.LogWarning($"FROM: {this} | WARNING: {message}");
   }
   
   #endregion
}
