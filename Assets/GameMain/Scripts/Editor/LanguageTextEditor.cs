namespace StarForce.LocalizationGenerator
{
    using UnityEditor;
    using UnityEngine;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(LanguageText))]
    public class LanguageTextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            LanguageText myTarget = (LanguageText) target;

            EditorGUILayout.IntField("stringId", myTarget.Id);


            serializedObject.ApplyModifiedProperties();
        }
    }
}