using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.SettingsManagement;

namespace NewgroundsIO.editor.settings
{
    /// <summary> Additional searchable field to supplement those missing in <see cref="SettingsGUI"/>. </summary>
    static class SearchableLayoutExtras
    {
        /// <summary> Make an enum popup searchable selection field. </summary>
        internal static Enum SearchableEnumPopup(GUIContent label, Enum selected, string searchContext)
        {
            return !MatchSearchGroups(searchContext, label.text)
                    ? selected
                    : EditorGUILayout.EnumPopup(label, selected);
        }

        /// <summary> An int field that implements search filtering. </summary>
        /// <param name="label">Label in front of the value field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="searchContext">A string representing the current search query. Empty or null strings are to be treated as matching any value.</param>
        /// <returns>The value that has been set by the user.</returns>
        internal static int SearchableIntField(GUIContent label, int value, string searchContext)
        {
            return !MatchSearchGroups(searchContext, label.text)
                    ? value
                    : EditorGUILayout.IntField(label, value);
        }

        /// <summary> . </summary>
        /// <param name="searchContext"> A string representing the current search query. Empty or null strings are to be treated as matching any value. </param>
        /// <param name="label"></param>
        /// <returns></returns>
        static bool MatchSearchGroups(string searchContext, string label)
        {
            if (searchContext == null) return true;
            string ctx = searchContext.Trim();
            if (string.IsNullOrEmpty(ctx)) return true;

            string[] split = searchContext.Split(' ');
            return split.Any(x => !string.IsNullOrEmpty(x) && label.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) > -1);
        }

        /// <summary> A searchable settings text field with an icon in front. </summary>
        public static string SearchableIconTextField(GUIContent content, string value, string searchContext, Color textColor, Texture2D icon)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (SearchCheck(content.text, searchContext)) GUILayout.Box(icon); // Icon

                using (new EditorGUILayout.VerticalScope())
                {
                    // Vertical space to align the text field with the icon
                    if (icon) GUILayout.Space(Mathf.Round(icon.height / 2f));

                    float oldWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 145;
                    Color oldTextColor = GUI.contentColor;
                    GUI.contentColor = textColor;
                    value = SettingsGUILayout.SearchableTextField(content, value, searchContext);
                    GUI.contentColor = oldTextColor;
                    EditorGUIUtility.labelWidth = oldWidth;
                }
            }
            return value;
        }

        /// <summary> Draws the field for selecting folders, optionally searchable. </summary>
        public static string FolderPathField(string folder, GUIContent title, string searchContext = "")
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                folder = SettingsGUILayout.SearchableTextField(title, folder, searchContext);
                if (SearchCheck(title.text, searchContext))
                {
                    if (GUILayout.Button("…", EditorStyles.miniButtonRight, GUILayout.Width(22)))
                    {
                        folder = EditorUtility.OpenFolderPanel(title.text, folder, string.Empty);
                        GUIUtility.keyboardControl = 0;
                    }
                }
            }
            return folder;
        }

        /// <summary> Checks whether a GUI control should be displayed based on the search keywords. </summary>
        public static bool SearchCheck(string label, string searchContext = "")
        {
            if (string.IsNullOrWhiteSpace(searchContext)) return true; // don't search at all
            return label.IndexOf(searchContext, 0, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
