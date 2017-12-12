using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KansusAnimator.Editor
{
    /// <summary>
    /// Adds the given define symbols to PlayerSettings define symbols.
    /// Just add your own define symbols to the Symbols property at the below.
    /// </summary>
    [InitializeOnLoad]
    public class DefineSymbolsConfigurator : UnityEditor.Editor
    {
        const string ADDED_SYMBOLS_KEY = "K_ANIMATOR_ADDED_DEFINE_SYMBOLS";

        /// <summary>
        /// Symbols that will be added to the editor
        /// </summary>
        public static readonly string[] Symbols = new string[] {
            "KANSUSGAMES",
            "KANSUSGAMES_KANIMATOR",
             "LEANTWEEN"
        };

        /// <summary>
        /// Add define symbols as soon as Unity gets done compiling.
        /// </summary>
        static DefineSymbolsConfigurator()
        {
            string projectID = PlayerSettings.productGUID.ToString();
            string addedSymbolsKey = "K_ANIMATOR_ADDED_DEFINE_SYMBOLS." + projectID;
            
            if (!EditorPrefs.GetBool(addedSymbolsKey, false))
            {
                string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup
                );

                List<string> allDefines = definesString.Split(';').ToList();
                allDefines.AddRange(Symbols.Except(allDefines));

                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup,
                    string.Join(";", allDefines.ToArray())
                );

                Debug.Log("Successfully registered K-Animator Define Symbols");

                EditorPrefs.SetBool(addedSymbolsKey, true);
            }
        }
    }
}