using UnityEngine;

public class DespawnTrigger : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private AudioManager audioManager;

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject other = col.gameObject;
        
        if (other.tag == "CorrectAnswer")
        {
            gameManager.MissedCorrectAnswers++;
            audioManager.PlayMissedSound();
        }

        Destroy(other);
    }
}
