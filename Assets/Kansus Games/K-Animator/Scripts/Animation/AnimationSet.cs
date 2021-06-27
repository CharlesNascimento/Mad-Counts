using KansusGames.KansusAnimator.Animation.Base;

namespace KansusGames.KansusAnimator.Animation
{
    /// <summary>
    /// Defines a holder for a set of three animations.
    /// </summary>
    /// <typeparam name="T">The model of the entrance animation.</typeparam>
    /// <typeparam name="U">The model of the exit animation.</typeparam>
    /// <typeparam name="V">The model of the idle animation.</typeparam>
    public abstract class AnimationSet<T, U, V>
        where T : Base.Animation
        where U : Base.Animation
        where V : IdleAnimation
    {
        #region Fields

        private T inAnimation;
        private U outAnimation;
        private V pingPongAnimation;

        #endregion

        #region Properties

        /// <summary>
        /// The "in" animation.
        /// </summary>
        public T InAnimation
        {
            get
            {
                return inAnimation;
            }
        }

        /// <summary>
        /// The "out" animation.
        /// </summary>
        public U OutAnimation
        {
            get
            {
                return outAnimation;
            }
        }

        /// <summary>
        /// The "idle" animation.
        /// </summary>
        public V PingPongAnimation
        {
            get
            {
                return pingPongAnimation;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new AnimationSet.
        /// </summary>
        /// <param name="inAnimation">The "in" animation.</param>
        /// <param name="outAnimation">The "out" animation.</param>
        /// <param name="pingPongAnimation">The "idle" animation.</param>
        public AnimationSet(T inAnimation, U outAnimation, V pingPongAnimation)
        {
            this.inAnimation = inAnimation;
            this.outAnimation = outAnimation;
            this.pingPongAnimation = pingPongAnimation;
        }

        #endregion
    }

    #region Implementations

    /// <summary>
    /// Holds a set of three move animations: In, Out and idle.
    /// </summary>
    public class MoveAnimationSet : AnimationSet<MoveInAnimation, MoveOutAnimation, MoveIdleAnimation>
    {
        /// <summary>
        /// Creates a new MoveAnimationSet.
        /// </summary>
        /// <param name="inAnimation">The "in" animation.</param>
        /// <param name="outAnimation">The "out" animation.</param>
        /// <param name="pingPongAnimation">The "idle" animation.</param>
        public MoveAnimationSet(MoveInAnimation inAnimation,
            MoveOutAnimation outAnimation,
            MoveIdleAnimation pingPongAnimation)
            : base(inAnimation, outAnimation, pingPongAnimation)
        {
        }
    }

    /// <summary>
    /// Holds a set of three rotate animations: In, Out and idle.
    /// </summary>
    public class RotateAnimationSet : AnimationSet<RotateInAnimation, RotateOutAnimation, RotateIdleAnimation>
    {
        /// <summary>
        /// Creates a new RotateAnimationSet.
        /// </summary>
        /// <param name="inAnimation">The "in" animation.</param>
        /// <param name="outAnimation">The "out" animation.</param>
        /// <param name="pingPongAnimation">The "idle" animation.</param>
        public RotateAnimationSet(RotateInAnimation inAnimation,
            RotateOutAnimation outAnimation,
            RotateIdleAnimation pingPongAnimation)
            : base(inAnimation, outAnimation, pingPongAnimation)
        {
        }
    }

    /// <summary>
    /// Holds a set of three scale animations: In, Out and idle.
    /// </summary>
    public class ScaleAnimationSet : AnimationSet<ScaleInAnimation, ScaleOutAnimation, ScaleIdleAnimation>
    {
        /// <summary>
        /// Creates a new ScaleAnimationSet.
        /// </summary>
        /// <param name="inAnimation">The "in" animation.</param>
        /// <param name="outAnimation">The "out" animation.</param>
        /// <param name="pingPongAnimation">The "idle" animation.</param>
        public ScaleAnimationSet(ScaleInAnimation inAnimation,
            ScaleOutAnimation outAnimation,
            ScaleIdleAnimation pingPongAnimation)
            : base(inAnimation, outAnimation, pingPongAnimation)
        {
        }
    }

    /// <summary>
    /// Holds a set of three fade animations: In, Out and idle.
    /// </summary>
    public class FadeAnimationSet : AnimationSet<FadeInAnimation, FadeOutAnimation, FadeIdleAnimation>
    {
        /// <summary>
        /// Creates a new FadeAnimationSet.
        /// </summary>
        /// <param name="inAnimation">The "in" animation.</param>
        /// <param name="outAnimation">The "out" animation.</param>
        /// <param name="pingPongAnimation">The "idle" animation.</param>
        public FadeAnimationSet(FadeInAnimation inAnimation,
            FadeOutAnimation outAnimation,
            FadeIdleAnimation pingPongAnimation)
            : base(inAnimation, outAnimation, pingPongAnimation)
        {
        }
    }

    #endregion
}