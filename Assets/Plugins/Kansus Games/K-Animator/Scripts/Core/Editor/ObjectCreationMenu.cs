using UnityEditor;
using UnityEngine;

namespace KansusAnimator.Editor
{
    public class ObjectCreationMenu
    {
        [MenuItem("GameObject/Kansus Games/K-Animator/K-Animator Manager", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            GameObject go = new GameObject
            {
                transform = { localPosition = new Vector3(0f, 0f, 0f) },
                name = "K-Animator Manager"
            };

            AnimatorConfiguration defaultConfiguration = GetFirstScriptableObject<AnimatorConfiguration>();
            go.AddComponent<AnimationManager>(x => x.Configuration = defaultConfiguration);

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        public static T GetFirstScriptableObject<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("K-Animator Configuration" + " t:" + typeof(T).Name);

            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return null;
        }
    }
}