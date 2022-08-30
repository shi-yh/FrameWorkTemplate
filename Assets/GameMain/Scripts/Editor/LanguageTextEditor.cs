using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using GameFramework;
using LitJson;

namespace GameMain.LocalizationGenerator
{
    using TMPro;
    using UnityEditor.Presets;
    using UnityEngine.UI;
    using UnityEditor;
    using UnityEngine;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(LanguageText))]
    public class LanguageTextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LanguageText myTarget = (LanguageText) target;

            EditorGUILayout.IntField("LanguageId", myTarget.Id);

            LanguageTextMode mode = myTarget.TextMode;

            mode = (LanguageTextMode) EditorGUILayout.EnumPopup("预设样式", myTarget.TextMode);

            if (mode != myTarget.TextMode)
            {
                myTarget.TextMode = mode;

                if (mode != LanguageTextMode.None)
                {
                    Preset ps = AssetDatabase.LoadAssetAtPath($"Assets/GameMain/Preset/{myTarget.GetActiveText()}/{mode.ToString()}.preset", typeof(Preset)) as Preset;

                    Component cm = myTarget.GetComponent(myTarget.GetActiveText());

                    Debug.Log(ps.ApplyTo(cm));

                    EditorUtility.SetDirty(cm);

                    AssetDatabase.SaveAssets();
                }
            }

            if (myTarget.Id != -1 && GUILayout.Button("设置字符串"))
            {
            }

            if (myTarget.HasInit()&&GUILayout.Button("上传更新字符串"))
            {
                int id = CollectLanguage(myTarget.GetText());

                myTarget.Id = id;
            }
        }

        private int CollectLanguage(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return -1;
            }
            
            UTF8Encoding utf8 = new UTF8Encoding();

            text = utf8.GetString(utf8.GetBytes(text));

            TextAsset ta = AssetDatabase.LoadAssetAtPath($"Assets/GameMain/Localization/Language.txt", typeof(TextAsset)) as TextAsset;

            if (ta == null)
            {
                Debug.LogError("Assets/GameMain/Localization/Language.txt 不存在");
            }

            JsonData jd = JsonMapper.ToObject(ta.text);

            for (int i = 0; i < jd.Count; i++)
            {
                ///如果有对应的
                if (jd[i]["ChineseSimplified"].ToString().Equals(text))
                {
                    return int.Parse(jd[i]["Id"].ToString());
                }
            }

            ///有远端后改远端数据库自增
            int id = int.Parse(jd[^1]["Id"].ToString())+1;

            JsonData temp = JsonMapper.ToObject(jd[^1].ToJson());

            temp["Id"] = id;
            temp["ChineseSimplified"] = text;

            jd.Add(temp);
            
            try
            {
                using (FileStream fileStream = new FileStream("Assets/GameMain/Localization/Language.txt", FileMode.Create, FileAccess.Write))
                {
                    using (TextWriter sw = new StreamWriter(fileStream,  Encoding.UTF8))
                    {

                        string result = jd.ToJson();
                        
                        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
                        result = reg.Replace(result, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

                        
                        sw.WriteLine(result);
                    }
                }
            
               
                
                Debug.Log(Utility.TextUtility.Format("Parse Language.txt success.", "Assets/GameMain/Localization/Language.txt"));
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.TextUtility.Format("Parse Language.txt failure, exception is '{1}'.", "Assets/GameMain/Localization/Language.txt", exception));
            }

            AssetDatabase.Refresh();
            
            return id;
        }
    }
}