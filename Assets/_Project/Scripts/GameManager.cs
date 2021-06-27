using I2.Loc;
using KansusGames.MadCounts.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KansusGames.MadCounts.Game
{
    public class GameManager : MonoBehaviour
    {
        private const float CHALKBOARD_FRAME_WIDTH = 0.8f;

        public GameObject[] objects;

        [SerializeField]
        private LocalizationParamsManager lostAnswersLabel;

        [SerializeField]
        private Text scoreLabel;

        [SerializeField]
        private LocalizationParamsManager expressionLabel;

        [SerializeField]
        private int maxMissedCorrectAnswers = 5;

        [SerializeField]
        private float spawnInterval = 2.8f;

        [SerializeField]
        private float minSpawnInterval = 2.4f;

        [SerializeField]
        private float spawnIntervalDecrease = 0.025f;

        private int missedCorrectAnswers;

        public static int score;

        private string expression;
        private int evaluation;

        private ExpressionGenerator expressionGenerator;

        private float leftScreenBound;
        private float rightScreenBound;
        private float upperScreenBound = 5.5f;

        private List<Vector3> latestAnswers = new List<Vector3>(5);
        private GameDifficulty gameDifficulty;

        public int Score
        {
            set
            {
                score = value;

                scoreLabel.text = Score.ToString();
            }
            get
            {
                return score;
            }
        }

        public int MissedCorrectAnswers
        {
            set
            {
                missedCorrectAnswers = value;

                lostAnswersLabel.SetParameterValue("LostAnswers", missedCorrectAnswers + "/" + maxMissedCorrectAnswers);

                if (MissedCorrectAnswers >= maxMissedCorrectAnswers)
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
            get
            {
                return missedCorrectAnswers;
            }
        }

        void Start()
        {
            gameDifficulty = (GameDifficulty)PlayerPrefs.GetInt("Difficulty");

            lostAnswersLabel.SetParameterValue("LostAnswers", 0 + "/" + maxMissedCorrectAnswers);

            Vector3 screenWidthWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));
            rightScreenBound = screenWidthWorld.x - CHALKBOARD_FRAME_WIDTH;
            leftScreenBound = -(screenWidthWorld.x - CHALKBOARD_FRAME_WIDTH);

            expressionGenerator = new ExpressionGenerator(GeneratorDataForDifficulty(gameDifficulty));

            score = 0;
            StartSpawning();
        }

        public void StartSpawning()
        {
            if (score != 0 && spawnInterval > minSpawnInterval)
            {
                spawnInterval -= spawnIntervalDecrease;
                Debug.Log("Game got faster. + " + spawnInterval);
            }

            DestroyAllAnswers();
            RefreshExpression();
            StopAllCoroutines();
            StartCoroutine(Spawn(spawnInterval));
        }

        private void SpawnAnswer()
        {
            int answer = Random.Range(0, 5);
            Vector3 position = new Vector3(Random.Range(leftScreenBound, rightScreenBound), upperScreenBound, 0);
            int i = 0;

            while (!IsValidPosition(position) && i < 500)
            {
                position = new Vector3(Random.Range(leftScreenBound, rightScreenBound), upperScreenBound, 0);
                i++;
            }

            if (latestAnswers.Count == 5)
            {
                latestAnswers.RemoveAt(latestAnswers.Count - 1);
            }

            latestAnswers.Insert(0, position);

            if (answer == 0)
            {
                GameObject obj = Instantiate(objects[0], position, Quaternion.identity);
                TextMesh text = obj.GetComponent<TextMesh>();
                text.text = evaluation.ToString();
            }
            else
            {
                GameObject obj = Instantiate(objects[1], position, Quaternion.identity);
                TextMesh text = obj.GetComponent<TextMesh>();
                text.text = CloseNumber(evaluation, gameDifficulty).ToString();
            }
        }

        IEnumerator Spawn(float interval)
        {
            while (true)
            {
                SpawnAnswer();
                yield return new WaitForSeconds(interval);
            }
        }

        public void RefreshExpression()
        {
            var generatedExpression = expressionGenerator.Generate();
            expression = generatedExpression.Expression;
            evaluation = (int)generatedExpression.Result;

            expressionLabel.SetParameterValue("ExpressionToSolve", expression);

            Debug.Log("Expression: " + expression);
            Debug.Log("Evaluation: " + evaluation);
        }

        public void DestroyAllAnswers()
        {
            GameObject[] correctAnswers = GameObject.FindGameObjectsWithTag("CorrectAnswer");

            foreach (GameObject item in correctAnswers)
            {
                Destroy(item);
            }

            GameObject[] wrongAnswers = GameObject.FindGameObjectsWithTag("WrongAnswer");

            foreach (GameObject item in wrongAnswers)
            {
                Destroy(item);
            }
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private int CloseNumber(int number, GameDifficulty gameDifficulty)
        {
            int result;

            do
            {
                result = Random.Range(number - 10, number + 10);
            }
            while (result == number);

            if (result < 0 && gameDifficulty != GameDifficulty.Hard)
            {
                result *= -1;

                if (result == number)
                {
                    result++;
                }
            }

            return result;
        }

        private bool IsValidPosition(Vector3 position)
        {
            foreach (var item in latestAnswers)
            {
                if (position.x <= item.x + 1f && position.x >= item.x - 1f)
                {
                    return false;
                }
            }

            return true;
        }

        private ExpressionGeneratorData GeneratorDataForDifficulty(GameDifficulty gameDifficulty)
        {
            return gameDifficulty switch
            {
                GameDifficulty.VeryEasy => new ExpressionGeneratorData
                {
                    AllowedOperators = new List<string>() { "+", "-" },
                },
                GameDifficulty.Easy => new ExpressionGeneratorData
                {
                    AllowedOperators = new List<string>() { "+", "-", "*" },
                },
                GameDifficulty.Normal => new ExpressionGeneratorData
                {
                    AllowedOperators = new List<string>() { "+", "-", "*", "/" },
                    ForceIntegerResult = true
                },
                GameDifficulty.Hard => new ExpressionGeneratorData
                {
                    AllowedOperators = new List<string>() { "+", "-", "*", "/" },
                    ForceIntegerResult = true,
                    AllowNegativeResults = true
                },
                _ => new ExpressionGeneratorData(),
            };
        }
    }
}
