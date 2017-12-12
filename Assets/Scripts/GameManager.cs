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
    private float speed = 2.5f;

    private int missedCorrectAnswers;

    public static int score;

    private string expression;
    private int evaluation;

    private ExpressionGenerator expressionGenerator;
    private ExpressionEvaluator evaluator;

    private float leftScreenBound;
    private float rightScreenBound;
    private float upperScreenBound = 5.5f;

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

            lostFoodLabel.text = "Perdidos: " + missedCorrectAnswers + "/" + maxMissedCorrectAnswers;

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
        lostFoodLabel.text = "Perdidos: " + 0 + "/" + maxMissedCorrectAnswers;

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
        if (Score % 5 == 0 && speed > 0.3f)
        {
            speed -= 0.05f;
            Debug.Log("Game got faster. + " + speed);
        }

        DestroyAllAnswers();
        RefreshExpression();
        Invoke("CreateObjects", 0.5f);
    }

    private void CreateObjects()
    {
        int answer = Random.Range(0, 5);

        if (answer == 0)
        {
            Vector3 position = new Vector3(Random.Range(leftScreenBound, rightScreenBound), upperScreenBound, 0);
            GameObject obj = Instantiate(objects[0], position, Quaternion.identity);
            TextMesh text = obj.GetComponent<TextMesh>();
            text.text = evaluation.ToString();
        }
        else
        {
            Vector3 position = new Vector3(Random.Range(leftScreenBound, rightScreenBound), upperScreenBound, 0);
            GameObject obj = Instantiate(objects[1], position, Quaternion.identity);
            TextMesh text = obj.GetComponent<TextMesh>();
            text.text = CloseNumber(evaluation).ToString();
        }
        
        Invoke("CreateObjects", speed);
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
}
