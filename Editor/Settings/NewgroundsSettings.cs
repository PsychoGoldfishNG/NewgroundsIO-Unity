using UnityEditor;
using UnityEditor.SettingsManagement;

namespace NewgroundsIO.editor.settings
{
    /// <summary> A class handling the Project Settings for Newgrounds. </summary>
    static partial class NewgroundsSettings
    {
        /// <summary>Opens the Newgrounds settings.</summary>        
        [MenuItem(NgPath.MenuTools + "Open Settings", priority = -100)]
        internal static void OpenSettings()
        {
            SettingsService.OpenProjectSettings(NgPath.SettingsPath);
        }
        
        //---------- API Tools settings ------------------------------------------

        const string ApiToolsCategory = "api";
        [UserSetting] static NgSettings<string> appId = new(AppId, string.Empty);
        [UserSetting] static NgSettings<string> aesBase64Key = new(EncryptionKey, string.Empty);
        
        const string AppId = ApiToolsCategory + ".appId";
        /// <summary>The global setting for the unique ID of your app as found in the 'API Tools' tab of your Newgrounds.com project.</summary>
        public static string GetAppId() => NgSettingsManager.instance.Get<string>(AppId);

        const string EncryptionKey = ApiToolsCategory + ".aesBase64Key";
        /// <summary>The global setting for a base64-encoded, 128-bit AES encryption key as found in the 'API Tools' tab of your Newgrounds.com project.</summary>
        public static string GetEncryptonKey() => NgSettingsManager.instance.Get<string>(EncryptionKey);

        [UserSettingBlock("API Tools")]
        static void ApiToolsSettingsGUI(string searchContext)
        {
            using (new NgSettingsScope())
            {
                appId.SetValue(ApiToolsDrawers.DrawAppID(appId, searchContext));
                aesBase64Key.SetValue(ApiToolsDrawers.DrawEncryptionKey(aesBase64Key, searchContext));
            }
        }


    }
}
