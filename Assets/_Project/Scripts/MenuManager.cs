using I2.Loc;
using KansusGames.KansusAnimator.Animator;
using KansusGames.MadCounts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KansusGames.MadCounts.UI
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainPanel;

        [SerializeField]
        private GameObject aboutPanel;

        [SerializeField]
        private Text titleLabel;

        public LocalizationParamsManager bestScoreLabel;
        public LocalizationParamsManager scoreLabel;

        void Start()
        {
            SetupScoreLabel();
            KAnimator mainAnimator = mainPanel.GetComponent<KAnimator>();
            mainAnimator.AnimateIn();
        }

        void SetupScoreLabel()
        {
            scoreLabel.SetParameterValue("Score", GameManager.score.ToString());

            if (GameManager.score > 0)
            {
                if (PlayerPrefs.GetInt("Score", 0) < GameManager.score)
                {
                    PlayerPrefs.SetInt("Score", GameManager.score);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                Destroy(scoreLabel.gameObject);
            }

            bestScoreLabel.SetParameterValue("Highscore", PlayerPrefs.GetInt("Score", 0).ToString());
            GameManager.score = 0;
        }

        public void ShowAboutPanel()
        {
            aboutPanel.SetActive(true);

            KAnimator mainAnimator = mainPanel.GetComponent<KAnimator>();
            KAnimator aboutAnimator = aboutPanel.GetComponent<KAnimator>();

            mainAnimator.AnimateOut();
            aboutAnimator.AnimateIn();
        }

        public void ShowMainPanel()
        {
            KAnimator mainAnimator = mainPanel.GetComponent<KAnimator>();
            KAnimator aboutAnimator = aboutPanel.GetComponent<KAnimator>();

            mainAnimator.AnimateIn();
            aboutAnimator.AnimateOut();
        }

        public void StartGame(int gameDifficulty)
        {
            SceneManager.LoadScene("Game");
            PlayerPrefs.SetInt("Difficulty", gameDifficulty);
        }

        public void SizeTitleToLanguage()
        {
            if (LocalizationManager.CurrentLanguageCode == "pt")
            {
                titleLabel.fontSize = 110;
            }
            else
            {
                titleLabel.fontSize = 150;
            }
        }
    }
}
