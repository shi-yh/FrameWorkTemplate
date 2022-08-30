using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(UIGenerBehavior))]
    public class UIScriptGenerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UIGenerBehavior behavior = (UIGenerBehavior) target;
            if (GUILayout.Button("生成代码"))
            {
                UIScriptGener.GenScript(behavior.gameObject);
            }
        }
    }
}