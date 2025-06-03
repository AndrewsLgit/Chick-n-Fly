using UnityEngine;

namespace Player.Runtime
{
    public class Platform : BigBrother
    {
        #region Private Variables  
  
        [SerializeField] private Collider2D _collider;  
  
        #endregion  
  
        #region Unity API  
  
        void OnCollisionEnter2D(Collision2D collision)  
        {  
            if (collision.gameObject.TryGetComponent<PlayerCharacter>(out PlayerCharacter player))  
            {        
                player.m_currentPlatform = this;  
            }
        }  
  
        void OnCollisionExit2D(Collision2D collision)  
        {  
            if (collision.gameObject.TryGetComponent<PlayerCharacter>(out PlayerCharacter player))  
            {        
                player.m_currentPlatform = null;
            }    
        }  
  
        #endregion
    }
}