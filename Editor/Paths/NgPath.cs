using UnityEngine;

namespace NewgroundsIO.editor
{
    static class NgPath
    {
        /// <summary> The internal identifier of the package. </summary>
        internal const string PackageName = "io.newgrounds.unity";
        
        /// <summary> The default namespace of the package. </summary>
        /// <remarks> This is based on the namespace of the <see cref="Core"/> class. </remarks>
        internal static readonly string RootNamespace = typeof(Core).Namespace;
        
        /// <summary> The root folder of the Unity project. </summary>
        internal static readonly string ProjectRoot = Application.dataPath[..Application.dataPath.LastIndexOf('/')];

        /// <summary> The path to the package root relative to the <see cref="ProjectRoot"/>. </summary>
        internal const string PackageRoot = "Packages/" + PackageName;

        /// <summary>Base path to runnable commands via the top context menu.</summary>
        internal const string MenuTools = "Window/Newgrounds/";
        
        /// <summary> The path to these settings in Unity's Project Settings window. </summary>
        internal const string SettingsPath = "Project/Newgrounds";

        /// <summary> The name of the JSON settings file created in the Project Settings directory. </summary>
        internal const string SettingsFileName = "NewgroundsSettings";

        /// <summary> Joins paths with a forward slash separator, as required by Unity. </summary>
        internal static string Combine(params string[] paths)
        {
            return string.Join("/", paths);
        }

        /// <summary> Creates a path to a file inside the Newgrounds.io package folder. </summary>
        /// <param name="paths">Parts of a single path to the file from the root of the Newgrounds.io package folder.</param>
        /// <remarks>
        ///     A simple <see cref="string.Join(string, string[])"/> with the package's root path.
        /// </remarks>
        internal static string CombinePackageRoot(params string[] paths)
        {
            return Combine(PackageRoot, Combine(paths));
        }

    }
}
