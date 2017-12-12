namespace KansusAnimator.Animations
{
    /// <summary>
    /// Defines a holder for a set of three animations: In, Out and Ping Pong.
    /// </summary>
    /// <typeparam name="T">The model of the "in" animation.</typeparam>
    /// <typeparam name="U">The model of the "out" animation.</typeparam>
    /// <typeparam name="V">The model of the "ping pong" animation.</typeparam>
    public abstract class AnimationSet<T, U, V>
        where T : Animation
        where U : Animation
        where V : PingPongAnimation
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
        /// The "ping pong" animation.
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
        /// <param name="pingPongAnimation">The "ping pong" animation.</param>
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
    /// Holds a set of three move animations: In, Out and Ping Pong.
    /// </summary>
    public class MoveAnimationSet : AnimationSet<MoveInAnimation, MoveOutAnimation, MovePingPongAnimation>
    {
        /// <summary>
        /// Creates a new MoveAnimationSet.
        /// </summary>
        /// <param name="inAnimation">The "in" animation.</param>
        /// <param name="outAnimation">The "out" animation.</param>
        /// <param name="pingPongAnimation">The "ping pong" animation.</param>
        public MoveAnimationSet(MoveInAnimation inAnimation,
            MoveOutAnimation outAnimation,
            MovePingPongAnimation pingPongAnimation)
            : base(inAnimation, outAnimation, pingPongAnimation)
        {
        }
    }

    /// <summary>
    /// Holds a set of three rotate animations: In, Out and Ping Pong.
    /// </summary>
    public class RotateAnimationSet : AnimationSet<RotateInAnimation, RotateOutAnimation, RotatePingPongAnimation>
    {
        /// <summary>
        /// Creates a new RotateAnimationSet.
        /// </summary>
        /// <param name="inAnimation">The "in" animation.</param>
        /// <param name="outAnimation">The "out" animation.</param>
        /// <param name="pingPongAnimation">The "ping pong" animation.</param>
        public RotateAnimationSet(RotateInAnimation inAnimation,
            RotateOutAnimation outAnimation,
            RotatePingPongAnimation pingPongAnimation)
            : base(inAnimation, outAnimation, pingPongAnimation)
        {
        }
    }

    /// <summary>
    /// Holds a set of three scale animations: In, Out and Ping Pong.
    /// </summary>
    public class ScaleAnimationSet : AnimationSet<ScaleInAnimation, ScaleOutAnimation, ScalePingPongAnimation>
    {
        /// <summary>
        /// Creates a new ScaleAnimationSet.
        /// </summary>
        /// <param name="inAnimation">The "in" animation.</param>
        /// <param name="outAnimation">The "out" animation.</param>
        /// <param name="pingPongAnimation">The "ping pong" animation.</param>
        public ScaleAnimationSet(ScaleInAnimation inAnimation,
            ScaleOutAnimation outAnimation,
            ScalePingPongAnimation pingPongAnimation)
            : base(inAnimation, outAnimation, pingPongAnimation)
        {
        }
    }

    /// <summary>
    /// Holds a set of three fade animations: In, Out and Ping Pong.
    /// </summary>
    public class FadeAnimationSet : AnimationSet<FadeInAnimation, FadeOutAnimation, FadePingPongAnimation>
    {
        /// <summary>
        /// Creates a new FadeAnimationSet.
        /// </summary>
        /// <param name="inAnimation">The "in" animation.</param>
        /// <param name="outAnimation">The "out" animation.</param>
        /// <param name="pingPongAnimation">The "ping pong" animation.</param>
        public FadeAnimationSet(FadeInAnimation inAnimation,
            FadeOutAnimation outAnimation,
            FadePingPongAnimation pingPongAnimation)
            : base(inAnimation, outAnimation, pingPongAnimation)
        {
        }
    }

    #endregion
}