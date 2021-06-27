using UnityEngine;
using UnityEngine.SceneManagement;

namespace KansusGames.MadCounts.UI
{
    public class BackAndroid : MonoBehaviour
    {
        public string ParentLeveleName;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (ParentLeveleName != "Quit")
                    SceneManager.LoadScene(ParentLeveleName);
                else
                    Application.Quit();
            }
        }
    }
}
