using UnityEditor;

namespace NewgroundsIO.editor.settings
{
    /// <summary> Creates a <see cref="EditorGUI.ChangeCheckScope"/> that saves Newgrounds settings when changes are made. </summary>
    public class NgSettingsScope : EditorGUI.ChangeCheckScope
    {
        protected override void CloseScope()
        {
            if (changed) NgSettingsManager.Save();
            base.CloseScope();
        }
    }
}
