using System;
using UnityEditor;

namespace KansusAnimator.Animators.Inspector
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

            DisallowAttribute attribute = Attribute.GetCustomAttribute(targetBehaviour.GetType(), typeof(DisallowAttribute)) as DisallowAttribute;

            if (attribute != null)
            {
                foreach (AnimatorBehaviour behaviour in behaviours)
                {
                    foreach (Type disallowed in attribute.components)
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
}