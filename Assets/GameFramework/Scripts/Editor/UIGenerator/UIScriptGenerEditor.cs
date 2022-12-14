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
            if (GUILayout.Button("็ๆไปฃ็ "))
            {
                UIScriptGener.GenScript(behavior.gameObject);
            }
        }
    }
}