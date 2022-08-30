using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityGameFramework.Runtime
{
    public class UGUIEventListener : EventTrigger
    {
        public delegate void VoidDelegate(GameObject go);

        public delegate void VoidDelegate_Drag(GameObject go, Vector2 delta);

        public event VoidDelegate onClick;
        public event VoidDelegate onDown;
        public event VoidDelegate onEnter;
        public event VoidDelegate onExit;
        public event VoidDelegate onUp;
        public event VoidDelegate onSelect;
        public event VoidDelegate onUpdateSelect;
        public event VoidDelegate_Drag onDrag;
        public event VoidDelegate onScroll;
        public event VoidDelegate onBeginDrag;
        public event VoidDelegate onEndDrag;

        public static UGUIEventListener Get(GameObject go)
        {
            UGUIEventListener listener = go.GetComponent<UGUIEventListener>();

            if (listener == null)
            {
                listener = InitGo(go);
            }

            return listener;
        }

        private static UGUIEventListener InitGo(GameObject go)
        {
            UGUIEventListener listener = go.AddComponent<UGUIEventListener>();

            ///DO something

            return listener;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && eventData.dragging == false)
            {
                onClick?.Invoke(gameObject);
            }
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null) onDown(gameObject);
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null) onEnter(gameObject);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null) onExit(gameObject);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if(eventData.button==PointerEventData.InputButton.Left)
                if (onUp != null) onUp(gameObject);
        }
        public override void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null) onSelect(gameObject);
        }
        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null) onUpdateSelect(gameObject);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null) onDrag(gameObject,eventData.delta);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (onBeginDrag != null) onBeginDrag(gameObject);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (onEndDrag != null) onEndDrag(gameObject);
        }

        public override void OnScroll(PointerEventData eventData)
        {
            if (onScroll != null) onScroll(gameObject);
        }
    }
}