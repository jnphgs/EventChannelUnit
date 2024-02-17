using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace EventChannelUnit.Editor.Utility
{
    
    public class InitializeNodeLibraryWindow: EditorWindow
    {
        private const string WindowTitle = "Initialize EventChannelUnit";
        private const string RuntimeAssemblyName = "EventChannelUnit.Runtime";
        
        [MenuItem("Window/Event Channel Unit/Initialize")]
        public static void ShowWindow()
        {
            var window = GetWindow<InitializeNodeLibraryWindow>(WindowTitle);
            window.minSize = new Vector2(200, 100);
            window.maxSize = new Vector2(400, 200);
            window.Focus();
        }

        private void OnGUI()
        {
            if (!VSUsageUtility.isVisualScriptingUsed)
            {
                GUILayout.Space(10);
                GUILayout.Label("Initialize Visual Scripting");
                if (GUILayout.Button("Ok"))
                {
                    VSUsageUtility.isVisualScriptingUsed = true;
                }
            }
            else
            {
                var coreConfig = BoltCore.Configuration;
                var assemblyOptionsMetadata = coreConfig.GetMetadata(nameof(coreConfig.assemblyOptions));
                if(!assemblyOptionsMetadata.Contains((LooseAssemblyName)RuntimeAssemblyName))
                {
                    GUILayout.Space(10);
                    GUILayout.Label("Add Assembly Options And Rebuild Node Library");
                    if (GUILayout.Button("Ok"))
                    {
                        AddAssemblyOptions();
                    }
                }
                else
                {
                    GUILayout.Space(10);
                    GUILayout.Label("Already Initialized.");
                }
            }
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            EditorApplication.delayCall += CheckAssemblyOptions;
        }
        private static void CheckAssemblyOptions()
        {
            if (!VSUsageUtility.isVisualScriptingUsed)
            {
                ShowWindow();
                return;
            }
            var coreConfig = BoltCore.Configuration;
            var assemblyOptionsMetadata = coreConfig.GetMetadata(nameof(coreConfig.assemblyOptions));
            if(!assemblyOptionsMetadata.Contains((LooseAssemblyName)RuntimeAssemblyName))
            {
                ShowWindow();
            }
        }
        
        private static void AddAssemblyOptions()
        {
            var coreConfig = BoltCore.Configuration;
            var assemblyOptionsMetadata = coreConfig.GetMetadata(nameof(coreConfig.assemblyOptions));
            assemblyOptionsMetadata.Add((LooseAssemblyName)RuntimeAssemblyName);
            assemblyOptionsMetadata.Save();
            coreConfig.SaveProjectSettingsAsset(true);
            Codebase.UpdateSettings();
            UnitBase.Rebuild();
        }
        
    }
}
