using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class ClickableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{

    public Action ClickFunc = null;
    public Action MouseRightClickFunc = null;
    public Action MouseMiddleClickFunc = null;
    public Action MouseDownOnceFunc = null;
    public Action MouseUpFunc = null;
    public Action MouseOverOnceTooltipFunc = null;
    public Action MouseOutOnceTooltipFunc = null;
    public Action MouseOverOnceFunc = null;
    public Action MouseOutOnceFunc = null;
    public Action<PointerEventData> OnPointerClickFunc;
    private Action internalOnPointerEnterFunc, internalOnPointerExitFunc, internalOnPointerClickFunc;

    public bool hoverBehaviour_Move = false;
    private Vector2 posExit, posEnter;
    public bool triggerMouseOutFuncOnClick = false;
    private bool mouseOver;
    private float mouseOverPerSecFuncTimer;

    public virtual void OnPointerEnter(PointerEventData eventData) {
        if (internalOnPointerEnterFunc != null) {
            internalOnPointerEnterFunc();
        }

        if (hoverBehaviour_Move) {
            transform.localPosition = posEnter;
        }

        if (MouseOverOnceFunc != null) {
            MouseOverOnceFunc();
        }

        if (MouseOverOnceTooltipFunc != null) {
            MouseOverOnceTooltipFunc();
        }

        mouseOver = true;
        mouseOverPerSecFuncTimer = 0f;
    }

    public virtual void OnPointerExit(PointerEventData eventData) {
        if (internalOnPointerExitFunc != null) {
            internalOnPointerExitFunc();
        }

        if (hoverBehaviour_Move) {
            transform.localPosition = posExit;
        }

        if (MouseOutOnceFunc != null) {
            MouseOutOnceFunc();
        }

        if (MouseOutOnceTooltipFunc != null) {
            MouseOutOnceTooltipFunc();
        }
        mouseOver = false;
    }

    public virtual void OnPointerClick(PointerEventData eventData) {

        if (eventData.button == PointerEventData.InputButton.Left) {
            if (triggerMouseOutFuncOnClick) {
                OnPointerExit(eventData);
            }
            if (ClickFunc != null) ClickFunc();
        }
        if (eventData.button == PointerEventData.InputButton.Right) {
            if (MouseRightClickFunc != null) {
                MouseRightClickFunc();
            }
        }
            
        if (eventData.button == PointerEventData.InputButton.Middle) {
            if (MouseMiddleClickFunc != null) {
                MouseMiddleClickFunc();
            }
        }
            
    }
}