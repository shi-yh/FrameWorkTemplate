using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    public class UIScriptGener
    {
        public static void GenScript(GameObject rootGo)
        {
            string uiName = rootGo.name;

            string viewClassName = uiName + ((uiName.Contains("View") || uiName.Contains("view")) ? "" : "View");

            CodeGener gener = new CodeGener(UIView.uiViewNameSpace, viewClassName);

            gener.AddBaseType("UIView");
            gener.AddImport("UnityGameFramework.Runtime");

            GenerateScripts(rootGo, gener);

            gener.GenCSharp(Path.Combine(Application.dataPath, UIView.uiViewScriptPath));
        }

        private static void GenerateScripts(GameObject root, CodeGener gener)
        {
            if (root.name.StartsWith(UIView.genableSign))
            {
                string goName = root.name.Replace(UIView.genableSign, string.Empty);

                var coms = root.GetComponents<Component>();

                foreach (Component com in coms)
                {
                    if (com == null)
                    {
                        continue;
                    }

                    System.Type comType = com.GetType();

                    if (UIView.uiCompTypeList.Contains(comType))
                    {
                        gener.AddMemberProperty(comType, UIView.comNameDict[comType] + goName);
                    }
                }

                gener.AddMemberProperty(typeof(GameObject), UIView.goNamePrefix + goName);
            }

            if (root.transform.childCount > 0)
            {
                for (int i = 0; i < root.transform.childCount; i++)
                {
                    GenerateScripts(root.transform.GetChild(i).gameObject, gener);
                }
            }
        }
    }
}