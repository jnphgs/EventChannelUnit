using Unity.VisualScripting;
using UnityEditor;

namespace EventChannelUnit.Editor.Utility
{
    [InitializeOnLoad]
    public static class NodeLibraryUpdater
    {
        private const string RuntimeAssemblyName = "EventChannelUnit.Runtime";

        static NodeLibraryUpdater()
        {
            var coreConfig = BoltCore.Configuration;
            var assemblyOptionsMetadata = coreConfig.GetMetadata(nameof(coreConfig.assemblyOptions));
            if(!assemblyOptionsMetadata.Contains((LooseAssemblyName)RuntimeAssemblyName))
            {
                EditorApplication.delayCall += AddAssemblyOptions;
            }
        }
        
        private static void AddAssemblyOptions()
        {
            var coreConfig = BoltCore.Configuration;
            var assemblyOptionsMetadata = coreConfig.GetMetadata(nameof(coreConfig.assemblyOptions));
            if(!assemblyOptionsMetadata.Contains((LooseAssemblyName)RuntimeAssemblyName))
            {
                assemblyOptionsMetadata.Add((LooseAssemblyName)RuntimeAssemblyName);
                assemblyOptionsMetadata.Save();
                coreConfig.SaveProjectSettingsAsset(true);
                Codebase.UpdateSettings();
                UnitBase.Rebuild();
            }
        }
        
    }
}
