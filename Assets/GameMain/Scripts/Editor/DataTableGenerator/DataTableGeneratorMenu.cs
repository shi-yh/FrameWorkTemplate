//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.IO;
using GameFramework;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor.DataTableTools
{
    public sealed class DataTableGeneratorMenu
    {
        [MenuItem("Tools/Generate DataTables")]
        private static void GenerateDataTables()
        {
            foreach (string dataTableName in GetDataTableNames())
            {
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.TextUtility.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }

        private static string[] GetDataTableNames()
        {
            string dataTablesPath = Application.dataPath + @"/GameMain/DataTables";
            DirectoryInfo directoryInfo = new DirectoryInfo(dataTablesPath);
            FileInfo[] fis = directoryInfo.GetFiles("*.txt", SearchOption.AllDirectories);
            string[] dataTableNames = new string[fis.Length];
            for (int i = 0; i < fis.Length; i++)
            {
                dataTableNames[i] = Path.GetFileNameWithoutExtension(fis[i].Name);
            }

            return dataTableNames;
        }


        [MenuItem("Tools/Generate Config", false, 1)]
        private static void GenerateConfig()
        {
            string configPath = Application.dataPath + @"/GameMain/Configs/DefaultConfig";


            DataTableProcessor dataTableProcessor = DataTableGenerator.CreateConfigTableProcessor(configPath);
            
            DataTableGenerator.GenerateDataFile(dataTableProcessor, configPath);
            
            AssetDatabase.Refresh();
        }
    }
}