using UnityEngine;

namespace KansusGames.MadCounts.Game
{
    public class DespawnTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private AudioManager audioManager;

        void OnTriggerEnter2D(Collider2D col)
        {
            GameObject other = col.gameObject;

            if (other.CompareTag("CorrectAnswer"))
            {
                gameManager.MissedCorrectAnswers++;
                audioManager.PlayMissedSound();
            }

            if (other.CompareTag("CorrectAnswer") || other.CompareTag("WrongAnswer"))
            {
                Destroy(other);
            }
        }
    }
}
