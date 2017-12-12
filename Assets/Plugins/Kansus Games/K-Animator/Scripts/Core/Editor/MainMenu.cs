using UnityEditor;

namespace KansusAnimator.Editor
{
    /// <summary>
    /// Class responsible for building the main menu options for K-Animator.
    /// </summary>
    public class MainMenu
    {
        [MenuItem("Kansus Games/K-Animator/Tween systems/Activate ITween")]
        public static void ActivateITween()
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!symbols.Contains("ITWEEN"))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols + ";ITWEEN");
            }
        }

        [MenuItem("Kansus Games/K-Animator/Tween systems/Deactivate ITween")]
        public static void DeactivateITween()
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (symbols.Contains("ITWEEN"))
            {
                symbols = symbols.Replace(";ITWEEN", "");
                symbols = symbols.Replace("ITWEEN", "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
            }
        }

        [MenuItem("Kansus Games/K-Animator/Tween systems/Activate LeanTween")]
        public static void ActivateLeanTween()
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!symbols.Contains("LEANTWEEN"))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols + ";LEANTWEEN");
            }
        }

        [MenuItem("Kansus Games/K-Animator/Tween systems/Deactivate LeanTween")]
        public static void DeactivateLeanTween()
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (symbols.Contains("LEANTWEEN"))
            {
                symbols = symbols.Replace(";LEANTWEEN", "");
                symbols = symbols.Replace("LEANTWEEN", "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
            }
        }

        [MenuItem("Kansus Games/K-Animator/Tween systems/Activate DOTween")]
        public static void ActivateDOTween()
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!symbols.Contains("DOTWEEN"))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols + ";DOTWEEN");
            }
        }

        [MenuItem("Kansus Games/K-Animator/Tween systems/Deactivate DOTween")]
        public static void DeactivateDOTween()
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (symbols.Contains("DOTWEEN"))
            {
                symbols = symbols.Replace(";DOTWEEN", "");
                symbols = symbols.Replace("DOTWEEN", "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
            }
        }
    }
}