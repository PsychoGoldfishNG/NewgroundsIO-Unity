using UnityEditor;
using UnityEditor.SettingsManagement;

namespace NewgroundsIO.editor.settings
{
    /// <summary> Settings provider for Newgrounds editor settings. </summary>
    static class NgSettingsProvider
    {
        [SettingsProvider]
        static SettingsProvider CreateSettingsProvider()
        {
            var provider = new UserSettingsProvider(NgPath.SettingsPath,
                                                    NgSettingsManager.instance,
                                                    new[] { typeof(NgSettingsProvider).Assembly },
                                                    SettingsScope.Project);
            return provider;
        }
    }
}
