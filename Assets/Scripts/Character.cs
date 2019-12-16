using UnityEngine;
using System.Collections;


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

    [SerializeField]
    private bool invencible = false;

    private float screenCenter;

    private Animator anim;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        Vector3 screenWidthWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));
        screenCenter = 0;       
    }

    void Update()
    {
        //anim.SetInteger("speed", 0);
        Vector3 playerPosition = transform.position;   
        
        if (Input.GetMouseButton(0))
        {
            Vector3 clickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 clickPositionWorld = Camera.main.ScreenToWorldPoint(clickPosition);           

            if (clickPositionWorld.x > screenCenter)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, playerPosition.y, 0);
                spriteRenderer.flipX = false;
                anim.SetInteger("speed", 1);

            }
            else if (clickPositionWorld.x < screenCenter)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, playerPosition.y, 0);
                spriteRenderer.flipX = true;
                anim.SetInteger("speed", 1);
            }

        }
        else
        {

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, playerPosition.y, 0);
                spriteRenderer.flipX = false;
                anim.SetInteger("speed", 1);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, playerPosition.y, 0);
                spriteRenderer.flipX = true;
                anim.SetInteger("speed", 1);
            }
            else
            {
                anim.SetInteger("speed", 0);
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
        else if (col.tag == "WrongAnswer" && !invencible)
        {
            audioManager.PlayWrongSound();
            Application.LoadLevel("MainMenu");
        }
    }
}
