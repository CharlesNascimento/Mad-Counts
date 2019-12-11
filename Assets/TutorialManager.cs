using KansusGames.MadCounts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tutorialItems;

    [SerializeField]
    private PauseManager pauseManager;

    private int currentItem;

    private bool viewedTutorial;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            tutorialItems[currentItem].gameObject.SetActive(false);
            currentItem++;

            if(currentItem < tutorialItems.Length)
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
