using KansusAnimator.Animations;
using System;
using UnityEditor;
using UnityEngine;

namespace KansusAnimator.Animators.Inspector
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
            //rect.width = tex.width;
            rect.height = 128;
            GUILayout.Space(rect.height);
            GUI.DrawTexture(rect, tex, ScaleMode.ScaleToFit);

            serializedObject.Update();

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
                case EventType.DragPerform:
                    if (!dragArea.Contains(anEvent.mousePosition))
                    {
                        break;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (anEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (var dragged in DragAndDrop.objectReferences)
                        {
                            if (dragged is MoveInAnimation)
                            {
                                animator.MoveInAnimation = dragged as MoveInAnimation;
                            }
                            else if (dragged is MoveOutAnimation)
                            {
                                animator.MoveOutAnimation = dragged as MoveOutAnimation;
                            }
                            else if (dragged is MovePingPongAnimation)
                            {
                                animator.MovePingPongAnimation = dragged as MovePingPongAnimation;
                            }
                            else if (dragged is RotateInAnimation)
                            {
                                animator.RotateInAnimation = dragged as RotateInAnimation;
                            }
                            else if (dragged is RotateOutAnimation)
                            {
                                animator.RotateOutAnimation = dragged as RotateOutAnimation;
                            }
                            else if (dragged is RotatePingPongAnimation)
                            {
                                animator.RotatePingPongAnimation = dragged as RotatePingPongAnimation;
                            }
                            else if (dragged is ScaleInAnimation)
                            {
                                animator.ScaleInAnimation = dragged as ScaleInAnimation;
                            }
                            else if (dragged is ScaleOutAnimation)
                            {
                                animator.ScaleOutAnimation = dragged as ScaleOutAnimation;
                            }
                            else if (dragged is ScalePingPongAnimation)
                            {
                                animator.ScalePingPongAnimation = dragged as ScalePingPongAnimation;
                            }
                            else if (dragged is FadeInAnimation)
                            {
                                animator.FadeInAnimation = dragged as FadeInAnimation;
                            }
                            else if (dragged is FadeOutAnimation)
                            {
                                animator.FadeOutAnimation = dragged as FadeOutAnimation;
                            }
                            else if (dragged is FadePingPongAnimation)
                            {
                                animator.FadePingPongAnimation = dragged as FadePingPongAnimation;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    Event.current.Use();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowAssignedFields()
        {
            ShowAnimationField("Move In", animator.MoveInAnimation, x => animator.MoveInAnimation = x);
            ShowAnimationField("Move Out", animator.MoveOutAnimation, x => animator.MoveOutAnimation = x);
            ShowAnimationField("Move Ping Pong", animator.MovePingPongAnimation, x => animator.MovePingPongAnimation = x);

            ShowAnimationField("Rotate In", animator.RotateInAnimation, x => animator.RotateInAnimation = x);
            ShowAnimationField("Rotate Out", animator.RotateOutAnimation, x => animator.RotateOutAnimation = x);
            ShowAnimationField("Rotate Ping Pong", animator.RotatePingPongAnimation, x => animator.RotatePingPongAnimation = x);

            ShowAnimationField("Scale In", animator.ScaleInAnimation, x => animator.ScaleInAnimation = x);
            ShowAnimationField("Scale Out", animator.ScaleOutAnimation, x => animator.ScaleOutAnimation = x);
            ShowAnimationField("Scale Ping Pong", animator.ScalePingPongAnimation, x => animator.ScalePingPongAnimation = x);

            ShowAnimationField("Fade In", animator.FadeInAnimation, x => animator.FadeInAnimation = x);
            ShowAnimationField("Fade Out", animator.FadeOutAnimation, x => animator.FadeOutAnimation = x);
            ShowAnimationField("Fade Ping Pong", animator.FadePingPongAnimation, x => animator.FadePingPongAnimation = x);
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
                    setter(null);
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}