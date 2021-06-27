namespace KansusGames.KansusAnimator.Core
{
    /// <summary>
    /// The tween engine used by the animators.
    /// </summary>
    public enum TweenEngine
    {
#if LEANTWEEN
        LeanTween,
#endif

#if ITWEEN
        iTween,
#endif

#if DOTWEEN
        DOTween
#endif
    }

    /// <summary>
    /// Indicates the animation types that will run automatically
    /// when the scene starts.
    /// </summary>
    public enum InitialAnimationType
    {
        In,
        Idle,
        Out,
        All
    }

    /// <summary>
    /// The context of the animation. 
    /// </summary>
    public enum AnimationContext
    {
        Self,
        Children,
        SelfAndChildren
    }

    /// <summary>
    /// The behavior of the animator if it must start a new animation,
    /// but there is another one currently running in the same game object.
    /// </summary>
    public enum AnimationInterruptionBehaviour
    {
        Interrupt,
        GiveUp
    }

    /// <summary>
    /// The easing function of the animation.
    /// </summary>
    public enum EaseType
    {
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InSine,
        OutSine,
        InOutSine,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        linear,
        spring,
        InBounce,
        OutBounce,
        InOutBounce,
        InBack,
        OutBack,
        InOutBack,
        InElastic,
        OutElastic,
        InOutElastic
    }

    /// <summary>
    /// Preset positions where a move animation can go to or come from.
    /// </summary>
    public enum PresetPosition
    {
        ParentPosition,
        LocalPosition,
        UpperScreenEdge,
        LeftScreenEdge,
        RightScreenEdge,
        BottomScreenEdge,
        UpperLeft,
        UpperCenter,
        UpperRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
        SelfPosition
    }

    /// <summary>
    /// The context in which a move animation is run.
    /// </summary>
    public enum MoveContext
    {
        World,
        Local,
        Self
    }
}
