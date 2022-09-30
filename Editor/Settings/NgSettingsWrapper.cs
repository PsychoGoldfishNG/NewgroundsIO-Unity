using UnityEditor;
using UnityEditor.SettingsManagement;

namespace NewgroundsIO.editor.settings
{
    /// <summary> A wrapper to access settings directly with the <see cref="NgSettingsManager"/> singleton. </summary>
    /// <typeparam name="T"> The type of the setting to save/retrieve. </typeparam>
    class NgSettings<T> : UserSetting<T>
    {
        public NgSettings(string key, T value, SettingsScope scope = SettingsScope.Project)
                : base(NgSettingsManager.instance, key, value, scope) { }

        NgSettings(Settings settings, string key, T value, SettingsScope scope = SettingsScope.Project)
                : base(settings, key, value, scope) { }
    }
}
