using KansusGames.KansusAnimator.Animation;
using KansusGames.KansusAnimator.Animation.Base;
using KansusGames.KansusAnimator.Core;
using System;
using UnityEditor;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animator.Inspector
{
    /// <summary>
    /// Custom editor for the KAnimator component.
    /// </summary>
    [CustomEditor(typeof(KAnimator))]
    public class KAnimatorInspector : UnityEditor.Editor
    {
        KAnimator animator;

        public GUISkin kSkin;
        GUILayoutOption[] deleteButtonOptions = { GUILayout.Width(16), GUILayout.Height(16) };
        private Texture2D tex;

        void OnEnable()
        {
            animator = (serializedObject.targetObject as KAnimator);
            tex = EditorGUIUtility.LoadRequired("Kansus Games/K-Animator/K-Animator.png") as Texture2D;
        }

        public override void OnInspectorGUI()
        {
            GUI.skin = kSkin;

            var rect = GUILayoutUtility.GetRect(0f, 0f, GUILayout.ExpandWidth(true));
            rect.height = 128;
            GUILayout.Space(rect.height);
            GUI.DrawTexture(rect, tex, ScaleMode.ScaleToFit);

            serializedObject.Update();

            var configuration = serializedObject.FindProperty("configuration");

            EditorGUILayout.PropertyField(configuration.FindPropertyRelative("startOut"));
            EditorGUILayout.PropertyField(configuration.FindPropertyRelative("hideAfterOutAnimation"));
            EditorGUILayout.PropertyField(configuration.FindPropertyRelative("idleAfterInAnimation"));
            EditorGUILayout.PropertyField(configuration.FindPropertyRelative("interruptionBehaviour"));
            EditorGUILayout.PropertyField(configuration.FindPropertyRelative("buttonsToDeactivateDuringAnimation"), true);

            var startAutomaticallyProperty = configuration.FindPropertyRelative("startAutomatically");
            EditorGUILayout.PropertyField(startAutomaticallyProperty);

            if (startAutomaticallyProperty.boolValue)
            {
                var initialAnimationTypeProperty = configuration.FindPropertyRelative("initialAnimationType");
                var initialAnimationType = (InitialAnimationType)initialAnimationTypeProperty.intValue;

                EditorGUILayout.PropertyField(initialAnimationTypeProperty);

                if (initialAnimationType == InitialAnimationType.All)
                {
                    EditorGUILayout.PropertyField(configuration.FindPropertyRelative("initialAnimationIdleDuration"));
                }
            }

            EditorGUILayout.LabelField("Animations", EditorStyles.boldLabel);

            ShowAssignedFields();

            EditorGUILayout.Space();

            var anEvent = Event.current;

            GUI.backgroundColor = new Color32(74, 0, 176, 255);
            var dragArea = GUILayoutUtility.GetRect(0f, 40f, GUILayout.ExpandWidth(true));
            GUI.Box(dragArea, "Drag your animations here");

            EditorGUILayout.Space();

            switch (anEvent.type)
            {
                case EventType.DragUpdated:
                    if (!dragArea.Contains(anEvent.mousePosition))
                    {
                        break;
                    }

                    var validObjects = true;

                    foreach (var dragged in DragAndDrop.objectReferences)
                    {
                        validObjects &= dragged is BaseAnimation;
                    }

                    DragAndDrop.visualMode = validObjects ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;

                    break;
                case EventType.DragPerform:
                    if (!dragArea.Contains(anEvent.mousePosition))
                    {
                        break;
                    }

                    DragAndDrop.AcceptDrag();

                    AssignDraggedObjectsToProperties(DragAndDrop.objectReferences);

                    Event.current.Use();

                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AssignDraggedObjectsToProperties(UnityEngine.Object[] draggedObjects)
        {
            foreach (var dragged in draggedObjects)
            {
                Debug.Log("Registered new animation");
                Undo.RecordObject(target, "Added K-Animator animation");

                if (dragged is MoveInAnimation)
                {
                    animator.MoveInAnimation = dragged as MoveInAnimation;
                }
                else if (dragged is MoveOutAnimation)
                {
                    animator.MoveOutAnimation = dragged as MoveOutAnimation;
                }
                else if (dragged is MoveIdleAnimation)
                {
                    animator.MoveIdleAnimation = dragged as MoveIdleAnimation;
                }
                else if (dragged is RotateInAnimation)
                {
                    animator.RotateInAnimation = dragged as RotateInAnimation;
                }
                else if (dragged is RotateOutAnimation)
                {
                    animator.RotateOutAnimation = dragged as RotateOutAnimation;
                }
                else if (dragged is RotateIdleAnimation)
                {
                    animator.RotateIdleAnimation = dragged as RotateIdleAnimation;
                }
                else if (dragged is ScaleInAnimation)
                {
                    animator.ScaleInAnimation = dragged as ScaleInAnimation;
                }
                else if (dragged is ScaleOutAnimation)
                {
                    animator.ScaleOutAnimation = dragged as ScaleOutAnimation;
                }
                else if (dragged is ScaleIdleAnimation)
                {
                    animator.ScaleIdleAnimation = dragged as ScaleIdleAnimation;
                }
                else if (dragged is FadeInAnimation)
                {
                    animator.FadeInAnimation = dragged as FadeInAnimation;
                }
                else if (dragged is FadeOutAnimation)
                {
                    animator.FadeOutAnimation = dragged as FadeOutAnimation;
                }
                else if (dragged is FadeIdleAnimation)
                {
                    animator.FadeIdleAnimation = dragged as FadeIdleAnimation;
                }
                else
                {
                    continue;
                }
            }
        }

        private void ShowAssignedFields()
        {
            ShowAnimationField("Move In", animator.MoveInAnimation, x => animator.MoveInAnimation = x);
            ShowAnimationField("Move Out", animator.MoveOutAnimation, x => animator.MoveOutAnimation = x);
            ShowAnimationField("Move idle", animator.MoveIdleAnimation, x => animator.MoveIdleAnimation = x);

            ShowAnimationField("Rotate In", animator.RotateInAnimation, x => animator.RotateInAnimation = x);
            ShowAnimationField("Rotate Out", animator.RotateOutAnimation, x => animator.RotateOutAnimation = x);
            ShowAnimationField("Rotate idle", animator.RotateIdleAnimation, x => animator.RotateIdleAnimation = x);

            ShowAnimationField("Scale In", animator.ScaleInAnimation, x => animator.ScaleInAnimation = x);
            ShowAnimationField("Scale Out", animator.ScaleOutAnimation, x => animator.ScaleOutAnimation = x);
            ShowAnimationField("Scale idle", animator.ScaleIdleAnimation, x => animator.ScaleIdleAnimation = x);

            ShowAnimationField("Fade In", animator.FadeInAnimation, x => animator.FadeInAnimation = x);
            ShowAnimationField("Fade Out", animator.FadeOutAnimation, x => animator.FadeOutAnimation = x);
            ShowAnimationField("Fade idle", animator.FadeIdleAnimation, x => animator.FadeIdleAnimation = x);
        }

        /// <summary>
        /// Renders an animation field in the inspector.
        /// </summary>
        /// <typeparam name="T">The type of the animation.</typeparam>
        /// <param name="label">The label for the field.</param>
        /// <param name="obj">The instance of the animation.</param>
        /// <param name="setter">A setter action.</param>
        private void ShowAnimationField<T>(String label, T obj, Action<T> setter) where T : UnityEngine.Object
        {
            if (obj != null)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PrefixLabel(label);
                var newAnimation = (T)EditorGUILayout.ObjectField(obj, typeof(T), false);

                GUI.backgroundColor = new Color32(74, 0, 176, 255);

                bool isDeletePressed = GUILayout.Button("x", deleteButtonOptions);

                GUI.backgroundColor = Color.white;

                if (newAnimation != null)
                {
                    setter(newAnimation);
                }

                if (isDeletePressed)
                {
                    Undo.RecordObject(target, "Removed K-Animator animation");
                    setter(null);
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}