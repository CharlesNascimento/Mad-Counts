using KansusAnimator.Animators;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel;

    [SerializeField]
    private GameObject aboutPanel;

    public Text bestScoreLabel;
    public Text scoreLabel;

    // Use this for initialization
    void Start()
    {
        SetupScoreLabel();
        KAnimator mainAnimator = mainPanel.GetComponent<KAnimator>();
        mainAnimator.AnimateIn();
    }

    void SetupScoreLabel()
    {
        scoreLabel.text = "Score: " + GameManager.score.ToString();

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
            Destroy(scoreLabel);
        }

        bestScoreLabel.text = "HighScore: " + PlayerPrefs.GetInt("Score", 0).ToString();
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

    public void StartGame()
    {
        Application.LoadLevel("Game");
    }
}
