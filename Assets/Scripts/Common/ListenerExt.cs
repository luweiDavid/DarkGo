/****************************************************
    文件：ListenerExt.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/28 12:32:30
	功能：事件监听扩展类
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListenerExt : MonoBehaviour, IPointerDownHandler,
    IPointerUpHandler,IDragHandler
{
    public Action<PointerEventData> pointerDownCB;
    public Action<PointerEventData> pointerUpCB;
    public Action<PointerEventData> onDragCB;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pointerDownCB != null) {
            pointerDownCB(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDragCB != null)
        {
            onDragCB(eventData);
        }
    } 

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerUpCB != null)
        {
            pointerUpCB(eventData);
        }
    }
}