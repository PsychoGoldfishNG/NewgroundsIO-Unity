using UnityEditor;
using UnityEditor.SettingsManagement;

namespace NewgroundsIO.editor.settings
{
    /// <summary> Newgrounds editor settings manager. </summary>
    static class NgSettingsManager
    {

        static Settings _instance;
        /// <summary> A static instance of Newgrounds settings. Will be created when accessed for the first time. </summary>
        internal static Settings instance
        {
            get
            {
                return _instance ??= new Settings(new ISettingsRepository[]
                {
                        new PackageSettingsRepository(NgPath.PackageName, NgPath.SettingsFileName),
                        new UserSettingsRepository()
                });
            }
        }

        /// <summary> Saves all altered settings. </summary>
        public static void Save() => instance.Save();

        /// <summary> Retrieves a value from the settings. </summary>
        /// <typeparam name="T"> The type of the setting. </typeparam>
        /// <param name="key"> A <see langword="string"/> key under which the setting is saved. </param>
        /// <param name="scope"> Project or User setting? </param>
        /// <param name="fallback"> The default value returned if the setting isn't found. </param>
        public static T Get<T>(string key, SettingsScope scope = SettingsScope.Project, T fallback = default)
        {
            return instance.Get(key, scope, fallback);
        }

        /// <summary> Sets a setting. </summary>
        /// <typeparam name="T"> The type of the setting. </typeparam>
        /// <param name="key"> A <see langword="string"/> key to save the setting under. </param>
        /// <param name="value"> The value to set the setting to. </param>
        /// <param name="scope"> Project or User setting? </param>
        public static void Set<T>(string key, T value, SettingsScope scope = SettingsScope.Project)
        {
            instance.Set(key, value, scope);
        }

        /// <summary> Checks if a setting is present in the settings file. </summary>
        /// <typeparam name="T"> The type of the setting. </typeparam>
        /// <param name="key"> A <see langword="string"/> key under which the setting is saved. </param>
        /// <param name="scope"> Project or User setting? </param>
        public static bool ContainsKey<T>(string key, SettingsScope scope = SettingsScope.Project)
        {
            return instance.ContainsKey<T>(key, scope);
        }

    }
}
