using UnityEngine;

namespace KansusGames.MadCounts.Game
{
    /// <summary>
    /// Unity implementation of a pause manager.
    /// </summary>
    public class PauseManager : MonoBehaviour
    {  
       
        public void Pause(bool showMenu)
        {
            Time.timeScale = 0;
            
            if (showMenu)
            {

            }
        }
        
        public void Unpause()
        {
            Time.timeScale = 1;
        }
       
        public void TogglePause()
        {
            if (IsPaused()) Unpause(); else Pause(true);
        }
       
        public bool IsPaused()
        {
            return Time.timeScale == 0;
        }            
    }
}
