using System;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace NewgroundsIO.editor.settings
{
    static partial class NewgroundsSettings
    {
        /// <summary> Draws the API Tools settings category for Newgrounds Project System. </summary>
        internal static class ApiToolsDrawers
        {
            /// <summary> Draws the text field for editing App ID and shows a warning help box in case of problems. </summary>
            /// <returns> The App ID from the settings field, even if it appears invalid. </returns>
            internal static string DrawAppID(string appId, string searchContext, bool displayHelp = true)
            {

                Texture2D icon = Texture2D.redTexture; //NewgroundsIcons.GetIcon(NewgroundsIcons.SmallIcons.Puzzle);
                var guiContent = new GUIContent("App ID", "The unique ID of your app as found in the 'API Tools' tab of your project.");
                // guiContent.

                displayHelp = displayHelp && SearchableLayoutExtras.SearchCheck(guiContent.text, searchContext);

                if (displayHelp) EditorGUILayout.HelpBox("The unique ID of your app as found in the 'API Tools' tab of your project.", MessageType.None);
                bool invalidId = !ValidateAppId(appId, out Color textColor, out string warningText);

                appId = SearchableLayoutExtras.SearchableIconTextField(guiContent, appId, searchContext, textColor, icon);

                // Display warning (below, so the input field doesn't move around while typing)
                // if (displayHelp && invalidId) EditorMethods.IndentedHelpBox(warningText, MessageType.Warning, 3f);
                return appId;
            }

            /// <summary>Draws the text field for editing and shows a warning help box in case of problems.</summary>
            internal static string DrawEncryptionKey(string key, string searchContext, bool displayHelp = true)
            {
                Texture2D icon = Texture2D.redTexture; //NewgroundsIcons.GetIcon(NewgroundsIcons.SmallIcons.Key);
                var guiContent = new GUIContent("AES Encryption Key", "A base64-encoded, 128-bit AES encryption key as found in the 'API Tools' tab of your project.");
                
                displayHelp = displayHelp && SearchableLayoutExtras.SearchCheck(guiContent.text, searchContext);

                if (displayHelp) EditorGUILayout.HelpBox("A base64-encoded, 128-bit AES encryption key.", MessageType.None);
                bool invalidKey = !ValidateEncryptionKey(key, out Color textColor, out string warningText);

                key = SearchableLayoutExtras.SearchableIconTextField(guiContent, key, searchContext, textColor, icon);

                // Display warning (below, so the input field doesn't move around while typing)
                // if (displayHelp && invalidKey) EditorMethods.IndentedHelpBox(warningText, MessageType.Warning, 3f);
                return key;
            }

            /// <summary>Validates an encryption key, potentionally returns </summary>
            /// <param name="applicationId">A string to check for being a valid App ID.</param>
            /// <param name="textColor">A <see cref="Color"/> to use for the text field. 
            /// If any problems are encountered, the text colour changes to <see cref="ColorPalette.WarningColor"/>.</param>
            /// <param name="warningText">A specific warning text indicating the encountered problem.</param>
            static bool ValidateAppId(string applicationId, out Color textColor, out string warningText)
            {
                warningText = string.Empty;
                if (string.IsNullOrEmpty(applicationId))
                {
                    warningText = "You need to fill in your App ID for Newgrounds.io to work. \nYou can find it in the Project System / API Tools";
                }
                else if (applicationId == string.Empty) // DemoAppId)
                {
                    warningText = "This is the example App ID used in the demo. \nYou should supply your own from the Project System / API Tools.";
                }
                else if (!Regex.IsMatch(applicationId, @"^\d+?:[a-zA-Z0-9]+$")) // Number:AlphanumericChars
                {
                    warningText = "The App ID is probably in a wrong format.";
                }
                else
                {
                    textColor = Color.green; //ColorPalette.FineColor;
                    return true;
                }
                textColor = Color.yellow; //ColorPalette.WarningColor;
                return false;
            }

            /// <summary>Validates an encryption key, potentionally returns </summary>
            /// <param name="aes64Key">A string to check for being a valid AES encryption key.</param>
            /// <param name="textColor">A <see cref="Color"/> to use for the text field. 
            /// If any problems are encountered, the text colour changes to <see cref="ColorPalette.WarningColor"/>.</param>
            /// <param name="warningText">A specific warning text indicating the encountered problem.</param>
            static bool ValidateEncryptionKey(string aes64Key, out Color textColor, out string warningText)
            {
                warningText = string.Empty;
                if (string.IsNullOrEmpty(aes64Key))
                {
                    // Empty field
                    warningText = "You need to fill in your App ID for Newgrounds.io to work. \nYou can find it in the Project System / API Tools";
                }
                else if (aes64Key == string.Empty) //DemoEncryptionKey)
                {
                    // The user didn't change the default demo value
                    warningText = "This is the example encryption key used in the demo. \nYou should supply your own from the Project System / API Tools.";
                }
                else
                {
                    try
                    {
                        // See if it's possible to create an encryption key from the input string
                        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                        Convert.FromBase64String(aes64Key);
                        textColor = Color.green; //ColorPalette.FineColor;
                        return true;
                    }
                    catch (FormatException)
                    {
                        // Failed => invalid encryption key
                        warningText = "This encryption key is probably in a wrong format.";
                    }
                }
                textColor = Color.yellow; //ColorPalette.WarningColor;
                return false;
            }
        }

    }
}
