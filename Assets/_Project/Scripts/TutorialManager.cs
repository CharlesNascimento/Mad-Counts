using UnityEngine;

namespace KansusGames.MadCounts.Game
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] tutorialItems;

        [SerializeField]
        private PauseManager pauseManager;

        private int currentItem;

        private bool viewedTutorial;

        void Start()
        {
            viewedTutorial = PlayerPrefs.GetInt("ViewedTutorial", 0) == 1;

            if (viewedTutorial)
            {
                gameObject.SetActive(false);
            }
            else
            {
                pauseManager.Pause(false);
                tutorialItems[currentItem].gameObject.SetActive(true);
            }
        }

        void Update()
        {
            if (Input.anyKeyDown)
            {
                tutorialItems[currentItem].gameObject.SetActive(false);
                currentItem++;

                if (currentItem < tutorialItems.Length)
                {
                    tutorialItems[currentItem].gameObject.SetActive(true);
                }
                else
                {
                    PlayerPrefs.SetInt("ViewedTutorial", 1);
                    pauseManager.Unpause();
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
