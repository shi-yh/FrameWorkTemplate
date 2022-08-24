using System;
using System.Collections.Generic;
using System.Text;

namespace StarForce.LocalizationGenerator
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

            if (GUILayout.Button("上传更新字符串"))
            {
                CollectLanguage(myTarget.GetText());
            }
        }

        private void CollectLanguage(string text)
        {        
            UTF8Encoding utf8 = new UTF8Encoding();

            text = utf8.GetString(utf8.GetBytes(text));
            
            // Dictionary<>
            
        }
    }
}