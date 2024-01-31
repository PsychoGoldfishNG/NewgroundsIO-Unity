using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.UnityLinker;

namespace NewgroundsIO.editor.build
{
    public class LinkXmlInstaller : IUnityLinkerProcessor
    {
        int IOrderedCallback.callbackOrder => 0;

        const string LinkXmlName = "link.xml";
        static readonly string linkXmlRootPath = NgPath.GetPackageRoot();

        string IUnityLinkerProcessor.GenerateAdditionalLinkXmlFile(BuildReport report, UnityLinkerBuildPipelineData data)
        {
            string[] xmls = System.IO.Directory.GetFiles(linkXmlRootPath, LinkXmlName, SearchOption.AllDirectories);
            return (xmls.Length > 0)
                ? Path.GetFullPath(xmls[0])
                : string.Empty;
        }
    }
}