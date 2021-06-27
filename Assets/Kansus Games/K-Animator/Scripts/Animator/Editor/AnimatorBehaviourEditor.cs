using KansusGames.KansusAnimator.Attribute;
using System;
using UnityEditor;

namespace KansusGames.KansusAnimator.Animator.Inspector
{
    /// <summary>
    /// Custom editor for the components that inherit from AnimatorBehaviour.
    /// </summary>
    [CustomEditor(typeof(AnimatorBehaviour), true)]
    class AnimatorBehaviourEditor : UnityEditor.Editor
    {

        private void OnEnable()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

            AnimatorBehaviour targetBehaviour = serializedObject.targetObject as AnimatorBehaviour;

            AnimatorBehaviour[] behaviours = targetBehaviour.gameObject.GetComponents<AnimatorBehaviour>();

            var attribute = System.Attribute.GetCustomAttribute(targetBehaviour.GetType(), typeof(DisallowAttribute));

            if (attribute == null)
            {
                return;
            }

            foreach (AnimatorBehaviour behaviour in behaviours)
            {
                foreach (Type disallowed in (attribute as DisallowAttribute).components)
                {
                    if (behaviour.GetType() == disallowed)
                    {
                        EditorUtility.DisplayDialog(
                            "Invalid operation.",
                            "Specific animators cannot coexist with K-Animator!",
                            "OK"
                        );

                        DestroyImmediate(targetBehaviour);
                        return;
                    }
                }
            }
        }
    }
}