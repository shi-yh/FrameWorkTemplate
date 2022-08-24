using System;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using Object = System.Object;

namespace StarForce.LocalizationGenerator
{
    public enum LanguageTextMode
    {
        None,
        Content,
        FirstTitle,
        SecondTitle,
        ThirdTitle
    }
    
    [ExecuteInEditMode]
    public class LanguageText : MonoBehaviour
    {
        private TextMeshPro _textMeshPro;
        private Text _text;
        private TextMeshProUGUI _textMeshProUGUI;

        /// <summary>
        /// 是否是显示Id的模式
        /// </summary>
        private bool _showId;

        private bool _init;

        private string _languageStr;

        public int Id { get; set; } = -1;

        public LanguageTextMode TextMode { get; set; }

        private void Awake()
        {
            InitComponent();

            // GameEntry.Localization.
        }

        private void OnDestroy()
        {
            
        }

        public string GetText()
        {
            string str = "";

            if (_text!=null)
            {
                str = _text.text;
            }

            if (_textMeshPro!=null)
            {
                str = _textMeshPro.text;
            }

            if (_textMeshProUGUI!=null)
            {
                str = _textMeshProUGUI.text;
            }
            
            
            return str;
        }
        

        public void InitComponent()
        {
            if (_init)
            {
                return;
            }

            _init = true;

            if (_text == null && _textMeshPro == null && _textMeshProUGUI == null)
            {
                _text = GetComponent<Text>();

                _textMeshPro = GetComponent<TextMeshPro>();

                _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            }

            if (_text != null)
            {
                if (_text.raycastTarget)
                {
                    _text.raycastTarget = false;
                }
            }
            else if (_textMeshPro != null)
            {
                if (_textMeshPro.raycastTarget)
                {
                    _textMeshPro.raycastTarget = false;
                }
            }
            else if (_textMeshProUGUI != null)
            {
                if (_textMeshProUGUI.raycastTarget)
                {
                    _textMeshProUGUI.raycastTarget = false;
                }
            }
            else
            {
                Debug.LogError($"{gameObject.name}:初始化LanguageText错误");
            }
            
        }

        
        /// <summary>
        /// 改变多语言展示模式
        /// </summary>
        /// <param name="showId"></param>
        public void ChangeShowState(bool showId)
        {
            if (showId == _showId)
            {
                return;
            }

            _showId = showId;
            
            RefreshText(_languageStr);   
        }

        public void SetFont(Font font,TMP_FontAsset fontAsset)
        {
            if (_text != null)
            {
                _text.font = font;
            }
            else if (_textMeshPro!=null)
            {
                _textMeshPro.font = fontAsset;
            }
            else if (_textMeshProUGUI.text!=null)
            {
                _textMeshProUGUI.font = fontAsset;
            }
        }
        
        public void RefreshText(string str)
        {
            InitComponent();

            _languageStr = str;

            if (_showId)
            {
                str = Id.ToString();
            }

            if (_text != null)
            {
                _text.text = str;
            }
            else if (_textMeshPro!=null)
            {
                _textMeshPro.text = str;
            }
            else if (_textMeshProUGUI.text!=null)
            {
                _textMeshProUGUI.text = str;
            }
        }

        public string GetActiveText()
        {
            if (_text!=null)
            {
                return  _text.GetType().Name;
            }

            if (_textMeshPro!=null)
            {
                return _textMeshPro.mesh.GetType().Name;
            }

            if (_textMeshProUGUI!=null)
            {
                return _textMeshProUGUI.GetType().Name;
            }
            
            return "";

        }
        
        
        
    }
}