using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce.LocalizationGenerator
{
    [ExecuteInEditMode]
    public class LanguageText : MonoBehaviour
    {
        private int _id = -1;
        private TextMeshPro _textMeshPro;
        private Text _text;
        private TextMeshProUGUI _textMeshProUGUI;

        /// <summary>
        /// 是否是显示Id的模式
        /// </summary>
        private bool _showId;

        private bool _init;

        private string _languageStr;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }


        private void Awake()
        {
            InitComponent();

            // GameEntry.Localization.
        }

        private void OnDestroy()
        {
            
        }

        private void InitComponent()
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
                str = _id.ToString();
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
    }
}