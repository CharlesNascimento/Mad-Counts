using UnityEngine;

namespace KansusGames.MadCounts.Util
{
    /// <summary>
    /// This script aligns an object to either the left or right edge of the screen
    /// </summary>
    public class ScreenAligner : MonoBehaviour
    {
        [SerializeField]
        private AlignSide alignTo = AlignSide.Left;

        [SerializeField]
        private Vector3 offset = Vector3.zero;

        void Start()
        {
            var renderer = GetComponent<Renderer>();
            float width = 0;

            if (renderer != null)
            {
                width = renderer.bounds.size.x;
            }

            if (alignTo == AlignSide.Left)
            {
                transform.position = new Vector3(
                    Camera.main.ScreenToWorldPoint(Vector3.zero).x + (width / 2),
                    transform.position.y,
                    transform.position.z
                );
            }
            else if (alignTo == AlignSide.Right)
            {
                transform.position = new Vector3(
                    Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x - (width / 2),
                    transform.position.y,
                    transform.position.z
                );
            }

            transform.position += offset;
        }

        public enum AlignSide
        {
            Up, Down, Left, Right
        }
    }
}
