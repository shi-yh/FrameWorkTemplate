using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGameFramework.Runtime
{
    public class UIView 
    {
        //前缀
        public static readonly string genableSign = "g-";
        
        //自定义UIView命名空间
        public static readonly string uiViewNameSpace = "GameMain";
        
        //UIView储存位置
        public static readonly string uiViewScriptPath = "GameMain/Scripts/UI/View";
        
        private Dictionary<string, Component> comDict;
        
        //有效的组件
        public static readonly List<Type> uiCompTypeList = new List<Type>()
        {
            typeof(Image),
            typeof(Button),
            typeof(Text),
            typeof(Slider),
            typeof(ScrollRect),
            typeof(InputField),
            typeof(ParticleSystem),
            typeof(TextMeshProUGUI),
            typeof(Toggle),
            typeof(TMP_Dropdown),
            typeof(TMP_InputField),
            typeof(ToggleGroup)
        };
        
        public static readonly Dictionary<Type, string> comNameDict = new Dictionary<Type, string>()
        {
            {typeof(Image), "img"},
            {typeof(Button), "btn"},
            {typeof(Text), "txt"},
            {typeof(Slider), "slider"},
            {typeof(ScrollRect), "scrt"},
            {typeof(InputField), "inptf"},
            {typeof(ParticleSystem), "particle"},
            {typeof(TextMeshProUGUI), "tmp"},
            {typeof(Toggle), "tog"},
            {typeof(TMP_Dropdown), "tmpDropDown"},
            {typeof(TMP_InputField), "tmpInput"},
            {typeof(ToggleGroup), "toGroup"},
        };
        
        public static readonly string goNamePrefix = "go";

        public void Init(MonoBehaviour root)
        {
            comDict = new Dictionary<string, Component>();

            GetUIComponents(root.gameObject);
        }

        private void GetUIComponents(GameObject root)
        {
            if (root.name.StartsWith(genableSign))
            {
                string comGoName = root.name.Replace(genableSign, string.Empty);

                Component[] coms = root.GetComponents<Component>();

                PropertyInfo goProp = GetType().GetProperty(goNamePrefix + comGoName);
                if (goProp!=null)
                {
                    goProp.SetValue(this,root,new object[0]);
                }

                foreach (Component com in coms)
                {
                    if (com==null)
                    {
                        continue;
                    }

                    Type comType = com.GetType();

                    if (uiCompTypeList.Contains(comType))
                    {
                        StringBuilder builder = new StringBuilder();

                        string name = (builder.Append(comNameDict[comType]).Append(comGoName)).ToString();

                        if (!comDict.ContainsKey(name))
                        {
                            comDict.Add(name,com);
                        }
                        else
                        {
                            Debug.LogError("重名！" + name);
                        }
                        
                        //赋予组件值
                        PropertyInfo prop = GetType().GetProperty(name);
                        if (prop != null)
                        {
                            prop.SetValue(this, com, new object[0]);
                        }
                    }
                }
            }
            
            if (root.transform.childCount>0)
            {
                for (int i = 0; i < root.transform.childCount; i++)
                {
                    GetUIComponents(root.transform.GetChild(i).gameObject);
                }
            }
        }
    }

}
