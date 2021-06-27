using KansusGames.KansusAnimator.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KansusGames.KansusAnimator.Animator
{
    /// <summary>
    /// Holds the configuration of an animator.
    /// </summary>
    [Serializable]
    public class AnimatorConfiguration
    {
        [Header("General")]

        [SerializeField]
        [Tooltip("Specifies whether the game object will be initialy in position for the" +
            " entrance animation.")]
        public bool startOut = false;

        [SerializeField]
        public bool hideAfterOutAnimation = false;

        [SerializeField]
        [Tooltip("Indicates whether the idle animation should start automatically after" +
            " the end of the entrance animation.")]
        public bool idleAfterInAnimation = true;

        [SerializeField]
        [Tooltip("The behavior of the animator if it must start a new animation, but there is another " +
            "one currently running in the same game object.")]
        public AnimationInterruptionBehaviour interruptionBehaviour = AnimationInterruptionBehaviour.Interrupt;

        [SerializeField]
        public List<Button> buttonsToDeactivateDuringAnimation;

        [Header("Start animation")]

        [SerializeField]
        [Tooltip("Indicates whether the animator will start automatically when added to the scene.")]
        public bool startAutomatically;

        [SerializeField]
        [Tooltip("If the animator is set to start automatically, this indicates the animation types that will run.")]
        public InitialAnimationType initialAnimationType = InitialAnimationType.All;

        [SerializeField]
        [Tooltip("The duration of the initial idle animation, if the animator is set to start automatically.")]
        public float initialAnimationIdleDuration = 5f;
    }
}