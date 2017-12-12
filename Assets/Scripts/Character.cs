using UnityEngine;

public class Character : MonoBehaviour
{
    private const float CHALKBOARD_FRAME_WIDTH = 0.5f;
    
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float speed = 5f;

    private float halfSpriteWidth;
    private float screenCenter;
    private float leftScreenBound;
    private float rightScreenBound;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Vector3 screenWidthWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));
        halfSpriteWidth = spriteRenderer.bounds.size.x / 2f;
        screenCenter = 0;
        rightScreenBound = screenWidthWorld.x - halfSpriteWidth - CHALKBOARD_FRAME_WIDTH;
        leftScreenBound = -(screenWidthWorld.x - halfSpriteWidth - CHALKBOARD_FRAME_WIDTH);
    }

    void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 clickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
			Vector3 clickPositionWorld = Camera.main.ScreenToWorldPoint(clickPosition);
            Vector3 playerPosition = transform.position;

            if (clickPositionWorld.x > screenCenter && playerPosition.x < rightScreenBound)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, playerPosition.y, 0);
                spriteRenderer.flipX = false;
            }
            else if (clickPositionWorld.x < screenCenter && playerPosition.x > leftScreenBound)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, playerPosition.y, 0);
                spriteRenderer.flipX = true;
            }
        }
    }

	void OnTriggerEnter2D(Collider2D col) 
	{
		if (col.tag == "CorrectAnswer")
		{
            audioManager.PlayCorrectSound();
            gameManager.Score++;
            gameManager.StartSpawning();
		}
		else if (col.tag == "WrongAnswer")
		{
            audioManager.PlayWrongSound();
            Application.LoadLevel("MainMenu");
		}
	}
}
