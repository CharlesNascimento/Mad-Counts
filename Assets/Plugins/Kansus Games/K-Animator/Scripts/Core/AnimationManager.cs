using KansusAnimator.Animators;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KansusAnimator
{
    /// <summary>
    /// Global animation manager. Holds the configuration for all the animations in its scene.
    /// Also contains utility methods that can be used before, during or after an animation.
    /// </summary>
    public class AnimationManager : MonoBehaviour
    {
        #region Fields

        private static AnimationManager instance;

        [SerializeField]
        private AnimatorConfiguration configuration;

        #endregion

        #region Properties

        public static AnimationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AnimationManager>();
                    if (Application.isPlaying && (instance != null))
                    {
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }
                return instance;
            }
            set
            {
                instance = Instance;
                if (Application.isPlaying && (instance != null))
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
        }

        public AnimatorConfiguration Configuration
        {
            get
            {
                return configuration;
            }
            set
            {
                configuration = value;
            }
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else if (this != instance)
            {
                Destroy(gameObject);
            }
        }

        #endregion
        
        #region Animation

        /// <summary>
        /// Plays the "in" animation of the game object associated with the given transform.
        /// </summary>
        /// <param name="transform">The transform of the game object to animate.</param>
        /// <param name="animateChildren">Whether the children of the given game object must be animated as well.</param>
        public void AnimateIn(Transform transform, bool animateChildren)
        {
            if (transform.gameObject.activeSelf)
            {
                KAnimator uiAnimator = transform.gameObject.GetComponent<KAnimator>();

                if ((uiAnimator != null) && uiAnimator.enabled)
                {
                    uiAnimator.AnimateIn();
                }

                Button button = transform.gameObject.GetComponent<Button>();

                if (button != null)
                {
                    button.interactable = true;
                }

                if (animateChildren)
                {
                    foreach (Transform childTransform in transform)
                    {
                        AnimateIn(childTransform, animateChildren);
                    }
                }
            }
        }

        /// <summary>
        /// Plays all the "in" animations in the current scene.
        /// </summary>
        public void AnimateInAll()
        {
            if (gameObject.activeSelf)
            {
                KAnimator[] animators = FindObjectsOfType<KAnimator>();
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].AnimateIn(AnimationContext.Self);
                }
            }
        }

        /// <summary>
        /// Plays the "out" animation of the game object associated with the given transform.
        /// </summary>
        /// <param name="transform">The transform of the game object to animate.</param>
        /// <param name="animateChildren">Whether the children of the given game object must be animated as well.</param>
        public void AnimateOut(Transform transform, bool animateChildren)
        {
            if (transform.gameObject.activeSelf)
            {
                KAnimator uiAnimator = transform.gameObject.GetComponent<KAnimator>();
                if ((uiAnimator != null) && uiAnimator.enabled)
                {
                    uiAnimator.AnimateOut();
                }
                Button button = transform.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    button.interactable = false;
                }
                if (animateChildren)
                {
                    foreach (Transform childTransform in transform)
                    {
                        AnimateOut(childTransform, animateChildren);
                    }
                }
            }
        }

        /// <summary>
        /// Plays all the "out" UI animations in the current scene.
        /// </summary>
        public void AnimateOutAll()
        {
            if (gameObject.activeSelf)
            {
                KAnimator[] animators = FindObjectsOfType<KAnimator>();
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].AnimateOut(AnimationContext.Self);
                }
            }
        }

        #endregion

        #region Utility

        /// <summary>
        /// Returns the parent canvas of the given transform.
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public Canvas GetParentCanvas(Transform trans)
        {
            for (Transform transform = trans.parent; transform != null; transform = transform.parent)
            {
                Canvas canvas = transform.gameObject.GetComponent<Canvas>();
                if (canvas != null)
                {
                    return canvas;
                }
            }
            return null;
        }

        #endregion
    }
}