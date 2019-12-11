using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const float CHALKBOARD_FRAME_WIDTH = 0.5f;

    public GameObject[] objects;
    
    [SerializeField]
    private Text lostFoodLabel;

    [SerializeField]
    private Text scoreLabel;

    [SerializeField]
    private Text expressionLabel;
    
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
    private ExpressionEvaluator evaluator;

    private float leftScreenBound;
    private float rightScreenBound;
    private float upperScreenBound = 5.5f;

    private  List<Vector3> lastAnswers = new List<Vector3>(5);

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

            lostFoodLabel.text = "Falhas: " + missedCorrectAnswers + "/" + maxMissedCorrectAnswers;

            if (MissedCorrectAnswers >= maxMissedCorrectAnswers)
            {
                Application.LoadLevel("MainMenu");
            }
        }
        get
        {
            return missedCorrectAnswers;
        }
    }

    void Start()
    {
        lostFoodLabel.text = "Falhas: " + 0 + "/" + maxMissedCorrectAnswers;

        Vector3 screenWidthWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));
        rightScreenBound = screenWidthWorld.x - CHALKBOARD_FRAME_WIDTH;
        leftScreenBound = -(screenWidthWorld.x - CHALKBOARD_FRAME_WIDTH);

        expressionGenerator = new ExpressionGenerator(2, 0, 10);
        evaluator = new ExpressionEvaluator();

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
        Debug.Log("count " + lastAnswers.Count);
        Vector3 position = new Vector3(Random.Range(leftScreenBound, rightScreenBound), upperScreenBound, 0);
        int i = 0;
        while (!IsValidPosition(position) && i < 500)
        {   
            position = new Vector3(Random.Range(leftScreenBound, rightScreenBound), upperScreenBound, 0);
            i++;
            Debug.Log("i " + i);
        }

        if (lastAnswers.Count == 5)
        {
            lastAnswers.RemoveAt(lastAnswers.Count - 1);
        }
       
        lastAnswers.Insert(0, position);

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
            text.text = CloseNumber(evaluation).ToString();
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
        expression = expressionGenerator.Generate();
        evaluation = evaluator.Evaluate(expression);
        expressionLabel.text = "Resolva: " + expression;

        Debug.Log("Expression: " + expression);
        Debug.Log("Evaluation: " + evaluation);
    }

    public void DestroyAllAnswers()
    {
        DestroyCorrectAnswers();
        DestroyWrongAnswers();
    }

    private void DestroyCorrectAnswers()
    {
        GameObject[] answer = GameObject.FindGameObjectsWithTag("CorrectAnswer");

        foreach (GameObject item in answer)
        {
            Destroy(item);
        }
    }

    private void DestroyWrongAnswers()
    {
        GameObject[] answer = GameObject.FindGameObjectsWithTag("WrongAnswer");

        foreach (GameObject item in answer)
        {
            Destroy(item);
        }
    }

    private int CloseNumber(int number)
    {
        int result = 0;

        do
        {
            result = Random.Range(number - 10, number + 10);
        }
        while (result == number);

        return result;
    }

    public void GoToMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    private bool IsValidPosition(Vector3 position)
    {
        foreach (var item in lastAnswers)
        {
            if (position.x <= item.x + 1f && position.x >= item.x - 1f)
            {
                return false;
            }
        }

        return true;
    }
}
