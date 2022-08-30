using System.Collections;
using System.Collections.Generic;
using GameMain.LocalizationGenerator;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TextTools : Editor
{
    [MenuItem("CONTEXT/Text/转化为TextMeshProUI")]
    private static void ChangeText2TMP(MenuCommand menuCommand)
    {
        // The RigidBody component can be extracted from the menu command using the context field.
        var text = menuCommand.context as Text;
        var textSize = text.fontSize;
        var color = text.color;
        var target = text.gameObject;
        string str = text.text;
        bool hoverflow = text.horizontalOverflow == HorizontalWrapMode.Overflow;

        DestroyImmediate(text);


        var textMeshPro = target.GetOrAddComponent<TextMeshProUGUI>();
        textMeshPro.fontSize = textSize;
        textMeshPro.color = color;
        textMeshPro.text = str;
        textMeshPro.enableWordWrapping = hoverflow;
        textMeshPro.raycastTarget = false;
        
        target.GetComponent<LanguageText>()?.InitComponent();

        EditorUtility.SetDirty(target);
    }

    [MenuItem("CONTEXT/TextMeshProUGUI/转化为普通的Text组件")]
    private static void ChangeTMP2Text(MenuCommand menuCommand)
    {
        // The RigidBody component can be extracted from the menu command using the context field.
        var text = menuCommand.context as TextMeshProUGUI;
        var textSize = text.fontSize;
        var color = text.color;
        var target = text.gameObject;
        string str = text.text;
        bool hoverflow = text.enableWordWrapping;
        if (text != null)
        {
            DestroyImmediate(text);
        }

        var temp = target.GetOrAddComponent<Text>();
        temp.fontSize = (int) textSize;
        temp.color = color;
        temp.text = str;
        temp.horizontalOverflow = hoverflow ? HorizontalWrapMode.Overflow : HorizontalWrapMode.Wrap;
        temp.verticalOverflow = VerticalWrapMode.Overflow;
        temp.raycastTarget = false;
        target.GetComponent<LanguageText>().InitComponent();

        EditorUtility.SetDirty(target);
    }
}