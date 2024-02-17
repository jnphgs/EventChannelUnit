using Unity.VisualScripting;
using UnityEditor.Callbacks;

namespace EventChannelUnit.Editor.Utility
{
    public static class NodeLibraryUpdater
    {
        private const string RuntimeAssemblyName = "EventChannelUnit.Runtime";
        
        [DidReloadScripts]
        private static void OnScriptReloaded()
        {
            UpdateAssemblyOptions();
        }

        private static void UpdateAssemblyOptions()
        {
            var coreConfig = BoltCore.Configuration;
            var assemblyOptionsMetadata = coreConfig.GetMetadata(nameof(coreConfig.assemblyOptions));
            if(!assemblyOptionsMetadata.Contains((LooseAssemblyName)RuntimeAssemblyName))
            {
                assemblyOptionsMetadata.Add((LooseAssemblyName)RuntimeAssemblyName);
                assemblyOptionsMetadata.Save();
                assemblyOptionsMetadata.configuration.SaveProjectSettingsAsset(true);
                Codebase.UpdateSettings();
                UnitBase.Rebuild();
            }
        }
        
    }
}
