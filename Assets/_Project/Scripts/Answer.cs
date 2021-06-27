using UnityEngine;

namespace KansusGames.MadCounts.Game
{
    public class Answer : MonoBehaviour
    {
        [SerializeField]
        private float speed = 2.5f;

        void FixedUpdate()
        {
            transform.position = new Vector2(
                transform.position.x,
                transform.position.y - speed * Time.deltaTime
            );
        }
    }
}
